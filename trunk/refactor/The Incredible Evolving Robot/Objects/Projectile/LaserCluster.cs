using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Misc;
using Microsoft.Xna.Framework.Graphics;
using Tier.Handlers;

namespace Tier.Objects.Destroyable.Projectile
{
    class LaserCluster : Tier.Objects.Projectile
    {
        #region properties
        public List<Laser> Lasers { get; private set; }
        private List<Laser> toBeRemoved;
        public float RadSpread { get; set; }
        private int clusterCapacity = 8;
        #endregion

        public LaserCluster(Game game, Position sourcePos, float spread)
            : base(game, true, sourcePos, 40)
        {
            RadSpread = spread;
            Lasers = new List<Laser>(clusterCapacity);
            toBeRemoved = new List<Laser>();

            TimeToLive = 750;
            Model = TierGame.ContentHandler.GetModel("Laser");
            Scale = 0.075f;
            //Sort = SortFilter.Bloom;

            ModelMeta = new ModelMeta(Model);
            Initialize();
        }

        public override void Initialize()
        {
            base.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2 + MathHelper.Pi);

            Position posWithSpread = new Position(Position);
            Vector3 upVector = Vector3.Transform(Vector3.Up, Position.Front);
            Vector3 rightVector = Vector3.Transform(Vector3.Right, Position.Front);
            for (int i = 0; i < clusterCapacity; i++)
            {
                Quaternion randomSpread = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(upVector, (RadSpread * Options.Random.Next(-1000, 1000)) / 1000f),
                                                                             Quaternion.CreateFromAxisAngle(rightVector, (RadSpread * Options.Random.Next(-1000, 1000)) / 1000f));

                posWithSpread.Front = Quaternion.Concatenate(posWithSpread.Front, randomSpread);
                Laser l = new Laser(Game, posWithSpread);
                Lasers.Add(l);
            }
            addBoundingBar(Vector3.One * RadSpread, Vector3.One * RadSpread, Vector3.Zero);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            RemoveLasers();

            for (int i = 0; i < BoundingBoxMetas.Count; i++)
            {
                float fact = (gameTime.ElapsedGameTime.Milliseconds * 0.002f) * (1 * (RadSpread * 50));

                if (BoundingBoxMetas[i].GetType() == typeof(BoundingBarMeta))
                {
                    BoundingBarMeta bar = (BoundingBarMeta)BoundingBoxMetas[i];

                    bar.BoundsLeft = new Vector3(bar.BoundsLeft.X + fact, bar.BoundsLeft.Y + fact, bar.BoundsLeft.Z);
                    bar.BoundsRight = new Vector3(bar.BoundsRight.X + fact, bar.BoundsRight.Y + fact, bar.BoundsRight.Z);
                }
            }

            for (int i = 0; i < Lasers.Count; i++)
            {
                Lasers[i].Update(gameTime);
            }
            UpdateBoundingObjects();

            base.Update(gameTime);
        }

        public void RemoveLasers()
        {
            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (Lasers.Contains(toBeRemoved[i]))
                    Lasers.Remove(toBeRemoved[i]);
            }

            toBeRemoved.Clear();
        }

        public void RemoveLaser(Laser laser)
        {
            toBeRemoved.Add(laser);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Lasers.Count; i++)
            {
                Lasers[i].Draw(gameTime);
#if DEBUG && BOUNDRENDER
                this.lasers[i].DrawBoundingObjects();
#endif
                /*
				foreach (ModelMesh mesh in Model.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						effect.World = Matrix.CreateScale(Scale) *
							RotationFix *
							Matrix.CreateFromQuaternion(Lasers[i].Position.Front) *
							Matrix.CreateTranslation(Lasers[i].Position.Coordinate);
						effect.View = GameHandler.Camera.View;
						effect.Projection = GameHandler.Camera.Projection;
					}
					mesh.Draw();
				}
                */
            }
        }
    }
}
