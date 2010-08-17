using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;

namespace Tier.Source.Helpers.Cameras
{
  public class TrackingCamera : Camera
  {
    #region Properties
    private GameObject parent;
    private float distance;

    public float Distance
    {
      get { return distance; }
      set { distance = value; }
    }
	
    /// <summary>
    /// The object which will be tracked
    /// </summary>
    public GameObject Parent
    {
      get { return parent; }
      set { parent = value; }
    }
    #endregion

    private float rot;

    public TrackingCamera(TierGame game, GameObject obj) : base(game)
    {
      this.parent = obj;
      this.rot = 0;
    }

    public void Rotate(float rotation)
    {      
      this.rot += rotation;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      Vector3 diff = Vector3.Transform(new Vector3(0, 0, distance), Matrix.CreateRotationY(this.rot));

      if (this.parent != null)
      {
        this.position = this.Parent.Position + diff;

        this.game.GameHandler.View =
          Matrix.CreateLookAt(this.position, this.parent.Position, Vector3.Up);
      }
      else
      {
        this.position = diff;

        this.game.GameHandler.View =
          Matrix.CreateLookAt(this.position, Vector3.Zero, Vector3.Up);
      }
    }
  }
}
