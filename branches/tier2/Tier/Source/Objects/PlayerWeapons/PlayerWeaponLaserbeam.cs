using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;

namespace Tier.Source.Objects.PlayerWeapons
{
  public class PlayerWeaponLaserbeam : PlayerWeapon
  {
    private LaserbeamProjectile laserbeam;
    private float damage;
 
    public PlayerWeaponLaserbeam(TierGame game)
      : base(game, 0)
    {
      laserbeam = new LaserbeamProjectile(game);
      damage = 100;

      if (game.ObjectHandler.InitializeFromBlueprint<LaserbeamProjectile>(laserbeam, "LaserBeamProjectile"))
      {
        laserbeam.Initialize();
        laserbeam.LaserbeamTexture = this.game.ContentHandler.GetAsset<Texture2D>("Laserbeam-player");
        laserbeam.Color = Color.Green;
      }
    }

    public override void AttachWeapon()
    {
      this.game.GameHandler.AddObject(laserbeam);
    }

    protected override void DoShoot(Vector3 direction)
    {
      laserbeam.IsVisible = true;
      laserbeam.Position = this.game.GameHandler.Player.Position;
     // laserbeam.Target = laserbeam.Position + (laserbeam.Direction * 40);
      laserbeam.Direction = direction;

      if (this.game.GameHandler.Player.IsQuadDamage)
      {
        laserbeam.Color = Color.Blue;
      }
      else
      {
        laserbeam.Color = Color.Green;
      }

      // Determine size of laserbeam by shooting a ray into gamespace
      Ray r = new Ray(this.game.GameHandler.Player.Position, direction);
      GameObject obj = null;
      float distance = float.MaxValue;
      if(PublicMethods.IsRayIntersection(this.game, ref r, out obj, out distance))
      {
        // Player hit an object, change the target of the laserbeam
        laserbeam.Target = laserbeam.Position + (laserbeam.Direction * distance);
        // And deal damage :)
        obj.DestroyableModifier.IsHit(this.damage);
      }
    }

    public override void RemoveWeapon()
    {
      if (laserbeam != null)
      {
        this.game.GameHandler.RemoveObject(laserbeam, Tier.Source.Handlers.GameHandler.ObjectType.AlphaBlend);        
      }
    }

    public void ResetBeam()
    {
      this.laserbeam.IsVisible = false;
    }
  }
}
