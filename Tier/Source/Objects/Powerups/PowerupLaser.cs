using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects.PlayerWeapons;

namespace Tier.Source.Objects.Powerups
{
  public class PowerupLaser : Powerup
  {
    public PowerupLaser(TierGame game)
      : base(game)
    { }

    public override void DoPowerup()
    {
      this.Game.SoundHandler.Play("VoiceLaserbeam");
      this.Game.GameHandler.Player.SwitchWeapon(new PlayerWeaponLaserbeam(Game));
    }
  }
}
