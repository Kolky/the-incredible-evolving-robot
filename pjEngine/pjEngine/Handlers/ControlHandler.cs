using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace pjEngine.Handlers
{
  public enum ControlType
  {
    Windows, Xbox360
  };

  public class ControlHandler : GameComponent
  {
    private ControlType type;
    private KeyboardState currentKeyboardState, previousKeyboardState;
    private GamePadState currentGamePadState, previousGamePadState;

    public ControlHandler(Game game, ControlType type)
      : base(game)
    {
      this.type = type;
    }

    public override void Update(GameTime gameTime)
    {
      switch (type)
      {
        case ControlType.Windows:
          updateWindows();
          break;
        case ControlType.Xbox360:
          break;
        default:
          break;
      }
    }

    private void updateWindows()
    {
      this.currentKeyboardState = this.previousKeyboardState;
      this.currentKeyboardState = Keyboard.GetState();
    }

    private void updateXbox360()
    { }

    public bool CheckKey(Keys key)
    {
      return CheckKey(key, false);
    }

    public bool CheckKey(Keys key, bool sticky)
    {
      if (sticky)
      {
        foreach (Keys k in this.currentKeyboardState.GetPressedKeys())
        {
          if (k == key)
            return true;
        }

        return false;
      }
      else
      {
        return (this.currentKeyboardState.IsKeyDown(key) && this.previousKeyboardState.IsKeyUp(key));
      }
    }
  }
}
