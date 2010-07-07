using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TryOut.Input
{
    public class KeyboardAction : BaseAction
    {
        public Keys Key { get; private set; }
        private Boolean Pressed;

        public KeyboardAction(ActionMethod method, Keys key)
            : this(method, key, ActionState.ActionState_Pressed)
        {
        }

        public KeyboardAction(ActionMethod method, Keys key, ActionState state)
            : base(method, state)
        {
            Key = key;
            Pressed = false;
        }

        public override void Execute(PlayerIndex player)
        {
            switch (State)
            {
                case ActionState.ActionState_Pressed:
                    if (Keyboard.GetState(player).IsKeyDown(Key) && !Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Held:
                    if (Keyboard.GetState(player).IsKeyDown(Key) && Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Released:
                    if (Keyboard.GetState(player).IsKeyUp(Key) && Pressed)
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
            Pressed = Keyboard.GetState(player).IsKeyDown(Key);
        }

        public override string ToString()
        {
            return String.Concat("KeyboardAction: ", Enum.GetName(typeof(Keys), Key), ", State: ", Enum.GetName(typeof(ActionState), State));
        }
    }
}
