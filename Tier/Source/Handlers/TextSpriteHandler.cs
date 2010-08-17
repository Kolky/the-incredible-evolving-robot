using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Handlers
{
  public class TextSpriteHandler
  {
    #region Privates
    private TierGame game;
    private List<Text> texts;
    private List<Sprite> sprites;
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    #endregion

    public TextSpriteHandler(TierGame game)
    {
      this.game = game;
      this.texts = new List<Text>();
      this.sprites = new List<Sprite>();
    }

    public void AddText(Text text)
    {
      lock (this)
      {
        this.texts.Add(text);
      }
    }

    public Text CreateText(string text, Vector2 position, Color color)
    {
      lock (this)
      {
        Text t = new Text(text, position, color);
        texts.Add(t);

        return t;
      }
    }

    public Sprite CreateSprite(Texture2D texture, Vector2 position)
    {
      lock (this)
      {
        Sprite s = new Sprite(texture, position);
        this.sprites.Add(s);

        return s;
      }
    }

    public void Draw(GameTime gameTime)
    {
      lock (this)
      {
        this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

        // Draw sprites
        foreach (Sprite s in this.sprites)
        {
          if (!s.IsVisible)
            continue;

          switch (s.Type)
          {
            case SpriteType.NORMAL:
              this.spriteBatch.Draw(s.Texture, s.Position, null, s.Color, s.Rotation, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
              break;
            case SpriteType.DESTINATION_RECTANGLE:
              this.spriteBatch.Draw(s.Texture, s.Rectangle, null, s.Color, s.Rotation, s.Origin, SpriteEffects.None, s.Depth);
              break;
          }
        }

        // Draw 2d text
        foreach (Text t in this.texts)
        {
          if (!t.IsVisible)
            continue;

          this.spriteBatch.DrawString(this.font, t.Value, t.Position, t.Color, 0, Vector2.Zero, t.Scale,
            SpriteEffects.None, 0);
        }

        this.spriteBatch.End();
      }
    }

    public SpriteBatch GetSpriteBatch()
    {
      return this.spriteBatch;
    }

    public void RemoveSprite(Sprite s)
    {
      this.sprites.Remove(s);
    }

    public void RemoveText(Text t)
    {
      t.Dispose();
      this.texts.Remove(t);
    }

    public void Initialize()
    {
      this.spriteBatch = new SpriteBatch(game.GraphicsDevice);      
      this.font = game.Content.Load<SpriteFont>("Fonts//Bold"); 
        //game.ContentHandler.GetAsset<SpriteFont>("Bold");
    }
  }
}
