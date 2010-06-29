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
        public int ID { get; set; }
        public SortFilter Sort { get; set; }
        public virtual Position Position { get; set; }
        public float Scale { get; set; }
        public Model Model { get; set; }
        public String ModelName { get; set; }
        public Texture Texture { get; set; }
        public BasicEffect Effect { get; set; }
        public Matrix RotationFix { get; set; }
        public Boolean IsCollidable { get; set; }
        public ModelMeta ModelMeta { get; set; }

        #region Draw helpers
        public bool FirstInList { get; set; }
        public bool LastInList { get; set; }
        public bool IsInstanced { get; set; }
        #endregion
        #endregion

        public BasicObject(Game game)
            : this(game, false)
        {
        }

        public BasicObject(Game game, Boolean isCollidable)
            : base(game)
        {
            Position = new Position();
            RotationFix = Matrix.Identity;
            Sort = SortFilter.Other;
            IsCollidable = isCollidable;
            ModelName = String.Empty;

            ID = Options.IdCounter++;
        }

        public override void Initialize()
        {
            if (Model != null)
            {
                try
                {
                    foreach (ModelMesh mesh in Model.Meshes)
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
            if (Model != null)
            {
                for (int i = 0; i < Model.Meshes.Count; i++)
                {
                    foreach (BasicEffect effect in Model.Meshes[i].Effects)
                    {
                        effect.World = Matrix.CreateScale(Scale) *
                            RotationFix *
                            Matrix.CreateFromQuaternion(Position.Front) *
                            Matrix.CreateTranslation(Position.Coordinate);
                        effect.View = GameHandler.Camera.View;
                        effect.Projection = GameHandler.Camera.Projection;
                    }
                    Model.Meshes[i].Draw();
                }
            }
        }

        #region IComparable Members
        public virtual int CompareTo(object obj)
        {
            BasicObject otherObject = (BasicObject)obj;

            if (Sort < otherObject.Sort)
            {
                return -1;
            }
            else if (Sort > otherObject.Sort)
            {
                return 1;
            }
            else if (IsInstanced && otherObject.IsInstanced) //beide instanced
            {
                if (ModelName.GetHashCode() < otherObject.ModelName.GetHashCode())
                {
                    return -1;
                }
                else if (ModelName.GetHashCode() > otherObject.ModelName.GetHashCode())
                {
                    return 1;
                }
                return 0;
            }
            else if (IsInstanced) //deze instanced
            {
                return 1;
            }
            else if (otherObject.IsInstanced) //andere instanced
            {
                return -1;
            }
            else //beide niet instanced
            {
                if (GetType().GetHashCode() < otherObject.GetType().GetHashCode())
                {
                    return -1;
                }
                else if (GetType().GetHashCode() > otherObject.GetType().GetHashCode())
                {
                    return 1;
                }
                return 0;
            }
        }
        #endregion
    }
}
