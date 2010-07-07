using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TryOut.Input
{
    public class GamePadButtonAction : BaseAction
    {
        public Buttons Button { get; private set; }
        private Boolean Pressed;

        public GamePadButtonAction(ActionMethod method, Buttons button)
            : this(method, button, ActionState.ActionState_Pressed)
        {
        }

        public GamePadButtonAction(ActionMethod method, Buttons button, ActionState state)
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
                    if (GamePad.GetState(player).IsButtonDown(Button) && !Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Held:
                    if (GamePad.GetState(player).IsButtonDown(Button) && Pressed)
                    {
                        Method(this);
                    }
                    break;
                case ActionState.ActionState_Released:
                    if (GamePad.GetState(player).IsButtonUp(Button) && Pressed)
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
            Pressed = GamePad.GetState(player).IsButtonDown(Button);
        }

        public override string ToString()
        {
            return String.Concat("GamePadButtonAction: ", Enum.GetName(typeof(Buttons), Button), ", State: ", Enum.GetName(typeof(ActionState), State));
        }
    }
}
