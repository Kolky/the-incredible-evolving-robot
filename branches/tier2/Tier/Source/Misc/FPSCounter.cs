using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;

namespace Tier.Source.Misc
{
  public class FPSCounter
  {
    #region Privates
    private int fps;
    private int currentFps;
    private int elapsedMillis;
    private TierGame game;
    private Text text;
    #endregion

    public FPSCounter(TierGame game)
    {
      this.game = game;
      this.text = game.TextSpriteHandler.CreateText(
                    "fps",
                    new Vector2(10, 80),
                    Color.Red);
    }

    public void Hide()
    {
      this.text.IsVisible = false;
    }

    public void Show()
    {
      this.text.IsVisible = true;
    }

    public void Update(GameTime gameTime)
    {
      this.currentFps++;
      this.elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;

      if (this.elapsedMillis >= 1000)
      {        
        this.fps = this.currentFps;
        this.elapsedMillis = 0; this.currentFps = 0;

        this.text.Value = String.Format("FPS: {0}", this.fps);
      }
    }
  }
}
