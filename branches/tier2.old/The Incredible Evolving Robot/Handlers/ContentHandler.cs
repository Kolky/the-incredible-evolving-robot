using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tier.Misc;
using System.IO;
#if XBOX360
using InstancedModelSample;
#else
using Instanced;
#endif

namespace Tier.Handlers
{
	/// <summary>Contains data about the model and collision using either or both BoundingShere(s) and BoundingBox(es)</summary>
	public class ModelMeta
	{
		#region Properties
		private Model model;
		public Model Model
		{
			get { return model; }
			set { model = value; }
		}

		private InstancedModel modelInstanced;
		public InstancedModel ModelInstanced
		{
			get { return modelInstanced; }
			set { modelInstanced = value; }
		}
		#endregion

		public ModelMeta(Model model) : this(model, null) { }
		public ModelMeta(InstancedModel modelInstanced) : this(null, modelInstanced) { }
		public ModelMeta(Model model, InstancedModel modelInstanced)
		{
			this.Model = model;
			this.ModelInstanced = modelInstanced;
		}
	}

  public class AnimatedTexture
  {
    private List<Texture2D> frames;
    public List<Texture2D> Frames
    {
      get { return frames; }
      set { frames = value; }
    }

    public AnimatedTexture()
    {
      frames = new List<Texture2D>();
    }
  }

  public class ContentHandler
  {
    #region Properties
    private Hashtable effects;
    public Hashtable Effects
    {
      get { return effects; }
      set { effects = value; }
    }

    private Hashtable textures;
    public Hashtable Textures
    {
      get { return textures; }
      set { textures = value; }
    }

    private Hashtable animatedTextures;
    public Hashtable AnimatedTextures
    {
      get { return animatedTextures; }
      set { animatedTextures = value; }
    }

    private Hashtable modelMetas;
    public Hashtable ModelMetas
    {
      get { return modelMetas; }
      set { modelMetas = value; }
    }

    private Hashtable instancedModels;
    #endregion

    public ContentHandler()
    {
      this.Effects = new Hashtable();
      this.Textures = new Hashtable();
      this.ModelMetas = new Hashtable();
      this.animatedTextures = new Hashtable();
      this.instancedModels = new Hashtable();
		}

		#region modelMmeta modifiers (set and getters)
		public Model GetModel(String name)
    {
      return ((ModelMeta)this.ModelMetas[name]).Model;
    }

    public ModelMeta GetModelMeta(String name)
    {
			return this.ModelMetas.ContainsKey(name)? ((ModelMeta)this.ModelMetas[name]): null;
    }

		/// <summary>Create a model without collision detection</summary>
    public void setModel(String name, Model model)
    {
      this.ModelMetas.Add(name, new ModelMeta(model));
    }
		#endregion

		#region animated texture
		public void setAnimatedTexture(String name, String filepath)
    {
      int texNumber = 1;

      AnimatedTexture at = new AnimatedTexture();
			String temp;
      while (File.Exists(String.Format("./{0}{1}.xnb", filepath, texNumber.ToString("0000"))))
      {
        temp = String.Format("{0}{1}", filepath, texNumber.ToString("0000"));

        //Read all textures in this animation
        at.Frames.Add(TierGame.Content.Load<Texture2D>(temp));

        // animatedTextures.Add(name+texNumber, );
        texNumber++;
      }
      texNumber = 0;

      this.animatedTextures.Add(name, at);
    }

    public Texture2D getAnimatedTexture(String name, int index)
    {
      return ((AnimatedTexture)this.animatedTextures[name]).Frames[index];
    }

    public int getAnimatedTextureCount(String name)
    {
      return ((AnimatedTexture)this.animatedTextures[name]).Frames.Count;
		}
		#endregion

		#region EFFECT
		public void setEffect(String name, Effect effect)
		{
			this.Effects.Add(name, effect);
		}

		public Effect getEffect(String name)
		{
			return (Effect)this.Effects[name];
		}
		#endregion

    #region instancedModels
    public void setInstancedModel(String name, InstancedModel model)
    {
			this.ModelMetas.Add(name, new ModelMeta(model));
    }

    public InstancedModel getInstancedModel(String name)
    {
			return ((ModelMeta)this.ModelMetas[name]).ModelInstanced;
    }
    #endregion

  }
}