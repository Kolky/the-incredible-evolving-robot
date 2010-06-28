using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
#if WINDOWS
	public class InputPC : Input
	{
		private int lastMouseX, lastMouseY;
		private List<MouseKey> lastMouse;
    private List<Keys> lastState;
    public List<Keys> LastState
    {
      get { return lastState; }
    }

		private ControlsSetPC pcKeys;
		public ControlsSetPC PCKeys
		{
			get { return this.pcKeys; }
			set { this.pcKeys = value; }
		}


    public InputPC()
    {
      this.lastState = new List<Keys>();
			this.lastMouse = new List<MouseKey>();
			this.pcKeys = new ControlsSetPC();
		}

		public override Boolean checkKey(Keys key, Boolean sticky)
    {
      if (this.LastState.Contains(key))
      {
				return (sticky)? true: Keyboard.GetState().IsKeyUp(key);
      }
      else
        return false;
		}

		#region Mouse
		public Boolean checkMouseKey(MouseKey key)
		{
			return this.checkMouseKey(key, false);
		}
		public Boolean checkMouseKey(MouseKey key, Boolean sticky)
		{
			if (lastMouse.Contains(key))
			{
				if (sticky)
					return true;
				else
				{
					switch (key)
					{
						case MouseKey.LEFTBUTTON:
							if (Mouse.GetState().LeftButton == ButtonState.Released)
								return true;
							else
								return false;
						case MouseKey.MIDDLEBUTTON:
							if (Mouse.GetState().MiddleButton == ButtonState.Released)
								return true;
							else
								return false;
						case MouseKey.RIGHTBUTTON:
							if (Mouse.GetState().RightButton == ButtonState.Released)
								return true;
							else
								return false;
						default:
							return false;
					}
				}
			}
			else
				return false;
		}
		public MouseMovement checkMouseMovement()
		{
			MouseMovement mouseMovement = new MouseMovement();
			mouseMovement.X = Mouse.GetState().X;
			mouseMovement.Y = Mouse.GetState().Y;
			return mouseMovement;
		}

		public MouseMovement checkMouseDirectionXY()
		{
			MouseMovement mouseMovement = new MouseMovement();
			mouseMovement.X = this.lastMouseX - Mouse.GetState().X;
			mouseMovement.Y = this.lastMouseY - Mouse.GetState().Y;
			return mouseMovement;
		}
		public MouseDirection checkMouseDirection()
		{
			if (Mouse.GetState().X > this.lastMouseX)
			{
				if (Mouse.GetState().Y > this.lastMouseY)
					return MouseDirection.RIGHTDOWN;
				else if (Mouse.GetState().Y < this.lastMouseY)
					return MouseDirection.RIGHTUP;
				else if (Mouse.GetState().Y == this.lastMouseY)
					return MouseDirection.RIGHT;
				else
					return MouseDirection.NONE;
			}
			else if (Mouse.GetState().X < this.lastMouseX)
			{
				if (Mouse.GetState().Y > this.lastMouseY)
					return MouseDirection.LEFTDOWN;
				else if (Mouse.GetState().Y < this.lastMouseY)
					return MouseDirection.LEFTUP;
				else if (Mouse.GetState().Y == this.lastMouseY)
					return MouseDirection.LEFT;
				else
					return MouseDirection.NONE;
			}
			else if (Mouse.GetState().X == this.lastMouseX)
			{
				if (Mouse.GetState().Y > this.lastMouseY)
					return MouseDirection.DOWN;
				else if (Mouse.GetState().Y < this.lastMouseY)
					return MouseDirection.UP;
				else if (Mouse.GetState().Y == this.lastMouseY)
					return MouseDirection.NONE;
				else
					return MouseDirection.NONE;
			}
			else
				return MouseDirection.NONE;
		}
		#endregion

		#region keyAction
		public override Boolean checkKeyAction(GameAction action, Boolean sticky)
		{
			ActionSetPC set = this.pcKeys.getSetByAction(action);

			if (set.IsKeyboard)
				return this.checkKey(set.Key, sticky);
			else if (set.IsMouse)
				return this.checkMouseKey(set.MouseKey, sticky);
			else
				return false;
		}
		#endregion

		public override void Update()
    {
			//Mouse
			this.lastMouseX = Mouse.GetState().X;
			this.lastMouseY = Mouse.GetState().Y;

			this.lastMouse.Clear();
			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				lastMouse.Add(MouseKey.LEFTBUTTON);
			if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
				lastMouse.Add(MouseKey.MIDDLEBUTTON);
			if (Mouse.GetState().RightButton == ButtonState.Pressed)
				lastMouse.Add(MouseKey.RIGHTBUTTON);

			//Keyboard
			this.LastState.Clear();
			foreach (Keys key in Keyboard.GetState().GetPressedKeys())
				this.LastState.Add(key);
    }
	}
#endif
}
