using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Turrets
{
  public class ShotgunTurret : Turret
  {
    private float spread;

    public ShotgunTurret(TierGame game)
      : base(game)
    {
      spread = MathHelper.PiOver4 / 20;
    }

    public override void Shoot(Vector3 position)
    {
      if (timeElapsed >= this.cooldown)
      {
        this.Game.SoundHandler.Play("Shotgun");
        timeElapsed = 0;

        float x = -spread;
        float y = -spread;
        float xStep = spread;
        float yStep = spread;

        for (int i = 0; i < 9; i++)
        {
          Vector3 pos = DetermineProjectileSpawnPosition();
          Vector3 dif = position - pos;
          dif.Normalize();
          Quaternion rot =
            Quaternion.CreateFromAxisAngle(Vector3.Up, x) *
            Quaternion.CreateFromAxisAngle(Vector3.Right, y);

          // Spread the shotgun shells out a little
          dif = Vector3.Transform(dif, rot);

          this.Game.ProjectileHandler.CreateParticle(
            pos, dif,
            ProjectileHandlerType.PHY_BOSS_SHOTGUN);

          if (x < spread)
          {
            x += xStep;
          }
          else
          {
            x = -spread;
            // Every 4 projectile should have a different rotation across X axis
            y += yStep;
          }
        }
      }
    }

  }
}
