using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;

namespace Tier.Source.Objects.Projectiles
{
  public class LaserbeamProjectile : Projectile
  {
    #region Properties
    private Vector3 direction;
    private Vector3 objectForward;
    private Vector3 target;

    public Texture2D LaserbeamTexture
    {
      get { return this.laserbeamTexture; }
      set { this.laserbeamTexture = value; }
    }
    
    public Vector3 Target
    {
      get { return target; }
      set { target = value; }
    }
	
    public Vector3 ObjectForward
    {
      get { return objectForward; }
      set { objectForward = value; }
    }
	
    public Vector3 Direction
    {
      get { return direction; }
      set { direction = value; }
    }
	
    private Billboard laserbeam;
    public VertexPositionTexture[] verts;
    public short[] ib;
    #endregion

    private float blastScale;
    private bool blastScaleUp;
    private Texture2D laserbeamTexture;
    private bool isCollisionEnabled;

    public LaserbeamProjectile(TierGame game)
      : this(game, true)
    { }

    public LaserbeamProjectile(TierGame game, bool isCollisionEnabled)
      : base(game)
    {
      direction = Vector3.Forward;
      this.laserbeam = new Billboard(game, BillboardType.AnimatedTexture);
      this.Rotation = Quaternion.Identity;
      this.blastScale = 1.0f;
      this.blastScaleUp = true;
      this.isCollisionEnabled = isCollisionEnabled;
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);

      obj.Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      ((LaserbeamProjectile)obj).verts = new VertexPositionTexture[]
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

      ((LaserbeamProjectile)obj).ib = new short[] { 0, 1, 2, 2, 3, 0 };
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      if (!IsVisible)
        return;

      base.Update(gameTime);
      
      if (blastScaleUp)
        blastScale += (gameTime.ElapsedGameTime.Milliseconds / 250.0f) * 0.5f;
      else
        blastScale -= (gameTime.ElapsedGameTime.Milliseconds / 250.0f) * 0.5f;

      if (blastScale >= 1.5f)
      {
        blastScale = 1.5f;
        blastScaleUp = !blastScaleUp;
      }
      else if (blastScale <= 1.0f)
      {
        blastScale = 1.0f;
        blastScaleUp = !blastScaleUp;
      }
      
      // Update beam direction
      this.direction = this.target - this.Position;
      this.direction.Normalize();

      // Collision
      if (this.isCollisionEnabled)
      {
        Ray r = new Ray(this.Position, Direction);
        float? distance = null;

        this.Game.GameHandler.Player.CollisionModifier.CheckCollision(r, out distance);
        if (distance != null)
        {
          this.Game.GameHandler.Player.DestroyableModifier.IsHit(0.001f);
        }
      }

      this.IsVisible = false;
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      if (!IsVisible)
        return;

      this.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);

      this.Effect.Begin();
      this.Effect.CurrentTechnique.Passes[0].Begin();
      DrawBeam(gameTime);
      this.Effect.CurrentTechnique.Passes[0].End();
      this.Effect.End();
    }

    private void DrawBeam(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.Effect.Parameters["Texture"].SetValue(this.laserbeamTexture);
      this.Effect.Parameters["coloroverlay"].SetValue(this.Color.ToVector4());
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);

      // Rotation to player
      float angle = (float)Math.Acos(Vector3.Dot(Vector3.Up, direction));
      Vector3 cross = Vector3.Cross(Vector3.Up, direction);
      if (cross != Vector3.Zero)
      {
        cross.Normalize();
      }

      float length = (this.target - this.Position).Length();
      
      Quaternion rot = Quaternion.CreateFromAxisAngle(cross, angle);
      Vector3 vecScale = new Vector3(1, length, 1);
      Vector3 vecRotationAxis = Vector3.Transform(Vector3.Up, rot);

      this.Effect.Parameters["matWorld"].SetValue(
        Matrix.CreateScale(vecScale) *
        Matrix.CreateConstrainedBillboard(
          this.Position,
          this.Game.GameHandler.Camera.Position,
          vecRotationAxis,
          this.Game.GameHandler.Camera.ForwardVector,
          Vector3.Transform(Vector3.Forward, this.Rotation))
      );
      this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
        this.verts, 0, 4, ib, 0, 2);
    }
  }
}
