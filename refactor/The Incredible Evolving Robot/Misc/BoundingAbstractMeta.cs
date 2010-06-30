using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Handlers;

namespace Tier.Misc
{
    public abstract class BoundingAbstractMeta
    {
        #region Properties
        public Vector3 Center { get; set; }
        public Vector3 Offset { get; set; }
        public virtual BoundingBox Box
        {
            get { return new BoundingBox(); }    
        }
        public virtual BoundingSphere Sphere
        {
            get { return new BoundingSphere(); }
        }
        public Boolean IsBox { get; protected set; }
        #endregion

#if DEBUG && BOUNDRENDER
        private static BasicEffect effect;
        private static VertexDeclaration decl;
        private const int maxPoints = 8;
        private static short[] indices =
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        static BoundingAbstractMeta()
        {
            effect = new BasicEffect(TierGame.Device, null);
            effect.VertexColorEnabled = true;
            decl = new VertexDeclaration(TierGame.Device, VertexPositionColor.VertexElements);

        }
#endif

        public BoundingAbstractMeta(Vector3 center, Vector3 offset)
        {
            Center = center;
            Offset = offset;
            IsBox = true;

        }

        public virtual Boolean CheckCollision(BoundingAbstractMeta obj)
        {
            if(IsBox)
            {
                return (obj.IsBox ? Box.Intersects(obj.Box) : Box.Intersects(obj.Sphere));
            }
            else
            {
                return (obj.IsBox ? Sphere.Intersects(obj.Box) : Sphere.Intersects(obj.Sphere));
            }
        }

        public void Update(Vector3 position, Quaternion rotation)
        {
            Center = position + Vector3.Transform(Offset, rotation);
        }

        public void Update(Vector3 position, Matrix matrix)
        {
            Center = position + Vector3.Transform(Offset, matrix);
        }

#if DEBUG && BOUNDRENDER
        public abstract void Draw();
        public abstract void Draw(Color color);

        protected static void DrawVertices(VertexPositionColor[] vertices)
        {
            effect.Begin();
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = GameHandler.Camera.View;
            effect.Projection = GameHandler.Camera.Projection;
            TierGame.Device.VertexDeclaration = decl;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                TierGame.Device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length, indices, 0, 12);
                pass.End();
            }

            effect.End();
        }
#endif
    }
}
