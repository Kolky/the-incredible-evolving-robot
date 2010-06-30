using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Misc;
using Tier.Menus;
using Tier.Objects.Attachable;
using Tier.Objects.Basic;
using Tier.Handlers;

namespace Tier.Objects
{
    abstract public class DestroyableObject : MovableObject
    {
        #region Properties
        public bool Exploded { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }

        public List<BoundingAbstractMeta> BoundingVolumes { get; private set; }
        #endregion

        public DestroyableObject(Game game, Boolean isCollidable)
            : base(game, isCollidable)
        {
            BoundingVolumes = new List<BoundingAbstractMeta>();

            Health = MaxHealth = 1000;	//Enemy should always have the most health      
        }

        public override void Update(GameTime gameTime)
        {
            if (Health < 0 && !Exploded)
                Explode(this);

            if (!threadStarted && threadDone)
            {
                GameHandler.MenuState = new NextLevelMenu();
                threadDone = false;
            }

            base.Update(gameTime);
        }

        public void ObjectHit()
        {
            Health -= 5;
        }

        #region Bounding Objects
        public bool DetectCollision(DestroyableObject dest)
        {
            for (int i = 0; i < BoundingVolumes.Count; i++)
            {
                for (int n = 0; n < dest.BoundingVolumes.Count; n++)
                {
                    if (BoundingVolumes[i].CheckCollision(dest.BoundingVolumes[n]))
                        return true;
                }
            }
            return false;
        }

        public void addBoundingShere(float radius, Vector3 offset)
        {
            BoundingVolumes.Add(new BoundingSphereMeta(Position.Coordinate, radius, offset));
        }

        public void addBoundingBox(Vector3 bounds, Vector3 offset)
        {
            BoundingVolumes.Add(new BoundingBoxMeta(Position.Coordinate, bounds, offset));
        }

        public void addBoundingBar(Vector3 boundsLeft, Vector3 boundsRight, Vector3 offset)
        {
            BoundingVolumes.Add(new BoundingBarMeta(Position.Coordinate, boundsLeft, boundsRight, offset));
        }

        public virtual void UpdateBoundingObjects()
        {
            for (int i = 0; i < BoundingVolumes.Count; i++)
            {
                BoundingVolumes[i].Update(Position.Coordinate, Position.Front);
            }
        }

#if DEBUG && BOUNDRENDER
        /// <summary>Only draws the customized boundingSphere's, NOT the parts</summary>
        public void DrawBoundingObjects()
        {
            for (int i = 0; i < BoundingVolumes.Count; i++)
            {
                BoundingVolumes[i].Draw();
            }
        }
#endif
        #endregion

        private Boolean threadStarted;
        private Boolean threadDone;

        private void WaitThread()
        {
            threadStarted = true;
            Thread.Sleep(2500);
            threadStarted = false;
            threadDone = true;
        }

        public virtual void Explode(BasicObject parent)
        {
            Exploded = true;
            //TierGame.Audio.playSFX3D("explosion", this, 2); //Limits the explosion to two at a time (for this events at any case)
            TierGame.GameHandler.Boss.Health -= MaxHealth;
            if (TierGame.GameHandler.Boss.Health > MaxHealth)
                TierGame.GameHandler.Boss.Health -= MaxHealth;
            else
                TierGame.GameHandler.Boss.Health = 1;

            GameHandler.ObjectHandler.AddObject(new ExplosionCluster(Game, new Vector3(2, 2, 2), Position.Coordinate, 50));


            if (this.GetType() == typeof(Enemy) && !threadStarted)
            {
                new Thread(new ThreadStart(WaitThread)).Start();
            }
        }
    }
}