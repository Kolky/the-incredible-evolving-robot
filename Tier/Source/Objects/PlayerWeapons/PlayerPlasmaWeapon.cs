using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Handlers;
using Tier.Source.Handlers.BossBehaviours;
using Tier.Source.Helpers;
using Tier.Source.Objects.Projectiles;

namespace Tier.Source.Objects.PlayerWeapons
{
  public class PlayerPlasmaWeapon : PlayerWeapon
  {
    private float shootSpread;
    private float damage;

    public PlayerPlasmaWeapon(TierGame game)
      : base(game, game.Options.PlayerWeapon_Plasma_Cooldown_Level1)
    {
      shootSpread = game.Options.PlayerWeapon_Plasma_Spread_Level1;
      damage = game.Options.PlayerWeapon_Plasma_BaseDamage;
    }

    public override void AttachWeapon()
    {
    }

    protected override void DoShoot(Vector3 direction)
    {
      this.game.SoundHandler.Play("PlayerGunfire");

      Random r = this.game.Random;

      double x = -shootSpread + (r.NextDouble() * (shootSpread * 2));
      double y = -shootSpread + (r.NextDouble() * (shootSpread * 2));

      direction = Vector3.Transform(direction,
        Quaternion.CreateFromAxisAngle(Vector3.Up, (float)x) *
        Quaternion.CreateFromAxisAngle(Vector3.Right, (float)y));
      
      Projectile p = this.game.ProjectileHandler.GetProjectileBlueprint(ProjectileHandlerType.PHT_PLAYER_PLASMA);
      if (p != null)
      {
        CheckForCollision(this.game.GameHandler.Player.Position, direction, p.Damage);

        this.game.ProjectileHandler.CreateParticle(
          this.game.GameHandler.Player.Position,
          direction,
          ProjectileHandlerType.PHT_PLAYER_PLASMA);

      }
      
    }

    /// <summary>
    /// Fires a Ray in the direction the projectile is moving. The ray is used to check which object is hit.
    /// </summary>
    private void CheckForCollision(Vector3 position, Vector3 direction, int damage)
    {
      Ray r = new Ray(position, direction);
      GameObject obj = null;

      BossBehaviour behaviour = null;

      if (this.game.BehaviourHandler.GetBehaviour(BossBehaviourType.BBT_ENERGYSHIELD, out behaviour) &&
        ((EnergyShieldBehaviour)behaviour).IsBlockProjectile(direction))
      {
        // Projectile hit energyshield, show particle
      }
      else
      {
        if (PublicMethods.IsRayIntersection(this.game, ref r, out obj))
        {
          this.game.GameHandler.AddDamagedObject(obj, damage);
          //obj.DestroyableModifier.IsHit(damage);
        }
      }
    }

    public override void RemoveWeapon()
    {      
    }

    public override void Reset()
    {
      base.Reset();

      damage = game.Options.PlayerWeapon_Plasma_BaseDamage;
      shootSpread = game.Options.PlayerWeapon_Plasma_Spread_Level1;
      this.cooldown = game.Options.PlayerWeapon_Plasma_Cooldown_Level1;
    }

    public override void Upgrade()
    {
      base.Upgrade();

      damage =
        game.Options.PlayerWeapon_Plasma_BaseDamage +
        this.timesUpgraded * game.Options.PlayerWeapon_Plasma_DamageUpgrade;

      if (this.weaponLevel == PlayerWeaponLevel.PWL_TWO)
      {
        shootSpread = game.Options.PlayerWeapon_Plasma_Spread_Level2;
        this.cooldown = game.Options.PlayerWeapon_Plasma_Cooldown_Level2;
      }
      else if (this.weaponLevel == PlayerWeaponLevel.PWL_THREE)
      {
        shootSpread = game.Options.PlayerWeapon_Plasma_Spread_Level3;
        this.cooldown = game.Options.PlayerWeapon_Plasma_Cooldown_Level3;
      }
      else if (this.weaponLevel == PlayerWeaponLevel.PWL_FOUR)
      {
        shootSpread = game.Options.PlayerWeapon_Plasma_Spread_Level4;
        this.cooldown = game.Options.PlayerWeapon_Plasma_Cooldown_Level4;
      }
    }
  }
}
