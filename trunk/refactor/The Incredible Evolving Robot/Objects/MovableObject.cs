using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects
{
    abstract public class MovableObject : BasicObject
    {
        #region Properties
        public Movement Movement { get; set; }
        #endregion

        public MovableObject(Game game, Boolean isCollidable)
            : base(game, isCollidable)
        {
            Movement = new Movement();
        }

        public override void Update(GameTime gameTime)
        {
            Position.Coordinate += Movement.Velocity * (gameTime.ElapsedGameTime.Milliseconds * 0.001f);
            base.Update(gameTime);
        }
    }
}
