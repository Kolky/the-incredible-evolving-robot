using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers.Cameras;
using Tier.Source.Objects;
using Tier.Source.Handlers;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Objects.PlayerWeapons;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.GameStates.Tutorial
{
  public class TutorialBossState : GameState
  {
    #region Properties
    private DelayCamera camera;
    private SkyBox skybox;
    private TutorialTextHandler textHandler;
    private int timeElapsed;
    private enum TutorialBossStateStatus
    {
      TBSS_INTRO, TBSS_BOSS, TBSS_GROWNBOSS
    }
    private TutorialBossStateStatus status;
    private int timeSinceBossGrown;
    private int timeSinceBossDestroyed; 
    #endregion

    public TutorialBossState(TierGame game)
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

    private void ChangeState(TutorialBossStateStatus newstatus)
    {
      switch (newstatus)
	    {
		    case TutorialBossStateStatus.TBSS_INTRO:
          this.textHandler.AddText("The goal of the game is to destroy T.I.E.R.", 2000);          
          break;
        case TutorialBossStateStatus.TBSS_BOSS:
          ChangeStateBoss();
          break;
        case TutorialBossStateStatus.TBSS_GROWNBOSS:
          ChangeStateGrownBoss();
          break;
	    }

      this.timeElapsed = 0;
      this.status = newstatus;
    }

    private void ChangeStateBoss()
    {
      this.game.BossGrowthHandler.Start();
      this.textHandler.AddText("T.I.E.R. will grow stronger every time it is destroyed", 3000);
      this.textHandler.AddText("It can become pretty big!", 2000);
    }

    private void ChangeStateGrownBoss()
    {
      this.textHandler.AddText("To go to the next level, the core has to be destroyed.", 3000);      
      this.textHandler.AddText("The core's health is determined by T.I.E.R's size.", 2000);      
      this.textHandler.AddText("Destroy the other pieces before shooting at the core.", 2000);
      this.textHandler.AddText("The outer pieces have the least health!", 2000);
      this.textHandler.AddText("Destroy T.I.E.R.!", 1, true);
    }

    public override void Enter(GameState previousState)
    {
      skybox = new SkyBox(this.game);
      if (this.game.ObjectHandler.InitializeFromBlueprint<SkyBox>(skybox, "Skybox"))
      {
        this.game.GameHandler.AddObject(skybox);
      }

      timeSinceBossGrown = 0;
      this.game.GameHandler.Camera = this.camera;
      this.game.GameHandler.Player.IsVisible = true;
      this.game.InterfaceHandler.Show();

      ChangeState(TutorialBossStateStatus.TBSS_INTRO);
    }

    public override void Leave()
    {
      this.game.BossGrowthHandler.Stop();
      this.game.InterfaceHandler.Hide();
      this.game.BossCompositionHandler.Reset();
      this.game.GameHandler.RemoveObject(skybox, GameHandler.ObjectType.Skybox);      
      this.textHandler.Stop();  
    }

    public override void Update(GameTime gameTime)
    {
      BossPiece core = this.game.BossGrowthHandler.Core;

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

      switch (this.status)
      {
        case TutorialBossStateStatus.TBSS_INTRO:
          if (this.timeElapsed >= 3000)
          {
            ChangeState(TutorialBossStateStatus.TBSS_BOSS);
          }
          break;
        case TutorialBossStateStatus.TBSS_BOSS:
          this.timeSinceBossGrown += gameTime.ElapsedGameTime.Milliseconds;
          if(timeElapsed >= 4000)
          {
            for (int i = 0; i < 20; i++)
            {
              int count = 1;              
              core.GrowableModifier.Grow(ref count, core);
            }
            this.game.BossCompositionHandler.DetermineBossPiecesBoundingBox();
            ChangeState(TutorialBossStateStatus.TBSS_GROWNBOSS);
          }
          else if (timeSinceBossGrown >= 500)
          {
            timeSinceBossGrown = 0;
            int count = 1;
            core.GrowableModifier.Grow(ref count, core);         
          }
          break;
        case TutorialBossStateStatus.TBSS_GROWNBOSS:
          UpdateBossGrown(gameTime);
          break;
      }
    }

    private void UpdateBossGrown(GameTime gameTime)
    {
      BossPiece core = this.game.BossGrowthHandler.Core;

      if (
        core != null &&
        core.DestroyableModifier.IsDestroyed &&
        core.DestroyableModifier.ExplosionHandler.IsDone)
      {
        // done
        this.textHandler.UnstickCurrentText();

        this.timeSinceBossDestroyed += gameTime.ElapsedGameTime.Milliseconds;

        if (timeSinceBossDestroyed >= 1500)
        {
          this.game.ChangeState(new TutorialPowerupsState(this.game));
        }
      }
      
      if (timeElapsed >= 19000)
      {
        this.game.ProjectileHandler.Update(gameTime);
        this.game.ScoreHandler.Update(gameTime);

        // Shoot
        GamePadState state = GamePad.GetState(this.game.MainControllerIndex);
        if (state.Triggers.Right > 0)
        {
          this.game.GameHandler.Player.Shoot();
        }
      }
    }
  }
}
