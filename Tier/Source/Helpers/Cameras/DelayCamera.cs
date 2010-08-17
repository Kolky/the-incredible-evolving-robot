using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;
using Tier.Source.Misc;

namespace Tier.Source.Helpers.Cameras
{
  public class DelayCamera : Camera
  {
    #region Properties
    private float retainPercentage;
    private float addPercentage;
    private GameObject source;
    private Vector3 offset;
    private double delayTimer = 0;
    private bool isLocked;

    /// <summary>
    /// Percentage of new state which will be added.
    /// </summary>
    public float AddPercentage
    {
      get { return addPercentage; }
      set { addPercentage = value; }
    }

    public bool IsLocked
    {
      get { return isLocked; }
      set { isLocked = value; }
    }

	  /// <summary>
	  /// Percentage of previous state which will be retained.
	  /// </summary>
    public float RetainPercentage
    {
      get { return retainPercentage; }
      set { retainPercentage = value; }
    }
	   
    public GameObject Source
    {
      get { return source; }
      set { source = value; }
    }
    
    public Vector3 Offset
    {
      get { return offset; }
      set { if (!isLocked) offset = value; }
    }    
    #endregion

    public DelayCamera(Vector3 position, Vector3 target, GameObject source, TierGame game)
      : this(position, target, source, Vector3.Zero, game)
    {
    }

    public DelayCamera(Vector3 position, Vector3 target, GameObject source, Vector3 offset, TierGame game)
      : base(game)
    {
      this.Source = source;
			this.Offset = offset;
			this.Target = this.Source.Position;

      this.up = Vector3.Up;
      this.forward = Vector3.Forward;

      this.retainPercentage = game.Options.DelayCamera_RetainPercentage;
      this.addPercentage = game.Options.DelayCamera_AddPercentage;

      this.game.GameHandler.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.33f, 0.1f, 10000.0f);
    }

    public void BypassDelay()
    {
      this.position =
        this.source.Position +
        Vector3.Transform(this.offset, this.source.Rotation);
    }

    public override void Update(GameTime gameTime)
    {
      this.delayTimer += gameTime.ElapsedGameTime.Milliseconds;

      float length = (Vector3.Zero - this.position).Length();
      if(length > 160)
        length = 160;

      if(length > 40)
      {
        float amount = (length - 40) / 120.0f;
        this.retainPercentage = 0.8f - 0.3f * amount;
        this.addPercentage = 1.0f - this.retainPercentage;
      }

      if (this.delayTimer >= this.game.Options.DelayCamera_DelayTimerLimit)
      {
        this.delayTimer = 0;

        this.target = this.source.Position;
        this.position = 
          this.position * this.retainPercentage +
          ((this.source.Position + Vector3.Transform(this.offset, this.source.Rotation)) * this.addPercentage);
        
        this.up = this.up * this.retainPercentage +
         (Vector3.Transform(Vector3.Up, this.source.Rotation) * this.addPercentage);

        this.forward = this.forward * this.retainPercentage +
          (Vector3.Transform(Vector3.Forward, this.source.Rotation) * this.addPercentage);
      }

      CreateLookatMatrix();
    }
  }
}