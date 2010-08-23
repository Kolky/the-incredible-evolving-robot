using System;
using System.Collections.Generic;
using System.Text;
using Tier.Objects.Destroyable;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Menus;

namespace Tier.Misc
{
  public class HUD
  {
    #region Properties
    private int timeLeft;
    public int TimeLeft
    {
      get { return timeLeft; }
      set
      {
        if (value > 1000)
          timeLeft = 999;
        else if (value < 0)
          timeLeft = 0;
        else
          timeLeft = value;
      }
    }

    private int totalTime;
    public int TotalTime
    {
      get { return totalTime; }
    }	

    private int score;
    public int Score
    {
      get { return score; }
    }

    private int elapsedMs = 0;
    private int hits = 0;
    
    #endregion		

    public HUD()
		{
      this.TimeLeft = Options.Game.StartTime;
      this.totalTime = 0;
		}

    public void Start()
    {
#if DEBUG
			TierGame.TextHandler.AddItem("hit", "Hit: " + this.hits + " ", new Vector2(10,70), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
#endif

      TierGame.TextHandler.AddItem("score", "Score: " + String.Format("{0:000000}", this.Score), new Vector2(10, TierGame.Instance.Window.ClientBounds.Height - 35), Color.Red);
      TierGame.TextHandler.AddItem("timeLeft", "Time: " + String.Format("{0:000}", this.TimeLeft) + " ", Vector2.Zero, Color.Red);
      TierGame.TextHandler.ChangePosition("timeLeft", new Vector2(TierGame.Instance.Window.ClientBounds.Width - (int)(TierGame.TextHandler.GetTextWidth("timeLeft").X), TierGame.Instance.Window.ClientBounds.Height - 35));
    }

		public void HitUpdate()
		{
#if DEBUG
			TierGame.TextHandler.ChangeText("hit", "Hit: " + (++this.hits));
#endif
		}

    public void AddScore(int points)
    {
      this.score += points;
      TierGame.TextHandler.ChangeText("score", "Score: " + String.Format("{0:000000}", this.score));
    }

		public void AddTime(int timeSec)
		{
			this.TimeLeft += timeSec;
		}

    public void End()
    {
      this.TimeLeft = Options.Game.StartTime;
      this.score = 0;
      TierGame.TextHandler.RemoveItem("timeLeft");
    }

		/// <summary>takes penalty at the timer</summary>
		/// <returns>wether no time left</returns>
		public void Die()
		{
      if (this.TimeLeft > Options.Player.DiePenalty)
        this.TimeLeft -= Options.Player.DiePenalty;
			else
        this.TimeLeft = 0;
    }

		public Boolean HasTimeLeft()
		{
      return (this.TimeLeft > 0);
		}

    public void Update(GameTime gameTime)
    {
      this.elapsedMs += gameTime.ElapsedGameTime.Milliseconds;
      if (this.elapsedMs >= 1000)
      {
        //TimeLeft
        this.elapsedMs -= 1000;

        if (this.TimeLeft > 0)
        {
          this.TimeLeft--;
          this.totalTime++;
          this.score += 1;
        }
      }

      if (!this.HasTimeLeft())
        GameHandler.MenuState = new GameOverMenu();

      TierGame.TextHandler.ChangeText("score", "Score: " + String.Format("{0:000000}", this.score));
      TierGame.TextHandler.ChangeText("timeLeft", "Time: " + String.Format("{0:000}", this.timeLeft) + " ");
    }

    public void Draw(GameTime gameTime)
    {
    }
  }
}
