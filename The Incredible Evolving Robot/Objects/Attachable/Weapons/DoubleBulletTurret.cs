using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Destroyable.Projectile;

namespace Tier.Objects.Attachable.Weapons
{
  public class DoubleBulletTurret : Weapon
  {
    #region Properties
    private Quaternion cannonRotation;
    private int updateCoolDown = 0;
    #endregion

    public DoubleBulletTurret(Game game, AttachableObject source)
      : base(game, source)
    {
      this.ModelName = "DoubleBulletTurret";
      this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
      this.CoolDownFire = 150;
      this.Scale = 0.5f ;

      this.Initialize();

      this.cannonRotation = Quaternion.Concatenate(
        Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver4),
        Quaternion.CreateFromAxisAngle(Vector3.Forward,MathHelper.PiOver2));
    }   

    public override void Fire()
    {
      if (base.canFire())
      {
        Position fixPos = new Position(this.Position);

        Vector3 upVector = Vector3.Transform(Vector3.Up, this.cannonRotation);
        fixPos.Front = Quaternion.Concatenate(this.cannonRotation, Quaternion.CreateFromAxisAngle(upVector, MathHelper.Pi));

        GameHandler.ObjectHandler.AddObject(new DoubleBullet(this.Game, fixPos));
        
        base.Fire();
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.Fire();

      if (this.updateCoolDown >= Options.DoubleBullet.UpdateCheck)
      {
        Vector3 newTarget = Vector3.Subtract(-GameHandler.Player.Position.Coordinate, this.Position.Coordinate);
        Vector3 oldTarget = Vector3.Forward;

        newTarget.Normalize();

        float theta = (float)Math.Acos(Vector3.Dot(newTarget, oldTarget));

        Vector3 cross = Vector3.Cross(oldTarget, newTarget);
        cross.Normalize();

        if (cross.X.Equals(float.NaN))
          cross.X = 0f;
        if (cross.Y.Equals(float.NaN))
          cross.Y = 0f;
        if (cross.Z.Equals(float.NaN))
          cross.Z = 0f;
        if (theta.Equals(float.NaN))
          theta = 0f;

        Quaternion rotation = Quaternion.CreateFromAxisAngle(cross, theta);
        this.cannonRotation = Quaternion.Slerp(this.cannonRotation, rotation, 0.1f);

        this.updateCoolDown = 0;
      }
      this.updateCoolDown++;

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Model != null)
      {
        foreach (ModelMesh mesh in this.Model.Meshes)
        {
          foreach (BasicEffect effect in mesh.Effects)
          {
            effect.View = GameHandler.Camera.View;
            effect.Projection = GameHandler.Camera.Projection;

            if (mesh.Name.Equals("mesh_cannon"))
            {
                effect.World = Matrix.CreateScale(this.Scale) *
                  //Matrix.CreateFromQuaternion(this.Position.Front) *
                  Matrix.CreateFromQuaternion(this.cannonRotation) *
                  this.spawnMatrix *
                  Matrix.CreateTranslation(this.Position.Coordinate);
            }
            else
            {
                effect.World = Matrix.CreateScale(this.Scale) *
                  Matrix.CreateFromQuaternion(this.Position.Front) *
                  this.spawnMatrix *
                  Matrix.CreateTranslation(this.Position.Coordinate);
            }            
          }
          mesh.Draw();
        }
      }
    }
  }
}
