using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Handlers;

namespace Tier.Misc
{
	public class BoundingBoxRenderer
	{
		//Private properties
		private const int maxPoints = 8;
		private VertexPositionColor[] vertices =new VertexPositionColor[maxPoints];
		private const int maxIndices = 24;
		private short[] indices = new short[maxIndices];
    private BasicEffect effect;
    private VertexDeclaration decl;
		
		public BoundingBoxRenderer()
		{
			#region Setup Indices
			// setup box indices
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 1;
			indices[3] = 2;
			indices[4] = 2;
			indices[5] = 3;
			indices[6] = 3;
			indices[7] = 0;

			indices[8] = 4;
			indices[9] = 5;
			indices[10] = 5;
			indices[11] = 6;
			indices[12] = 6;
			indices[13] = 7;
			indices[14] = 7;
			indices[15] = 4;

			indices[16] = 0;
			indices[17] = 4;
			indices[18] = 1;
			indices[19] = 5;
			indices[20] = 2;
			indices[21] = 6;
			indices[22] = 3;
			indices[23] = 7;
			#endregion

      this.effect = new BasicEffect(TierGame.Device, null);
      this.effect.VertexColorEnabled = true;
      this.decl = new VertexDeclaration(TierGame.Device, VertexPositionColor.VertexElements);
    }

    public void Draw(BoundingBarMeta bar, Color color)
    {
      this.vertices[0] = new VertexPositionColor(bar.Center + bar.BoundsRight, color);
      this.vertices[1] = new VertexPositionColor(new Vector3(bar.Center.X - bar.BoundsLeft.X, bar.Center.Y + bar.BoundsRight.Y, bar.Center.Z + bar.BoundsRight.Z), color);
      this.vertices[2] = new VertexPositionColor(new Vector3(bar.Center.X - bar.BoundsLeft.X, bar.Center.Y + bar.BoundsRight.Y, bar.Center.Z - bar.BoundsLeft.Z), color);
      this.vertices[3] = new VertexPositionColor(new Vector3(bar.Center.X + bar.BoundsRight.X, bar.Center.Y + bar.BoundsRight.Y, bar.Center.Z - bar.BoundsLeft.Z), color);

      this.vertices[4] = new VertexPositionColor(new Vector3(bar.Center.X + bar.BoundsRight.X, bar.Center.Y - bar.BoundsLeft.Y, bar.Center.Z + bar.BoundsRight.Z), color);
      this.vertices[5] = new VertexPositionColor(new Vector3(bar.Center.X - bar.BoundsLeft.X, bar.Center.Y - bar.BoundsLeft.Y, bar.Center.Z + bar.BoundsRight.Z), color);
      this.vertices[6] = new VertexPositionColor(bar.Center - bar.BoundsLeft, color);
      this.vertices[7] = new VertexPositionColor(new Vector3(bar.Center.X + bar.BoundsRight.X, bar.Center.Y - bar.BoundsLeft.Y, bar.Center.Z - bar.BoundsLeft.Z), color);

      this.DrawVertices();
    }

		public void Draw(BoundingBoxMeta box, Color color)
		{
			this.vertices[0] = new VertexPositionColor(box.Center + box.Bounds, color);
      this.vertices[1] = new VertexPositionColor(new Vector3(box.Center.X - box.Bounds.X, box.Center.Y + box.Bounds.Y, box.Center.Z + box.Bounds.Z), color);
      this.vertices[2] = new VertexPositionColor(new Vector3(box.Center.X - box.Bounds.X, box.Center.Y + box.Bounds.Y, box.Center.Z - box.Bounds.Z), color);
      this.vertices[3] = new VertexPositionColor(new Vector3(box.Center.X + box.Bounds.X, box.Center.Y + box.Bounds.Y, box.Center.Z - box.Bounds.Z), color);

      this.vertices[4] = new VertexPositionColor(new Vector3(box.Center.X + box.Bounds.X, box.Center.Y - box.Bounds.Y, box.Center.Z + box.Bounds.Z), color);
      this.vertices[5] = new VertexPositionColor(new Vector3(box.Center.X - box.Bounds.X, box.Center.Y - box.Bounds.Y, box.Center.Z + box.Bounds.Z), color);
      this.vertices[6] = new VertexPositionColor(box.Center - box.Bounds, color);
      this.vertices[7] = new VertexPositionColor(new Vector3(box.Center.X + box.Bounds.X, box.Center.Y - box.Bounds.Y, box.Center.Z - box.Bounds.Z), color);

      this.DrawVertices();
    }

    private void DrawVertices()
    {
      this.effect.Begin();
      this.effect.VertexColorEnabled = true;
      this.effect.World = Matrix.Identity;
      this.effect.View = GameHandler.Camera.View;
      this.effect.Projection = GameHandler.Camera.Projection;
      TierGame.Device.VertexDeclaration = decl;

      foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
      {
        pass.Begin();
          TierGame.Device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length, indices, 0, 12);
        pass.End();
      }

      this.effect.End();
		}
	}
}
