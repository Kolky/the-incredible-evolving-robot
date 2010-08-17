using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.GameStates
{
  public class ControlsState : GameState
  {
    private Sprite background;

    public ControlsState(TierGame game)
      : base(game)
    {
      this.game.ContentHandler.Load("Content//Xml//DataCollections//ControlsState.xml", "ControlsState");
    }

    public override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(this.game.MainControllerIndex).Buttons.Back == ButtonState.Pressed)
      {
        this.game.ChangeState(this.game.TitleScreenState);
      }    
    }

    public override void Enter(GameState previousState)
    {
      Texture2D tex = this.game.ContentHandler.GetAsset<Texture2D>("controls");
      background = this.game.TextSpriteHandler.CreateSprite(null, Vector2.Zero);        

      background.Rectangle = new Rectangle(0, 0,
        this.game.GraphicsDevice.PresentationParameters.BackBufferWidth,
        this.game.GraphicsDevice.PresentationParameters.BackBufferHeight);
      background.Type = SpriteType.DESTINATION_RECTANGLE;
      background.Texture = tex;
    }

    public override void Leave()
    {
      this.game.TextSpriteHandler.RemoveSprite(this.background);
//      this.game.ContentHandler.UnLoad("ControlsState");      
    }
  }
}
