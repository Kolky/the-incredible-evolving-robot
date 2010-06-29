using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;

namespace Tier.Misc
{
#if DEBUG && BOUNDRENDER
	// http://sharky.bluecog.co.nz/?page_id=113
	public class BoundingSphereRenderer
  {
    #region Properties
    public static float RADIANS_FOR_90DEGREES = MathHelper.ToRadians(90);//(float)(Math.PI / 2.0);
		public static float RADIANS_FOR_180DEGREES = RADIANS_FOR_90DEGREES * 2;

		protected VertexBuffer buffer;
		protected VertexDeclaration vertexDecl;

		private BasicEffect basicEffect;

		private const int CIRCLE_NUM_POINTS = 32;
		private IndexBuffer indexBuffer;
		private VertexPositionNormalTexture[] vertices;
    #endregion

    public void OnCreateDevice()
		{
      this.basicEffect = new BasicEffect(TierGame.Device, null);
			this.CreateShape();
		}

		public void CreateShape()
		{
			this.vertexDecl = new VertexDeclaration(TierGame.Device, VertexPositionNormalTexture.VertexElements);

			double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;
			this.vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS + 1];
      this.vertices[0] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.One);

			for (int i = 1; i <= CIRCLE_NUM_POINTS; i++)
			{
				float x = (float)Math.Round(Math.Sin(angle * i), 4);
				float y = (float)Math.Round(Math.Cos(angle * i), 4);
				Vector3 point = new Vector3(x, y,	0.0f);

        this.vertices[i] = new VertexPositionNormalTexture(point, Vector3.Forward, new Vector2());
			}

			// Initialize the vertex buffer, allocating memory for each vertex
      this.buffer = new VertexBuffer(TierGame.Device, VertexPositionNormalTexture.SizeInBytes * (this.vertices.Length), BufferUsage.Points);

			// Set the vertex buffer data to the array of vertices
      this.buffer.SetData<VertexPositionNormalTexture>(this.vertices);

      this.InitializeLineStrip();
		}

		private void InitializeLineStrip()
		{
			// Initialize an array of indices of type short
			short[] lineStripIndices = new short[CIRCLE_NUM_POINTS + 1];

			// Populate the array with references to indices in the vertex buffer
			for (int i = 0; i < CIRCLE_NUM_POINTS; i++)
			{
				lineStripIndices[i] = (short)(i + 1);
			}

			lineStripIndices[CIRCLE_NUM_POINTS] = 1;

			// Initialize the index buffer, allocating memory for each index
      this.indexBuffer = new IndexBuffer(TierGame.Device, sizeof(short) * lineStripIndices.Length, BufferUsage.Points, IndexElementSize.SixteenBits);

			// Set the data in the index buffer to our array
      this.indexBuffer.SetData<short>(lineStripIndices);
		}

		public void Draw(BoundingSphere bs, Color color)
		{
			if (bs != null)
			{
				Matrix scaleMatrix = Matrix.CreateScale(bs.Radius);
				Matrix translateMat = Matrix.CreateTranslation(bs.Center);
				Matrix rotateYMatrix = Matrix.CreateRotationY(RADIANS_FOR_90DEGREES);
				Matrix rotateXMatrix = Matrix.CreateRotationX(RADIANS_FOR_90DEGREES);

				TierGame.Device.RenderState.DepthBufferEnable = true;
        TierGame.Device.RenderState.DepthBufferWriteEnable = true;
        TierGame.Device.RenderState.AlphaBlendEnable = false;
        TierGame.Device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

				// effect is a compiled effect created and compiled elsewhere
				// in the application
        this.basicEffect.EnableDefaultLighting();
        this.basicEffect.View = GameHandler.Camera.View;
        this.basicEffect.Projection = GameHandler.Camera.Projection;

        this.basicEffect.Begin();
        foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
        {
          pass.Begin();

          TierGame.Device.VertexDeclaration = this.vertexDecl;

          TierGame.Device.Vertices[0].SetSource(this.buffer, 0, VertexPositionNormalTexture.SizeInBytes);
          TierGame.Device.Indices = this.indexBuffer;

          this.basicEffect.World = scaleMatrix * translateMat;
          this.basicEffect.DiffuseColor = color.ToVector3();
          this.basicEffect.CommitChanges();

          TierGame.Device.DrawIndexedPrimitives(PrimitiveType.LineStrip, 0, 0, CIRCLE_NUM_POINTS + 1, 0, CIRCLE_NUM_POINTS);

          this.basicEffect.World = rotateYMatrix * scaleMatrix * translateMat;
          this.basicEffect.DiffuseColor = color.ToVector3() * 0.5f;
          this.basicEffect.CommitChanges();

          TierGame.Device.DrawIndexedPrimitives(PrimitiveType.LineStrip, 0, 0, CIRCLE_NUM_POINTS + 1, 0, CIRCLE_NUM_POINTS);

          this.basicEffect.World = rotateXMatrix * scaleMatrix * translateMat;
          this.basicEffect.DiffuseColor = color.ToVector3() * 0.5f;
          this.basicEffect.CommitChanges();

          TierGame.Device.DrawIndexedPrimitives(PrimitiveType.LineStrip, 0, 0, CIRCLE_NUM_POINTS + 1, 0, CIRCLE_NUM_POINTS);

          pass.End();
        }
        this.basicEffect.End();
			}
		}
	}
#endif
}
