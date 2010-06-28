using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Objects.Destroyable.Projectile;
using Tier.Misc;

namespace Tier.Objects.Attachable.Weapons
{
  class RocketTurret : Weapon
  {
    #region Properties
    private Matrix[] transforms;
    #endregion

    public RocketTurret(Game game, AttachableObject source)
      : base(game, source, false)
    {
			this.ModelName = "RocketTurret";
      this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
			this.ModelMeta = (IsCollidable) ? TierGame.ContentHandler.GetModelMeta(ModelName) : null;
      this.CoolDownFire = 2000;
      this.Scale = 1.0f;

      this.Initialize();
    }

    #region Weapon overrides
    public override void Initialize()
    {
      base.Initialize();

      this.transforms = new Matrix[this.Model.Bones.Count];
      this.Model.CopyAbsoluteBoneTransformsTo(this.transforms);

      this.Effect = new BasicEffect(this.GraphicsDevice, null);
      this.Effect.EnableDefaultLighting();
      this.Effect.PreferPerPixelLighting = true;
    }

    public override void Fire()
    {
      if (base.canFire())
      {
        // Position of cannon with no rotation: {0, 0, 1.7f} Shoot three rockets at y = -0.25f, 0 and 0.25f
        for (int i = -1; i <= 1; i++)
        {
          Rocket r = new Rocket(this.Game, this.Position);
          r.Movement.Velocity = -r.Movement.Velocity;
          Vector3 cannonPosition = Vector3.Transform(new Vector3(0, i * 0.25f, 1.7f), this.Position.Front);
          r.Position.Coordinate = Vector3.Add(r.Position.Coordinate, cannonPosition);

          GameHandler.ObjectHandler.AddObject(r);         
        }

        base.Fire();
      }
    }   

    public override void Update(GameTime gameTime)
    {
      this.Position = new Position(Source.Position);

      this.Fire();
      
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Effect != null)
      {
        foreach (ModelMesh mesh in this.Model.Meshes)
        {
          foreach (ModelMeshPart part in mesh.MeshParts)
          {
            part.Effect = this.Effect;
          }

          this.Effect.Projection = GameHandler.Camera.Projection;
          this.Effect.View = GameHandler.Camera.View;
          this.Effect.World = transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.Position.Front) *
            this.spawnMatrix *
            Matrix.CreateTranslation(this.Position.Coordinate);

          mesh.Draw();
        }
      }
    } 
    #endregion
  }
}
