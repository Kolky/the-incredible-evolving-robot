using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Handlers;

namespace Tier.Source.Helpers
{
  public class VisualScore
  {
    #region Properties
    private bool isDone;
    private Vector2 movement;
    private int score;
    private Text text;

    public Text Text
    {
      get { return text; }
      set { text = value; }
    }
	
    public int Score
    {
      get { return score; }
      set { score = value; }
    }
	
    public bool IsDone
    {
      get { return isDone; }
      set { isDone = value; }
    }
    #endregion

    private float timeElapsed;
    private ScoreHandler handler;

    public VisualScore(string text, Vector2 position, Color color, TierGame game)
    {
      this.text = game.TextSpriteHandler.CreateText(text, position, color);
      this.movement = game.Options.ScorePosition - position;
      this.timeElapsed = 0;
      this.isDone = false;
      this.handler = game.ScoreHandler;
    }

    public void Update(GameTime gameTime)
    {
      if (this.isDone)
        return;

      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      // Move to position of score
      this.text.Position += movement * (gameTime.ElapsedGameTime.Milliseconds / 1500.0f);

      if (this.timeElapsed >= 1500.0f)
      {
        this.isDone = true;
        this.handler.AddScore(this.score);
      }
    }
  }
}
