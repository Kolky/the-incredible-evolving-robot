using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Misc;

namespace Tier.Source.Objects.Projectiles
{
  public class RocketProjectile : Projectile
  {
    #region Properties
    private int timeElapsed, particleTimeElapsed;
    private Texture2D texture;
    private float aoeSize;
    private Vector3 direction;
    private int aoeDamage;

    public int AoeDamage
    {
      set { aoeDamage = value; }
    }

    public Vector3 Direction
    {
      set { direction = value; }
    }

    public float AoeSize
    {
      get { return aoeSize; }
      set { aoeSize = value; }
    }
    #endregion

    public RocketProjectile(TierGame game)
      : base(game)
    { }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      obj.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi);
      ((RocketProjectile)obj).texture = this.Game.ContentHandler.GetAsset<Texture2D>("Rocket-color");
    }

    private void DoCollision()
    {
      // Collision detection
      GameObjectSegment segment = this.Game.GameHandler.Octree.Segment;
      Vector3 dir = this.MovableModifier.Velocity;
      dir.Normalize();
      Ray r = new Ray(this.Position, dir);

      foreach (GameObject segmentObject in segment.Objects)
      {
        float? distance = null;

        segmentObject.CollisionModifier.CheckCollision(r, out distance);

        if (distance <= 1.0f)
        {
          segmentObject.DestroyableModifier.IsHit(this.Damage);
          DoExplosion();
          break;
        }
      }
    }

    private void DoExplosion()
    {
      // Create explosion (which does AoE damage)
      // Spawn a billboard with the explosion
      Billboard b = new Billboard(this.Game, BillboardType.AnimatedTexture);
      this.Game.ObjectHandler.InitializeFromBlueprint<Billboard>(b, "Explosion");

      b.Scale = new Vector3(aoeSize);
      b.Position = this.Position;
      this.Game.GameHandler.AddObject(b);
      // Calculate which objects are hit by building a boundingsphere
      BoundingSphere sphere = new BoundingSphere(this.Position, aoeSize);
      // Detect which objects are hit
      GameObjectSegment segment = this.Game.GameHandler.Octree.Segment;
      foreach (GameObject segmentObject in segment.Objects)
      {
        if (segmentObject.CollisionModifier != null &&
          segmentObject.CollisionModifier.CheckCollision(sphere) != ContainmentType.Disjoint)
        {
          segmentObject.DestroyableModifier.IsHit(this.aoeDamage);
        }
      }

      // Remove missile
      this.Game.GameHandler.RemoveObject(
        this,
        Tier.Source.Handlers.GameHandler.ObjectType.DefaultTextured);

    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);

      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      particleTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      // Spawn some smoke particles every few milliseconds
      /*if (particleTimeElapsed >= 250)
      {
        particleTimeElapsed = 0;
        SpawnParticle();
      }*/

      DoCollision();

      float amount = (MathHelper.Pi * 2) * (gameTime.ElapsedGameTime.Milliseconds / 500.0f);
      this.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, amount);
    }

    public override void Draw(GameTime gameTime)
    {
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);
      this.Effect.Parameters["Texture"].SetValue(texture);
      this.Effect.Parameters["colorOverlay"].SetValue(new Vector4(1, 1, 1, 1));

      this.Effect.Begin();

      this.Effect.CurrentTechnique.Passes[0].Begin();
      foreach (ModelMesh mesh in Model.Meshes)
      {
        this.Effect.Parameters["matWorld"].SetValue(
          this.Game.GameHandler.World *
          this.transforms[mesh.ParentBone.Index] *
          Matrix.CreateScale(this.Scale) *
          Matrix.CreateFromQuaternion(this.Rotation) *
          Matrix.CreateTranslation(this.Position));

        foreach (ModelMeshPart meshPart in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);
          this.GraphicsDevice.Indices = mesh.IndexBuffer;
          this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
            meshPart.BaseVertex, 0,
            meshPart.NumVertices,
            meshPart.StartIndex,
            meshPart.PrimitiveCount);
        }
      }

      this.Effect.CurrentTechnique.Passes[0].End();
      this.Effect.End();
    }

    private void SpawnParticle()
    {
      Billboard b = new Billboard(this.Game, BillboardType.AnimatedTexture);
      this.Game.ObjectHandler.InitializeFromBlueprint<Billboard>(b, "Smoke");
      b.Scale = new Vector3(1);
      b.Position = this.Position;

      this.Game.GameHandler.AddObject(b);
    }
  }
}