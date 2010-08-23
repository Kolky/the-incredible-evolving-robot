using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
  public class BoundingBarMeta
  {
    #region Properties
    private Vector3 offset;
    public Vector3 Offset
    {
      get { return offset; }
      set { offset = value; }
    }

    public BoundingBox Box
    {
      get { return new BoundingBox(center - boundsLeft, center + boundsRight); }
    }

    private Vector3 center;
    public Vector3 Center
    {
      get { return center; }
      set { center = value; }
    }

    private Vector3 boundsLeft;
    public Vector3 BoundsLeft
    {
      get { return boundsLeft; }
      set { boundsLeft = value; }
    }
    private Vector3 boundsRight;
    public Vector3 BoundsRight
    {
      get { return boundsRight; }
      set { boundsRight = value; }
    }
    #endregion

    public BoundingBarMeta(Vector3 center, Vector3 boundsLeft, Vector3 boundsRight, Vector3 offset)
    {
      this.Center = center;
      this.BoundsLeft = boundsLeft;
      this.BoundsRight = boundsRight;
      this.Offset = offset;
    }
  }
}
