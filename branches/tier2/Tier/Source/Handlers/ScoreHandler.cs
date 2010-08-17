using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Handlers
{
  public class ScoreHandler
  {
    #region Privates
    private int score;
    private Text text;
    private TierGame game;
    private List<VisualScore> visualscores;
    private bool isVisible;

    public bool IsVisible
    {
      get 
      {
        if (this.text == null)
          return false;

        return this.text.IsVisible; 
      }
    }
    #endregion 

    public ScoreHandler(TierGame game)
    {
      this.game = game;
      this.score = 0;
      this.visualscores = new List<VisualScore>();
    }

    public void AddScore(int score)
    {
      this.score += score;
      this.text.Value = String.Format("Score: {0}", this.score);
    }

    public void AddVisualScore(VisualScore score)
    {
      this.visualscores.Add(score);
    }

    public void Hide()
    {
      if(text != null)
        text.IsVisible = false;
    }

    public void Reset()
    {
      this.score = 0;
    }
    
    public void Start()
    {
      if (this.text == null)
      {
        this.text = game.TextSpriteHandler.CreateText(
          "Score: 0",
          new Vector2(10, 40), Color.Yellow);
      }

      this.text.Value = "Score: 0";
      this.text.IsVisible = true;
    }

    public void Stop()
    {
      this.text.IsVisible = false;      
    }

    public void Show()
    {
      Start();
      text.IsVisible = true;
    }

    public void Update(GameTime gameTime)
    {
      this.text.Value = String.Format("Score: {0}", this.score);
      
      foreach (VisualScore score in this.visualscores)
      {
        score.Update(gameTime);
      }

      if (this.visualscores.Count > 0 && this.visualscores[0].IsDone)
      {
        this.game.TextSpriteHandler.RemoveText(this.visualscores[0].Text);
        this.visualscores.RemoveAt(0);
      }
    }
  }
}
