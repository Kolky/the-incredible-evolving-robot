using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers.BossBehaviours;

namespace Tier.Source.Handlers
{
  public enum BossBehaviourType
  {
    Idle, 
    BBT_ROTATING, 
    BBT_INSTANTROTATING, 
    BBT_FRENZY,
    BBT_CHARGING, 
    BBT_ENERGYSHIELD, 
    BBT_ROCKETCOMBO, 
    BBT_ROAMING,
    BBT_LASERBEAMBATTLE,
    BBT_COREFIRE
  };

  public class BossBehaviourHandler
  {
    #region Properties
    private Matrix transform;
    private Vector3 translation;
    private BossBehaviourType currentBehaviour;
    private List<BossBehaviour> behaviours;
    private float timeElapsed;
    private TierGame game;
    private Matrix baseTransform;
    private bool isLocked;

    public bool IsLocked
    {
      get { return IsLocked; }
      set { isLocked = value; } 
    }  
   
    public BossBehaviourType CurrentBehaviour
    {
      get { return currentBehaviour; }
      set { currentBehaviour = value; }
    }
	
    public Vector3 Translation
    {
      get { return translation; }
      set { translation = value; }
    }

    public Matrix Transform
    {
      get { return transform; }
      set { transform = value; }
    }
    #endregion

    private int timeToNext;

    public BossBehaviourHandler(TierGame game)
    {
      this.isLocked = false;
      this.transform = this.baseTransform = Matrix.Identity;
      this.currentBehaviour = BossBehaviourType.Idle;
      this.behaviours = new List<BossBehaviour>();
      this.game = game;
      timeElapsed = 0;

      //this.behaviours.Add(new RoamingBehaviour(game, this));
      this.behaviours.Add(new RotatingBehaviour(game, this));
      this.behaviours.Add(new ChargeBehaviour(game, this));
      this.behaviours.Add(new InstantRotatingBehaviour(game, this));
      //this.behaviours.Add(new EnergyShieldBehaviour(game, this));
      this.behaviours.Add(new RocketComboBehaviour(game, this));
      this.behaviours.Add(new FrenzyBehaviour(game, this));
      this.behaviours.Add(new LaserbeamBattleBehaviour(game, this));
      this.behaviours.Add(new CoreFireBehaviour(game, this));

      Reset();
    }

    public void Reset()
    {
      foreach (BossBehaviour beha in this.behaviours)
      {
        beha.Transform = Matrix.Identity;
        beha.Disable();
      }

      //EnableBehaviour(BossBehaviourType.Roaming);
      //EnableBehaviour(BossBehaviourType.Rotating);
      //EnableBehaviour(BossBehaviourType.BBT_COREFIRE);

      DetermineTimeToNext();

      #if !DEBUG
            //EnableBehaviour(BossBehaviourType.Rotating);
      #endif
    }

    public void Draw(GameTime gameTime)
    {
      foreach (BossBehaviour b in this.behaviours)
      {
        b.Draw(gameTime);
      }
    }

    public void DisableBehaviour(BossBehaviourType type)
    {
      foreach (BossBehaviour behaviour in this.behaviours)
      {
        if (behaviour.Type == type)
        {
          behaviour.Disable();
          return;
        }
      }
    }
    
    public void Update(GameTime gameTime)
    {
      this.transform = Matrix.Identity;
      if(!isLocked)
        timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      Matrix translation, rotation = translation = Matrix.Identity;

      foreach (BossBehaviour b in this.behaviours)
      {
        rotation *= Matrix.CreateFromQuaternion(Quaternion.CreateFromRotationMatrix(b.Transform));
        translation *= Matrix.CreateTranslation(b.Transform.Translation);

        if (b.IsEnabled)
        {
          b.Update(gameTime);          
        }
      }

#if !DEBUG
      if (this.timeElapsed >= this.timeToNext && 
        this.game.BossGrowthHandler.Core.DestroyableModifier.Health > 0)
      {
        StartRandomBehaviour();
        this.timeElapsed = 0;
        DetermineTimeToNext();
      }
#endif

      this.transform = rotation * translation;
    }

    private void DetermineTimeToNext()
    {
      this.timeElapsed = 0;
      this.timeToNext = this.game.Random.Next(30000, 60000);
    }

    private void StartRandomBehaviour()
    {
      switch (this.game.Random.Next(3))
      {
        case 0:
          EnableBehaviour(BossBehaviourType.BBT_CHARGING);
          break;
        case 1:
          EnableBehaviour(BossBehaviourType.BBT_FRENZY);
          break;
        case 2:
          EnableBehaviour(BossBehaviourType.BBT_LASERBEAMBATTLE);
          break;
      }

      this.isLocked = true;
    }

    public bool IsBehaviourEnabled(BossBehaviourType type)
    {
      /*foreach (BossBehaviour b in this.behaviours)
      {
        if (b.Type == type)
          return b.IsEnabled;
      }

      return false;*/

      for (int i = 0; i < this.behaviours.Count; i++)
      {
        if (this.behaviours[i].Type == type)
          return this.behaviours[i].IsEnabled;
      }

      return false;
    }

    public void ResetBehaviour(BossBehaviourType type)
    {
      foreach (BossBehaviour b in this.behaviours)
      {
        if (b.Type == type)
        {
          b.Transform = Matrix.Identity;
          return;
        }
      }
    }
    
    public bool GetBehaviour(BossBehaviourType type, out BossBehaviour behaviour)
    {
      behaviour = null;

      foreach (BossBehaviour b in this.behaviours)
      {
        if (b.Type == type)
        {
          behaviour = b;
          return true;
        }
      }

      return false;
    }

    /* TODO: Herschrijven naar ActivateBehaviour. 
      - Alle behaviour moeten van tevoren aanwezig zijn in deze handler
      - In de update kan dan de transform van alle behaviour samengevoegd worden tot een.
      - Behaviour moeten dan een start en stop functionaliteit krijgen
     */
    public void EnableBehaviour(BossBehaviourType type)
    {
      foreach (BossBehaviour behaviour in this.behaviours)
      {
        if (behaviour.Type == type)
        {
          behaviour.Enable();
          return;
        }
      }
    }
  }
}
