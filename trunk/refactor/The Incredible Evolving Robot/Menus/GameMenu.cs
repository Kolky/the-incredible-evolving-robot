using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Tier.Handlers;
using Tier.Misc;
using Tier.Controls;

namespace Tier.Menus
{
    class GameMenu : MenuState
    {
        public GameMenu()
        {
            GameHandler.HUD.Start();
        }

        public override void Update(GameTime gameTime)
        {
            GameHandler.HUD.Update(gameTime);

            if ((TierGame.Input.GetType() == typeof(InputXBOX) && TierGame.Input.checkKey(Tier.Controls.GamePadKey.BACK)) || TierGame.Input.checkKey(Keys.Escape))
            {
                GameHandler.MenuState = new PauseMenu();
            }
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            TierGame.Device.Clear(Options.Colors.GameMenu.ClearColor);
#else
            base.Draw(gameTime);
#endif

            GameHandler.HUD.Draw(gameTime);
        }
    }
}
