using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;
using Tier.Objects;
using Tier.Test;

namespace Tier
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TierGame : Game
    {
        #region Properties
        public static ContentHandler ContentHandler { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static GraphicsDevice Device { get; private set; }
        public static GameHandler GameHandler { get; private set; }
        public static TextHandler TextHandler { get; private set; }
        public static TierGame Instance { get; private set; }
        public static Input Input { get; private set; }
#if XBOX360
		public static InputXBOX InputXBOX
        {
            get { return (InputXBOX)Input; }
        }
#else
        public static InputPC InputPC
        {
            get { return (InputPC)Input; }
        }
#endif
        private int fpsCurrent = 0;
        private int fpsElapsedMs = 0;

#if DEBUG && BOUNDRENDER
		public static BoundingSphereRenderer BoundingSphereRender { get; private set; }
		public static BoundingBoxRenderer BoundingBoxRenderer { get; private set; }
#endif

        public static Audio Audio { get; private set; }
        #endregion

        public TierGame()
        {
            #region Highscore Crap
            HighScores.AddEntry("Alex", Options.Random.Next(10, 50000), 1337, 1337);
            HighScores.AddEntry("Maarten", Options.Random.Next(10, 50000), 1337, 1337);
            HighScores.AddEntry("Jonathan", Options.Random.Next(10, 50000), 1337, 1337);
            HighScores.AddEntry("Ted", Options.Random.Next(10, 50000), 1337, 1337);
            #endregion

            Instance = this;
#if DEBUG && BOUNDRENDER
			TierGame.BoundingSphereRender = new BoundingSphereRenderer();
#endif

#if XBOX360
			Input = new InputXBOX();
#else
            Input = new InputPC();
#endif
            //Audio = new Audio("Content\\Audio\\Audio.xgs", "Content\\Audio\\Wave Bank.xwb", "Content\\Audio\\Sound Bank.xsb");
            Graphics = new GraphicsDeviceManager(this);
            Content = new ContentManager(Services);
            ContentHandler = new ContentHandler();
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
                    Graphics.PreferredBackBufferWidth = iWidth;
                    Graphics.PreferredBackBufferHeight = iHeight;
                    Graphics.IsFullScreen = bFullScreen;
                    Graphics.ApplyChanges();
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
                        Graphics.PreferredBackBufferWidth = iWidth;
                        Graphics.PreferredBackBufferHeight = iHeight;
                        Graphics.IsFullScreen = bFullScreen;
                        Graphics.ApplyChanges();
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
            Device = Graphics.GraphicsDevice;
            AnimatedBillboard.vd = new VertexDeclaration(Device, AnimatedBillboard.MyVertexPositionTexture.VertexElements);
#if XBOX360
      Boolean fullScreen = true;
#else
            Boolean fullScreen = false;
#endif

            if (!InitGraphicsMode(1280, 800, fullScreen))
            {
                if (!InitGraphicsMode(1024, 768, fullScreen))
                {
                    if (!InitGraphicsMode(800, 600, fullScreen))
                    {
                        if (!InitGraphicsMode(640, 480, fullScreen))
                        {
                            Exit();
                        }
                    }
                }
            }

#if WINDOWS
            Device.RenderState.MultiSampleAntiAlias = true;
            Device.PresentationParameters.MultiSampleType = MultiSampleType.FourSamples;
            Device.PresentationParameters.MultiSampleQuality = 2;
            Graphics.PreferMultiSampling = true;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();
#endif

            TextHandler = new TextHandler();
            GameHandler = new GameHandler(this);
            TextHandler.AddItem("fps", "Fps:", new Vector2(10f, 25f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
            TextHandler.Initialize();
#if DEBUG
            TextHandler.AddItem("time", "", new Vector2(10f, 10f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);

            TextHandler.AddItem("objects", "Objects: " + GameHandler.ObjectHandler.Count, new Vector2(10f, 40f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
            TextHandler.AddItem("reso", "Resolution: " + Window.ClientBounds.Width + "x" + Window.ClientBounds.Height, new Vector2(10f, 55f), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);

            Axis.Init();
#endif
            base.Initialize();

#if DEBUG && BOUNDRENDER
            BoundingBoxRenderer = new BoundingBoxRenderer();
#endif
        }

        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        [Obsolete]
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
#if DEBUG && BOUNDRENDER
            BoundingSphereRender.OnCreateDevice();
#endif
            GameHandler.LoadGraphicsContent(loadAllContent);
        }

        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        [Obsolete]
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                Instance.Content.Unload();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameHandler.Update(gameTime);

#if DEBUG
            TextHandler.ChangeText("time", gameTime.TotalGameTime.ToString());
            TextHandler.ChangeText("objects", "Objects: " + GameHandler.ObjectHandler.Count);
#endif
            Input.Update();
            //Audio.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GameHandler.Draw(gameTime);
            try
            {
                TextHandler.Draw(gameTime);
            }
            catch (Exception)
            {
            }
            fpsCurrent++;
            fpsElapsedMs += gameTime.ElapsedGameTime.Milliseconds;
            if (fpsElapsedMs >= 1000)
            {
                TextHandler.ChangeText("fps", "Fps: " + fpsCurrent);
                fpsElapsedMs = 0;
                fpsCurrent = 0;
            }
#if DEBUG
            Axis.Draw();
#endif
            base.Draw(gameTime);
        }
    }
}