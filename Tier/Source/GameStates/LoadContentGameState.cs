using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Objects;

namespace Tier.Source.GameStates
{
  public class LoadContentGameState : GameState
  {
    #region Privates
    private Sprite logo;
    private int timeElapsed;
    private bool isOn, isLoadingAssets;
    private Thread threadLoadAssets;
    private enum LoadContentGameStateStatus
    {
      LCGSS_LOADING, 
      LCGSS_WAITING_FOR_INPUT, 
      LCGSS_STARTING_UP
    };
    LoadContentGameStateStatus status;
    #endregion

    private Text textWaiting;

    public LoadContentGameState(TierGame game)
      : base(game)
    {
        isLoadingAssets = true;
    }

    private void ChangeStateLoading()
    {
      this.game.TextSpriteHandler.Initialize();

      Texture2D tex = this.game.Content.Load<Texture2D>("Textures//Logo");
      logo = this.game.TextSpriteHandler.CreateSprite(
        tex, 
        new Vector2(640, 360));
      //logo.Origin = new Vector2(256, 256);

      ThreadStart start = new ThreadStart(UpdateLogo);
      threadLoadAssets = new Thread(start);
      threadLoadAssets.Start();

      LoadAssets();
    }

    private void ChangeStateStartingUp()
    {
    }

    private void ChangeStateWaitingForInput()
    {
      this.isLoadingAssets = false;
      textWaiting = this.game.TextSpriteHandler.CreateText(
        "Press start",
        new Vector2(640, 600),
        Color.Yellow);      
    }

    private void ChangeState(LoadContentGameStateStatus newstatus)
    {
      switch (newstatus)
      {
        case LoadContentGameStateStatus.LCGSS_LOADING:
          ChangeStateLoading();
          break;
        case LoadContentGameStateStatus.LCGSS_WAITING_FOR_INPUT:
          ChangeStateWaitingForInput();
          break;
        case LoadContentGameStateStatus.LCGSS_STARTING_UP:
          break;
      }

      status = newstatus;
    }

    public override void Enter(GameState previousState)
    {
      ChangeState(LoadContentGameStateStatus.LCGSS_LOADING);

      lock (this)
      {
        ChangeState(LoadContentGameStateStatus.LCGSS_WAITING_FOR_INPUT);
      }
    }

    public override void Leave()
    {
      this.game.TextSpriteHandler.RemoveSprite(logo);
      this.game.TextSpriteHandler.RemoveText(textWaiting);
      threadLoadAssets = null;
    }

    private void LoadAssets()
    {
      this.game.ContentHandler.Load("Content//Xml//DataCollections//Default.xml", "Default");
      // Load all objects
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects//BossPieces//Tedris");
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects");
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects//Projectiles");
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects//Turrets");
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects//Powerups");
      this.game.InitializeAfterContentLoaded();
    }

    private void UpdateLoading(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (timeElapsed >= 250)
      {
        timeElapsed = 0;

        switch (isOn)
        {
          case true:
            logo.Color = Color.White;
            break;
          case false:
            logo.Color = new Color(255, 255, 255, 220);
            break;
        }

        isOn = !isOn;
      }

      // Done loading assets
      if (!isLoadingAssets)
      {
        ChangeState(LoadContentGameStateStatus.LCGSS_WAITING_FOR_INPUT);
      }
    }

    private void UpdateWaitingForInput(GameTime gameTime)
    {
      // Check all connected controllers for a start press
      for (PlayerIndex i = PlayerIndex.One;i< PlayerIndex.Four;i++)
      {
        GamePadState state = GamePad.GetState(i);

        if (state.IsButtonDown(Buttons.Start))
        {
          // Start pressed on a controller. This will be the main controller
          this.game.MainControllerIndex = i;
          ChangeState(LoadContentGameStateStatus.LCGSS_STARTING_UP);
        }
      }
    }

    private void UpdateStartingUp(GameTime gameTime)
    {
      this.game.ChangeState(this.game.TitleScreenState);
      //this.game.ChangeState(new GameTestState(this.game));
    }

    private void UpdateLogo()
    {
      while (true)
      {
        Thread.Sleep(10);

        // Update logo with particles

        lock (this)
        {
          switch (status)
          {
            case LoadContentGameStateStatus.LCGSS_WAITING_FOR_INPUT:
              if (!this.isLoadingAssets)
                UpdateWaitingForInput(null);
              break;
            case LoadContentGameStateStatus.LCGSS_STARTING_UP:
              UpdateStartingUp(null);
              return;
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
    }
  }
}