using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
#if WINDOWS
	public class ActionSetPC
	{
		#region Properties
		private GameAction action;
		public GameAction Action
		{
			get { return action; }
			set { action = value; }
		}

		private Keys key;
		public Keys Key
		{
			get { return key; }
			set { key = value; }
		}

		private MouseKey mouseKey;
		public MouseKey MouseKey
		{
			get { return mouseKey; }
			set { mouseKey = value; }
		}

		private Boolean isKeyboard = false;
		public Boolean IsKeyboard
		{
			get { return isKeyboard; }
			set { isKeyboard = value; }
		}

		private Boolean isMouse = false;
		public Boolean IsMouse
		{
			get { return isMouse; }
			set { isMouse = value; }
		}
		#endregion

    /// <summary>
    /// Empty Constructor
    /// </summary>
    public ActionSetPC()
    {
    }
    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="set">Set to Copy</param>
		public ActionSetPC(ActionSetPC set)
		{
      this.Action = set.Action;

      if (set.IsKeyboard)
      {
        this.Key = set.Key;
        this.IsKeyboard = true;
      }
      else if (set.IsMouse)
      {
        this.MouseKey = set.MouseKey;
        this.IsMouse = true;
      }
		}
    /// <summary>
    /// Define action to a keyboard key
    /// </summary>
    /// <param name="action">the action</param>
    /// <param name="key">a keyboard key</param>
		public ActionSetPC(GameAction action, Keys key)
		{
			this.Action = action;
			this.Key = key;

			this.isKeyboard = true;
		}
    /// <summary>
    /// Define action to a mouse key
    /// </summary>
    /// <param name="action">the action</param>
    /// <param name="mouseKey">a mouse key</param>
		public ActionSetPC(GameAction action, MouseKey mouseKey)
		{
			this.Action = action;
			this.MouseKey = mouseKey;

			this.isMouse = true;
		}

		public override string ToString()
		{
			if (this.IsKeyboard)
				return this.Key.ToString();
			else if (this.IsMouse)
				return this.MouseKey.ToString();
			else
				return "None";
		}
	}	
#endif
}
