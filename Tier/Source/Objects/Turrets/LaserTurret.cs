using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Turrets
{
  public class LaserTurret : Turret
  {
    public LaserTurret(TierGame game)
      : base(game)
    {
      this.cooldown = 100;
    }

    public override void Shoot(Microsoft.Xna.Framework.Vector3 position)
    {
      if (timeElapsed >= this.cooldown)
      {
        timeElapsed = 0;
        
        Vector3 pos = DetermineProjectileSpawnPosition();
        Vector3 dir = position - pos;
        dir.Normalize();

        this.Game.ProjectileHandler.CreateParticle(
          pos, dir, 
          ProjectileHandlerType.PHT_BOSS_LASER);
      }
    }
  }
}
