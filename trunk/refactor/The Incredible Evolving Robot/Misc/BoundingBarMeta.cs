using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
    public class BoundingBarMeta : BoundingAbstractMeta
    {
        #region Properties
        public Vector3 BoundsLeft { get; set; }
        public Vector3 BoundsRight { get; set; }
        public override BoundingBox Box
        {
            get { return new BoundingBox(Center - BoundsLeft, Center + BoundsRight); }
        }
        #endregion

        public BoundingBarMeta(Vector3 center, Vector3 boundsLeft, Vector3 boundsRight, Vector3 offset)
            : base(center, offset)
        {
            BoundsLeft = boundsLeft;
            BoundsRight = boundsRight;
        }
    }
}
