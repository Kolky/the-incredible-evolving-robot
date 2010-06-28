using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects
{
  public class Movement
  {
    #region Properties
    private Quaternion rotation;
    public Quaternion Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }

    private Vector3 velocity;
    public Vector3 Velocity
    {
      get { return velocity; }
      set { velocity = value; }
    }	
    #endregion

    public Movement()
    {
      this.Rotation = Quaternion.Identity;
      this.Velocity = Vector3.Zero;
    }
  }
}
