using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TryOut.Input
{
    public enum GamePadTrigger
    {
        GamePadTrigger_Left,
        GamePadTrigger_Right
    }

    public class GamePadTriggerAction : BaseAction
    {
        public GamePadTrigger Trigger { get; private set; }
        public float TriggerDepth { get; private set; }

        public GamePadTriggerAction(ActionMethod method, GamePadTrigger trigger, float triggerDepth)
            : base(method, ActionState.ActionState_Pressed)
        {
            Trigger = trigger;
            TriggerDepth = triggerDepth;
        }

        public override void Execute(PlayerIndex player)
        {
            switch (Trigger)
            {
                case GamePadTrigger.GamePadTrigger_Left:
                    if (GamePad.GetState(player).Triggers.Left >= TriggerDepth)
                    {
                        Method(this);
                    }
                    break;
                case GamePadTrigger.GamePadTrigger_Right:
                    if (GamePad.GetState(player).Triggers.Right >= TriggerDepth)
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
        }

        public override string ToString()
        {
            return String.Concat("GamePadTriggerAction: ", Enum.GetName(typeof(GamePadTrigger), Trigger), ", Depth: ", TriggerDepth);
        }
    }
}
