using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tier.Source.Objects.Powerups
{
  public class PowerupQuad :  Powerup
  {
    public PowerupQuad(TierGame game)
      : base(game)
    { }

    public override void DoPowerup()
    {
      this.Game.SoundHandler.Play("VoiceQuad");
      this.Game.GameHandler.Player.AddPlayerModifier(
        new PlayerQuadDamageModifier(this.Game, this.Game.GameHandler.Player));
    }
  }
}
