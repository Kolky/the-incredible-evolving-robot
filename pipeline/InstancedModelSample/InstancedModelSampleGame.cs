#region File Description
//-----------------------------------------------------------------------------
// InstancedModelSampleGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Instanced;
#endregion

namespace InstancedModelSample
{
    /// <summary>
    /// Sample showing how to efficiently render many copies of a model, using
    /// hardware instancing to draw more than one copy in a single GPU batch.
    /// </summary>
    public class InstancedModelSampleGame : Microsoft.Xna.Framework.Game
    {
        #region Fields


        GraphicsDeviceManager graphics;
        ContentManager content;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;


        // Instanced model rendering.
        const int InitialInstanceCount = 1000;

        List<SpinningInstance> instances;
        Matrix[] instanceTransforms;
        InstancedModel instancedModel;


        // Measure the framerate.
        int frameRate;
        int frameCounter;
        TimeSpan elapsedTime;


        // Input handling.
        KeyboardState lastKeyboardState;
        GamePadState lastGamePadState;
        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;


        #endregion

        #region Initialization


        public InstancedModelSampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            graphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;

            // Most games will want to leave both these values set to true to ensure
            // smoother updates, but when you are doing performance work it can be
            // useful to set them to false in order to get more accurate measurements.
            IsFixedTimeStep = false;

            graphics.SynchronizeWithVerticalRetrace = false;

            // Initialize the list of instances.
            instances = new List<SpinningInstance>();

            for (int i = 0; i < InitialInstanceCount; i++)
                instances.Add(new SpinningInstance());
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                
                spriteFont = content.Load<SpriteFont>("Content/Font");

                instancedModel = content.Load<InstancedModel>("Content/Models/rocket");
            }
        }
      

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            // Update the position of each spinning instance.
            foreach (SpinningInstance instance in instances)
            {
                instance.Update(gameTime);
            }

            // Measure our framerate.
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            } 
            
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            // Calculate camera matrices.
            Viewport viewport = device.Viewport;

            float aspectRatio = (float)viewport.Width / (float)viewport.Height;

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 15),
                                              Vector3.Zero, Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    1, 
                                                                    100);

            // Set renderstates for drawing 3D models.
            RenderState renderState = device.RenderState;

            renderState.DepthBufferEnable = true;
            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;

            // Gather instance transform matrices into a single array.
            Array.Resize(ref instanceTransforms, instances.Count);

            for (int i = 0; i < instances.Count; i++)
            {
                instanceTransforms[i] = instances[i].Transform;
            }

            // Draw all the instances in a single call.
            instancedModel.DrawInstances(instanceTransforms, view, projection);

            DrawOverlayText();
            
            // Measure our framerate.
            frameCounter++;

            base.Draw(gameTime);
        }


        /// <summary>
        /// Helper for drawing the help text overlay.
        /// </summary>
        void DrawOverlayText()
        {
            string text = string.Format("Frames per second: {0}\n" +
                                        "Instances: {1}\n" +
                                        "Technique: {2}\n\n" +
                                        "A = Change technique\n" +
                                        "X = Add instances\n" +
                                        "Y = Remove instances\n",
                                        frameRate,
                                        instances.Count,
                                        instancedModel.InstancingTechnique);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, text, new Vector2(65, 65), Color.Black);
            spriteBatch.DrawString(spriteFont, text, new Vector2(64, 64), Color.White);

            spriteBatch.End();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Handles input for quitting or changing settings.
        /// </summary>
        void HandleInput()
        {
            lastKeyboardState = currentKeyboardState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            // Change the number of instances more quickly if there are
            // already lots of them. This avoids you having to sit there
            // for hours with your finger on the "increase" button!
            int instanceChangeRate = Math.Max(instances.Count / 100, 1);

            // Increase the number of instances?
            if (currentKeyboardState.IsKeyDown(Keys.X) ||
                currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                for (int i = 0; i < instanceChangeRate; i++)
                {
                    instances.Add(new SpinningInstance());
                }
            }

            // Decrease the number of instances?
            if (currentKeyboardState.IsKeyDown(Keys.Y) ||
                currentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                for (int i = 0; i < instanceChangeRate; i++)
                {
                    if (instances.Count == 0)
                        break;

                    instances.RemoveAt(instances.Count - 1);
                }
            }

            // Change which instancing technique we are using?
            if ((currentKeyboardState.IsKeyDown(Keys.A) &&
                 lastKeyboardState.IsKeyUp(Keys.A)) ||
                (currentGamePadState.Buttons.A == ButtonState.Pressed &&
                 lastGamePadState.Buttons.A == ButtonState.Released))
            {
                InstancingTechnique technique = instancedModel.InstancingTechnique;

                // Look for the next valid technique.
                do
                {
                    technique++;

                    // Wrap if we reach the end of the possible techniques.
                    if (technique > InstancingTechnique.NoInstancingOrStateBatching)
                        technique = 0;
                }
                while (!instancedModel.IsTechniqueSupported(technique));

                instancedModel.SetInstancingTechnique(technique);
            }
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (InstancedModelSampleGame game = new InstancedModelSampleGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
