using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Turrets
{
  public class PlasmaTurret : Turret
  {
    public PlasmaTurret(TierGame game)
      : base(game)
    { }

    public override void Shoot(Microsoft.Xna.Framework.Vector3 position)
    {
      this.state = TurretState.Idle;

      if (timeElapsed >= this.cooldown)
      {
        timeElapsed = 0;
        this.Game.SoundHandler.Play("BossFire");

        Vector3 pos = Vector3.Transform(this.Position +
                              Vector3.Transform(this.Origin, this.Rotation),
                              this.Game.BehaviourHandler.Transform);

        Projectile pro = new PlasmaProjectile(this.Game);
        Vector3 velo = position - pos;
        velo.Normalize();

        this.Game.ProjectileHandler.CreateParticle(
          pos,
          velo,
          ProjectileHandlerType.PHT_BOSS_PLASMA);
      }   
    }
  }
}
