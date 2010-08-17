using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Handlers
{
  public class TutorialTextHandler
  {
    private TierGame game;
    private Queue<TutorialText> texts;
    private TutorialText currentText;

    public TutorialTextHandler(TierGame game)
    {
      this.game = game;
      this.texts = new Queue<TutorialText>();
      this.currentText = null;
    }

    public void AddText(string text, int ttl)
    {
      AddText(text, ttl, false);
    }

    public void AddText(string text, int ttl, bool isSticky)
    {
      Text t = this.game.TextSpriteHandler.CreateText(
        text,
        new Vector2(this.game.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.1f,
                    this.game.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.35f),
        Color.Black);
      t.IsVisible = false;

      if (t != null)
      {
        TutorialText tutorialText = new TutorialText(this.game, t, ttl, isSticky);
        texts.Enqueue(tutorialText);
      }
    }

    public void Stop()
    {
      if(currentText != null)
        currentText.Remove();

      UnstickCurrentText();
      while (texts.Count > 0)
      {
        TutorialText t = texts.Dequeue();
        t.Remove();        
      }
    }

    public void UnstickCurrentText()
    {
      if(this.currentText != null)
        this.currentText.Unstick();
    }

    public void Update(GameTime gameTime)
    {
      if (this.currentText == null &&
        this.texts.Count > 0)
      {
        this.currentText = texts.Dequeue();
        this.currentText.Start();
      }

      if (this.currentText != null)
      {
        if (this.currentText.IsDone)
        {
          this.currentText = null;
          return;
        }

        this.currentText.Update(gameTime);
      }
    }
  }
}
