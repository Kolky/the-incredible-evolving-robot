using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;

namespace Tier.Menus
{
    class HighScoreMenu : MenuState
    {
        #region Properties
        private Type type;
        private List<String> scoreEntries = new List<String>();
        #endregion

        public HighScoreMenu(Type type)
        {
            this.type = type;
        }

        public override void Initialize()
        {
            base.Initialize();

            TierGame.TextHandler.ChangeText("headerText", "Highscores");

            this.MenuItems.AddItem("Back");

            float vert = 210f;

            this.scoreEntries.Add("player");
            this.scoreEntries.Add("score");
            TierGame.TextHandler.AddItem("player", "Player", new Vector2(this.Center - 100 - TierGame.TextHandler.GetTextWidth("player").Y, 180f), Options.Colors.HighScoreMenu.HeaderColor, true, false);
            TierGame.TextHandler.AddItem("score", "Score", new Vector2(this.Center + 100 + TierGame.TextHandler.GetTextWidth("score").Y, 180f), Options.Colors.HighScoreMenu.HeaderColor, true, false);

            LinkedList<HighScores.HighScoreEntry>.Enumerator iter = HighScores.ListEntries().GetEnumerator();
            while (iter.MoveNext())
            {
                this.scoreEntries.Add("player" + iter.Current.Player + iter.Current.Score);
                this.scoreEntries.Add("score" + iter.Current.Player + iter.Current.Score);
                TierGame.TextHandler.AddItem("player" + iter.Current.Player + iter.Current.Score, iter.Current.Player, new Vector2(this.Center - 100 - TierGame.TextHandler.GetTextWidth("player" + iter.Current.Player + iter.Current.Score).Y, vert), Options.Colors.HighScoreMenu.ScoreColor, true, false);
                TierGame.TextHandler.AddItem("score" + iter.Current.Player + iter.Current.Score, iter.Current.Score.ToString(), new Vector2(this.Center + 100 + TierGame.TextHandler.GetTextWidth("score" + iter.Current.Player + iter.Current.Score).Y, vert), Options.Colors.HighScoreMenu.ScoreColor, true, false);
                vert += 30f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if ((TierGame.Input.GetType() == typeof(InputXBOX) && TierGame.InputXBOX.checkKey(GamePadKey.A) || TierGame.InputXBOX.checkKey(GamePadKey.BACK)) || TierGame.Input.checkKey(Keys.Enter) || TierGame.Input.checkKey(Keys.Escape))
            {
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
            TierGame.Device.Clear(Options.Colors.HighScoreMenu.ClearColor);
        }
#endif

        public override void Dispose()
        {
            base.Dispose();

            foreach (String entry in this.scoreEntries)
                TierGame.TextHandler.RemoveItem(entry);
        }
    }
}
