using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TryOut.Input;

namespace TryOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TierGame : Game
    {
        public GraphicsDeviceManager Graphics { get; set; }
        public static Dictionary<ActionType, BaseAction> Actions { get; set; }
        private List<MiscObject> Objects;

        static TierGame()
        {
            Actions = new Dictionary<ActionType, BaseAction>();
        }

        public TierGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Objects = new List<MiscObject>();
        }

        private bool InitGraphicsMode(int width, int height, bool isFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (isFullScreen == false)
            {
                if (width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width &&
                    height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                {
                    Graphics.PreferredBackBufferWidth = width;
                    Graphics.PreferredBackBufferHeight = height;
                    Graphics.IsFullScreen = isFullScreen;
                    Graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set. To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if (displayMode.Width == width && displayMode.Height == height)
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        Graphics.PreferredBackBufferWidth = width;
                        Graphics.PreferredBackBufferHeight = height;
                        Graphics.IsFullScreen = isFullScreen;
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
#if XBOX360
            Boolean isFullScreen = true;
#else
            Boolean isFullScreen = false;
#endif

            if (!InitGraphicsMode(1280, 800, isFullScreen))
            {
                if (!InitGraphicsMode(1024, 768, isFullScreen))
                {
                    if (!InitGraphicsMode(800, 600, isFullScreen))
                    {
                        if (!InitGraphicsMode(640, 480, isFullScreen))
                        {
                            Exit();
                        }
                    }
                }
            }

#if WINDOWS
            GraphicsDevice.RenderState.MultiSampleAntiAlias = true;
            GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.FourSamples;
            GraphicsDevice.PresentationParameters.MultiSampleQuality = 2;
            Graphics.PreferMultiSampling = true;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();
#endif

            InitActions();

            Objects.Add(new MiscObject());

            base.Initialize();
        }

        private void InitActions()
        {
            Actions.Add(ActionType.ActionType_Fire, BaseAction.Create(ActionHandler, MouseButton.MouseButton_Right, ActionState.ActionState_Pressed));
            Actions.Add(ActionType.ActionType_AltFire, BaseAction.Create(ActionHandler, MouseButton.MouseButton_Right, ActionState.ActionState_Released));
            Actions.Add(ActionType.ActionType_MegaFire, BaseAction.Create(ActionHandler, MouseButton.MouseButton_Right, ActionState.ActionState_Held));
            Actions.Add(ActionType.ActionType_Exit, BaseAction.Create(ExitHandler, Keys.Escape));
        }

        private void ActionHandler(BaseAction caller)
        {
            Console.WriteLine(caller);
        }

        private void ExitHandler(BaseAction caller)
        {
            Console.WriteLine(caller);
            Exit();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            foreach (BaseAction action in Actions.Values)
            {
                action.Execute();
                action.Update();
            } 

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
