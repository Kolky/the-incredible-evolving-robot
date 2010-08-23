using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Objects.Destroyable;
using Tier.Misc;

namespace Tier.Camera
{
  public class DelayCamera : Camera
  {
    #region Properties
    private Player source;
    public Player Source
    {
      get { return source; }
      set { source = value; }
    }

    private Vector3 offset;
    public Vector3 Offset
    {
      get { return offset; }
      set { offset = value; }
    }

		private Vector3 offsetPlayer;

    private double delayTimer = 0;
    #endregion

    public DelayCamera(Vector3 position, Vector3 target, Player source)
      : this(position, target, source, Vector3.Zero)
    {
    }

    public DelayCamera(Vector3 position, Vector3 target, Player source, Vector3 offset)
      : base(position, target)
    {
      this.Source = source;
			this.offsetPlayer = this.Offset = offset;
			this.Target = this.Source.Position.Coordinate;
    }

		public override void Update(GameTime gameTime)
		{
      if (this.delayTimer >= Options.Camera.DelayTimerLimit)
			{
        // Reset the timer
        this.delayTimer -= Options.Camera.DelayTimerLimit;
				this.offsetPlayer.Z = this.offset.Z + this.Source.DepthToShereRadius;
				this.Rotation = (this.Rotation * Options.Camera.DelayRotationLength) + (this.Source.Position.Front * (1f - Options.Camera.DelayRotationLength));
				this.Target = (this.Target * Options.Camera.DelayCoordinateLength) + (this.Source.Position.Coordinate * (1f - Options.Camera.DelayCoordinateLength)) + Vector3.Transform(this.offsetPlayer, this.Rotation);
			}
      this.delayTimer += gameTime.ElapsedGameTime.Milliseconds;

			base.Update(gameTime);
		}
  }
}