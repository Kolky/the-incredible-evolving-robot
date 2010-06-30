using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

#if DEBUG && BOUNDRENDER
        public override void Draw()
        {
            Draw(Color.White);
        }

        public override void Draw(Color color)
        {
            VertexPositionColor[] vertices =
            {
                new VertexPositionColor(Center + BoundsRight, color),
                new VertexPositionColor(new Vector3(Center.X - BoundsLeft.X, Center.Y + BoundsRight.Y, Center.Z + BoundsRight.Z), color),
                new VertexPositionColor(new Vector3(Center.X - BoundsLeft.X, Center.Y + BoundsRight.Y, Center.Z - BoundsLeft.Z), color),
                new VertexPositionColor(new Vector3(Center.X + BoundsRight.X, Center.Y + BoundsRight.Y, Center.Z - BoundsLeft.Z), color),
                new VertexPositionColor(new Vector3(Center.X + BoundsRight.X, Center.Y - BoundsLeft.Y, Center.Z + BoundsRight.Z), color),
                new VertexPositionColor(new Vector3(Center.X - BoundsLeft.X, Center.Y - BoundsLeft.Y, Center.Z + BoundsRight.Z), color),
                new VertexPositionColor(Center - BoundsLeft, color),
                new VertexPositionColor(new Vector3(Center.X + BoundsRight.X, Center.Y - BoundsLeft.Y, Center.Z - BoundsLeft.Z), color)
            };

            DrawVertices(vertices);
        }
#endif
    }
}
