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
  class HomingRocketTurret : Weapon
  {
    #region Properties
    private Matrix[] transforms;
    #endregion

    public HomingRocketTurret(Game game, AttachableObject source)
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
    public override void Fire()
    {
      if (base.canFire())
      {
        HomingRocket r = new HomingRocket(this.Game, this.Position);
        
        Vector3 cannonPosition = Vector3.Transform(new Vector3(0, 0f, 1.7f), this.Position.Front);
        r.Position.Coordinate = Vector3.Add(r.Position.Coordinate, cannonPosition);
        r.Movement.Velocity = -r.Movement.Velocity;

        GameHandler.ObjectHandler.AddObject(r);

        base.Fire();
      }
    }

    public override void Initialize()
    {
      base.Initialize();

      if (this.Model != null)
      {
        this.transforms = new Matrix[this.Model.Bones.Count];
        this.Model.CopyAbsoluteBoneTransformsTo(this.transforms);

        this.Effect = new BasicEffect(this.GraphicsDevice, null);
        this.Effect.Projection = GameHandler.Camera.Projection;
        this.Effect.EnableDefaultLighting();
        this.Effect.PreferPerPixelLighting = true;        
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.Position = new Position(this.Source.Position);

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

          this.Effect.View = GameHandler.Camera.View;
          this.Effect.Projection = GameHandler.Camera.Projection;
          this.Effect.World = this.transforms[mesh.ParentBone.Index] *
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
