using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;

namespace Tier.Source.Objects.Projectiles
{
  public class RocketComboRocket : Projectile
  {
    #region Properties
    private Vector3 spawnDirection;
    private float timeElapsed;

    public Vector3 SpawnDirection
    {
      get { return spawnDirection; }
      set { spawnDirection = value; }
    }
	
    private enum RocketComboRocketState
    {
      SPAWNING, FOLLOWING_PLAYER
    };

    private RocketComboRocketState state;
    #endregion

    private RocketTrail trail;
    private float timeSinceTrailUpdate;
    private Vector3 lastPosition;

    public RocketComboRocket(TierGame game)
      : base(game)
    {
      state = RocketComboRocketState.SPAWNING;
      trail = new RocketTrail(game);
      timeSinceTrailUpdate = 0;
      lastPosition = Vector3.One;

      game.GameHandler.AddObject(trail);
    }

    private void changeState(RocketComboRocketState newstate)
    {
      switch (newstate)
      {
        case RocketComboRocketState.SPAWNING:
          break;
        case RocketComboRocketState.FOLLOWING_PLAYER:
          break;
      }

      this.state = newstate;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Effect =
        this.Game.Content.Load<Effect>("Effects//DeferredRendering//RenderGBuffer").Clone(this.Game.GraphicsDevice);
      this.Effect.Parameters["Texture"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Turret-color"));
      this.Effect.Parameters["SpecularMap"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Turret-specular"));
      this.Effect.Parameters["NormalMap"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Turret-normal"));
      this.Effect.Parameters["colorOverlay"].SetValue(Vector4.One);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.IsVisible)
        return;

      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);
      this.Effect.Parameters["Texture"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Rocket-color"));
      this.Effect.Parameters["SpecularMap"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Turret-specular"));
      this.Effect.Parameters["NormalMap"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>("Turret-normal"));

      this.Effect.Begin();

      foreach (ModelMesh mesh in Model.Meshes)
      {
        this.Effect.Parameters["matWorld"].SetValue(
            this.Game.GameHandler.World *
            this.transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.Rotation) *
            Matrix.CreateScale(this.Scale) *
            Matrix.CreateTranslation(this.Position));

        this.Effect.CurrentTechnique.Passes[0].Begin();

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

    public void Explode()
    {
      this.Game.GameHandler.RemoveObject(this, this.Type);
      this.Game.GameHandler.RemoveObject(this.trail, this.trail.Type);

      // Spawn explosion billboard
      Billboard b = new Billboard(this.Game, BillboardType.AnimatedTexture);
      this.Game.ObjectHandler.InitializeFromBlueprint<Billboard>(b, "Explosion");
      b.Scale = new Vector3(2);
      b.Position = this.Position;
      this.Game.GameHandler.AddObject(b);
    }
    
    private void rotateToDirection(Vector3 direction)
    {
      Vector3 axis = Vector3.Cross(Vector3.Backward, direction);
      float angle = (float)Math.Acos(Vector3.Dot(Vector3.Backward, direction));

      this.Rotation = Quaternion.CreateFromAxisAngle(axis, angle);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.timeSinceTrailUpdate += gameTime.ElapsedGameTime.Milliseconds;

      if (this.timeSinceTrailUpdate > 200)
      {
        this.timeSinceTrailUpdate = 0;

        if (this.lastPosition != Vector3.Zero)
        {
          this.trail.AddPoint(this.lastPosition);
          this.trail.AddPoint(this.Position);
        }

        this.lastPosition = this.Position;
      }

      switch (this.state)
      {
        case RocketComboRocketState.SPAWNING:
          UpdateSpawning(gameTime);
          break;
        case RocketComboRocketState.FOLLOWING_PLAYER:
          UpdateFollowing(gameTime);
          break;
      }
    }

    private void UpdateFollowing(GameTime gameTime)
    {
      Vector3 dir = this.Game.GameHandler.Player.Position - this.Position;
      dir.Normalize();

      this.MovableModifier.Velocity = dir * Speed;
      rotateToDirection(dir);

      Vector3 diff = this.Position - this.Game.GameHandler.Player.Position;
      if (diff.Length() <= 1.0f)
      {
        Explode();        
      }
    }
    
    private void UpdateSpawning(GameTime gameTime)
    {
      if(this.timeElapsed >= 1000.0f)
        changeState(RocketComboRocketState.FOLLOWING_PLAYER);
      
      this.MovableModifier.Velocity = spawnDirection * Speed;
      rotateToDirection(spawnDirection);
    }
  }
}
