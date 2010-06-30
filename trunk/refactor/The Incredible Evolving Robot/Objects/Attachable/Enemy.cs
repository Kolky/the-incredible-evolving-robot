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
        public bool IsSpawning { get; set; }
        public new BasicEffect Effect { get; set; }
        public BossGrowth GrowthPattern { get; set; }
        public List<BlockPiece> Pieces { get; private set; }

        private BossGrower bossGrower;
        private Thread growerThread;
        private int elapsedMillis;
        #endregion

        public Enemy(Game game)
            : base(game)
        {
            GrowthPattern = new BossGrowth();
            Pieces = new List<BlockPiece>();

            bossGrower = new BossGrower();
            bossGrower.Parent = this;

            MaxHealth = 10000; // DestroyableObject must be always smaller than this value
        }

        #region BlockPiece Overrides
        public override void Initialize()
        {
            base.Initialize();

            Effect = new BasicEffect(GraphicsDevice, null);
            Effect.EnableDefaultLighting();
            Effect.PreferPerPixelLighting = true;

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsSpawning)
            {
                //Console.WriteLine(elapsedMillis);
                elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedMillis >= 5000)
                {
                    Movement.Velocity = Vector3.Zero;
                    IsSpawning = false;
                    elapsedMillis = 0;
                }

                UpdateVelocity(this, Movement.Velocity);
            }

            UpdateBoundingObjects();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Exploded)
                return;

            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            for (int i = 0; i < Model.Meshes.Count; i++)
            {
                ModelMesh mesh = Model.Meshes[i];
                Effect.View = GameHandler.Camera.View;
                Effect.Projection = GameHandler.Camera.Projection;
                Effect.World = transforms[mesh.ParentBone.Index] *
                    Matrix.CreateFromQuaternion(Position.Front) *
                    SpawnMatrix *
                    Matrix.CreateTranslation(Position.Coordinate);

                switch (i)
                {
                    case 0:
                        Effect.DiffuseColor = Options.Colors.Boss.CoreColor;
                        Effect.Alpha = 1.0f;
                        GraphicsDevice.RenderState.AlphaBlendEnable = false;
                        GraphicsDevice.RenderState.FillMode = FillMode.Solid;
                        mesh.Draw();

                        break;
                    case 1:
                        Effect.DiffuseColor = Options.Colors.Boss.BlockColor;
                        Effect.Alpha = 0.65f;
                        GraphicsDevice.RenderState.AlphaBlendEnable = true;
                        GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                        GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                        GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                        GraphicsDevice.RenderState.FillMode = FillMode.Solid;
                        mesh.Draw();

                        Effect.Alpha = 1.0f;
                        Effect.DiffuseColor = Options.Colors.Boss.LineColor;
                        GraphicsDevice.RenderState.AlphaBlendEnable = false;
                        GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
                        mesh.Draw();
                        break;
                }
            }

            GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        }
        #endregion

        public void GrowBoss()
        {
            growerThread = new Thread(new ThreadStart(bossGrower.Grow));
            growerThread.Start();
        }

        public void Spawn()
        {
            Vector3 newPos = Vector3.Subtract(Vector3.Zero, GameHandler.Player.Position.Coordinate);
            SetPosition(this, Matrix.CreateTranslation(newPos));
            newPos.Normalize();
            Movement.Velocity = -newPos / 18.35f;


            IsSpawning = true;
            elapsedMillis = 0;

            //Health = MaxHealth; //BOSS reset Health
            Health = UpdateHealth(this) + TierGame.GameHandler.CurrentLevel * 1200;	//This add the BOSS Health + all it's children
            GameHandler.HUD.AddScore(20 + (TierGame.GameHandler.CurrentLevel * 5));
            GameHandler.HUD.AddTime(20 + (TierGame.GameHandler.CurrentLevel * 5));
        }
    }
}
