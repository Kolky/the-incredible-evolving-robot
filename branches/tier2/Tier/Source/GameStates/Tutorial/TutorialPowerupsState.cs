using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;
using Tier.Source.Handlers;
using Tier.Source.Helpers.Cameras;
using Tier.Source.Helpers;
using Tier.Source.Objects.Powerups;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.GameStates.Tutorial
{
  public class TutorialPowerupsState : GameState
  {
    #region Properties
    private SkyBox skybox;
    private DelayCamera camera;
    private TutorialTextHandler textHandler;
    private int timeElapsed;
    private enum TutorialPowerupsStateStatus
    {
      TPSS_INTRO, TPSS_GET_POWERUP, TPSS_SHOW_POWERUPS
    };
    private TutorialPowerupsStateStatus status;
    private bool isPowerupsSpawned;

    #endregion

    private Powerup p1, p2, p3, p4, p5;

    public TutorialPowerupsState(TierGame game)
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

    private void ChangeState(TutorialPowerupsStateStatus newstatus)
    {
      switch (newstatus)
      {
        case TutorialPowerupsStateStatus.TPSS_INTRO:
          this.textHandler.AddText("Powerups drop from destroyed pieces of T.I.E.R.", 3000);
          this.textHandler.AddText("Powerups upgrade health or weapons.", 2500);
          this.textHandler.AddText("The powerups remain ingame for a limited time only.", 2000);          
          break;
        case TutorialPowerupsStateStatus.TPSS_GET_POWERUP:
          PowerupSpawner.Spawn(Vector3.Zero, this.game);        
          this.textHandler.AddText("When the player is near a powerup, the camera zooms out.", 2000);
          this.textHandler.AddText("They spawn like this..", 2000);
          this.textHandler.AddText("Get three powerups!", 1, true);
          break;
        case TutorialPowerupsStateStatus.TPSS_SHOW_POWERUPS:
          RemovePowerups();
          this.textHandler.AddText("To conclude, here is an overview of the powerups.", 2000);
          this.textHandler.AddText("The color of the ring determines the rarity.", 2000);
          this.textHandler.AddText("The powerups are ordered in rarity, from left to right.", 2000);
          this.textHandler.AddText("This concludes the tutorial. Press back to stop.", 1, true);
          break;
      }

      this.status = newstatus;
      this.timeElapsed = 0;
    }

    private void CreatePowerups()
    {
      {
        p1 = new PowerupHealth(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Powerup>(p1, "PowerupHealth"))
        {
          p1.Position = new Vector3(-30,0, 20);
          p1.RemoveObjectModifier(p1.TemporaryModifier);
          p1.ChangeState(PowerupState.PS_SPAWNED_LOCKED);
          this.game.GameHandler.AddObject(p1);
        }
      }

      {
        p2 = new PowerupWeaponUpgrade(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Powerup>(p2, "PowerupWeaponUpgrade"))
        {
          p2.Position = new Vector3(-15, 0, 20);
          p2.RemoveObjectModifier(p2.TemporaryModifier);
          p2.ChangeState(PowerupState.PS_SPAWNED_LOCKED);
          this.game.GameHandler.AddObject(p2);
        }
      }

      {
        p3 = new PowerupQuad(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Powerup>(p3, "PowerupQuad"))
        {
          p3.Position = new Vector3(0, 0, 20);
          p3.RemoveObjectModifier(p3.TemporaryModifier);
          p3.ChangeState(PowerupState.PS_SPAWNED_LOCKED);

          this.game.GameHandler.AddObject(p3);
        }
      }

      {
        p4 = new PowerupRocket(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Powerup>(p4, "PowerupRocket"))
        {
          p4.Position = new Vector3(15, 0, 20);
          p4.RemoveObjectModifier(p4.TemporaryModifier);
          p4.ChangeState(PowerupState.PS_SPAWNED_LOCKED);

          this.game.GameHandler.AddObject(p4);
        }
      }

      {
        p5 = new PowerupLaser(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Powerup>(p5, "PowerupLaser"))
        {
          p5.Position = new Vector3(30, 0, 20);
          p5.RemoveObjectModifier(p5.TemporaryModifier);
          p5.ChangeState(PowerupState.PS_SPAWNED_LOCKED);

          this.game.GameHandler.AddObject(p5);
        }
      }
    }

    public override void Enter(GameState previousState)
    {
      skybox = new SkyBox(this.game);
      if (this.game.ObjectHandler.InitializeFromBlueprint<SkyBox>(skybox, "Skybox"))
      {
        this.game.GameHandler.AddObject(skybox);
      }

      isPowerupsSpawned = false;

      this.game.GameHandler.Camera = this.camera;
      this.game.GameHandler.Player.IsVisible = true;
      this.game.InterfaceHandler.Show();

      ChangeState(TutorialPowerupsStateStatus.TPSS_INTRO);
    }

    public override void Leave()
    {
      this.game.InterfaceHandler.Hide();
      this.game.GameHandler.RemoveObject(skybox, GameHandler.ObjectType.Skybox);
      this.textHandler.Stop();

      this.game.GameHandler.RemoveObject(p1, GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.RemoveObject(p2, GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.RemoveObject(p3, GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.RemoveObject(p4, GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.RemoveObject(p5, GameHandler.ObjectType.DefaultTextured);
    }

    private void RemovePowerups()
    { }

    public override void Update(GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
      if (state.Buttons.Back == ButtonState.Pressed)
      {
        this.game.ChangeState(this.game.TitleScreenState);
        return;
      }

      this.game.GameHandler.Update(gameTime);
      this.textHandler.Update(gameTime);
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (this.status)
      {
        case TutorialPowerupsStateStatus.TPSS_INTRO:
          UpdateIntro(gameTime);   
          break;
        case TutorialPowerupsStateStatus.TPSS_GET_POWERUP:
          UpdateGetPowerups(gameTime);
          break;
        case TutorialPowerupsStateStatus.TPSS_SHOW_POWERUPS:
          UpdateShowPowerups(gameTime);
          break;
      }
    }

    private void UpdateIntro(GameTime gameTime)
    {
      if (this.timeElapsed >= 11000)
      {
        ChangeState(TutorialPowerupsStateStatus.TPSS_GET_POWERUP);
      }

      this.game.GameHandler.Player.Update(gameTime);   
    }

    private void UpdateGetPowerups(GameTime gameTime)
    {
      if(      
        this.game.StatisticsHandler.GetStatistic(StatisticType.ST_POWERUPCOUNT) >= 3)
      {
        this.textHandler.UnstickCurrentText();
        ChangeState(TutorialPowerupsStateStatus.TPSS_SHOW_POWERUPS);
        RemovePowerups();
        return;
      }

      if (this.timeElapsed >= 5000)
      {
        this.timeElapsed = 0;
        PowerupSpawner.Spawn(Vector3.Zero, this.game);
      }

      this.game.GameHandler.Player.Update(gameTime);
      this.game.GameHandler.DetermineIfPlayerIsNearPowerup();
    }

    private void UpdateShowPowerups(GameTime gameTime)
    {
      if (this.timeElapsed >= 2500 && !isPowerupsSpawned)
      {
        this.game.GameHandler.Player.Position = new Vector3(0, 0, 40);
        this.camera.Offset = new Vector3(0, 0, 40);
        this.camera.Source.Rotation = Quaternion.Identity;
        this.game.GameHandler.Player.IsVisible = false;
        this.isPowerupsSpawned = true;


        CreatePowerups();
      }
    }
  }
}