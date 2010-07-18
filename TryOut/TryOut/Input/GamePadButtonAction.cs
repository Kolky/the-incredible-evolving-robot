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

        public GamePadButtonAction(PlayerIndex player, ActionMethod method, Buttons button)
            : this(player, method, button, ActionState.ActionState_Pressed)
        {
        }

        public GamePadButtonAction(PlayerIndex player, ActionMethod method, Buttons button, ActionState state)
            : base(player, method, state)
        {
            Button = button;
            Pressed = false;
        }

        public override void Execute(ActionType type)
        {
            switch (State)
            {
                case ActionState.ActionState_Pressed:
                    if (GamePad.GetState(Player).IsButtonDown(Button) && !Pressed)
                    {
                        Method(type, this);
                    }
                    break;
                case ActionState.ActionState_Held:
                    if (GamePad.GetState(Player).IsButtonDown(Button) && Pressed)
                    {
                        Method(type, this);
                    }
                    break;
                case ActionState.ActionState_Released:
                    if (GamePad.GetState(Player).IsButtonUp(Button) && Pressed)
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
            Pressed = GamePad.GetState(Player).IsButtonDown(Button);
        }

        public override string ToString()
        {
            return String.Concat("GamePadButtonAction: ", Enum.GetName(typeof(Buttons), Button), ", State: ", Enum.GetName(typeof(ActionState), State));
        }
    }
}
