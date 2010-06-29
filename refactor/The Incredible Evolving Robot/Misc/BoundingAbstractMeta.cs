using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Misc
{
    public abstract class BoundingAbstractMeta
    {
        #region Properties
        public Vector3 Center { get; set; }
        public Vector3 Offset { get; set; }
        public abstract BoundingBox Box { get; }
        #endregion

        public BoundingAbstractMeta(Vector3 center, Vector3 offset)
        {
            Center = center;
            Offset = offset;
        }
    }
}
