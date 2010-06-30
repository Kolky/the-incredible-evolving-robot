using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Handlers;

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

#if DEBUG && BOUNDRENDER
        public override void Draw()
        {
            Draw(Color.White);
        }

        public override void Draw(Color color)
        {
            VertexPositionColor[] vertices =
            {
                new VertexPositionColor(Center + Bounds, color),
                new VertexPositionColor(new Vector3(Center.X - Bounds.X, Center.Y + Bounds.Y, Center.Z + Bounds.Z), color),
                new VertexPositionColor(new Vector3(Center.X - Bounds.X, Center.Y + Bounds.Y, Center.Z - Bounds.Z), color),
                new VertexPositionColor(new Vector3(Center.X + Bounds.X, Center.Y + Bounds.Y, Center.Z - Bounds.Z), color),
                new VertexPositionColor(new Vector3(Center.X + Bounds.X, Center.Y - Bounds.Y, Center.Z + Bounds.Z), color),
                new VertexPositionColor(new Vector3(Center.X - Bounds.X, Center.Y - Bounds.Y, Center.Z + Bounds.Z), color),
                new VertexPositionColor(Center - Bounds, color),
                new VertexPositionColor(new Vector3(Center.X + Bounds.X, Center.Y - Bounds.Y, Center.Z - Bounds.Z), color)
            };

            DrawVertices(vertices);
        }
#endif
    }
}
