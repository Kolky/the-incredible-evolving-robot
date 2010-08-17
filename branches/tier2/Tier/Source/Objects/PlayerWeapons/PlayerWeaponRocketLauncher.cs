using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects.Projectiles;

namespace Tier.Source.Objects.PlayerWeapons
{
  public class PlayerWeaponRocketLauncher :  PlayerWeapon
  {
    public PlayerWeaponRocketLauncher(TierGame game)
      : base(game, game.Options.PlayerWeapon_Rocket_Cooldown)
    { }

    protected override void DoShoot(Vector3 direction)
    {
      RocketProjectile pro = new RocketProjectile(this.game);

      if (this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(pro, "Rocket"))
      {
        Vector3 myForward = Vector3.Forward; // Vector3.Transform(Vector3.Forward, this.game.GameHandler.Player.Rotation);
        float angle = (float)Math.Acos(Vector3.Dot(myForward, direction));
        Vector3 cross = Vector3.Cross(myForward, direction);
        cross.Normalize();

        pro.Rotation = Quaternion.CreateFromAxisAngle(cross, angle);
        pro.Direction = direction;
        pro.Position = this.game.GameHandler.Player.Position;        
        pro.MovableModifier.Velocity = direction * pro.Speed;
        pro.Color = Microsoft.Xna.Framework.Graphics.Color.White;
        pro.AoeDamage =
          this.game.Options.PlayerWeapon_Rocket_AoE_BaseDamage +
          this.game.Options.PlayerWeapon_Rocket_AoE_DamageUpgrade * this.timeElapsed;
        pro.Damage =
          this.game.Options.PlayerWeapon_Rocket_BaseDamage +
          this.game.Options.PlayerWeapon_Rocket_DamageUpgrade * this.timesUpgraded;
        pro.AoeSize =
          this.game.Options.PlayerWeapon_Rocket_AoE_Distance +
          this.game.Options.PlayerWeapon_Rocket_AoE_DistanceUpgrade * this.timesUpgraded;

        this.game.GameHandler.AddObject(pro);
      }
    }

    public override void AttachWeapon()
    {
    }

    public override void RemoveWeapon()
    {
    }
  }
}
