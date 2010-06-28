using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Objects.Attachable.Weapons;
using Tier.Handlers;
using Tier.Misc;

namespace Tier.Objects.Attachable
{
  class TurretHolder : AttachableObject
  {
    #region Properties
    public override Position Position
    {
      get { return this.weapon.Position; }
      set
      {
        if (this.Weapon != null)
          this.Weapon.Position = value;
      }
    }
    private Weapon weapon;
    public Weapon Weapon
    {
      get { return weapon; }
      set
      {
        if (this.Weapon != null)
          GameHandler.ObjectHandler.RemoveObject(this.Weapon);
        weapon = value;
      }
    }
    #endregion

    public TurretHolder(Game game)
      : base(game, true, null)
    {
      this.Weapon = new DoubleBulletTurret(this.Game, this);
      this.ModelName = this.Weapon.ModelName;
      this.AddConnection(Vector3.Zero, Vector3.Forward);

      this.ModelMeta = (IsCollidable) ? TierGame.ContentHandler.GetModelMeta(ModelName) : null;

      this.addBoundingBox(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0.5f));

      this.MaxHealth = 500;

      this.Initialize();
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Weapon.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.Exploded)
        return;

      this.UpdateBoundingObjects();
      base.Update(gameTime);
      this.Weapon.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Exploded)
        return;

      this.Weapon.Draw(gameTime);
    }

    public override void UpdateVelocity(AttachableObject parent, Vector3 velocity)
    {
      base.UpdateVelocity(parent, velocity);

      this.weapon.SpawnMatrix *= Matrix.CreateTranslation(velocity);
    }

    public override void SetPosition(AttachableObject parent, Matrix translation)
    {
      base.SetPosition(parent, translation);
      this.weapon.SpawnMatrix = translation;
    }

    public override int UpdateHealth(AttachableObject parent)
    {
      int totalHealth = base.UpdateHealth(parent);
      this.weapon.Health = this.MaxHealth;
      this.weapon.Exploded = false;
			return totalHealth + this.weapon.Health;
    }

    public override void UpdateBoundingObjects()
    {
      base.UpdateBoundingObjects();
      this.weapon.UpdateBoundingObjects();
    }
  }
}