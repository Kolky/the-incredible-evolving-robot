using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers.Cameras;

namespace Tier.Source.Handlers
{
  public enum RocketComboRendererState
  {
    IDLE, RUNNING
  };
  /// <summary>
  /// Designed to take over the renderer of the maingamestate when the rocket combo behaviour is enabled
  /// </summary>
  public class RocketComboRenderer
  {
    private TierGame game;
    private Sprite topScreen, bottomScreen;
    private RocketComboRendererState state;
    private TrackingCamera camTopScreen;
    private TrackingCamera camBottomScreen;

    public RocketComboRenderer(TierGame game)
    {
      this.game = game;
      this.state = RocketComboRendererState.IDLE;
      this.topScreen = game.TextSpriteHandler.CreateSprite(this.game.ResolveTargetHandler.TopScreenTarget, Vector2.Zero);
      this.bottomScreen = game.TextSpriteHandler.CreateSprite(this.game.ResolveTargetHandler.BottomScreenTarget, Vector2.Zero);

      this.camTopScreen = new TrackingCamera(game, this.game.BossGrowthHandler.Core);
      this.camBottomScreen = new TrackingCamera(game, this.game.GameHandler.Player);      
    }

    private void changeState(RocketComboRendererState newstate)
    {
      switch (newstate)
      {
        case RocketComboRendererState.IDLE:
          this.game.TextSpriteHandler.RemoveSprite(this.bottomScreen);
          this.game.TextSpriteHandler.RemoveSprite(this.topScreen);
          this.bottomScreen.IsVisible = false;
          this.topScreen.IsVisible = false;
          break;
        case RocketComboRendererState.RUNNING:
          {
            int width = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            this.topScreen.Rectangle = new Rectangle(0, 0, width, height / 2);
            this.topScreen.Type = SpriteType.DESTINATION_RECTANGLE;
            this.bottomScreen.Rectangle = new Rectangle(0, height / 2, width, height / 2);
            this.bottomScreen.Type = SpriteType.DESTINATION_RECTANGLE;

            this.camTopScreen.Parent = this.game.BossGrowthHandler.Core;
            this.camTopScreen.Distance = 40;
            this.camBottomScreen.Parent = this.game.GameHandler.Player;
            this.camBottomScreen.Distance = 40; 
          }
          break;
      }

      this.state = newstate;
    }

    public void Draw(GameTime gameTime)
    {
      if (this.state == RocketComboRendererState.IDLE)
        return;

      DrawTopScreen(gameTime);
      this.game.GraphicsDevice.ResolveBackBuffer(this.game.ResolveTargetHandler.TopScreenTarget);
      DrawBottomScreen(gameTime);
      this.game.GraphicsDevice.ResolveBackBuffer(this.game.ResolveTargetHandler.BottomScreenTarget);
      
      this.game.TextSpriteHandler.Draw(gameTime);
      //this.game.BehaviourHandler.Draw(gameTime);
    }

    private void DrawTopScreen(GameTime gameTime)
    {
      this.camTopScreen.Update(gameTime);
    }

    private void DrawBottomScreen(GameTime gameTime)
    {
      this.game.GameHandler.Camera.Update(gameTime);
    }

    public bool IsActive()
    {
      return state == RocketComboRendererState.RUNNING;
    }

    public void Update(GameTime gameTime)
    {
      if (this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_ROCKETCOMBO) &&
        state == RocketComboRendererState.IDLE)
      {
        changeState(RocketComboRendererState.RUNNING);
      }

      switch (state)
      {
        case RocketComboRendererState.IDLE:
          break;
        case RocketComboRendererState.RUNNING:
          if (!this.game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_ROCKETCOMBO))
          {
            changeState(RocketComboRendererState.IDLE);
          }
          break;
      }
    }
  }
}
