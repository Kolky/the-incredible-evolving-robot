using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Misc;

namespace Tier.Objects
{
  abstract public class BasicObject : DrawableGameComponent, IComparable
  {
    #region Properties
    private int id;
    public int ID
    {
      get { return id; }
      set { id = value; }
    }

    private SortFilter sort;
    public SortFilter Sort
    {
      get { return sort; }
      set { sort = value; }
    }

    private Position position;
    public virtual Position Position
    {
      get { return position; }
      set { position = value; }
    }

    private float scale;
    public float Scale
    {
      get { return scale; }
      set { scale = value; }
    }	

    private Model model;
    public Model Model
    {
      get { return model; }
      set { model = value; }
    }

		private String modelName;
		public String ModelName
		{
			get { return modelName; }
			set { modelName = value; }
		}

    private Texture texture;
    public Texture Texture
    {
      get { return texture; }
      set { texture = value; }
    }

    private BasicEffect effect;
    public BasicEffect Effect
    {
      get { return effect; }
      set { effect = value; }
    }

    private Matrix rotationFix;
    public Matrix RotationFix
    {
      get { return rotationFix; }
      set { rotationFix = value; }
    }

		private Boolean isCollidable;
		public Boolean IsCollidable
		{
			get { return isCollidable; }
			set { isCollidable = value; }
		}

    private ModelMeta modelMeta;
    public ModelMeta ModelMeta
    {
      get { return modelMeta; }
      set { modelMeta = value; }
    }


    #region Draw helpers
    private bool firstInList = false;
    public bool FirstInList
    {
      get { return firstInList; }
      set { firstInList = value; }
    }

    private bool lastInList = false;
    public bool LastInList
    {
      get { return lastInList; }
      set { lastInList = value; }
    }

    private bool isInstanced = false;

    public bool IsInstanced
    {
      get { return isInstanced; }
      set { isInstanced = value; }
    }
	
    #endregion
    #endregion

    public BasicObject(Game game)
			: this(game, false)
		{
		}


		public BasicObject(Game game, Boolean isCollidable)
			: base(game)
    {
      this.Position = new Position();
      this.RotationFix = Matrix.Identity;
      this.Sort = SortFilter.Other;
			this.IsCollidable = isCollidable;
      this.ModelName = String.Empty;

      this.ID = Options.IdCounter++;
		}

    public override void Initialize()
    {
      if (this.Model != null)
      {
        try
        {
          foreach (ModelMesh mesh in this.Model.Meshes)
          {
            foreach (BasicEffect effect in mesh.Effects)
            {
              effect.EnableDefaultLighting();
              effect.PreferPerPixelLighting = true;
            }
          }
        }
        catch (Exception)
        { }        
      }

      base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.model != null)
			{
				for (int i = 0; i < this.Model.Meshes.Count; i++)
				{
					foreach (BasicEffect effect in this.Model.Meshes[i].Effects)
					{
						effect.World = Matrix.CreateScale(this.Scale) *
							RotationFix *
							Matrix.CreateFromQuaternion(this.Position.Front) *							
              Matrix.CreateTranslation(this.Position.Coordinate);
						effect.View = GameHandler.Camera.View;
						effect.Projection = GameHandler.Camera.Projection;
					}
					this.Model.Meshes[i].Draw();
				}
      }
		}

		#region IComparable Members
    public virtual int CompareTo(object obj)
    {
      BasicObject otherObject = (BasicObject)obj;

      if (this.Sort < otherObject.Sort)
      {
        return -1;
      }
      else if (this.Sort > otherObject.Sort)
      {
        return 1;
      }
      else if (this.IsInstanced && otherObject.IsInstanced) //beide instanced
      {
        if (this.ModelName.GetHashCode() < otherObject.ModelName.GetHashCode())
        {
          return -1;
        }
        else if (this.ModelName.GetHashCode() > otherObject.ModelName.GetHashCode())
        {
          return 1;
        }
        return 0;
      }
      else if (this.IsInstanced) //deze instanced
      {
        return 1;
      }
      else if (otherObject.IsInstanced) //andere instanced
      {
        return -1;
      }
      else //beide niet instanced
      {
        if (this.GetType().GetHashCode() < otherObject.GetType().GetHashCode())
        {
          return -1;
        }
        else if (this.GetType().GetHashCode() > otherObject.GetType().GetHashCode())
        {
          return 1;
        }
        return 0;
      }
    }
    #endregion
  }
}
