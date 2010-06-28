using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
	public class BoundingSphereMeta
	{
		#region Properties
    private Vector3 offset;
    public Vector3 Offset
		{
      get { return offset; }
      set { offset = value; }
		}

		private BoundingSphere sphere;
		public BoundingSphere Sphere
		{
			get { return sphere; }
      set { sphere = value; }
		}

		public Vector3 Center
		{
			get { return sphere.Center; }
      set { sphere.Center = value; }
		}
		#endregion

    public BoundingSphereMeta(BoundingSphere sphere, Vector3 offset)
		{
      this.Sphere = sphere;
      this.Offset = offset;
		}
	}
}
