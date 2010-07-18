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

        public KeyboardAction(PlayerIndex player, ActionMethod method, Keys key)
            : this(player, method, key, ActionState.ActionState_Pressed)
        {
        }

        public KeyboardAction(PlayerIndex player, ActionMethod method, Keys key, ActionState state)
            : base(player, method, state)
        {
            Key = key;
            Pressed = false;
        }

        public override void Execute(ActionType type)
        {
            switch (State)
            {
                case ActionState.ActionState_Pressed:
                    if (Keyboard.GetState(Player).IsKeyDown(Key) && !Pressed)
                    {
                        Method(type, this);
                    }
                    break;
                case ActionState.ActionState_Held:
                    if (Keyboard.GetState(Player).IsKeyDown(Key) && Pressed)
                    {
                        Method(type, this);
                    }
                    break;
                case ActionState.ActionState_Released:
                    if (Keyboard.GetState(Player).IsKeyUp(Key) && Pressed)
                    {
                        Method(type, this);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Update()
        {
            Pressed = Keyboard.GetState(Player).IsKeyDown(Key);
        }

        public override string ToString()
        {
            return String.Concat("KeyboardAction: ", Enum.GetName(typeof(Keys), Key), ", State: ", Enum.GetName(typeof(ActionState), State));
        }
    }
}
