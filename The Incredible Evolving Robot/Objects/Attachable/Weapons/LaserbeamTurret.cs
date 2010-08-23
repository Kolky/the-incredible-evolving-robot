using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Handlers;
using Tier.Objects.Destroyable.Projectile;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects.Attachable.Weapons
{
  class LaserbeamTurret : Weapon
  {
    private Quaternion cannonRotation;

    public LaserbeamTurret(Game game, AttachableObject source)
      : base(game, source)
    {
      this.ModelName = "LaserbeamTurret";
      this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
      this.CoolDownFire = 7000;      
      this.Scale = 0.5f;

      this.Initialize();

      this.cannonRotation = Quaternion.Concatenate(
        Quaternion.CreateFromAxisAngle(Vector3.Right, 0),
        Quaternion.CreateFromAxisAngle(Vector3.Forward, 0));
    }

    #region Weapon overrides
    public override void Fire()
    {
      if (base.canFire())
      {
        Position fixPos = new Position(this.Position);
        fixPos.Front = fixPos.Front * this.cannonRotation;

        Vector3 upVector = Vector3.Transform(Vector3.Up, fixPos.Front);
        fixPos.Front = Quaternion.Concatenate(fixPos.Front, Quaternion.CreateFromAxisAngle(upVector, MathHelper.Pi));

        GameHandler.ObjectHandler.AddObject(new Laserbeam(this.Game, fixPos));

        base.Fire();
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.Fire();

      base.Update(gameTime);
    }
    #endregion
  }
}
