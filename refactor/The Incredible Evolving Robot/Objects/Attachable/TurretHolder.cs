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
            get { return weapon.Position; }
            set
            {
                if (Weapon != null)
                    Weapon.Position = value;
            }
        }
        private Weapon weapon;
        public Weapon Weapon
        {
            get { return weapon; }
            set
            {
                if (Weapon != null)
                    GameHandler.ObjectHandler.RemoveObject(Weapon);
                weapon = value;
            }
        }
        #endregion

        public TurretHolder(Game game)
            : base(game, true, null)
        {
            Weapon = new DoubleBulletTurret(Game, this);
            ModelName = Weapon.ModelName;
            AddConnection(Vector3.Zero, Vector3.Forward);

            ModelMeta = (IsCollidable) ? TierGame.ContentHandler.GetModelMeta(ModelName) : null;

            addBoundingBox(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0.5f));

            MaxHealth = 500;

            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            Weapon.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Exploded)
                return;

            UpdateBoundingObjects();
            base.Update(gameTime);
            Weapon.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Exploded)
                return;

            Weapon.Draw(gameTime);
        }

        public override void UpdateVelocity(AttachableObject parent, Vector3 velocity)
        {
            base.UpdateVelocity(parent, velocity);

            Weapon.SpawnMatrix *= Matrix.CreateTranslation(velocity);
        }

        public override void SetPosition(AttachableObject parent, Matrix translation)
        {
            base.SetPosition(parent, translation);
            Weapon.SpawnMatrix = translation;
        }

        public override int UpdateHealth(AttachableObject parent)
        {
            int totalHealth = base.UpdateHealth(parent);
            Weapon.Health = MaxHealth;
            Weapon.Exploded = false;
            return totalHealth + Weapon.Health;
        }

        public override void UpdateBoundingObjects()
        {
            base.UpdateBoundingObjects();
            Weapon.UpdateBoundingObjects();
        }
    }
}