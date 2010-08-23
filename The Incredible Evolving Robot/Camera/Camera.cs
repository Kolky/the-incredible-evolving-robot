using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Misc;

namespace Tier.Camera
{
  abstract public class Camera
  {
    #region Properties
    private Vector3 direction;
    /// <summary>
    /// Returns the direction the camera is looking at
    /// </summary>
    public Vector3 Direction
    {
      get { return direction; }
      set { direction = value; }
    }
	

    private Vector3 position;
    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }

    public Vector3 RealPosition
    {
      get
      {
        return Vector3.Transform(
      this.Position,
      this.Rotation);
      }
    }
	

    private Vector3 target;
    public Vector3 Target
    {
      get { return target; }
      set { target = value; }
    }

    private Vector3 up;
    public Vector3 Up
    {
      get { return up; }
    }

    private Vector3 forward;
    public Vector3 Forward
    {
      get { return forward; }
    }

    private Quaternion rotation;
    public Quaternion Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }

    private Matrix view;
    public Matrix View
    {
      get { return view; }
      set { view = value; }
    }

    private Matrix projection;
    public Matrix Projection
    {
      get { return projection; }
      set { projection = value; }
    }	
    #endregion

    public Camera(Vector3 position, Vector3 target)
    {
      this.Position = position;
      this.Target = target;
      this.Rotation = Quaternion.Identity;

      this.Projection = Matrix.CreatePerspectiveFieldOfView(
        Options.Camera.FieldOfView,
        Options.Camera.AspectRatio,
        Options.Camera.NearPlaneDistance,
        Options.Camera.FarPlaneDistance);
      this.View = Matrix.CreateLookAt(
        this.Position,
        this.Target,
        Vector3.Up);
    }

    public virtual void Rotate(float yaw, float pitch, float roll)
    {
      this.Rotation *= Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
    }

    public virtual void Update(GameTime gameTime)
    {
      this.View = Matrix.CreateLookAt(
        Vector3.Transform(
          this.Position,
          this.Rotation),
        this.Target,
        Vector3.Transform(
          Vector3.Up,
          this.Rotation));

      // The normalized difference vector between the current target and position is the direction
      this.direction = Vector3.Subtract(Vector3.Zero, this.target);
      this.direction.Normalize();
      
      this.up = Vector3.Transform(Vector3.Up, this.Rotation);
      this.forward = Vector3.Transform(Vector3.Forward, this.Rotation);
    }
  }
}
