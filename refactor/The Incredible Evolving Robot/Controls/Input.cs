using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
    #region Enums & Structs
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

    public abstract class Input
    {
		public Boolean checkKey(GamePadKey key)
		{
			return this.checkKey(key, false);
		}
		public virtual Boolean checkKey(GamePadKey key, Boolean sticky)
        {
            return false;
        }
        public Boolean checkKey(Keys key)
        {
            return this.checkKey(key, false);
        }
        public virtual Boolean checkKey(Keys key, Boolean sticky)
        {
            return false;
        }

        public virtual Boolean checkKeyAction(GameAction action, Boolean sticky)
        {
            return false;
        }
        public virtual void Update()
        {

        }
    }
}
