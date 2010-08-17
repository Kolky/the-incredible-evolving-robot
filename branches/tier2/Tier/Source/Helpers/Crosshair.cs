using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.Helpers
{
  public class Crosshair : GameObject
  {
    #region Properties
    // this constant controls how fast the gamepad moves the cursor. this constant
    // is in pixels per second.
    private const float CursorSpeed = 250.0f;
    private Sprite sprite;

    public new Vector2 Position
    {
      get { return sprite.Position; }
      set { sprite.Position = value; }
    }
    #endregion

    public Crosshair(TierGame game)
      : base(game)
    {      
    }

    public void Hide()
    {
      this.sprite.IsVisible = false;
    }

    public override void Initialize()
    {
      base.Initialize();

      Texture2D texCrosshair = this.Game.ContentHandler.GetAsset<Texture2D>("crosshair");

      this.sprite = this.Game.TextSpriteHandler.CreateSprite(
        texCrosshair,
        new Vector2(
          this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, 
          this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2));

      this.sprite.Origin = new Vector2(texCrosshair.Width / 2, texCrosshair.Height / 2);
      this.sprite.Type = SpriteType.NORMAL;
      this.sprite.Color = Color.Red;
      this.sprite.IsVisible = true;
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    { }

    public void Show()
    {
      this.sprite.IsVisible = true;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      GamePadState state = GamePad.GetState(this.Game.MainControllerIndex);
      this.sprite.Position += state.ThumbSticks.Right * new Vector2(20, -20);

      Vector2 delta = Vector2.Zero;
      Vector2 position = this.sprite.Position;

      if (this.Game.IsActive)
      {
        Viewport vp = this.GraphicsDevice.Viewport;

        position += delta * CursorSpeed *
          (float)gameTime.ElapsedGameTime.TotalSeconds;
        position.X = MathHelper.Clamp(position.X, vp.X, vp.X + vp.Width);
        position.Y = MathHelper.Clamp(position.Y, vp.Y, vp.Y + vp.Height);
      }

      this.sprite.Position = position;
    }
  }
}
