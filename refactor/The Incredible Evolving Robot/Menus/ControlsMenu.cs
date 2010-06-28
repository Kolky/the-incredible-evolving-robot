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
  class ControlsMenu : MenuState
  {
    #region Properties
    private Type type;
    #endregion

    public ControlsMenu(Type type)
    {
      this.type = type;
    }

    public override void Initialize()
    {
      base.Initialize();

      TierGame.TextHandler.ChangeText("headerText", "Controls");

      this.MenuItems.AddItem("Back");
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

#if XBOX360
      if (TierGame.InputXBOX.checkKey(GamePadKey.A) || TierGame.InputXBOX.checkKey(GamePadKey.BACK))
      {
#else
      if (TierGame.Input.checkKey(Keys.Enter) || TierGame.Input.checkKey(Keys.Escape))
      {
#endif
        if (this.type == typeof(StartMenu))
          GameHandler.MenuState = new StartMenu();
        else if (this.type == typeof(PauseMenu))
          GameHandler.MenuState = new PauseMenu();
        else if (this.type == typeof(GameOverMenu))
          GameHandler.MenuState = new GameOverMenu();
        else if (this.type == typeof(NextLevelMenu))
          GameHandler.MenuState = new NextLevelMenu();
      }
    }

#if DEBUG
    public override void Draw(GameTime gameTime)
    {
      TierGame.Device.Clear(Options.Colors.ControlsMenu.ClearColor);
    }
#endif
  }
}