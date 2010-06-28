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
    private int health;
    private bool exploded;

    public bool Exploded
    {
      get { return exploded; }
      set { exploded = value; }
    }

		private int maxHealth;

		public int MaxHealth
		{
			get { return maxHealth; }
			set { maxHealth = value; }
		}
	


    public int Health
    {
      get { return health; }
      set { health = value; }
    }

    private List<BoundingBoxMeta> boundingBoxMetas;
    public List<BoundingBoxMeta> BoundingBoxMetas
    {
      get { return boundingBoxMetas; }
    }

    private List<BoundingSphereMeta> boundingSphereMetas;
    public List<BoundingSphereMeta> BoundingSphereMetas
    {
      get { return boundingSphereMetas; }
    }

    private List<BoundingBarMeta> boundingBarMetas;
    public List<BoundingBarMeta> BoundingBarMetas
    {
      get { return boundingBarMetas; }
    }
    #endregion
	
    public DestroyableObject(Game game, Boolean isCollidable)
      : base(game, isCollidable)
    {
      this.boundingBarMetas = new List<BoundingBarMeta>();
      this.boundingBoxMetas = new List<BoundingBoxMeta>();
      this.boundingSphereMetas = new List<BoundingSphereMeta>();     

      this.Health = this.MaxHealth = 1000;	//Enemy should always have the most health      
    }

    public override void Update(GameTime gameTime)
    {
      if (this.Health < 0 && !this.Exploded)
        this.Explode(this);

      if (!this.threadStarted && this.threadDone)
      {
        GameHandler.MenuState = new NextLevelMenu();
        this.threadDone = false;
      }

      base.Update(gameTime);
    }

    public void ObjectHit()
    {
      this.Health -= 5;
    }

    #region Bounding Objects
    public bool DetectCollision(DestroyableObject dest)
    {
      // BoundingSphere
      for (int i = 0; i < this.BoundingSphereMetas.Count; i++)
      {
        // BoundingSphere
        for (int n = 0; n < dest.BoundingSphereMetas.Count; n++)
          if (this.BoundingSphereMetas[i].Sphere.Intersects(dest.BoundingSphereMetas[n].Sphere))
            return true;

        // BoundingBox
        for (int n = 0; n < dest.BoundingBoxMetas.Count; n++)
          if (this.BoundingSphereMetas[i].Sphere.Intersects(dest.BoundingBoxMetas[n].Box))
            return true;

        // BoundingBar
        for (int n = 0; n < dest.BoundingBarMetas.Count; n++)
          if (this.BoundingSphereMetas[i].Sphere.Intersects(dest.BoundingBarMetas[n].Box))
            return true;
      }

      // BoundingBox
      for (int i = 0; i < this.BoundingBoxMetas.Count; i++)
      {
        // BoundingSphere
        for (int n = 0; n < dest.BoundingSphereMetas.Count; n++)
          if (this.BoundingBoxMetas[i].Box.Intersects(dest.BoundingSphereMetas[n].Sphere))
            return true;

        // BoundingBox
        for (int n = 0; n < dest.BoundingBoxMetas.Count; n++)
          if (this.BoundingBoxMetas[i].Box.Intersects(dest.BoundingBoxMetas[n].Box))
            return true;

        // BoundingBar
        for (int n = 0; n < dest.BoundingBarMetas.Count; n++)
          if (this.BoundingBoxMetas[i].Box.Intersects(dest.BoundingBarMetas[n].Box))
            return true;
      }

      // BoundingBar
      for (int i = 0; i < this.BoundingBarMetas.Count; i++)
      {
        // BoundingSphere
        for (int n = 0; n < dest.BoundingSphereMetas.Count; n++)
          if (this.BoundingBarMetas[i].Box.Intersects(dest.BoundingSphereMetas[n].Sphere))
            return true;

        // BoundingBox
        for (int n = 0; n < dest.BoundingBoxMetas.Count; n++)
          if (this.BoundingBarMetas[i].Box.Intersects(dest.BoundingBoxMetas[n].Box))
            return true;

        // BoundingBar
        for (int n = 0; n < dest.BoundingBarMetas.Count; n++)
          if (this.BoundingBarMetas[i].Box.Intersects(dest.BoundingBarMetas[n].Box))
            return true;
      }

      return false;
    }

    public void addBoundingShere(float radius, Vector3 offset)
    {
      this.BoundingSphereMetas.Add(new BoundingSphereMeta(new BoundingSphere(this.Position.Coordinate, radius), offset));
    }
    public void addBoundingBox(Vector3 bounds, Vector3 offset)
    {
      this.BoundingBoxMetas.Add(new BoundingBoxMeta(this.Position.Coordinate, bounds, offset));
    }
    public void addBoundingBar(Vector3 boundsLeft, Vector3 boundsRight, Vector3 offset)
    {
      this.BoundingBarMetas.Add(new BoundingBarMeta(this.Position.Coordinate, boundsLeft, boundsRight, offset));
    }

    public virtual void UpdateBoundingObjects()
    {
      for (int i = 0; i < this.BoundingSphereMetas.Count; i++)
        this.BoundingSphereMetas[i].Center = this.Position.Coordinate + Vector3.Transform(this.BoundingSphereMetas[i].Offset, this.Position.Front);
      for (int i = 0; i < this.BoundingBoxMetas.Count; i++)
        this.BoundingBoxMetas[i].Center = this.Position.Coordinate + Vector3.Transform(this.BoundingBoxMetas[i].Offset, this.Position.Front);
      for (int i = 0; i < this.BoundingBarMetas.Count; i++)
        this.BoundingBarMetas[i].Center = this.Position.Coordinate + Vector3.Transform(this.BoundingBarMetas[i].Offset, this.Position.Front);
    }

#if DEBUG && BOUNDRENDER
    /// <summary>Only draws the customized boundingSphere's, NOT the parts</summary>
    public void DrawBoundingObjects()
    {
      for (int i = 0; i < this.BoundingSphereMetas.Count; i++)
        TierGame.BoundingSphereRender.Draw(BoundingSphereMetas[i].Sphere, Color.White);
      for (int i = 0; i < this.BoundingBoxMetas.Count; i++)
        TierGame.BoundingBoxRenderer.Draw(this.BoundingBoxMetas[i], Color.White);
      for (int i = 0; i < this.BoundingBarMetas.Count; i++)
        TierGame.BoundingBoxRenderer.Draw(this.BoundingBarMetas[i], Color.White);
    }
#endif
    #endregion

    private Boolean threadStarted;
    private Boolean threadDone;

    private void WaitThread()
    {
      this.threadStarted = true;
      Thread.Sleep(2500);
      this.threadStarted = false;
      this.threadDone = true;
    }

    public virtual void Explode(BasicObject parent)
    {
      this.Exploded = true;
			//TierGame.Audio.playSFX3D("explosion", this, 2); //Limits the explosion to two at a time (for this events at any case)
			TierGame.GameHandler.Boss.Health -= this.MaxHealth;
			if (TierGame.GameHandler.Boss.Health > this.MaxHealth)
				TierGame.GameHandler.Boss.Health -= this.MaxHealth;
			else
				TierGame.GameHandler.Boss.Health = 1;

      GameHandler.ObjectHandler.AddObject(new ExplosionCluster(this.Game, new Vector3(2, 2, 2), this.Position.Coordinate, 50));


      if (this.GetType() == typeof(Enemy) && !this.threadStarted)
      {
        new Thread(new ThreadStart(this.WaitThread)).Start();
      }
    }
  }
}