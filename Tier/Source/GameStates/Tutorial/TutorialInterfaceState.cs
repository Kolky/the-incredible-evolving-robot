using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Handlers;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers.Cameras;
using Tier.Source.Objects;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.GameStates.Tutorial
{
  public class TutorialInterfaceState : GameState
  {
    #region Properties
    private int timeSinceVisualScore;
    private int timeSinceWeaponLevel;
    private TutorialTextHandler textHandler;
    private int timeElapsed;
    private DelayCamera camera;
    private SkyBox skybox;
    private enum TutorialInterfaceStatus
    {
      TIS_HEALTHBAR, TIS_SCORE, TIS_WEAPON_STATUS
    };
    private TutorialInterfaceStatus status;
    #endregion

    public TutorialInterfaceState(TierGame game)
      : base(game)
    {
      this.camera = new DelayCamera(
        Vector3.Zero,
        Vector3.Zero,
        this.game.GameHandler.Player,
        this.game.Options.DelayCamera_DefaultOffset,
        game);
      this.textHandler = new TutorialTextHandler(game);
    }

    private void ChangeStatus(TutorialInterfaceStatus newstatus)
    {
      switch (newstatus)
      {
        case TutorialInterfaceStatus.TIS_HEALTHBAR:
          ChangeStatusHealthBar();
          break;
        case TutorialInterfaceStatus.TIS_SCORE:
          ChangeStatusScore();
          break;
        case TutorialInterfaceStatus.TIS_WEAPON_STATUS:
          ChangeStatusWeaponStatus();
          break;
      }

      this.timeElapsed = 0;
      this.status = newstatus;
    }

    private void ChangeStatusHealthBar()
    {
      this.game.InterfaceHandler.Show(InterfaceComponent.IC_CROSSHAIR);

      this.textHandler.AddText("The next part of this tutorial will explain the interface", 2000);
      this.textHandler.AddText("The top of the screen will contain the healthbar", 2000);
      this.textHandler.AddText("It will slowly drain itself", 5000);
      this.textHandler.AddText("When it reaches zero, the game is over!", 3000);
    }

    private void ChangeStatusScore()
    {
      this.game.GameHandler.Player.DestroyableModifier.Health = 100;

      this.textHandler.AddText("The player's score is represented under the healthbar", 2000);
      this.textHandler.AddText("Whenever points are gained it is visualized this way", 2000);      
    }

    private void ChangeStatusWeaponStatus()
    {
      this.textHandler.AddText("The weaponstatus is the last interface component.", 2000);
      this.textHandler.AddText("It is represented in the lowerleft corner.", 2000);
      this.textHandler.AddText("Weapon level is raised by picking up powerups.", 2000);
      this.textHandler.AddText("For each powerup a green node will appear.", 3000);
      this.textHandler.AddText("Weapons unlock new abilities each level.", 3000);
    }
    
    public override void Enter(GameState previousState)
    {
      timeSinceWeaponLevel = timeSinceVisualScore = 0;
      skybox = new SkyBox(this.game);
      if (this.game.ObjectHandler.InitializeFromBlueprint<SkyBox>(skybox, "Skybox"))
      {
        this.game.GameHandler.AddObject(skybox);
      }

      this.game.GameHandler.Camera = this.camera;
      this.camera.BypassDelay();
      this.game.GameHandler.Player.IsVisible = true;
      this.game.GameHandler.Player.DestroyableModifier.Health = 100;
      
      ChangeStatus(TutorialInterfaceStatus.TIS_HEALTHBAR);
    }

    public override void Leave()    
    {
      this.game.GameHandler.RemoveObject(skybox, GameHandler.ObjectType.Skybox);
      this.game.InterfaceHandler.Hide();
      this.textHandler.Stop();
    }

    public override void Update(GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
      if (state.Buttons.Back == ButtonState.Pressed)
      {
        this.game.ChangeState(this.game.TitleScreenState);
        return;
      }

      this.game.GameHandler.Update(gameTime);
      this.game.GameHandler.Player.Update(gameTime);
      this.game.ProjectileHandler.Update(gameTime);
      this.textHandler.Update(gameTime);
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (status)
      {
        case TutorialInterfaceStatus.TIS_HEALTHBAR:
          UpdateHealthbar(gameTime);
          break;
        case TutorialInterfaceStatus.TIS_SCORE:
          UpdateScore(gameTime);
          break;
        case TutorialInterfaceStatus.TIS_WEAPON_STATUS:
          UpdateWeaponStatus(gameTime);
          break;
      }
    }

    private void UpdateHealthbar(GameTime gameTime)
    {
      if (this.timeElapsed >= 3500)
      {
        this.game.InterfaceHandler.Show(InterfaceComponent.IC_HEALTHBAR);
      }

      if (timeElapsed >= 17000)
      {
        ChangeStatus(TutorialInterfaceStatus.TIS_SCORE);
      }
      if (timeElapsed >= 13000)
      {
        this.game.GameHandler.Player.DestroyableModifier.Reset();
      }
      else if (timeElapsed >= 7000)
      {
        this.game.GameHandler.Player.DestroyableModifier.IsHit(
          1.0f * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
      }
    }

    private void UpdateScore(GameTime gameTime)
    {
      if (timeElapsed >= 8000)
      {
        ChangeStatus(TutorialInterfaceStatus.TIS_WEAPON_STATUS);        
      }
      else if (timeElapsed >= 4000)
      {
        this.game.ScoreHandler.Update(gameTime);
        timeSinceVisualScore += gameTime.ElapsedGameTime.Milliseconds;        

        if (timeSinceVisualScore >= 1000)
        {
          timeSinceVisualScore = 0;
          VisualScore score = new VisualScore(
            String.Format("Tutorial score! {0}", 1000),
            new Vector2(this.game.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.25f,
                        this.game.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.33f),
            Color.White,
            this.game);
          score.Score = 1000;
          this.game.ScoreHandler.AddVisualScore(score);
        }
      }
      else if (timeElapsed >= 500)
      {
        if(!this.game.ScoreHandler.IsVisible)
          this.game.InterfaceHandler.Show(InterfaceComponent.IC_SCORE);
      }
    }

    private void UpdateWeaponStatus(GameTime gameTime)
    {
      this.game.ScoreHandler.Update(gameTime);

      if (timeElapsed >= 25000)
      {
        // done
        this.game.ChangeState(new TutorialBossState(this.game));
      }
      else if (timeElapsed >= 9000)
      {
        timeSinceWeaponLevel += gameTime.ElapsedGameTime.Milliseconds;

        if (timeSinceWeaponLevel >= 500)
        {
          timeSinceWeaponLevel = 0;
          this.game.GameHandler.Player.PlayerWeapon.Upgrade();
        }
      }
      else if (timeElapsed >= 3500)
      {
        this.game.InterfaceHandler.Show(InterfaceComponent.IC_WEAPON_STATUS);
      }
    }
  }
}