using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Helpers
{
  public class Axis : DrawableGameComponent
  {
    #region Properties
    private BasicEffect basicEffect;
    private VertexPositionColor[] vertexAxis;
    private VertexDeclaration vertexDeclaration;
    private TierGame game;
    #endregion

    public Axis(TierGame game) : base(game)
    {
      this.game = game;
      base.Initialize();
      Init();
    }

    public void Init()
    {      
      this.basicEffect = new BasicEffect(this.GraphicsDevice, null);
      this.basicEffect.VertexColorEnabled = true;

      this.vertexDeclaration = new VertexDeclaration(this.GraphicsDevice, VertexPositionColor.VertexElements);

      this.vertexAxis = new VertexPositionColor[6];
      this.vertexAxis[0] = new VertexPositionColor(Vector3.UnitX * 1000f, Color.Red);
      this.vertexAxis[1] = new VertexPositionColor(-Vector3.UnitX * 1000f, Color.Red);
      this.vertexAxis[2] = new VertexPositionColor(Vector3.UnitY * 1000f, Color.Green);
      this.vertexAxis[3] = new VertexPositionColor(-Vector3.UnitY * 1000f, Color.Green);
      this.vertexAxis[4] = new VertexPositionColor(Vector3.UnitZ * 1000f, Color.Blue);
      this.vertexAxis[5] = new VertexPositionColor(-Vector3.UnitZ * 1000f, Color.Blue);            
    }

    public void Draw()
    {
      this.GraphicsDevice.VertexDeclaration = this.vertexDeclaration;

      this.basicEffect.View = this.game.GameHandler.View;
      this.basicEffect.Projection = this.game.GameHandler.Projection;

      this.basicEffect.Begin();
      foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
      {
        pass.Begin();
        this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, this.vertexAxis, 0, 3);
        pass.End();
      }
      this.basicEffect.End();
    }
  }
}
