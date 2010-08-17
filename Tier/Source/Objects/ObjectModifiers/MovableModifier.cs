using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using Tier.Source.ObjectModifiers;

namespace Tier.Source.Objects.ObjectModifiers
{
  public class MovableModifier : ObjectModifier
  {
    #region Properties
    private Vector3 velocity;

    public Vector3 Velocity
    {
      get { return velocity; }
      set { velocity = value; }
    }
    #endregion

    public MovableModifier(GameObject obj)
      : base(obj)
    {
      obj.MovableModifier = this;
    }

    public override void Update(GameTime gameTime)
    {
      Vector3 velo;
      Vector3.Multiply(
        ref this.velocity,
        this.Parent.Game.timeElapsedSinceUpdate,
        out velo);

      this.Parent.Position = Vector3.Add(this.Parent.Position, velo);
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new MovableModifier(parent);
      ((MovableModifier)objmod).Velocity = this.Velocity;
    }
  }
}
