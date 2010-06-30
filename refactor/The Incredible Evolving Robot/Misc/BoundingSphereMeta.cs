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
        public float Radius { get; set; }
        public BoundingSphere Sphere
        {
            get
            {
                return new BoundingSphere(Center, Radius);
            }
        }
        #endregion

        public BoundingSphereMeta(Vector3 center, float radius, Vector3 offset)
        {
            Center = center;
            Radius = radius;
            Offset = offset;
        }
    }
}
