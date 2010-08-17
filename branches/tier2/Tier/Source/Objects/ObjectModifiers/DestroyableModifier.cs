using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Handlers;
using Tier.Source.Misc;

namespace Tier.Source.ObjectModifiers
{
  public class DestroyableModifier : ObjectModifier
  {
    #region Properties
    private float health;    
    private int fuseTime;
    private bool isDestroyed;
    private int elapsedMillis;
    private const int FUSETIME = 250;
    private DestroyableModifierState state;
    private float startHealth;
    private ExplosionHandler explosionHandler;

    public ExplosionHandler ExplosionHandler
    {
      get { return explosionHandler; }
      set { explosionHandler = value; }
    }
	
    public float StartHealth
    {
      get { return startHealth; }
      set { startHealth = value; }
    }
	
    public bool IsDestroyed
    {
      get { return isDestroyed; }
      set { isDestroyed = value; }
    }
	
    public float Health
    {
      get { return health; }
      set { health = value; }
    }
    #endregion

    private enum DestroyableModifierState
    {
      Destroyed, Normal
    };

    public DestroyableModifier(GameObject parent)
      : base(parent)
    {
      parent.DestroyableModifier = this;
      this.state = DestroyableModifierState.Normal;      
    }

    public void Reset()
    {
      this.IsDestroyed = false;      
      this.fuseTime = 0;
      this.elapsedMillis = 0;
      this.state = DestroyableModifierState.Normal;
      this.Parent.IsVisible = true;

      if (this.explosionHandler != null)
      {
        this.explosionHandler.Dispose();
        this.explosionHandler = null;
      }

      this.health = 1;
    }

    public void IsHit(float damage)
    {
      this.Parent.Game.GameHandler.AddDamagedObject(this.Parent, damage);      
    }

    public override void Update(GameTime gameTime)
    {
      switch (this.state)
      {
        // Parent object is destroyed, spawn an Explosion at the predetermined time
        case DestroyableModifierState.Destroyed:
          if (this.elapsedMillis < this.fuseTime)
          {
            this.elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;            
          }
          else if (this.elapsedMillis >= this.fuseTime && !this.isDestroyed)
          {
            // Spawn powerup(s)
            if (this.Parent.GetType() == typeof(BossPiece) &&
              !((BossPiece)this.Parent).IsCore)            
            {
              PowerupSpawner.Spawn(this.Parent.Position, this.Parent.Game);
            } 

            this.isDestroyed = true;
            // When this object is exploded in a combo, notify player of it
            this.explosionHandler.ShowCombo(this.Parent.Game.Options.ComboTimeOnScreen);
            // Spawn a billboard with the explosion
            Billboard b = new Billboard(this.Parent.Game, BillboardType.AnimatedTexture);
            this.Parent.Game.ObjectHandler.InitializeFromBlueprint<Billboard>(b, "Explosion");
            b.Scale = new Vector3(5);
            b.Effect.CurrentTechnique = b.Effect.Techniques["AlphaBlendNonColorOverlay"];
            b.Position = Vector3.Transform(this.Parent.Position, this.Parent.Game.BehaviourHandler.Transform);
            this.Parent.Game.GameHandler.AddObject(b);

            // Play sound
            this.Parent.Game.SoundHandler.Play("Explosion");
          }
          break;
        case DestroyableModifierState.Normal:
          if (this.health <= 0)
          {
            if (this.explosionHandler == null)
            {
              this.explosionHandler = new ExplosionHandler(this);
              this.explosionHandler.Start();
              this.Parent.Game.GameHandler.AddExplosionHandler(this.explosionHandler);
            }

            Explode(this.Parent, 0);
            
            // Subtract a little bit of health from the parent object
            if (this.Parent.AttachableModifier != null)
            {
              foreach (Connector conn in this.Parent.AttachableModifier.Connectors)
              {                
                if (
                  conn.ConnectedTo != null &&
                  conn.ConnectedTo.DestroyableModifier != null &&
                  conn.ConnectedTo.AttachableModifier.Level < this.Parent.AttachableModifier.Level)
                {                  
                  conn.ConnectedTo.DestroyableModifier.Health -= this.startHealth * 0.5f;
                }
              }
            }
          }
          break;
      }      
    }

    /// <summary>
    /// The parent object, along with all objects attached will be destroyed.
    /// </summary>
    public void Explode(GameObject parent, int fuseTime)
    {      
      // Check whetever there are objects attached to this one. Explode them if they are.
      if (this.Parent.AttachableModifier != null)
      {
        int subFuseTime = fuseTime;
        foreach (Connector conn in this.Parent.AttachableModifier.Connectors)
        {
          if (  conn.ConnectedTo != null && 
                conn.ConnectedTo != parent && 
                conn.ConnectedTo.AttachableModifier.Level > this.Parent.AttachableModifier.Level)
          {
            subFuseTime += FUSETIME;
            conn.ConnectedTo.DestroyableModifier.ExplosionHandler = this.ExplosionHandler;            
            conn.ConnectedTo.DestroyableModifier.Explode(conn.Parent, subFuseTime);
          }
        }
      }

      this.Parent.IsVisible = false;
      this.elapsedMillis = 0; this.state = DestroyableModifierState.Destroyed;      
      this.fuseTime = fuseTime;      
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new DestroyableModifier(parent);
      ((DestroyableModifier)objmod).Health = this.Health;
      ((DestroyableModifier)objmod).IsDestroyed = false;
    }
  }
}