using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers.Cameras
{
  public abstract class Camera : GameComponent
  {
    #region Properties
    protected TierGame game;
    protected Vector3 position;
    protected Vector3 target;
    protected Vector3 up;
    protected Vector3 forward;
    protected bool done;
    protected Matrix lookat;

    public bool Done
    {
      get { return done; }
      set { done = value; }
    }

    public Matrix LookAtMatrix
    {
      get { return lookat; }
    }

    public Vector3 ForwardVector
    {
      get { return forward; }
      set { forward = value; }
    }

    public Vector3 UpVector
    {
      get { return up; }
      set { up = value; }
    }
	
    public Vector3 Target
    {
      get { return target; }
      set { target = value; }
    }
	
    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }
	
    public new TierGame Game
    {
      get { return game; }
      set { game = value; }
    }
    #endregion

    public Camera(TierGame game) : base(game) 
    {
      this.game = game;
      this.up = Vector3.Up;
      this.forward = Vector3.Forward;
    }

    public virtual void CreateLookatMatrix()
    {
      this.lookat = Matrix.CreateLookAt(this.position, this.target, this.up);
    }

    public override abstract void Update(GameTime gameTime);
  }
}
