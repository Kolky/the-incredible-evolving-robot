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
        public Model Model { get; private set; }
        public InstancedModel ModelInstanced { get; private set; }
        #endregion

        public ModelMeta(Model model) : this(model, null) { }
        public ModelMeta(InstancedModel modelInstanced) : this(null, modelInstanced) { }
        public ModelMeta(Model model, InstancedModel modelInstanced)
        {
            Model = model;
            ModelInstanced = modelInstanced;
        }
    }

    public class AnimatedTexture
    {
        public List<Texture2D> Frames { get; private set; }

        public AnimatedTexture()
        {
            Frames = new List<Texture2D>();
        }
    }

    public class ContentHandler
    {
        #region Properties
        public Hashtable Effects { get; private set; }
        public Hashtable Textures { get; private set; }
        public Hashtable AnimatedTextures { get; private set; }
        public Hashtable ModelMetas { get; private set; }
        private Hashtable instancedModels;
        #endregion

        public ContentHandler()
        {
            Effects = new Hashtable();
            Textures = new Hashtable();
            ModelMetas = new Hashtable();
            AnimatedTextures = new Hashtable();
            instancedModels = new Hashtable();
        }

        #region modelMmeta modifiers (set and getters)
        public Model GetModel(String name)
        {
            return ((ModelMeta)ModelMetas[name]).Model;
        }

        public ModelMeta GetModelMeta(String name)
        {
            return ModelMetas.ContainsKey(name) ? ((ModelMeta)ModelMetas[name]) : null;
        }

        /// <summary>Create a model without collision detection</summary>
        public void setModel(String name, Model model)
        {
            ModelMetas.Add(name, new ModelMeta(model));
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
                at.Frames.Add(TierGame.Instance.Content.Load<Texture2D>(temp));

                // animatedTextures.Add(name+texNumber, );
                texNumber++;
            }
            texNumber = 0;

            AnimatedTextures.Add(name, at);
        }

        public Texture2D getAnimatedTexture(String name, int index)
        {
            return ((AnimatedTexture)AnimatedTextures[name]).Frames[index];
        }

        public int getAnimatedTextureCount(String name)
        {
            return ((AnimatedTexture)AnimatedTextures[name]).Frames.Count;
        }
        #endregion

        #region EFFECT
        public void setEffect(String name, Effect effect)
        {
            Effects.Add(name, effect);
        }

        public Effect getEffect(String name)
        {
            return (Effect)Effects[name];
        }
        #endregion

        #region instancedModels
        public void setInstancedModel(String name, InstancedModel model)
        {
            ModelMetas.Add(name, new ModelMeta(model));
        }

        public InstancedModel getInstancedModel(String name)
        {
            return ((ModelMeta)ModelMetas[name]).ModelInstanced;
        }
        #endregion

    }
}