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
    private Movement movement;
    public Movement Movement
    {
      get { return movement; }
      set { movement = value; }
    }	
    #endregion

		public MovableObject(Game game, Boolean isCollidable)
      : base(game, isCollidable)
    {
      this.Movement = new Movement();
    }

    public override void Update(GameTime gameTime)
    {
      this.Position.Coordinate += this.Movement.Velocity * (gameTime.ElapsedGameTime.Milliseconds * 0.001f);
      base.Update(gameTime);
    }
  }
}
