using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Handlers;
using Tier.Misc;

namespace Tier.Menus
{
  class PauseMenu : MenuState
  {
    public override void Initialize()
    {
      base.Initialize();

      TierGame.TextHandler.ChangeText("headerText", "Pause");

      this.MenuItems.AddItem("Resume Game");
      this.MenuItems.AddItem("Highscores");
      this.MenuItems.AddItem("Options");
      this.MenuItems.AddItem("Controls");
      this.MenuItems.AddItem("Exit Game");
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

#if XBOX360
      if (TierGame.Input.checkKey(Tier.Controls.GamePadKey.A))
      {
#else
      if (TierGame.Input.checkKey(Keys.Enter))
      {
#endif
        switch (this.MenuItems.CurrentOption)
        {
          case 0: // resume
            GameHandler.MenuState = new GameMenu();
            break;
          case 1: // highscores
            GameHandler.MenuState = new HighScoreMenu(this.GetType());
            break;
          case 2: // options
            GameHandler.MenuState = new OptionsMenu(this.GetType());
            break;
          case 3: // controls
            GameHandler.MenuState = new ControlsMenu(this.GetType());
            break;
          case 4: // exit
            GameHandler.Game.Exit();
            break;
        }
#if XBOX360
      }
      else if (TierGame.Input.checkKey(Tier.Controls.GamePadKey.BACK))
#else
      }
      else if (TierGame.Input.checkKey(Keys.Escape))
#endif
        GameHandler.MenuState = new GameMenu();
    }

#if DEBUG
    public override void Draw(GameTime gameTime)
    {
      TierGame.Device.Clear(Options.Colors.PauseMenu.ClearColor);
    }
#endif
  }
}
