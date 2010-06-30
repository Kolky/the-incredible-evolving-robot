using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Objects.Destroyable.Projectile;

namespace Tier.Objects
{
    public class Projectile : DestroyableObject
    {
        #region Properties
        public int TimeToLive { get; protected set; }
        public int CurrentTime { get; private set; }
        #endregion

        public Projectile(Game game, Position sourcePos, int speed)
            : this(game, true, sourcePos, speed)
        {
        }

        public Projectile(Game game, Boolean isCollidable, Position sourcePos, int speed)
            : base(game, isCollidable)
        {
            Position = new Position(sourcePos);
            Movement.Velocity = Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(sourcePos.Front));
            Movement.Velocity.Normalize();
            Movement.Velocity *= speed;
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentTime >= TimeToLive)
                GameHandler.ObjectHandler.RemoveObject(this);
            CurrentTime += gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);
        }
    }
}