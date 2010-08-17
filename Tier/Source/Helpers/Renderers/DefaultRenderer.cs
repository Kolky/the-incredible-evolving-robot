using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Handlers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BloomPostprocess;

namespace Tier.Source.Helpers.Renderers
{
  public class DefaultRenderer : Renderer
  {
    private GraphicsDeviceManager manager;
    private bool isSplitScreen;
    private EffectParameterHandler effectParameterHandler;

    public DefaultRenderer(TierGame game)
      : base(game)
    {
      manager = game.GraphicsDeviceManager;
      game.BloomComponent = new BloomComponent(game);
      effectParameterHandler = new EffectParameterHandler(game);
    }

    public override void Draw(GameTime gameTime)
    {
#if DEBUG
      if (this.game.InterfaceHandler != null)
      {
        // Update fps counter
        this.game.InterfaceHandler.UpdateFpsCounter(gameTime);
      }
#endif
      // In order for the bloom to work correctly all non blooming objects will be drawn using ALPHA = 0
      this.game.GraphicsDevice.Clear(Color.TransparentBlack);
      this.effectParameterHandler.Update(gameTime);

      if (isSplitScreen)
      {
        //store the current viewport width
        int viewportWidth = this.game.GraphicsDevice.Viewport.Width;

        //get a copy of the current graphics device viewport
        Viewport viewport = this.game.GraphicsDevice.Viewport;
        //set the viewport width to half the screen width
        viewport.Width = viewportWidth / 2;
        //update the graphics device viewport
        this.game.GraphicsDevice.Viewport = viewport;

        this.game.GameHandler.View = this.cameraLeft;
        //render the left side of the screen
        DrawScene(gameTime);

        //move the viewport x to be in the middle of the screen
        viewport.X = viewportWidth / 2;
        //make sure the viewport width is only half the screen width
        viewport.Width = viewportWidth / 2;
        //update the graphics device viewport
        this.game.GraphicsDevice.Viewport = viewport;

        this.game.GameHandler.View = this.cameraRight;
        //render the right side of the screen
        DrawScene(gameTime);

        //reset the graphics device viewport X and width to their original values
        viewport.X = 0;
        viewport.Width = viewportWidth;

        //update the graphics device viewport
        this.game.GraphicsDevice.Viewport = viewport;
      }
      else
      {
        DrawScene(gameTime);
      }

      this.game.BloomComponent.Draw(gameTime);
    }

    private void DrawScene(GameTime gameTime)
    {
      // Draw non blooming objects first using
      this.game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.Skybox);
      this.game.GameHandler.Player.Draw(gameTime);
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.Deferred);
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.DefaultTextured);
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.Default);

      // Draw blooming objects
      this.game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
      this.game.GraphicsDevice.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
      this.game.GraphicsDevice.RenderState.AlphaDestinationBlend = Blend.DestinationAlpha;

      this.game.ProjectileHandler.Draw(gameTime);
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.Transparent);
      this.game.GameHandler.Draw(gameTime, GameHandler.ObjectType.AlphaBlend);

      this.game.TextSpriteHandler.Draw(gameTime);
    }

    public override void DisableSplitScreen()
    {
      isSplitScreen = false;
    }

    public override void EnableSplitScreen()
    {
      isSplitScreen = true;
    }
    
    public override void Initialize(int width, int height)
    {      
      manager.PreferredBackBufferFormat = SurfaceFormat.Color;
      manager.PreferredBackBufferWidth = width;
      manager.PreferredBackBufferHeight = height;
      manager.PreferMultiSampling = true;

#if !DEBUG
      manager.IsFullScreen = true;
      //manager.SynchronizeWithVerticalRetrace = true;
      //this.game.IsFixedTimeStep = true;
#endif
      manager.ApplyChanges();     
    }
  }
}
