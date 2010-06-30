using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Misc
{
    public class BoundingSphereMeta : BoundingAbstractMeta
    {
        #region Properties
        public float Radius { get; set; }
        public override BoundingSphere Sphere
        {
            get { return new BoundingSphere(Center, Radius); }
        }
        #endregion

        public BoundingSphereMeta(Vector3 center, float radius, Vector3 offset)
            : base(center, offset)
        {
            Radius = radius;
            IsBox = false;
        }

#if DEBUG && BOUNDRENDER
        public override void Draw()
        {
            TierGame.BoundingSphereRender.Draw(Sphere, Color.White);
        }

        public override void Draw(Color color)
        {
            TierGame.BoundingSphereRender.Draw(Sphere, color);
        }
#endif
    }
}
