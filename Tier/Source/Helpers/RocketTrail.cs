using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Helpers
{
  public class RocketTrail : GameObject
  {
    private List<VertexPositionColor> trailVertices;

    public RocketTrail(TierGame game) : base(game)
    {
      this.Type = Tier.Source.Handlers.GameHandler.ObjectType.AlphaBlend;
      this.trailVertices = new List<VertexPositionColor>();
      this.Effect = game.ContentHandler.GetAsset<Effect>("Default").Clone(game.GraphicsDevice);
    }

    public void AddPoint(Vector3 position)
    {
      trailVertices.Add(new VertexPositionColor(position, 
        Microsoft.Xna.Framework.Graphics.Color.White));
    }

    public override void Update(GameTime gameTime)
    {
      this.Effect.Parameters["matWorld"].SetValue(Matrix.Identity);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
    }

    public override void Draw(GameTime gameTime)
    {      
      if (trailVertices.Count > 2)
      {
        this.Effect.Begin();
        this.Effect.Parameters["color"].SetValue(Vector3.One);
        this.Effect.Techniques["Colored"].Passes[0].Begin();

        this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
          trailVertices.ToArray(), 0, trailVertices.Count / 2);

        this.Effect.Techniques["Colored"].Passes[0].End();
        this.Effect.End();
      }
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
