using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Objects.Turrets
{
  public class LaserBeamTurret : Turret
  {
    private LaserbeamProjectile projectile;

    public LaserBeamTurret(TierGame game)
      : base(game)
    {      
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      obj.Initialize();
    }

    public override void Initialize()
    {
      base.Initialize();
      projectile = new LaserbeamProjectile(this.Game);
      projectile.IsVisible = false;
      this.Game.ObjectHandler.InitializeFromBlueprint<Projectile>(projectile, "LaserBeamProjectile");
      projectile.LaserbeamTexture = this.Game.ContentHandler.GetAsset<Texture2D>("Laserbeam-boss");
      this.Game.GameHandler.AddObject(projectile);      
    }

    public override void Update(GameTime gameTime)
    {
      projectile.IsVisible = false;

      base.Update(gameTime);
    }

    public override void Shoot(Vector3 position)
    {      
      projectile.IsVisible = true;
      projectile.Color = Color.Red;
      Vector3 dir = position - this.Position;
      dir.Normalize();

      projectile.Target = position;
      projectile.Position = 
        Vector3.Transform(this.Position +
                          Vector3.Transform(this.Origin, this.Rotation),
                            this.Game.BehaviourHandler.Transform);
              
      State = TurretState.Shooting;
    }
  }
}
