using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Objects
{
  public class EnergyShield : GameObject
  {
    public EnergyShield(TierGame game)
      : base(game)
    {
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      this.Effect.Parameters["Texture"].SetValue(
        this.Game.ContentHandler.GetAsset<Texture2D>("EnergyShield"));
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Draw(gameTime);
      this.GraphicsDevice.RenderState.AlphaBlendEnable = false;
    }
  }
}
