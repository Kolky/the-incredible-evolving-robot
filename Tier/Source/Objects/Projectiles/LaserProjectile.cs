using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tier.Source.Objects.Projectiles
{
  public class LaserProjectile : Projectile
  {    
    protected Texture2D texProjectile;
    protected Texture2D texProjectileFront;
    protected VertexPositionTexture[] verts;
    protected short[] ib;
    protected Vector3 direction;
    protected Vector3 cameraToPlayer;
    protected float angle;
    protected Vector3 cross;
    protected ProjectileBillboardType billboardType;

    protected enum ProjectileBillboardType
    {
      PBT_NORMAL, PBT_CONSTRAINED
    };

    public LaserProjectile(TierGame game)
      : base(game)
    {
      texProjectile       = game.ContentHandler.GetAsset<Texture2D>("laser");
      texProjectileFront  = game.ContentHandler.GetAsset<Texture2D>("laserfront");
      Effect              = game.ContentHandler.GetAsset<Effect>("AlphaBlend").Clone(game.GraphicsDevice);
      this.Type           = Tier.Source.Handlers.GameHandler.ObjectType.AlphaBlend;

      //((LaserbeamProjectile)obj).BlastEffect = this.Effect.Clone(this.Game.GraphicsDevice);
      verts = new VertexPositionTexture[]
              {
                  new VertexPositionTexture(
                      new Vector3(-1,0,0),
                      new Vector2(1,1)),
                  new VertexPositionTexture(
                      new Vector3(-1,1,0),
                      new Vector2(0,1)),
                  new VertexPositionTexture(
                      new Vector3(1,1,0),
                      new Vector2(0,0)),
                  new VertexPositionTexture(
                      new Vector3(1,0,0),
                      new Vector2(1,0))
              };

      ib = new short[] { 0, 1, 2, 2, 3, 0 };

      Initialize();

      this.Position = new Vector3(20, 20, 0);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      direction = this.MovableModifier.Velocity;
      direction.Normalize();

      cameraToPlayer = this.Game.GameHandler.Player.Position - this.Game.GameHandler.Camera.Position;
      cameraToPlayer.Normalize();

      float angleToPlayer = (float)Math.Acos(Vector3.Dot(cameraToPlayer, direction));

      // Rotation to player
      angle = (float)Math.Acos(Vector3.Dot(Vector3.Up, direction));
      cross = Vector3.Cross(Vector3.Up, direction);
      if (cross != Vector3.Zero)
      {
        cross.Normalize();
      }

      if (angleToPlayer >= Math.PI * 0.96f)
      {
        // Angle too small, show front of projectile
        billboardType = ProjectileBillboardType.PBT_NORMAL;
      }
      else
      {
        billboardType = ProjectileBillboardType.PBT_CONSTRAINED;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);      
      Effect.Parameters["coloroverlay"].SetValue(Vector4.One);

      switch (this.billboardType)
      {
        case ProjectileBillboardType.PBT_NORMAL:
          DrawBillboardNormal(gameTime);
          break;
        case ProjectileBillboardType.PBT_CONSTRAINED:
          DrawConstrainedBillboard(gameTime);
          break;
      }
    }

    private void DrawBillboardNormal(GameTime gameTime)
    {
      this.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;
      Effect.Parameters["Texture"].SetValue(this.texProjectileFront);
      Effect.Parameters["matWorld"].SetValue(
        Matrix.CreateBillboard(
          this.Position,
          this.Game.GameHandler.Camera.Position,
          this.Game.GameHandler.Camera.UpVector,
          this.Game.GameHandler.Camera.ForwardVector));

      Effect.Begin();
      Effect.CurrentTechnique.Passes[0].Begin();

      this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
        PrimitiveType.TriangleList,
        this.verts, 0, 4, ib, 0, 2);

      Effect.CurrentTechnique.Passes[0].End();
      Effect.End();
    }

    private void DrawConstrainedBillboard(GameTime gameTime)
    {
      Quaternion rot = Quaternion.CreateFromAxisAngle(cross, angle);
      Vector3 vecScale = new Vector3(0.5f, 3f, 0.5f);
      Vector3 vecRotationAxis = Vector3.Transform(Vector3.Up, rot);

      this.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;
      Effect.Parameters["Texture"].SetValue(this.texProjectile);
      Effect.Parameters["matWorld"].SetValue(
        Matrix.CreateScale(vecScale) *
        Matrix.CreateConstrainedBillboard(
          this.Position,
          this.Game.GameHandler.Camera.Position,
          vecRotationAxis,
          null, null));

      Effect.Begin();
      Effect.CurrentTechnique.Passes[0].Begin();

      this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
        PrimitiveType.TriangleList,
        this.verts, 0, 4, ib, 0, 2);

      Effect.CurrentTechnique.Passes[0].End();
      Effect.End();
    }
  }
}
