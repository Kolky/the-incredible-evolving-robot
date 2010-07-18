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
        #region Input Actions
        private static Dictionary<ActionType, List<BaseAction>> Actions;
        private static List<KeyValuePair<ActionType, BaseAction>> QueuedActions;
        private static Boolean InActionUpdate;

        static TierGame()
        {
            Actions = new Dictionary<ActionType, List<BaseAction>>();
            QueuedActions = new List<KeyValuePair<ActionType, BaseAction>>();
            InActionUpdate = false;

            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                AddPlayerAction(ActionType.ActionType_JoinGame, BaseAction.Create(player, JoinGameController, Buttons.Start));
            }
            AddPlayerAction(ActionType.ActionType_JoinGame, BaseAction.Create(PlayerIndex.One, JoinGameKeyboard, Keys.Enter));
        }

        public static void AddPlayerAction(ActionType type, BaseAction action)
        {
            if (InActionUpdate)
            {
                QueuedActions.Add(new KeyValuePair<ActionType, BaseAction>(type, action));
            }
            else
            {
                if (Actions.ContainsKey(type))
                {
                    Actions[type].Add(action);
                }
                else
                {
                    List<BaseAction> playerActions = new List<BaseAction>();
                    playerActions.Add(action);

                    Actions.Add(type, playerActions);
                }
            }
        }

        public static void RemovePlayerActions(PlayerIndex player)
        {
            Dictionary<ActionType, List<BaseAction>>.Enumerator iter = Actions.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; )
                {
                    if (iter.Current.Value[i].Player == caller.Player)
                    {
                        iter.Current.Value.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        private static void UpdatePlayerActions()
        {
            InActionUpdate = true;

            Dictionary<ActionType, List<BaseAction>>.Enumerator iter = Actions.GetEnumerator();
            while(iter.MoveNext())
            {
                foreach (BaseAction action in iter.Current.Value)
                {
                    action.Execute(iter.Current.Key);
                    action.Update();
                }
            }

            InActionUpdate = false;

            while(QueuedActions.Count > 0)
            {
                AddPlayerAction(QueuedActions[0].Key, QueuedActions[0].Value);
                QueuedActions.RemoveAt(0);
            }
        }
        #endregion

        public GraphicsDeviceManager Graphics { get; set; }
        private List<MiscObject> Objects;

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
        }

        private void JoinGameController(ActionType type, BaseAction caller)
        {
            AddPlayerAction(ActionType.ActionType_LeaveGame, BaseAction.Create(caller.Player, LeaveGame, Buttons.Back));

            AddPlayerAction(ActionType.ActionType_Fire, BaseAction.Create(caller.Player, ActionHandler, GamePadTrigger.GamePadTrigger_Right, 0.3f));
            AddPlayerAction(ActionType.ActionType_AltFire, BaseAction.Create(caller.Player, ActionHandler, Buttons.RightShoulder));
            AddPlayerAction(ActionType.ActionType_MegaFire, BaseAction.Create(caller.Player, ActionHandler, GamePadTrigger.GamePadTrigger_Right, 0.9f));

            AddPlayerAction(ActionType.ActionType_Exit, BaseAction.Create(caller.Player, ExitHandler, Buttons.Back));
        }

        private void JoinGameKeyboard(ActionType type, BaseAction caller)
        {

        }

        private void LeaveGame(ActionType type, BaseAction caller)
        {
        }

        private void ActionHandler(ActionType type, BaseAction caller)
        {
            Console.WriteLine(caller);
        }

        private void ExitHandler(ActionType type, BaseAction caller)
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
            UpdatePlayerActions();

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
