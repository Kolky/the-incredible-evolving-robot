using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tier.Source.Objects.Powerups
{
  public class PowerupHealth : Powerup
  {
    public PowerupHealth(TierGame game)
      : base(game)
    { }

    public override void DoPowerup()
    {
      this.Game.SoundHandler.Play("VoiceHealthupgrade");
      this.Game.GameHandler.Player.DestroyableModifier.Health +=
        this.Game.Options.PowerupHealth_Lifegain;

      if (this.Game.GameHandler.Player.DestroyableModifier.Health > 100)
      {
        this.Game.GameHandler.Player.DestroyableModifier.Health = 100;
      }
    }
  }
}
