using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Handlers;
using Tier.Source.Helpers.Cameras;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Misc;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Tier.Source.GameStates.Tutorial;
using Tier.Source.Objects.PlayerWeapons;

namespace Tier.Source.GameStates
{
  public class TitleScreenState : GameState
  {
    #region Properties
    private PositionalCamera cam;
    private Sprite logo; 
    private float timeElapsed, rot;    
    private TitleScreenLogoManipulator logoManipulator;
    private SkyBox skyBox;
    private enum TitleScreenSelection
    {
      TSS_START,
      TSS_TUTORIAL,
      TSS_CREDITS,
      TSS_EXIT
    };

    private GamePadState previousGamePadState;
    private TitleScreenSelection selection;
    private Text txtStart, txtCredits, txtTutorial, txtExit, currentSelected;
    #endregion

    public TitleScreenState(TierGame game)
      : base(game)
    {
      cam = new PositionalCamera(game);
      selection = TitleScreenSelection.TSS_START;
    }

    private void DoSelection()
    {
      this.game.SoundHandler.Play("Select");

      switch (selection)
      {
        case TitleScreenSelection.TSS_START:
          this.game.ChangeState(this.game.MainGameState);
          break;
        case TitleScreenSelection.TSS_TUTORIAL:
          this.game.ChangeState(new TutorialStartState(this.game));
          break;
        case TitleScreenSelection.TSS_CREDITS:
          this.game.ChangeState(this.game.CreditsState);
          break;
        case TitleScreenSelection.TSS_EXIT:
          this.game.Exit();
          break;
      }
    }

    public override void Enter(GameState previousState)
    {
      this.game.GameHandler.Player.SwitchWeapon(new PlayerPlasmaWeapon(this.game));
      this.game.ProjectileHandler.Clear();
      this.game.BossGrowthHandler.Stop();
      this.game.GameHandler.Player.PlayerWeapon.Reset();
      this.game.StatisticsHandler.Reset();
      this.game.GameHandler.ClearObjects(GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.ClearObjects(GameHandler.ObjectType.AlphaBlend);
      this.game.GameHandler.ClearObjects(GameHandler.ObjectType.Transparent); 
      this.game.BossCompositionHandler.Reset();
      this.game.BehaviourHandler.Reset();
      this.game.GameHandler.Player.IsVisible = false;
      this.game.GameHandler.CurrentLevel = 0;
      this.timeElapsed = 0;
      this.Game.SoundHandler.Stop("IngameMusic");
      this.game.SoundHandler.Stop("TutorialLoop");
      this.Game.SoundHandler.Play("MenuMusic", true);

      skyBox = new SkyBox(game);
      game.ObjectHandler.InitializeFromBlueprint<SkyBox>(skyBox, "Skybox");
      game.GameHandler.AddObject(skyBox);

      // Hide all interface elements
      game.InterfaceHandler.Hide();

      if(!this.game.BossGrowthHandler.IsInitialized)
        this.game.BossGrowthHandler.Initialize();

      game.GameHandler.Camera = cam;
      cam.Position = new Vector3(0, 0, 40);
      cam.Target = Vector3.Zero;
      
      if (logo == null)
      {
        Texture2D tex = this.game.ContentHandler.GetAsset<Texture2D>("Logo");
        this.logo =
          this.game.TextSpriteHandler.CreateSprite(tex, Vector2.Zero);
        this.logo.Type = SpriteType.DESTINATION_RECTANGLE;
        this.logoManipulator = new TitleScreenLogoManipulator(logo);
      }

      this.logo.Rectangle = new Rectangle(
        this.game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - this.logo.Texture.Width / 2, 20,
        this.logo.Texture.Width, this.logo.Texture.Height);

      this.logoManipulator.AddTime(0);
      this.logoManipulator.AddTime(21300);
      this.logoManipulator.AddTime(42800);
      this.logoManipulator.AddTime(63900);
      this.logoManipulator.AddTime(85000);
      this.logoManipulator.Start();
      this.logo.IsVisible = true;

      int width = this.game.GraphicsDevice.PresentationParameters.BackBufferWidth;
      int height = this.game.GraphicsDevice.PresentationParameters.BackBufferHeight;

      txtStart = this.game.TextSpriteHandler.CreateText("Start", new Vector2(width * 0.1f, height * 0.85f), Color.White);
      txtTutorial = this.game.TextSpriteHandler.CreateText("Tutorial", new Vector2(width * 0.28f, height * 0.85f), Color.White);
      txtCredits = this.game.TextSpriteHandler.CreateText("Credits", new Vector2(width * 0.525f, height * 0.85f), Color.White);
      txtExit = this.game.TextSpriteHandler.CreateText("Exit", new Vector2(width * 0.75f, height * 0.85f), Color.White);

      this.game.ScoreHandler.Reset();
    }

    public override void Update(GameTime gameTime)
    {
      GamePadState currentGamepadState = GamePad.GetState(this.game.MainControllerIndex);

      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.game.GameHandler.Update(gameTime);      
      logoManipulator.Update(gameTime);

      if (currentGamepadState.Buttons.A == ButtonState.Pressed)
      {        
        DoSelection();
        return;
      }
      else if(currentGamepadState.ThumbSticks.Left.X >= 0.75f &&
        previousGamePadState.ThumbSticks.Left.X < 0.75f)
      {
        if(selection < TitleScreenSelection.TSS_EXIT)
        {
          selection++;
          this.game.SoundHandler.Play("Menu");
        }
      }
      else if (currentGamepadState.ThumbSticks.Left.X <= -0.75f &&
        previousGamePadState.ThumbSticks.Left.X > -0.75f)
      {
        if (selection > TitleScreenSelection.TSS_START)
        {
          selection--;
          this.game.SoundHandler.Play("Menu");
        }
      }

      previousGamePadState = currentGamepadState;

      txtStart.Color = txtTutorial.Color = txtCredits.Color = txtExit.Color = Color.Tomato;

      switch (selection)
      {
        case TitleScreenSelection.TSS_START:
          currentSelected = txtStart;
          break;
        case TitleScreenSelection.TSS_TUTORIAL:
          currentSelected = txtTutorial;
          break;
        case TitleScreenSelection.TSS_CREDITS:
          currentSelected = txtCredits;
          break;
        case TitleScreenSelection.TSS_EXIT:
          currentSelected = txtExit;
          break;
      }

      currentSelected.Color = Color.LimeGreen;
    }

    public override void Leave()
    {
      this.Game.SoundHandler.Stop("MenuMusic");
      this.logo.IsVisible = false;
      this.logoManipulator.Stop();
      this.logoManipulator.Clear();

      this.game.GameHandler.CurrentLevel = 1;
      // Remove all objects except the core
      this.game.GameHandler.ClearLevel();
      this.game.BehaviourHandler.Transform = Matrix.Identity;

      this.game.TextSpriteHandler.RemoveText(txtStart);
      this.game.TextSpriteHandler.RemoveText(txtTutorial);
      this.game.TextSpriteHandler.RemoveText(txtCredits);
      this.game.TextSpriteHandler.RemoveText(txtExit);

      this.game.GameHandler.RemoveObject(skyBox, GameHandler.ObjectType.Skybox);
    }
  }
}