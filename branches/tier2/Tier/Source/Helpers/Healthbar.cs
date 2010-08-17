using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Helpers
{
  public class Healthbar
  {
    #region Properties
    private Player player;
    private Sprite sprite;
    private TierGame game;
    private Vector2 screenSize;
    #endregion

    public Healthbar(Player player, TierGame game)
    {
      this.player = player;
      this.screenSize = new Vector2(game.GraphicsDevice.PresentationParameters.BackBufferWidth,
        game.GraphicsDevice.PresentationParameters.BackBufferHeight);
      this.game = game;
      this.sprite = game.TextSpriteHandler.CreateSprite(game.ContentHandler.GetAsset<Texture2D>("Healthbar"), new Vector2(10, 10));
      this.sprite.Type = SpriteType.DESTINATION_RECTANGLE;      
      Hide();
    }

    public void Show()
    {
      this.sprite.IsVisible = true;
    }

    public void Hide()
    {
      this.sprite.IsVisible = false;
    }

    public void Update(GameTime gameTime)
    {
      if (this.player.DestroyableModifier == null)
        return;

      if (this.player.DestroyableModifier.Health <= 25)
      {
        this.sprite.Color = Color.Red;
      }
      else if (this.player.DestroyableModifier.Health <= 50)
      {
        this.sprite.Color = Color.Gold;
      }
      else
        this.sprite.Color = Color.GreenYellow;

      float percentage = this.player.DestroyableModifier.Health / 100.0f;
      int width = (int)((this.screenSize.X - 20) * percentage);
      this.sprite.Rectangle = new Rectangle(10, 10, width, 25);
    }
  }
}
