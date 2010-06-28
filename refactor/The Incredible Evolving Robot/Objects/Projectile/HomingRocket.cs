using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Misc;

namespace Tier.Objects.Destroyable.Projectile
{
    public class HomingRocket : Tier.Objects.Projectile
    {
        #region Properties
        private int updateCoolDown = 0;
        #endregion

        public HomingRocket(Game game, Position sourcePos)
            : base(game, true, sourcePos, 10)
        {
            this.TimeToLive = 10000;
            this.ModelName = "Rocket";
            this.ModelMeta = (IsCollidable) ? TierGame.ContentHandler.GetModelMeta(ModelName) : null;
            this.Model = (this.ModelMeta != null) ? this.ModelMeta.Model : null;
            this.Scale = 0.025f;
            //this.Sort = SortFilter.OtherInstanced;
            //this.IsInstanced = true;

            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            //   this.RotationFix = Matrix.CreateRotationY(-MathHelper.PiOver2);
            this.RotationFix = Matrix.CreateRotationX(MathHelper.PiOver2);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.updateCoolDown >= Options.HomingRocket.UpdateCheck)
            {
                //deel van een seconde dat voorbij is    
                float elap = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

                Vector3 straightToTarget = Vector3.Subtract(GameHandler.Player.Position.Coordinate, this.Position.Coordinate);


                if (straightToTarget.LengthSquared() < Options.HomingRocket.MaxDistance)
                {
                    Vector3 oldDirection = this.Movement.Velocity;
                    Vector3 newDirection = Vector3.Add(oldDirection, straightToTarget);

                    oldDirection.Normalize();
                    newDirection.Normalize();

                    float angle = (float)Math.Acos(Vector3.Dot(oldDirection, newDirection));

                    Vector3 axis = Vector3.Cross(oldDirection, newDirection);
                    axis.Normalize();

                    if (angle > (Options.HomingRocket.MaxAngle * elap))
                    {
                        angle = Options.HomingRocket.MaxAngle * elap;
                        newDirection = Vector3.Transform(oldDirection, Matrix.CreateFromAxisAngle(axis, angle));
                    }
                    else if (angle.Equals(float.NaN))
                        angle = 0.0f;

                    Quaternion rotation = Quaternion.CreateFromAxisAngle(axis, angle);

                    this.Position.Front = Quaternion.Concatenate(this.Position.Front, rotation);
                    this.Movement.Velocity = newDirection * 5;
                }

                this.updateCoolDown = 0;
            }
            this.updateCoolDown++;

            base.Update(gameTime);
        }
    }
}