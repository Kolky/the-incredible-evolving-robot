using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Misc;

namespace Tier.Test
{
#if DEBUG
  public class Axis
  {
    #region Properties
    private static BasicEffect basicEffect;
    private static VertexPositionColor[] vertexAxis;
    private static VertexDeclaration vertexDeclaration;
    #endregion

    public static void Init()
    {      
      Axis.basicEffect = new BasicEffect(TierGame.Device, null);
      Axis.basicEffect.VertexColorEnabled = true;

      Axis.vertexDeclaration = new VertexDeclaration(TierGame.Device, VertexPositionColor.VertexElements);

      Axis.vertexAxis = new VertexPositionColor[6];
      Axis.vertexAxis[0] = new VertexPositionColor(Vector3.UnitX * 1000f, Options.Colors.Axis.XColor);
      Axis.vertexAxis[1] = new VertexPositionColor(-Vector3.UnitX * 1000f, Options.Colors.Axis.XColor);
      Axis.vertexAxis[2] = new VertexPositionColor(Vector3.UnitY * 1000f, Options.Colors.Axis.YColor);
      Axis.vertexAxis[3] = new VertexPositionColor(-Vector3.UnitY * 1000f, Options.Colors.Axis.YColor);
      Axis.vertexAxis[4] = new VertexPositionColor(Vector3.UnitZ * 1000f, Options.Colors.Axis.ZColor);
      Axis.vertexAxis[5] = new VertexPositionColor(-Vector3.UnitZ * 1000f, Options.Colors.Axis.ZColor);            
    }

    public static void Draw()
    {
      TierGame.Device.VertexDeclaration = Axis.vertexDeclaration;

      Axis.basicEffect.View = GameHandler.Camera.View;
      Axis.basicEffect.Projection = GameHandler.Camera.Projection;

      Axis.basicEffect.Begin();
      foreach (EffectPass pass in Axis.basicEffect.CurrentTechnique.Passes)
      {
        pass.Begin();
        TierGame.Device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, Axis.vertexAxis, 0, 3);
        pass.End();
      }
      Axis.basicEffect.End();
    }
  }
#endif
}
