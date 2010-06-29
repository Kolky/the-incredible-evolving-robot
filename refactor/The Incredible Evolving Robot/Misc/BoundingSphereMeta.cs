using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
    public class BoundingSphereMeta
    {
        #region Properties
        public Vector3 Offset { get; set; }
        public Vector3 Center { get; set; }
        public BoundingSphere Sphere { get; set; }
        #endregion

        public BoundingSphereMeta(BoundingSphere sphere, Vector3 offset)
        {
            Sphere = sphere;
            Offset = offset;
        }
    }
}
