using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Tier.Handlers;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects.Attachable
{
  public abstract class Weapon : AttachableObject
  {
    #region Properties
    private float cooldownFire = 15;
    public float CoolDownFire
    {
      set { cooldownFire = value; }
    }
    private float timeSinceLastFire = 0.0f;

    private BasicObject source;
    public BasicObject Source
    {
      get { return source; }
      set { source = value; }
    }

    private int damage;
    public int Damage
    {
      get { return damage; }
      set { damage = value; }
    }	
    #endregion

    public Weapon(Game game, AttachableObject source)
      : this(game, source, false)
    {
    }

    public Weapon(Game game, AttachableObject source, Boolean isCollidable)
			: base(game, isCollidable, source)
		{
			this.Source = source;			
		}

    public override void Update(GameTime gameTime)
    {
      this.timeSinceLastFire += gameTime.ElapsedGameTime.Milliseconds;
      base.Update(gameTime);
    }

    public virtual void Fire()
    {
      this.timeSinceLastFire = 0;
    }

    protected Boolean canFire()
    {
      if (this.timeSinceLastFire > this.cooldownFire && !TierGame.GameHandler.Boss.IsSpawning) 
        return true;

      return false;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Model != null)
      {
        for (int i = 0; i < this.Model.Meshes.Count; i++)
        {
          foreach (BasicEffect effect in this.Model.Meshes[i].Effects)
          {
            effect.World = Matrix.CreateScale(this.Scale) *
              RotationFix *
              Matrix.CreateFromQuaternion(this.Position.Front) *
              this.spawnMatrix *
              Matrix.CreateTranslation(this.Position.Coordinate);
            effect.View = GameHandler.Camera.View;
            effect.Projection = GameHandler.Camera.Projection;
          }

          this.Model.Meshes[i].Draw();
        }
      }
    }

    //sommige guns laten iets zien als ze aimen, denk aan dikke laser van enemy of piramide van speler
    virtual public void Aim(GameTime gameTime) { }
  }
}
