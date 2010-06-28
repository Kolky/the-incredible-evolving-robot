using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Camera;
using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;
using Tier.Objects;
using Tier.Objects.Basic;
using Tier.Objects.Destroyable;
using Tier.Test;

namespace Tier
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class TierGame : Game
  {
    #region Properties
    private static ContentHandler contenthandler;
    public static ContentHandler ContentHandler
    {
      get { return contenthandler; }
      set { contenthandler = value; }
    }

    private static GraphicsDeviceManager graphics;
    public static GraphicsDeviceManager Graphics
    {
      get { return graphics; }
    }

    private static GraphicsDevice device;
    public static GraphicsDevice Device
    {
      get { return device; }
    }

    private static ContentManager content;
    public static ContentManager Content
    {
      get { return content; }
    }

    private static GameHandler gameHandler;
    public static GameHandler GameHandler
    {
      get { return gameHandler; }
    }

    private static TextHandler textHandler;
    public static TextHandler TextHandler
    {
      get { return textHandler; }
    }

    private static TierGame instance;
    public static TierGame Instance
    {
      get { return instance; }
    }

    private static Input input;
    public static Input Input
    {
      get { return input; }
    }
#if XBOX360
		public static InputXBOX InputXBOX
    {
      get { return (InputXBOX)input; }
    }
#else
		public static InputPC InputPC
    {
      get { return (InputPC)input; }
    }
#endif
    private int fpsCurrent = 0;
    private int fpsElapsedMs = 0;
		#if DEBUG


		#if DEBUG && BOUNDRENDER
		private static BoundingSphereRenderer boundingSphereRender;
		public static BoundingSphereRenderer BoundingSphereRender
		{
			get { return boundingSphereRender; }
			set { boundingSphereRender = value; }
		}

		private static BoundingBoxRenderer boundingBoxRenderer;
		public static BoundingBoxRenderer BoundingBoxRenderer
		{
			get { return boundingBoxRenderer; }
			set { boundingBoxRenderer = value; }
		}
		#endif
		#endif

		private static Audio audio;
    public static Audio Audio
    {
      get { return audio; }
    }
    #endregion	

    public TierGame()
    {
      #region Highscore Crap
      HighScores.AddEntry("Alex", Options.Random.Next(10, 50000), 1337, 1337);
      HighScores.AddEntry("Maarten", Options.Random.Next(10, 50000), 1337, 1337);
      HighScores.AddEntry("Jonathan", Options.Random.Next(10, 50000), 1337, 1337);
      HighScores.AddEntry("Ted", Options.Random.Next(10, 50000), 1337, 1337);
      #endregion

      

      TierGame.instance = this;
#if DEBUG && BOUNDRENDER
			TierGame.BoundingSphereRender = new BoundingSphereRenderer();
#endif

#if XBOX360
			TierGame.input = new InputXBOX();
#else
			TierGame.input = new InputPC();
#endif
      //TierGame.audio = new Audio("Content\\Audio\\Audio.xgs", "Content\\Audio\\Wave Bank.xwb", "Content\\Audio\\Sound Bank.xsb");
      TierGame.graphics = new GraphicsDeviceManager(this);
      TierGame.content = new ContentManager(this.Services);
      TierGame.contenthandler = new ContentHandler();
    }

    private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
    {
      // If we aren't using a full screen mode, the height and width of the window can
      // be set to anything equal to or smaller than the actual screen size.
      if (bFullScreen == false)
      {
        if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
        && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
        {
          TierGame.Graphics.PreferredBackBufferWidth = iWidth;
          TierGame.Graphics.PreferredBackBufferHeight = iHeight;
          TierGame.Graphics.IsFullScreen = bFullScreen;
          TierGame.Graphics.ApplyChanges();
          return true;
        }
      }
      else
      {
        // If we are using full screen mode, we should check to make sure that the display
        // adapter can handle the video mode we are trying to set. To do this, we will
        // iterate thorugh the display modes supported by the adapter and check them against
        // the mode we want to set.
        foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
        {
          // Check the width and height of each mode against the passed values
          if ((dm.Width == iWidth) && (dm.Height == iHeight))
          {
            // The mode is supported, so set the buffer formats, apply changes and return
            TierGame.Graphics.PreferredBackBufferWidth = iWidth;
            TierGame.Graphics.PreferredBackBufferHeight = iHeight;
            TierGame.Graphics.IsFullScreen = bFullScreen;
            TierGame.Graphics.ApplyChanges();
            return true;
          }
        }
      }
      return false;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      TierGame.device = TierGame.graphics.GraphicsDevice;
      AnimatedBillboard.vd = new VertexDeclaration(TierGame.Device, AnimatedBillboard.MyVertexPositionTexture.VertexElements);
#if XBOX360
      Boolean fullScreen = true;
#else
      Boolean fullScreen = false;
#endif

      if (!this.InitGraphicsMode(1280, 800, fullScreen))
      {
        if (!this.InitGraphicsMode(1024, 768, fullScreen))
        {
          if (!this.InitGraphicsMode(800, 600, fullScreen))
          {
            if (!this.InitGraphicsMode(640, 480, fullScreen))
            {
              this.Exit();
            }
          }
        }
      }

#if WINDOWS
      TierGame.device.RenderState.MultiSampleAntiAlias = true;
      TierGame.device.PresentationParameters.MultiSampleType = MultiSampleType.FourSamples;
      TierGame.device.PresentationParameters.MultiSampleQuality = 2;
      TierGame.Graphics.PreferMultiSampling = true;
      TierGame.Graphics.SynchronizeWithVerticalRetrace = true;
      TierGame.Graphics.ApplyChanges();
#endif

      TierGame.textHandler = new TextHandler();
      TierGame.gameHandler = new GameHandler(this);
      TierGame.TextHandler.AddItem("fps", "Fps:", new Vector2(10f, 25f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
      TierGame.TextHandler.Initialize();
#if DEBUG
      TierGame.TextHandler.AddItem("time", "", new Vector2(10f, 10f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
      
      TierGame.TextHandler.AddItem("objects", "Objects: " + GameHandler.ObjectHandler.Count, new Vector2(10f, 40f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
      TierGame.textHandler.AddItem("reso", "Resolution: " + this.Window.ClientBounds.Width + "x" + this.Window.ClientBounds.Height, new Vector2(10f, 55f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);

      Axis.Init();
#endif

      base.Initialize();

#if DEBUG && BOUNDRENDER
      TierGame.BoundingBoxRenderer = new BoundingBoxRenderer();
#endif
    }

    /// <summary>
    /// Load your graphics content.  If loadAllContent is true, you should
    /// load content from both ResourceManagementMode pools.  Otherwise, just
    /// load ResourceManagementMode.Manual content.
    /// </summary>
    /// <param name="loadAllContent">Which type of content to load.</param>
    protected override void LoadGraphicsContent(bool loadAllContent)
		{
#if DEBUG && BOUNDRENDER
      TierGame.BoundingSphereRender.OnCreateDevice();
#endif
			TierGame.GameHandler.LoadGraphicsContent(loadAllContent);
    }

    /// <summary>
    /// Unload your graphics content.  If unloadAllContent is true, you should
    /// unload content from both ResourceManagementMode pools.  Otherwise, just
    /// unload ResourceManagementMode.Manual content.  Manual content will get
    /// Disposed by the GraphicsDevice during a Reset.
    /// </summary>
    /// <param name="unloadAllContent">Which type of content to unload.</param>
    protected override void UnloadGraphicsContent(bool unloadAllContent)
    {
      if (unloadAllContent)
      {
        TierGame.Content.Unload();
      }
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      TierGame.GameHandler.Update(gameTime);

#if DEBUG
      TierGame.TextHandler.ChangeText("time", gameTime.TotalGameTime.ToString());
      TierGame.TextHandler.ChangeText("objects", "Objects: " + GameHandler.ObjectHandler.Count);
#endif

      TierGame.Input.Update();
	  //TierGame.Audio.Update();

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      TierGame.GameHandler.Draw(gameTime);
      try { TierGame.TextHandler.Draw(gameTime); }
      catch (Exception ex) { }
      this.fpsCurrent++;
      this.fpsElapsedMs += gameTime.ElapsedGameTime.Milliseconds;
      if (this.fpsElapsedMs >= 1000)
      {
        TierGame.TextHandler.ChangeText("fps", "Fps: " + this.fpsCurrent);
        this.fpsElapsedMs = 0;
        this.fpsCurrent = 0;
      }
#if DEBUG


      Axis.Draw();
#endif

      base.Draw(gameTime);
    }
  }
}