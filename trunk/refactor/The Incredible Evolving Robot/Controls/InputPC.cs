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
        private List<MouseKey> LastMouse;
        private List<Keys> LastState;
        private ControlsSetPC PCKeys;

        public InputPC()
        {
            LastState = new List<Keys>();
            LastMouse = new List<MouseKey>();
            PCKeys = new ControlsSetPC();
        }

        public override Boolean checkKey(Keys key, Boolean sticky)
        {
            if (LastState.Contains(key))
            {
                return (sticky) ? true : Keyboard.GetState().IsKeyUp(key);
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
            if (LastMouse.Contains(key))
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
        #endregion

        #region keyAction
        public override Boolean checkKeyAction(GameAction action, Boolean sticky)
        {
            ActionSetPC set = PCKeys.getSetByAction(action);

            if (set.IsKeyboard)
                return checkKey(set.Key, sticky);
            else if (set.IsMouse)
                return checkMouseKey(set.MouseKey, sticky);
            else
                return false;
        }
        #endregion

        public override void Update()
        {
            //Mouse

            LastMouse.Clear();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                LastMouse.Add(MouseKey.LEFTBUTTON);
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                LastMouse.Add(MouseKey.MIDDLEBUTTON);
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                LastMouse.Add(MouseKey.RIGHTBUTTON);

            //Keyboard
            LastState.Clear();
            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                LastState.Add(key);
        }
    }
#endif
}
