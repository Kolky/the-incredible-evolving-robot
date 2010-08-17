using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers
{
  public class TutorialText
  {
    #region Properties
    private int ttl;
    private Text text;
    private enum TutorialTextState
    {
      TTS_DONE, TTS_FADEIN, TTS_FADEOUT, TTS_SHOWTEXT
    }
    private TutorialTextState state;
    private int timeElapsed;
    private bool isDone;
    private TierGame game;

    public bool IsDone
    {
      get { return isDone; }
    }
    #endregion

    private Color colorFadedIn, colorFadedOut;
    private bool isSticky;

    public TutorialText(TierGame game, Text text, int ttl, bool isSticky)
    {
      this.game = game;
      this.text = text;
      this.ttl = ttl;
      this.colorFadedIn = new Color(255, 255, 255, 255);
      this.colorFadedOut = new Color(255, 255, 255, 0);
      this.isSticky = isSticky;
    }

    private void ChangeState(TutorialTextState newstate)
    {
      switch (newstate)
      {
        case TutorialTextState.TTS_DONE:
          this.game.TextSpriteHandler.RemoveText(this.text);
          break;
        case TutorialTextState.TTS_FADEIN:
          this.text.IsVisible = true;
          break;
        case TutorialTextState.TTS_FADEOUT:
          break;
        case TutorialTextState.TTS_SHOWTEXT:
          break;
        default:
          break;
      }

      this.state = newstate;
      this.timeElapsed = 0;
    }

    public void Remove()
    {
      this.text.IsVisible = false;
      this.game.TextSpriteHandler.RemoveText(this.text);
    }
    
    public void Start()
    {
      ChangeState(TutorialTextState.TTS_FADEIN);
    }

    public void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (this.state)
      {
        case TutorialTextState.TTS_DONE:
          UpdateDone(gameTime);
          break;
        case TutorialTextState.TTS_FADEIN:
          UpdateFadeIn(gameTime);
          break;
        case TutorialTextState.TTS_SHOWTEXT:
          UpdateShowText(gameTime);
          break;
        case TutorialTextState.TTS_FADEOUT:
          UpdateFadeOut(gameTime);
          break;
      }
    }

    private void UpdateDone(GameTime gameTime)
    {
      if (this.timeElapsed >= 500)
      {
        this.isDone = true;
      }
    }

    private void UpdateFadeIn(GameTime gameTime)
    {
      if (this.timeElapsed >= 500)
      {
        ChangeState(TutorialTextState.TTS_SHOWTEXT);
        return;
      }

      float amount = this.timeElapsed / 500.0f;
      this.text.Color = Color.Lerp(colorFadedOut, colorFadedIn, amount);
    }

    private void UpdateFadeOut(GameTime gameTime)
    {
      if (this.timeElapsed >= 500)
      {
        ChangeState(TutorialTextState.TTS_DONE);
        return;
      }

      float amount = this.timeElapsed / 500.0f;
      this.text.Color = Color.Lerp(colorFadedIn, colorFadedOut, amount);
    }

    private void UpdateShowText(GameTime gameTime)
    {
      if (this.isSticky)
      {
      }
      else if (this.timeElapsed >= this.ttl)
      {
        ChangeState(TutorialTextState.TTS_FADEOUT);
      }
    }

    public void Unstick()
    {
      this.isSticky = false;
    }
  }
}
