using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;

namespace Tier.Source.Helpers.Cameras
{
  class RotatingCamera : Camera
  {
    private float rotation;
    private float step;
    private float ttl, timeElapsed;
    private GameObject parent;
    private float distance;

    public float Distance
    {
      get { return distance; }
      set { distance = value; }
    }
	

    public RotatingCamera(TierGame game)
      : base(game)
    {
      this.up = new Vector3(0, 1, 0);
      this.distance = 40.0f;
      this.rotation = 0.0f;
    }

    public void StartRotation(GameObject obj, float milliseconds, float rotation)
    {
      this.game.GameHandler.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.33f, 0.1f, 1000.0f);
      this.step = rotation;
      this.parent = obj;
      this.ttl = milliseconds;
      this.done = false;
    }

    public override void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      // Turn 90 degrees in a given timespan
      if (this.timeElapsed < this.ttl)
      {        
        this.rotation += this.step * (gameTime.ElapsedGameTime.Milliseconds / this.ttl);
        Vector3 diff = Vector3.Transform(new Vector3(0, 10, distance), Matrix.CreateRotationY(this.rotation));

        this.position = this.parent.Position + diff;
      }
      else
      {
        // Done
        this.done = true;
        this.timeElapsed = 0;
      }

      this.target = this.parent.Position;

      CreateLookatMatrix();
      //this.game.GameHandler.View = Matrix.CreateLookAt(this.position, this.parent.Position, this.up);
    }


  }
}