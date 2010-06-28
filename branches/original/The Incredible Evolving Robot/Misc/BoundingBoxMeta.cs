using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
  public class BoundingBoxMeta
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
      get { return new BoundingBox(center - bounds, center + bounds); }
    }

    private Vector3 center;
    public Vector3 Center
		{
      get { return center; }
      set { center = value; }
		}

    private Vector3 bounds;
    public Vector3 Bounds
		{
      get { return bounds; }
      set { bounds = value; }
		}
		#endregion

    public BoundingBoxMeta(Vector3 center, Vector3 bounds, Vector3 offset)
		{
      this.Center = center;
      this.Bounds = bounds;
      this.Offset = offset;
		}
  }
}
