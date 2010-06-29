using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;

namespace Tier.Menus
{
    class OptionsMenu : MenuState
    {
        #region Properties
        private Type type;
        #endregion

        public OptionsMenu(Type type)
        {
            this.type = type;
        }

        public override void Initialize()
        {
            base.Initialize();

            TierGame.TextHandler.ChangeText("headerText", "Options");

            if (Options.Settings.BGMVolume > 0)
                this.MenuItems.AddItem("Music", "Music On");
            else
                this.MenuItems.AddItem("Music", "Music Off");

            if (Options.Settings.SFXVolume > 0)
                this.MenuItems.AddItem("SFX", "SFX On");
            else
                this.MenuItems.AddItem("SFX", "SFX Off");

            if (TierGame.Graphics.IsFullScreen)
                this.MenuItems.AddItem("Fullscreen", "Fullscreen On");
            else
                this.MenuItems.AddItem("Fullscreen", "Fullscreen Off");


            this.MenuItems.AddItem("Back");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if ((TierGame.Input.GetType() == typeof(InputXBOX) && TierGame.Input.checkKey(Tier.Controls.GamePadKey.A)) || TierGame.Input.checkKey(Keys.Enter))
            {
                switch (this.MenuItems.CurrentOption)
                {
                    case 0: // start
                        if (Options.Settings.BGMVolume > 0)
                        {
                            Options.Settings.BGMVolume = 0;
                            this.MenuItems.ChangeItemText("Music", "Music Off");
                        }
                        else
                        {
                            Options.Settings.BGMVolume = 1f;
                            this.MenuItems.ChangeItemText("Music", "Music On");
                        }
                        //TierGame.Audio.setVolumeBGM(Options.Settings.BGMVolume);
                        break;
                    case 1: // start
                        if (Options.Settings.SFXVolume > 0)
                        {
                            Options.Settings.SFXVolume = 0;
                            //TierGame.Audio.setVolumeSFX(0f);
                            this.MenuItems.ChangeItemText("SFX", "SFX Off");
                        }
                        else
                        {
                            Options.Settings.SFXVolume = 1f;
                            this.MenuItems.ChangeItemText("SFX", "SFX On");
                        }
                        //TierGame.Audio.setVolumeSFX(Options.Settings.SFXVolume);
                        break;
                    case 2:
                        TierGame.Graphics.IsFullScreen = !TierGame.Graphics.IsFullScreen;
                        if (TierGame.Graphics.IsFullScreen)
                            this.MenuItems.ChangeItemText("Fullscreen", "Fullscreen On");
                        else
                            this.MenuItems.ChangeItemText("Fullscreen", "Fullscreen Off");
                        TierGame.Graphics.ApplyChanges();
                        break;
                    case 3: // exit
                        if (this.type == typeof(StartMenu))
                            GameHandler.MenuState = new StartMenu();
                        else if (this.type == typeof(PauseMenu))
                            GameHandler.MenuState = new PauseMenu();
                        else if (this.type == typeof(GameOverMenu))
                            GameHandler.MenuState = new GameOverMenu();
                        break;
                }
            }
            else if ((TierGame.Input.GetType() == typeof(InputXBOX) && TierGame.Input.checkKey(Tier.Controls.GamePadKey.BACK)) || TierGame.Input.checkKey(Keys.Escape))
            {
                if (this.type == typeof(StartMenu))
                    GameHandler.MenuState = new StartMenu();
                else if (this.type == typeof(PauseMenu))
                    GameHandler.MenuState = new PauseMenu();
                else if (this.type == typeof(GameOverMenu))
                    GameHandler.MenuState = new GameOverMenu();
            }
        }


#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            TierGame.Device.Clear(Options.Colors.OptionsMenu.ClearColor);
        }
#endif
    }
}