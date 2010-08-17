using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using BloomPostprocess;
using Tier.Source.Objects;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class FrenzyBehaviour : BossBehaviour
  {
    #region Properties
    private Vector3 axis;
    private float angle;
    private float duration;
    private float fireMultiplier;

    public float FireMultiplier
    {
      get { return fireMultiplier; }
      set { fireMultiplier = value; }
    }
	
    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    public Vector3 RotationAxis
    {
      get { return axis; }
      set { axis = value; }
    }

    public float Angle
    {
      get { return angle; }
      set { angle = value; }
    }
    #endregion

    private enum FrenzyBehaviourState
    {
      FBS_STARTING, FBS_SHOOTING, FBS_STOPPING
    };
    
    private FrenzyBehaviourState state;
    private float timeElapsed, wantedFireMultiplier;
    private Matrix transformAfterShooting;

    public FrenzyBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_FRENZY)
    {
      int ttl = 1000;

      this.TTL = ttl; this.Duration = ttl;      
    }

    private void ChangeState(FrenzyBehaviourState newstate)
    {
      this.timeElapsed = 0;

      switch (newstate)
      {
        case FrenzyBehaviourState.FBS_STARTING:
          {
            this.IsEnabled = true;
            this.Transform = Matrix.Identity;
            // Determine wanted fire multiplier according to level
            this.wantedFireMultiplier =
              this.game.Options.Frenzy_FireStartMultiplier +
              (this.game.Options.Frenzy_FireMultiplier *
              this.game.GameHandler.CurrentLevel);
            DetermineRotation();

            List<GameObject> objects = null;
            this.game.GameHandler.GetObjects(out objects, GameHandler.ObjectType.DefaultTextured);
            if (objects != null)
            {
              foreach (GameObject obj in objects)
              {
                if (obj.GetType() == typeof(BossPiece))
                {
                  ((BossPiece)obj).SetBloomTechnique();
                }
              }
            }
          }
          break;
        case FrenzyBehaviourState.FBS_SHOOTING:
          break;
        case FrenzyBehaviourState.FBS_STOPPING:
          {
            List<GameObject> objects = null;
            this.game.GameHandler.GetObjects(out objects, GameHandler.ObjectType.DefaultTextured);
            if (objects != null)
            {
              foreach (GameObject obj in objects)
              {
                if (obj.GetType() == typeof(BossPiece))
                {
                  ((BossPiece)obj).SetDefaultTechnique();
                }
              }
            }
            transformAfterShooting = this.Transform;
          }
          break;
      }

      this.state = newstate;
    }
    
    public override void Disable()
    {
      base.Disable();
      this.handler.IsLocked = false;
      // Reset base saturation to normal
      this.game.BloomComponent.Settings.BaseSaturation = 1;

      ChangeState(FrenzyBehaviourState.FBS_STOPPING);
    }

    private void DetermineRotation()
    {
      this.Angle = MathHelper.Pi * 2;
      
      switch (this.game.Random.Next(3))
      {
        case 0:
          this.RotationAxis = Vector3.Up;
          break;
        case 1:
          this.RotationAxis = Vector3.Left;
          break;
        case 2:
          this.RotationAxis = Vector3.Forward;
          break;
      }
    }

    public override void Enable()
    {
      ChangeState(FrenzyBehaviourState.FBS_STARTING);
    }

    public override void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (this.state)
      {
        case FrenzyBehaviourState.FBS_STARTING:
          if (this.timeElapsed >= this.game.Options.Frenzy_TimeToTurnRed)
          {
            ChangeState(FrenzyBehaviourState.FBS_SHOOTING);
          }

          this.fireMultiplier = 1.0f;
          this.game.BloomComponent.Settings.BaseSaturation =
            1.0f - 1.0f * (timeElapsed / this.game.Options.Frenzy_TimeToTurnRed);

          break;
        case FrenzyBehaviourState.FBS_SHOOTING:
          if (this.timeElapsed >= this.game.Options.Frenzy_TimeToShoot)
          {
            ChangeState(FrenzyBehaviourState.FBS_STOPPING);
          }

          // During this phase increase the fire speed according to time
          this.fireMultiplier =  1.0f + 
            wantedFireMultiplier * this.timeElapsed / this.game.Options.Frenzy_TimeToShoot;
          this.game.BloomComponent.Settings.BaseSaturation = 0;
          this.Transform *=
            Matrix.CreateFromAxisAngle(axis, angle * (gameTime.ElapsedGameTime.Milliseconds / duration));
          break;
        case FrenzyBehaviourState.FBS_STOPPING:
          if (this.timeElapsed >= this.game.Options.Frenzy_TimeToTurnRed)
          {
            this.Transform = Matrix.Identity;
            // Reset base saturation to normal
            this.game.BloomComponent.Settings.BaseSaturation = 1;
            Disable();
          }

          // Return to identity matrix
          this.Transform = Matrix.Lerp(transformAfterShooting, Matrix.Identity,
            this.timeElapsed / this.game.Options.Frenzy_TimeToTurnRed);

          break;
      }
    }
  }
}
