using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
    public class BoundingBoxMeta : BoundingAbstractMeta
    {
        #region Properties
        public Vector3 Bounds { get; set; }
        public override BoundingBox Box
        {
            get { return new BoundingBox(Center - Bounds, Center + Bounds); }
        }
        #endregion

        public BoundingBoxMeta(Vector3 center, Vector3 bounds, Vector3 offset)
            : base(center, offset)
        {
            Bounds = bounds;
        }
    }
}
