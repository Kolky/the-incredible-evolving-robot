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
		private List<Laser> lasers;
    public List<Laser> Lasers
    {
      get { return lasers; }
    }

    private List<Laser> toBeRemoved;

		private float radSpread;
		public float RadSpread
		{
			get { return this.radSpread; }
			set { this.radSpread = value; }
		}

		private int clusterCapacity = 8;
		#endregion

		public LaserCluster(Game game, Position sourcePos, float spread)
			: base(game, true, sourcePos, 40)
		{
			this.radSpread = spread;
			this.lasers = new List<Laser>(this.clusterCapacity);
      this.toBeRemoved = new List<Laser>();

			this.TimeToLive = 750;
			this.Model = TierGame.ContentHandler.GetModel("Laser");
			this.Scale = 0.075f;
			//this.Sort = SortFilter.Bloom;

			this.ModelMeta = new ModelMeta(this.Model);
			this.Initialize();
		}

		public override void Initialize()
		{
			base.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2 + MathHelper.Pi);

			Position posWithSpread = new Position(this.Position);
			Vector3 upVector = Vector3.Transform(Vector3.Up, this.Position.Front);
			Vector3 rightVector = Vector3.Transform(Vector3.Right, this.Position.Front);
			for (int i = 0; i < this.clusterCapacity; i++)
			{
        Quaternion randomSpread =
						Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(upVector, (radSpread * Options.Random.Next(-1000, 1000)) / 1000f),
																	 Quaternion.CreateFromAxisAngle(rightVector, (radSpread * Options.Random.Next(-1000, 1000)) / 1000f));

				posWithSpread.Front = Quaternion.Concatenate(posWithSpread.Front, randomSpread);
        Laser l = new Laser(this.Game, posWithSpread);
				this.Lasers.Add(l);
			}
      this.addBoundingBar(Vector3.One  * this.RadSpread, Vector3.One  * this.RadSpread, Vector3.Zero);

      base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
      RemoveLasers();

      for (int i = 0; i < this.BoundingBarMetas.Count; i++)
      {
        float fact = (gameTime.ElapsedGameTime.Milliseconds * 0.002f) * (1 * (this.RadSpread * 50));

        this.BoundingBarMetas[i].BoundsLeft = new Vector3(this.BoundingBarMetas[i].BoundsLeft.X + fact,
          this.BoundingBarMetas[i].BoundsLeft.Y + fact,
          this.BoundingBarMetas[i].BoundsLeft.Z);
        this.BoundingBarMetas[i].BoundsRight = new Vector3(this.BoundingBarMetas[i].BoundsRight.X + fact,
          this.BoundingBarMetas[i].BoundsRight.Y + fact,
          this.BoundingBarMetas[i].BoundsRight.Z);
      }

			for (int i = 0; i < this.Lasers.Count; i++)
			{
				this.Lasers[i].Update(gameTime);
			}
			this.UpdateBoundingObjects();

			base.Update(gameTime);
		}

    public void RemoveLasers()
    {
      for (int i = 0; i < this.toBeRemoved.Count; i++)
      {
        if (this.Lasers.Contains(this.toBeRemoved[i]))
          this.Lasers.Remove(this.toBeRemoved[i]);
      }

      this.toBeRemoved.Clear();
    }

    public void RemoveLaser(Laser laser)
    {
      this.toBeRemoved.Add(laser);
    }

		public override void Draw(GameTime gameTime)
		{
			for (int i = 0; i < this.lasers.Count; i++)
			{
        this.lasers[i].Draw(gameTime);
#if DEBUG && BOUNDRENDER
        this.lasers[i].DrawBoundingObjects();
#endif
        /*
				foreach (ModelMesh mesh in this.Model.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						effect.World = Matrix.CreateScale(this.Scale) *
							RotationFix *
							Matrix.CreateFromQuaternion(this.lasers[i].Position.Front) *
							Matrix.CreateTranslation(this.lasers[i].Position.Coordinate);
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
