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

        public List<BoundingAbstractMeta> BoundingBoxMetas { get; private set; }
        public List<BoundingSphereMeta> BoundingSphereMetas { get; private set; }
        #endregion

        public DestroyableObject(Game game, Boolean isCollidable)
            : base(game, isCollidable)
        {
            BoundingBoxMetas = new List<BoundingAbstractMeta>();
            BoundingSphereMetas = new List<BoundingSphereMeta>();

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
            // BoundingSphere
            for (int i = 0; i < BoundingSphereMetas.Count; i++)
            {
                // BoundingSphere
                for (int n = 0; n < dest.BoundingSphereMetas.Count; n++)
                    if (BoundingSphereMetas[i].Sphere.Intersects(dest.BoundingSphereMetas[n].Sphere))
                        return true;

                // BoundingBox
                for (int n = 0; n < dest.BoundingBoxMetas.Count; n++)
                    if (BoundingSphereMetas[i].Sphere.Intersects(dest.BoundingBoxMetas[n].Box))
                        return true;
            }

            // BoundingBox
            for (int i = 0; i < BoundingBoxMetas.Count; i++)
            {
                // BoundingSphere
                for (int n = 0; n < dest.BoundingSphereMetas.Count; n++)
                    if (BoundingBoxMetas[i].Box.Intersects(dest.BoundingSphereMetas[n].Sphere))
                        return true;

                // BoundingBox
                for (int n = 0; n < dest.BoundingBoxMetas.Count; n++)
                    if (BoundingBoxMetas[i].Box.Intersects(dest.BoundingBoxMetas[n].Box))
                        return true;
            }

            return false;
        }

        public void addBoundingShere(float radius, Vector3 offset)
        {
            BoundingSphereMetas.Add(new BoundingSphereMeta(new BoundingSphere(Position.Coordinate, radius), offset));
        }

        public void addBoundingBox(Vector3 bounds, Vector3 offset)
        {
            BoundingBoxMetas.Add(new BoundingBoxMeta(Position.Coordinate, bounds, offset));
        }

        public void addBoundingBar(Vector3 boundsLeft, Vector3 boundsRight, Vector3 offset)
        {
            BoundingBoxMetas.Add(new BoundingBarMeta(Position.Coordinate, boundsLeft, boundsRight, offset));
        }

        public virtual void UpdateBoundingObjects()
        {
            for (int i = 0; i < BoundingSphereMetas.Count; i++)
                BoundingSphereMetas[i].Center = Position.Coordinate + Vector3.Transform(BoundingSphereMetas[i].Offset, Position.Front);
            for (int i = 0; i < BoundingBoxMetas.Count; i++)
                BoundingBoxMetas[i].Center = Position.Coordinate + Vector3.Transform(BoundingBoxMetas[i].Offset, Position.Front);
        }

#if DEBUG && BOUNDRENDER
        /// <summary>Only draws the customized boundingSphere's, NOT the parts</summary>
        public void DrawBoundingObjects()
        {
            for (int i = 0; i < BoundingSphereMetas.Count; i++)
                TierGame.BoundingSphereRender.Draw(BoundingSphereMetas[i].Sphere, Color.White);
            for (int i = 0; i < BoundingBoxMetas.Count; i++)
            {
                if(BoundingBoxMetas[i].GetType() == typeof(BoundingBoxMeta))
                {
                    TierGame.BoundingBoxRenderer.Draw((BoundingBoxMeta)BoundingBoxMetas[i], Color.White);
                }
                else
                {
                    TierGame.BoundingBoxRenderer.Draw((BoundingBarMeta)BoundingBoxMetas[i], Color.White);
                }
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