using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TryOut.Input
{
    public enum MouseButton
    {
        MouseButton_Left,
        MouseButton_Middle,
        MouseButton_Right
    }

    public class MouseButtonAction : BaseAction
    {
        public MouseButton Button { get; private set; }
        private Boolean Pressed;

        public MouseButtonAction(ActionMethod method, MouseButton button)
            : this(method, button, ActionState.ActionState_Pressed)
        {
        }

        public MouseButtonAction(ActionMethod method, MouseButton button, ActionState state)
            : base(method, state)
        {
            Button = button;
            Pressed = false;
        }

        public override void Execute(PlayerIndex player)
        {
            switch (State)
            {
                case ActionState.ActionState_Pressed:
                    if (IsMouseButtonDown(Button) && !Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Held:
                    if (IsMouseButtonDown(Button) && Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Released:
                    if (IsMouseButtonUp(Button) && Pressed)
                    {
                        Method(this);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Update(PlayerIndex player)
        {
            Pressed = IsMouseButtonDown(Button);
        }

        public override string ToString()
        {
            return String.Concat("MouseButtonAction: ", Enum.GetName(typeof(MouseButton), Button), ", State: ", Enum.GetName(typeof(ActionState), State));
        }

        private static Boolean IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.MouseButton_Left:
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case MouseButton.MouseButton_Middle:
                    if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case MouseButton.MouseButton_Right:
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        private static Boolean IsMouseButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.MouseButton_Left:
                    if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;
                case MouseButton.MouseButton_Middle:
                    if (Mouse.GetState().MiddleButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;
                case MouseButton.MouseButton_Right:
                    if (Mouse.GetState().RightButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}
