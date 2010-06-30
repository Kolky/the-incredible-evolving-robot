using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Tier.Objects;
using Tier.Handlers;
using Tier.Misc;
using Microsoft.Xna.Framework.Graphics;
using Tier.Objects.Attachable;

namespace Tier.Objects.Destroyable.Projectile
{
    public class Laser : Tier.Objects.Projectile
    {
        public Laser(Game game, Position sourcePos)
            : base(game, sourcePos, 40)
        {
            TimeToLive = 750;
            ModelName = "Laser";
            Model = TierGame.ContentHandler.GetModel(ModelName);
            Scale = 0.075f;
            Sort = SortFilter.Other;

            addBoundingShere(0.1f, Vector3.Zero);
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2 + MathHelper.Pi);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateBoundingObjects();
            base.Update(gameTime);
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
    }
}
