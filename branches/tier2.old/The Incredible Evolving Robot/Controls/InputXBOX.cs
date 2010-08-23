using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
#if XBOX360
	public class InputXBOX : Input
	{
		private PlayerIndex playerIndex;
		private List<GamePadKey> lastState;
		private float lastTriggerLeft, lastTriggerRight;

    public InputXBOX()
      : this(PlayerIndex.One)
    {
    }

		public InputXBOX(PlayerIndex player)
		{
			this.playerIndex = player;
			this.lastState = new List<GamePadKey>();
			this.lastTriggerLeft = 0.0f;
			this.lastTriggerRight = 0.0f;
		}

    #region Vibration
    public Boolean setVibration(float left, float right)
    {
      if (GamePad.GetState(this.playerIndex).IsConnected)
        return GamePad.SetVibration(this.playerIndex, left, right);
      else
        return false;
    }
    #endregion

    #region Trigger
    public Boolean checkTrigger(GamePadTrigger trigger)
    {
      return this.checkTrigger(trigger, false);
    }
    public Boolean checkTrigger(GamePadTrigger trigger, Boolean sticky)
    {
      if (GamePad.GetState(this.playerIndex).IsConnected)
      {
        switch (trigger)
        {
          case GamePadTrigger.LEFT:
            if (sticky)
              return (GamePad.GetState(this.playerIndex).Triggers.Left > 0.0f ? true : false);
            else
            {
              if (this.lastTriggerLeft == 0.0f || this.lastTriggerLeft == 0.0 || this.lastTriggerLeft == 0)
                return (GamePad.GetState(this.playerIndex).Triggers.Left > 0.0f ? true : false);
              else
                return false;
            }
          case GamePadTrigger.RIGHT:
            if (sticky)
              return (GamePad.GetState(this.playerIndex).Triggers.Right > 0.0f ? true : false);
            else
            {
              if (this.lastTriggerRight == 0.0f || this.lastTriggerRight == 0.0 || this.lastTriggerRight == 0)
                return (GamePad.GetState(this.playerIndex).Triggers.Right > 0.0f ? true : false);
              else
                return false;
            }
          default:
            return false;
        }
      }
      else
        return false;
    }
    public float getTriggerDepth(GamePadTrigger trigger)
    {
      if (GamePad.GetState(this.playerIndex).IsConnected)
      {
        switch (trigger)
        {
          case GamePadTrigger.LEFT:
            return GamePad.GetState(this.playerIndex).Triggers.Left;
          case GamePadTrigger.RIGHT:
            return GamePad.GetState(this.playerIndex).Triggers.Right;
          default:
            return 0.0f;
        }
      }
      else
        return 0.0f;
    }
    #endregion

		#region Thumbstick
		private GamePadDirection getDirection(float x, float y)
		{
			if (x > 0.0f)
			{
				if (y > 0.0f)
					return GamePadDirection.RIGHTUP;
				else if (y < 0.0f)
					return GamePadDirection.RIGHTDOWN;
				else if (y == 0.0f)
					return GamePadDirection.RIGHT;
				else
					return GamePadDirection.NONE;
			}
			else if (x < 0.0f)
			{
				if (y > 0.0f)
					return GamePadDirection.LEFTUP;
				else if (y < 0.0f)
					return GamePadDirection.LEFTDOWN;
				else if (y == 0.0f)
					return GamePadDirection.LEFT;
				else
					return GamePadDirection.NONE;
			}
			else if (x == 0.0f)
			{
				if (y > 0.0f)
					return GamePadDirection.UP;
				else if (y < 0.0f)
					return GamePadDirection.DOWN;
				else if (y == 0.0f)
					return GamePadDirection.NONE;
				else
					return GamePadDirection.NONE;
			}
			else
				return GamePadDirection.NONE;
		}
		private GamePadDirection getStickDirection(GamePadStick stick)
		{
			if (GamePad.GetState(this.playerIndex).IsConnected)
			{
				switch (stick)
				{
					case GamePadStick.LEFT:
						return this.getDirection(GamePad.GetState(this.playerIndex).ThumbSticks.Left.X, GamePad.GetState(this.playerIndex).ThumbSticks.Left.Y);
					case GamePadStick.RIGHT:
						return this.getDirection(GamePad.GetState(this.playerIndex).ThumbSticks.Right.X, GamePad.GetState(this.playerIndex).ThumbSticks.Right.Y);
					default:
						return GamePadDirection.NONE;
				}
			}
			else
				return GamePadDirection.NONE;
		}
		public Vector2 getStickVector2(GamePadStick stick)
		{
			if (GamePad.GetState(this.playerIndex).IsConnected)
			{
				switch (stick)
				{
					case GamePadStick.LEFT:
						return GamePad.GetState(this.playerIndex).ThumbSticks.Left;
					case GamePadStick.RIGHT:
						return GamePad.GetState(this.playerIndex).ThumbSticks.Right;
					default:
						return Vector2.Zero;
				}
			}
			else
				return Vector2.Zero;
		}
		#endregion

		#region Buttons
		public override Boolean checkKey(GamePadKey key, Boolean sticky)
		{
			if (GamePad.GetState(this.playerIndex).IsConnected)
			{
				if (this.lastState.Contains(key))
				{
					if (sticky)
						return true;
					else
					{
						switch (key)
						{
							case GamePadKey.A:
								return (GamePad.GetState(this.playerIndex).Buttons.A == ButtonState.Released);
							case GamePadKey.B:
								return (GamePad.GetState(this.playerIndex).Buttons.B == ButtonState.Released);
							case GamePadKey.BACK:
								return (GamePad.GetState(this.playerIndex).Buttons.Back == ButtonState.Released);
							case GamePadKey.DPADDOWN:
								return (GamePad.GetState(this.playerIndex).DPad.Down == ButtonState.Released);
							case GamePadKey.DPADLEFT:
								return (GamePad.GetState(this.playerIndex).DPad.Left == ButtonState.Released);
							case GamePadKey.DPADRIGHT:
								return (GamePad.GetState(this.playerIndex).DPad.Right == ButtonState.Released);
							case GamePadKey.DPADUP:
								return (GamePad.GetState(this.playerIndex).DPad.Up == ButtonState.Released);
							case GamePadKey.LEFTSHOULDER:
								return (GamePad.GetState(this.playerIndex).Buttons.LeftShoulder == ButtonState.Released);
							case GamePadKey.LEFTSTICK:
								return (GamePad.GetState(this.playerIndex).Buttons.LeftStick == ButtonState.Released);
							case GamePadKey.RIGHTSHOULDER:
								return (GamePad.GetState(this.playerIndex).Buttons.RightShoulder == ButtonState.Released);
							case GamePadKey.RIGHTSTICK:
								return (GamePad.GetState(this.playerIndex).Buttons.RightStick == ButtonState.Released);
							case GamePadKey.START:
								return (GamePad.GetState(this.playerIndex).Buttons.Start == ButtonState.Released);
							case GamePadKey.X:
								return (GamePad.GetState(this.playerIndex).Buttons.X == ButtonState.Released);
							case GamePadKey.Y:
								return (GamePad.GetState(this.playerIndex).Buttons.Y == ButtonState.Released);
							default:
								return false;
						}
					}
				}
				else
					return false;
			}
			else
				return false;
		}

		public override Boolean checkKeyAction(GameAction action, Boolean sticky)
		{
			GamePadDirection gpd;

			switch(action)
			{
				case GameAction.MOVE_UP: gpd = getStickDirection(GamePadStick.LEFT); return ((gpd == GamePadDirection.UP) || (gpd == GamePadDirection.LEFTUP) || (gpd == GamePadDirection.RIGHTUP));
				case GameAction.MOVE_DOWN: gpd = getStickDirection(GamePadStick.LEFT); return (gpd == GamePadDirection.DOWN || (gpd == GamePadDirection.LEFTDOWN) || (gpd == GamePadDirection.RIGHTDOWN));
				case GameAction.MOVE_LEFT: gpd = getStickDirection(GamePadStick.LEFT); return (gpd == GamePadDirection.LEFT || (gpd == GamePadDirection.LEFTDOWN) || (gpd == GamePadDirection.LEFTUP));
				case GameAction.MOVE_RIGHT: gpd = getStickDirection(GamePadStick.LEFT); return (gpd == GamePadDirection.RIGHT || (gpd == GamePadDirection.RIGHTDOWN) || (gpd == GamePadDirection.RIGHTUP));
				case GameAction.ROLL_LEFT: gpd = getStickDirection(GamePadStick.RIGHT); return (gpd == GamePadDirection.UP || (gpd == GamePadDirection.LEFTUP) || (gpd == GamePadDirection.RIGHTUP));
				case GameAction.ROLL_RIGHT: gpd = getStickDirection(GamePadStick.RIGHT); return (gpd == GamePadDirection.DOWN || (gpd == GamePadDirection.LEFTDOWN) || (gpd == GamePadDirection.RIGHTDOWN));
				case GameAction.FIRE: return checkKey(GamePadKey.A);
				case GameAction.SPREAD_DOWN: return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
				case GameAction.SPREAD_UP: return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
				case GameAction.SPREAD_LEFT: return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
				case GameAction.SPREAD_RIGHT: return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
				case GameAction.SPREAD_INCREASE: return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
				case GameAction.SPREAD_DECREASE: return checkKey(Tier.Controls.GamePadKey.LEFTSHOULDER, sticky);
			}
			return false;
    }
    #endregion

		public override void Update()
		{
			this.lastTriggerLeft = GamePad.GetState(this.playerIndex).Triggers.Left;
			this.lastTriggerRight = GamePad.GetState(this.playerIndex).Triggers.Right;

			this.lastState.Clear();

			if (GamePad.GetState(this.playerIndex).Buttons.A == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.A);
			if (GamePad.GetState(this.playerIndex).Buttons.B == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.B);
			if (GamePad.GetState(this.playerIndex).Buttons.Back == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.BACK);
			if (GamePad.GetState(this.playerIndex).DPad.Down == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.DPADDOWN);
			if (GamePad.GetState(this.playerIndex).DPad.Left == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.DPADLEFT);
			if (GamePad.GetState(this.playerIndex).DPad.Right == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.DPADRIGHT);
			if (GamePad.GetState(this.playerIndex).DPad.Up == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.DPADUP);
			if (GamePad.GetState(this.playerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.LEFTSHOULDER);
			if (GamePad.GetState(this.playerIndex).Buttons.LeftStick == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.LEFTSTICK);
			if (GamePad.GetState(this.playerIndex).Buttons.RightShoulder == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.RIGHTSHOULDER);
			if (GamePad.GetState(this.playerIndex).Buttons.RightStick == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.RIGHTSTICK);
			if (GamePad.GetState(this.playerIndex).Buttons.Start == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.START);
			if (GamePad.GetState(this.playerIndex).Buttons.X == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.X);
			if (GamePad.GetState(this.playerIndex).Buttons.Y == ButtonState.Pressed)
				this.lastState.Add(GamePadKey.Y);
		}
	}
#endif
}
