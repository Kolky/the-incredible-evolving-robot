using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Objects.Turrets;
using Tier.Source.Misc;
using Tier.Source.Handlers;
using Tier.Source.Helpers.Cameras;
using Tier.Source.Helpers;

namespace Tier.Source.GameStates
{
  public class BossIntroductionState : GameState
  {
    #region Privates
    private int timeElapsed;
    private Vector3 movement;
    private enum CurrentIntroductionState
    {
      Moving, Rotating
    };
    private CurrentIntroductionState state;
    #endregion

    private PositionalCamera cam;
    private Matrix start, end1, end2;
    private Text textLevel, textStart;

    public BossIntroductionState(TierGame game)
      : base(game)
    {
      cam = new PositionalCamera(game);
      start = Matrix.CreateTranslation(new Vector3(0, 0, -400.0f));
      end1 = Matrix.CreateTranslation(new Vector3(0, 0, -10.0f));
      end2 = Matrix.Identity;

      textLevel = game.TextSpriteHandler.CreateText("", Vector2.Zero, Color.White);
      textStart = game.TextSpriteHandler.CreateText("Press start", 
        new Vector2(
        game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 120,
        game.GraphicsDevice.PresentationParameters.BackBufferHeight - 60)
        , Color.Gold);

      textLevel.IsVisible = false;
      textStart.IsVisible = false;
    }

    public override void Enter(GameState previousState)
    {
      base.Enter(previousState);
      this.timeElapsed = 0;
      this.game.GameHandler.NextLevel();
      this.game.GameHandler.RemovePowerups();
      this.game.ProjectileHandler.Clear();
      this.game.BehaviourHandler.Reset();
      this.game.GameHandler.Player.IsVisible = false;
      
      this.cam.Target = Vector3.Zero;
      this.cam.UpVector = Vector3.Up;
      this.game.GameHandler.Camera = this.cam;
      this.movement = Vector3.Zero - new Vector3(0, 0, -100.0f);
      this.state = CurrentIntroductionState.Moving;
      this.textLevel.IsVisible = true;
      this.textLevel.Value = string.Format("Level: {0}", this.game.GameHandler.CurrentLevel);      
    }

    public override void Leave()
    {
      this.game.BehaviourHandler.Transform = Matrix.Identity;
      textStart.IsVisible = false;
      textLevel.IsVisible = false;
      this.game.GameHandler.Player.IsVisible = true;
    }

    public override void Update(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.game.GameHandler.Update(gameTime);
      this.game.ProjectileHandler.Update(gameTime);

      this.game.BossCompositionHandler.DetermineBossPiecesBoundingBox();
      float length = this.game.BossCompositionHandler.GetLength() * 3.0f;

      if (length < 40.0f)
      {
        length = 40.0f;
      }

      this.cam.Position = new Vector3(0, 0, length);

      switch (state)
      {
        case CurrentIntroductionState.Moving:
          float x = (this.game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f - 80) * (timeElapsed / 3000.0f);
          this.textLevel.Position = new Vector2(x, this.game.GraphicsDevice.PresentationParameters.BackBufferHeight - 100); 
          // First two seconds the boss will fly at high speed
          if (timeElapsed < 2000.0f)
          {
            this.game.BehaviourHandler.Transform = Matrix.Lerp(start, end1, timeElapsed / 2000.0f);
          }
          // Last second slowly
          else
          {
            this.game.BehaviourHandler.Transform = Matrix.Lerp(end1, end2, (timeElapsed - 2000.0f) / 1000.0f);
          }

          if (timeElapsed >= 3000.0f)
          {
            state = CurrentIntroductionState.Rotating;
            timeElapsed = 0;
            textStart.IsVisible = true;
          }
          break;
        case CurrentIntroductionState.Rotating:          
          this.game.BehaviourHandler.Transform =
            Matrix.CreateRotationY((timeElapsed / 6000.0f) * MathHelper.Pi * 2);         
          break;
      }
      
      if (GamePad.GetState(this.game.MainControllerIndex).Buttons.Start == ButtonState.Pressed ||
          GamePad.GetState(this.game.MainControllerIndex).Buttons.A == ButtonState.Pressed)
      {                
        this.game.ChangeState(this.game.MainGameState);
      }
    }
  }
}
