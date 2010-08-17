using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Tier.Source.Handlers;
using pjEngine;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;
using Tier.Source.Helpers.Cameras;
using Tier.Helpers;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Misc;
using Tier.Source.Helpers;
using Tier.Source.Handlers.BossBehaviours;
using Tier.Source.ObjectModifiers;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework.Audio;
using Tier.Source.Objects.Turrets;
using Tier.Source.Objects.Powerups;
using Tier.Source.Objects.PlayerWeapons;

namespace Tier.Source.GameStates
{
  public class MainGameState : GameState
  {
    #region Properties
    private MainGameStateStatus status;

    public MainGameStateStatus Status
    {
      get { return status; }
      set { status = value; }
    }  
#if Windows
    private MouseState previousMouseState;
#else
    private GamePadState previousGamepadState;
#endif        
    public SkyBox skybox;   
    #endregion

    #region Privates
    private float timeElapsed;    
    private Text waitingStatusText;    
    private DelayCamera cam;    
    #endregion

    public enum MainGameStateStatus
    {
      WAITING, RUNNING
    };

    public MainGameState(TierGame game) : base(game)
    {
      Initialize();
      
      this.Game.BossGrowthPatternHandler = new BossGrowthPatternHandler();
      // Create and initialize BossGrowthHandler
      this.Game.BossGrowthHandler = new BossGrowthHandler(this.Game);      

      this.game.BossPieceTemplateHandler.LoadFromXml("Default");

      this.waitingStatusText = this.game.TextSpriteHandler.CreateText(
        "",
        new Vector2(this.game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, 
        this.game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2),
        Color.White);
      this.waitingStatusText.Scale = 2;
      this.game.TextSpriteHandler.RemoveText(this.waitingStatusText);
    }

    public override void Enter(GameState previousState)
    {
      base.Enter(previousState);

      if (this.cam == null)
      {
        // Setup player cam
        this.cam = new DelayCamera(
        new Vector3(0, 0, 40),
        Vector3.Zero,
        this.game.GameHandler.Player,
        this.game.Options.DelayCamera_DefaultOffset,
        game);
      }

      if (previousState == null ||
        previousState.GetType() == typeof(TitleScreenState))
      {
        // Clear previous boss composition
        this.game.BossCompositionHandler.Reset();
        this.game.BossGrowthHandler.Start();
        this.game.GameHandler.CurrentLevel = 1;
        this.game.GameHandler.Player.DestroyableModifier.Health = 100;

        BossPiece core = this.game.BossGrowthHandler.Core;

        // Attach default weapons
        for(int i=0;i<4;i++)
        {
          Turret t = null;
          
          switch(this.game.Random.Next(3))
          {
            case 0:
              t = new ShotgunTurret(this.game);
              this.game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "ShotgunTurret");
              break;
            case 1:
              t = new PlasmaTurret(this.game);
              this.game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "PlasmaTurret");
              break;
            case 2:
              t = new LaserBeamTurret(this.game);
              this.game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "LaserBeamTurret");
              break;
          }
         
          if(core.AttachableModifier.Attach(t, i + 6, 0))
            this.game.GameHandler.AddObject(t);
        }

        this.game.GameHandler.Player.SwitchWeapon(new PlayerPlasmaWeapon(this.game));
        this.game.ScoreHandler.Reset();
        this.game.ScoreHandler.Start();

        this.game.InterfaceHandler.Show(InterfaceComponent.IC_WEAPON_STATUS);
        
        /*
        #if DEBUG
          this.game.GameHandler.Player.DestroyableModifier.Health = 100;
          for (int i = 0; i < 250; i++)
          {
            int count = 1;
            core.GrowableModifier.Grow(ref count, core);
          }
          this.game.GameHandler.CurrentLevel = 10;
        #endif
         */
      }

      // Skybox
      if (this.skybox == null)
      {
        this.skybox = new SkyBox(this.game);
        this.game.ObjectHandler.InitializeFromBlueprint<SkyBox>(this.skybox, "Skybox");
        this.game.GameHandler.AddObject(this.skybox);
      }

      if(!this.game.BossGrowthHandler.IsInitialized)
        this.Game.BossGrowthHandler.Initialize();

      this.timeElapsed = 0;                
      
#if DEBUG
      this.status = MainGameStateStatus.RUNNING;       
#else
      this.status = MainGameStateStatus.WAITING;


#endif

      // Reset camera
      this.game.GameHandler.Camera = cam;
      this.cam.Position = new Vector3(0, 0, 40);
      this.cam.Target = Vector3.Zero;      
      // Reset player
      this.game.GameHandler.Player.Rotation = Quaternion.Identity;
      this.game.GameHandler.Player.Distance = 40;
      this.game.GameHandler.Player.IsVisible = true;      
      this.game.GameHandler.IsBossDestroyed = false;      
      // Add the player to the GameHandler
      this.game.GameHandler.Players.Clear();
      this.game.GameHandler.Players.Add(this.game.GameHandler.Player);

      this.game.TextSpriteHandler.AddText(this.waitingStatusText);

      this.Game.SoundHandler.Play("IngameMusic", true);

      this.game.BehaviourHandler.Reset();

      // Show interface
      this.game.InterfaceHandler.Show();
      //this.game.GameHandler.Player.SwitchWeapon(new PlayerWeaponLaserbeam(this.game));
      //this.game.GameHandler.Player.SwitchWeapon(new PlayerWeaponRocketLauncher(this.game));
    }

    public override void Leave()
    {
      this.game.Speed = 1;
      this.game.InterfaceHandler.Hide();
      this.game.TextSpriteHandler.RemoveText(this.waitingStatusText);
      this.game.ScoreHandler.Stop();      
    }

    public override void Update(GameTime gameTime)
    {      
      if (GamePad.GetState(this.game.MainControllerIndex).Buttons.Back == ButtonState.Pressed)
      {
        this.game.ChangeState(this.game.TitleScreenState);
        return;
      }

      UpdateAll(gameTime);

      switch (status)
      {
        case MainGameStateStatus.WAITING:
          UpdateWaiting(gameTime);
          break;
        case MainGameStateStatus.RUNNING:
          UpdateRunning(gameTime);
          break;
      }
    }

    private void UpdateAll(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.game.GameHandler.Player.Update(gameTime);
      this.Game.GameHandler.Update(gameTime);                    

      if (!this.game.GameHandler.Player.DestroyableModifier.IsDestroyed &&
          !this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_CHARGING))
      {
#if Windows        
        handleMouse(gameTime);
#else
        handleKeyboard(gameTime);
        handleController(gameTime);
#endif
      }
    }

    private void UpdateWaiting(GameTime gameTime)
    {
      if (this.timeElapsed < 1000.0f)
      {
        this.waitingStatusText.Value = "3";
        this.waitingStatusText.Color = Color.Red;
      }
      else if (this.timeElapsed < 2000.0f)
      {
        this.waitingStatusText.Value = "2";
        this.waitingStatusText.Color = Color.Orange;
      }
      else
      {
        this.waitingStatusText.Color = Color.GreenYellow;
        this.waitingStatusText.Value = "1";
      }

      if (this.timeElapsed >= 3000.0f)
      {
        this.status = MainGameStateStatus.RUNNING;
        this.game.TextSpriteHandler.RemoveText(this.waitingStatusText);
      }
    }

    private void UpdateRunning(GameTime gameTime)
    {
      this.game.BehaviourHandler.Update(gameTime);
      this.game.ScoreHandler.Update(gameTime);
      this.game.ProjectileHandler.Update(gameTime);

      this.game.GameHandler.DetermineIfPlayerIsNearPowerup();

      if (this.game.GameHandler.Player.DestroyableModifier.IsDestroyed &&
        this.game.GameHandler.Player.DestroyableModifier.ExplosionHandler.IsDone)
      {
        this.game.GameHandler.Player.DestroyableModifier.Reset();
        this.game.ChangeState(this.game.TitleScreenState);
      }

      if (
        !this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_LASERBEAMBATTLE) &&
        !this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_ROCKETCOMBO) &&
        !this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_CHARGING))          
      {
        this.game.GameHandler.Player.DestroyableModifier.Health -=
          this.game.Options.PlayerHealthLossPerSecond * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);  
      }
    }

    private void handleController(GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);

      // Shoot
      if (state.Triggers.Right > 0 && 
        this.status == MainGameStateStatus.RUNNING &&
        !this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_LASERBEAMBATTLE))
      {
        this.game.GameHandler.Player.Shoot();
      }

      previousGamepadState = state;
    }

    private void handleKeyboard(GameTime gameTime)
    {
#if WINDOWS && DEBUG
      KeyboardState kbs = Keyboard.GetState();

      if (kbs.IsKeyDown(Keys.PageUp) && this.previousKeyboardState.IsKeyUp(Keys.PageUp))
      {
        this.game.GameHandler.ShowNextBoss();               
      }

      if (kbs.IsKeyDown(Keys.D1) && this.previousKeyboardState.IsKeyUp(Keys.D1))
      {
        this.game.BehaviourHandler.EnableBehaviour(BossBehaviourType.BBT_ROTATING);
      }

      if (kbs.IsKeyDown(Keys.D2) && this.previousKeyboardState.IsKeyUp(Keys.D2))
      {
        this.game.BehaviourHandler.EnableBehaviour(BossBehaviourType.BBT_CHARGING);
      }

      if (kbs.IsKeyDown(Keys.D3) && this.previousKeyboardState.IsKeyUp(Keys.D3))
      {
        this.game.BehaviourHandler.EnableBehaviour(BossBehaviourType.BBT_FRENZY);
      }

      if (kbs.IsKeyDown(Keys.D4) && this.previousKeyboardState.IsKeyUp(Keys.D4))
      {
        this.game.BehaviourHandler.EnableBehaviour(BossBehaviourType.BBT_LASERBEAMBATTLE);
      }

      if (kbs.IsKeyDown(Keys.I) && this.previousKeyboardState.IsKeyUp(Keys.I))
      {
        this.game.GameHandler.Player.PlayerWeapon.Upgrade();
        this.game.GameHandler.Player.PlayerWeapon.Upgrade();
        this.game.GameHandler.Player.PlayerWeapon.Upgrade();
        this.game.GameHandler.Player.PlayerWeapon.Upgrade();
        this.game.GameHandler.Player.PlayerWeapon.Upgrade();
      }

      previousKeyboardState = kbs;
#endif
    }
  }
}