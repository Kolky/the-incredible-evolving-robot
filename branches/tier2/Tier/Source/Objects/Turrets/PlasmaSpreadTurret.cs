using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Turrets
{
  public class PlasmaSpreadTurret : Turret
  {
    Random r;

    public PlasmaSpreadTurret(TierGame game)
      : base(game)
    {
      r = new Random();
    }

    public override void Shoot(Vector3 position)
    {
      /*this.state = TurretState.Idle;

      if (timeElapsed >= this.cooldown)
      {
        timeElapsed = 0;
        this.Game.SoundHandler.Play("BossFire");

        double spread = 0.015;

        // Modify the direction of fire based on the spread
        double x = -spread +
          (r.NextDouble() * (spread * 2));
        double y = -spread +
          (r.NextDouble() * (spread * 2));

        diff = Vector3.Transform(diff,
          Quaternion.CreateFromAxisAngle(Vector3.Up, (float)x) *
          Quaternion.CreateFromAxisAngle(Vector3.Right, (float)y));

        Projectile pro = new PlasmaProjectile(this.Game);

        if (this.Game.ObjectHandler.InitializeFromBlueprint<Projectile>(pro, ProjectileType))
        {
          pro.Position =
            Vector3.Transform(this.Position +
                              Vector3.Transform(this.Origin, this.Rotation),
                              this.Game.BehaviourHandler.Transform);

          pro.MovableModifier.Velocity = diff * pro.Speed;

          this.Game.GameHandler.AddObject(pro);

          this.state = TurretState.Shooting;

          // Add lightsource
          this.Game.DeferredRendererHandler.AddLightSource(LightSourceType.Point,
                                                           this.Position + this.Origin +
                                                           this.Game.BehaviourHandler.Transform.Translation,
                                                           this.projectileColor, 10);
        }
      }   */
    }
  }
}
