using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
  #region Enums & Structs
#if XBOX360
  public enum GamePadTrigger
	{
		LEFT,
		RIGHT
	}
	public enum GamePadDirection
	{
		DOWN,
		LEFT,
		LEFTDOWN,
		LEFTUP,
		NONE,
		RIGHT,
		RIGHTDOWN,
		RIGHTUP,
		UP
	}
	public enum GamePadStick
	{
		LEFT,
		RIGHT
	}
  public enum GamePadKey
  {
    A,
    B,
    BACK,
    DPADDOWN,
    DPADLEFT,
    DPADRIGHT,
    DPADUP,
    LEFTSHOULDER,
    LEFTSTICK,
    RIGHTSHOULDER,
    RIGHTSTICK,
    START,
    X,
    Y
  }
#else
  public enum MouseKey
  {
    LEFTBUTTON,
    MIDDLEBUTTON,
    RIGHTBUTTON
  }
  public enum MouseDirection
  {
    DOWN,
    LEFT,
    LEFTDOWN,
    LEFTUP,
    NONE,
    RIGHT,
    RIGHTDOWN,
    RIGHTUP,
    UP,
  }
  public struct MouseMovement
  {
    public int X;
    public int Y;
  }
#endif

	public enum GameAction
	{
		FIRE,
		ROLL_LEFT,
		ROLL_RIGHT,
		MOVE_UP,
		MOVE_DOWN,
		MOVE_LEFT,
		MOVE_RIGHT,
		MOVE_FORWARD,
		MOVE_BACKWARD,
		SPREAD_INCREASE,
		SPREAD_DECREASE,
		SPREAD_UP,
		SPREAD_DOWN,
		SPREAD_LEFT,
		SPREAD_RIGHT,
    GROW_BOSS
	};
  #endregion

  abstract public class Input
  {
#if XBOX360
		public Boolean checkKey(GamePadKey key)
		{
			return this.checkKey(key, false);
		}
		abstract public Boolean checkKey(GamePadKey key, Boolean sticky);
#else
    public Boolean checkKey(Keys key)
    {
      return this.checkKey(key, false);
    }
    abstract public Boolean checkKey(Keys key, Boolean sticky);
#endif

    abstract public Boolean checkKeyAction(GameAction action, Boolean sticky);
		abstract public void Update();

  }
}
