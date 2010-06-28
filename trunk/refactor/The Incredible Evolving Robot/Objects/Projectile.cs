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
    private int timeToLive;
    public int TimeToLive
    {
      get { return timeToLive; }
      set { timeToLive = value; }
    }
    private int currentTime;
    public int CurrentTime
    {
      get { return currentTime; }
    }
    #endregion

		public Projectile(Game game, Position sourcePos, int speed)
			: this(game, true, sourcePos, speed)
		{
		}

		public Projectile(Game game, Boolean isCollidable, Position sourcePos, int speed)
      : base(game, isCollidable)
    {
      this.Position = new Position(sourcePos);
      this.Movement.Velocity = Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(sourcePos.Front));
      this.Movement.Velocity.Normalize();
      this.Movement.Velocity *= speed;
    }

    public override void Update(GameTime gameTime)
    {
			//Time To Live
      if (this.currentTime >= this.TimeToLive)
        GameHandler.ObjectHandler.RemoveObject(this);
      this.currentTime += gameTime.ElapsedGameTime.Milliseconds;
			// ---

      base.Update(gameTime);
    }

  }
}