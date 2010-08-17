using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tier.Source.Objects.Powerups
{
  public class PowerupWeaponUpgrade : Powerup
  {
    public PowerupWeaponUpgrade(TierGame game)
      : base(game)
    { }

    public override void DoPowerup()
    {
      this.Game.SoundHandler.Play("VoiceWeaponupgrade");
      this.Game.GameHandler.Player.UpgradeWeapon();  
    }
  }
}