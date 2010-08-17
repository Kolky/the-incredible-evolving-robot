using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects.PlayerWeapons;

namespace Tier.Source.Objects.Powerups
{
  public class PowerupRocket : Powerup
  {
    public PowerupRocket(TierGame game)
      : base(game)
    { }

    public override void DoPowerup()
    {
      this.Game.SoundHandler.Play("VoiceRocketlauncher");
      //this.Game.GameHandler.Player.SwitchWeapon(new PlayerWeaponRocketLauncher(this.Game));
    }
  }
}
