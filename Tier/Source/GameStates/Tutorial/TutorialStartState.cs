using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Helpers.Cameras;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Handlers;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.GameStates.Tutorial
{
  /// <summary>
  /// First state in tutorial.
  /// Introduces player to the game by:
  /// - Showing how the player controls his/her ship
  /// - Showing how the aiming is done
  /// </summary>
  public class TutorialStartState : GameState
  {
    #region Properties
    private DelayCamera camera;
    private TutorialTextHandler textHandler;
    private int timeElapsed;
    private enum TutorialStartStateStatus
    {
      TSSS_START, TSSS_MOVE, TSSS_AIM, TSSS_DONE, TSSS_FIRE
    }
    private TutorialStartStateStatus status;
    private SkyBox skybox;
    private int timePlayerMoved;
    private int timePlayerAimed;
    private int timeMoveStateWait;
    private int timePlayerFired;
    #endregion

    public TutorialStartState(TierGame game) :
      base(game)
    {
      camera = new DelayCamera(
        new Vector3(0, 0, 40),
        Vector3.Zero,
        this.game.GameHandler.Player,
        this.game.Options.DelayCamera_DefaultOffset,
        game);

      this.status = TutorialStartStateStatus.TSSS_START;
      textHandler = new TutorialTextHandler(game);
    }

    private void ChangeState(TutorialStartStateStatus newstatus)
    {
      switch (newstatus)
      {
        case TutorialStartStateStatus.TSSS_START:
          ChangeStateStart();
          break;
        case TutorialStartStateStatus.TSSS_MOVE:
          ChangeStateMove();
          break;
        case TutorialStartStateStatus.TSSS_AIM:
          ChangeStateAim();
          break;
      }

      this.status = newstatus;
    }

    private void ChangeStateAim()
    {
      this.game.InterfaceHandler.Show(InterfaceComponent.IC_CROSSHAIR);
    }
    
    private void ChangeStateMove()
    {
      this.game.GameHandler.Player.IsVisible = true;
    }

    private void ChangeStateStart()
    {
      timeMoveStateWait = timePlayerAimed = timePlayerMoved = timePlayerFired = 0;
      this.game.GameHandler.Player.IsVisible = false;
      this.game.GameHandler.Camera = camera;

      skybox = new SkyBox(this.game);
      if (this.game.ObjectHandler.InitializeFromBlueprint<SkyBox>(skybox, "Skybox"))
      {
        this.game.GameHandler.AddObject(skybox);
      }

      this.textHandler.AddText("Welcome to The Incredible Evolving Robot (T.I.E.R.)!", 2000);
      this.textHandler.AddText("This tutorial will help you get started in the game.", 2000);
      this.textHandler.AddText("Let's start with the controls!", 1000);
      this.textHandler.AddText("To move around use the left thumbstick.", 100, true);      
      this.textHandler.AddText("Great! Now for the aiming.", 1000);
      this.textHandler.AddText("To aim use the right thumbstick.", 100, true);
      this.textHandler.AddText("Shooting is done pulling the right trigger.", 3000);
      this.textHandler.AddText("Fire!", 100, true);
    }

    public override void Enter(GameState previousState)
    {
      this.game.SoundHandler.Play("TutorialLoop", true);
      ChangeState(TutorialStartStateStatus.TSSS_START);
    }

    public override void Leave()
    {
      this.game.InterfaceHandler.Hide();
      this.game.GameHandler.RemoveObject(skybox, GameHandler.ObjectType.Skybox);
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

      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.textHandler.Update(gameTime);
      this.game.GameHandler.Update(gameTime);
      this.game.GameHandler.Player.Update(gameTime);
      this.game.ProjectileHandler.Update(gameTime);

      switch (this.status)
      {
        case TutorialStartStateStatus.TSSS_START:
          UpdateStart(gameTime);
          break;
        case TutorialStartStateStatus.TSSS_MOVE:
          UpdateMove(gameTime);
          break;
        case TutorialStartStateStatus.TSSS_AIM:
          UpdateAim(gameTime);
          break;
        case TutorialStartStateStatus.TSSS_DONE:
          if (this.timeElapsed >= 2000)
          {
            // Done with this state
            this.game.ChangeState(new TutorialInterfaceState(this.game));
          }
          break;
        case TutorialStartStateStatus.TSSS_FIRE:
          UpdateFire(gameTime);
          break;
      }      
    }

    private void UpdateStart(GameTime gameTime)
    {
      if (this.timeElapsed >= 10000)
      {
        ChangeState(TutorialStartStateStatus.TSSS_MOVE);
      }
    }

    private void UpdateMove(GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
      if (state.ThumbSticks.Left != Vector2.Zero)
      {
        this.timePlayerMoved += gameTime.ElapsedGameTime.Milliseconds;
      }

      if (this.timePlayerMoved >= 3000)
      {
        timeMoveStateWait += gameTime.ElapsedGameTime.Milliseconds;
        this.textHandler.UnstickCurrentText();        
      }
      if (timeMoveStateWait >= 1000)
      {
        ChangeState(TutorialStartStateStatus.TSSS_AIM);
      }
    }

    private void UpdateAim(GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
      if (state.ThumbSticks.Right != Vector2.Zero)
      {
        this.timePlayerAimed += gameTime.ElapsedGameTime.Milliseconds;
      }

      if (this.timePlayerAimed >= 3000)
      {
        this.textHandler.UnstickCurrentText();
        timeElapsed = 0;
        ChangeState(TutorialStartStateStatus.TSSS_FIRE);
      }
    }

    private void UpdateFire(GameTime gameTime)
    {
      // Shoot
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
      if (state.Triggers.Right > 0)
      {
        this.game.GameHandler.Player.Shoot();
        this.timePlayerFired += gameTime.ElapsedGameTime.Milliseconds;
      }

      if (this.timePlayerFired >= 3000)
      {
        timeElapsed = 0;
        this.textHandler.UnstickCurrentText();
        ChangeState(TutorialStartStateStatus.TSSS_DONE);
      }
    }
  }
}