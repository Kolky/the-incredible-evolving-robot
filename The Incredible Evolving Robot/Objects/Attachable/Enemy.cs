using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Menus;
using Tier.Misc;
using Tier.Objects.Basic;
using Tier.Objects.Destroyable;
using Tier.Objects.Destroyable.Projectile;

namespace Tier.Objects.Attachable
{
  public class Enemy : BlockPiece
  {
    #region Properties
    private BossGrowth pattern;
    private BasicEffect effect;
    private bool isSpawning;

    public bool IsSpawning
    {
      get { return isSpawning; }
      set { isSpawning = value; }
    }
	
    public new BasicEffect Effect
    {
      get { return effect; }
      set { effect = value; }
    }
	
    public BossGrowth GrowthPattern
    {
      get { return pattern; }
      set { pattern = value; }
    }

    private List<BlockPiece> pieces;
    public List<BlockPiece> Pieces
    {
      get { return pieces; }
    }

    private BossGrower bossGrower;
    private Thread growerThread;
    private int elapsedMillis; 
    #endregion       

    public Enemy(Game game)
      : base(game)
    {
      this.pattern = new BossGrowth();
      this.pieces = new List<BlockPiece>();

      this.bossGrower = new BossGrower();
      this.bossGrower.Parent = this;

      this.MaxHealth = 10000; // DestroyableObject must be always smaller than this value
    }

    #region BlockPiece Overrides
    public override void Initialize()
    {
      base.Initialize();
      
      this.Effect = new BasicEffect(this.GraphicsDevice, null);
      this.Effect.EnableDefaultLighting();
      this.Effect.PreferPerPixelLighting = true;

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (ModelMeshPart part in mesh.MeshParts)
        {
          part.Effect = this.Effect;
        }
      }
    }   

    public override void Update(GameTime gameTime)
    {
      if (this.IsSpawning)
      {
        //Console.WriteLine(elapsedMillis);
        elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;

        if (elapsedMillis >= 5000)
        {
          this.Movement.Velocity = Vector3.Zero;
          this.IsSpawning = false;
          this.elapsedMillis = 0;
        }

        this.UpdateVelocity(this, this.Movement.Velocity);
      }

      this.UpdateBoundingObjects();

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Exploded)
        return;

      Matrix[] transforms = new Matrix[this.Model.Bones.Count];
      this.Model.CopyAbsoluteBoneTransformsTo(transforms);

      for (int i = 0; i < this.Model.Meshes.Count; i++)
      {
        ModelMesh mesh = this.Model.Meshes[i];
        this.Effect.View = GameHandler.Camera.View;
        this.Effect.Projection = GameHandler.Camera.Projection;
        this.Effect.World = transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.Position.Front) *
            this.SpawnMatrix *
            Matrix.CreateTranslation(this.Position.Coordinate);

        switch (i)
        {
          case 0:
            this.Effect.DiffuseColor = Options.Colors.Boss.CoreColor;
            this.Effect.Alpha = 1.0f;
            this.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            this.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
            mesh.Draw();

            break;
          case 1:
            this.Effect.DiffuseColor = Options.Colors.Boss.BlockColor;
            this.Effect.Alpha = 0.65f;
            this.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            this.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            this.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            this.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            this.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
            mesh.Draw();

            this.Effect.Alpha = 1.0f;
            this.Effect.DiffuseColor = Options.Colors.Boss.LineColor;
            this.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            this.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            mesh.Draw();
            break;
        }
      }

      this.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
    }
    #endregion

    public void GrowBoss()
    {
      this.growerThread = new Thread(new ThreadStart(this.bossGrower.Grow));
      this.growerThread.Start();
    }

    public void Spawn()
    {
      Vector3 newPos = Vector3.Subtract(Vector3.Zero, GameHandler.Player.Position.Coordinate);
      this.SetPosition(this, Matrix.CreateTranslation(newPos));
      newPos.Normalize();
      this.Movement.Velocity = -newPos / 18.35f;


      this.IsSpawning = true;
      this.elapsedMillis = 0;

			//this.Health = this.MaxHealth; //BOSS reset Health
      this.Health = this.UpdateHealth(this) + TierGame.GameHandler.CurrentLevel * 1200;	//This add the BOSS Health + all it's children
			GameHandler.HUD.AddScore(20 + (TierGame.GameHandler.CurrentLevel * 5));
			GameHandler.HUD.AddTime(20 + (TierGame.GameHandler.CurrentLevel * 5));
    }
  }
}
