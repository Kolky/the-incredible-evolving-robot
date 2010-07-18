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

        public GamePadTriggerAction(PlayerIndex player, ActionMethod method, GamePadTrigger trigger, float triggerDepth)
            : base(player, method, ActionState.ActionState_Pressed)
        {
            Trigger = trigger;
            TriggerDepth = triggerDepth;
        }

        public override void Execute(ActionType type)
        {
            switch (Trigger)
            {
                case GamePadTrigger.GamePadTrigger_Left:
                    if (GamePad.GetState(Player).Triggers.Left >= TriggerDepth)
                    {
                        Method(type, this);
                    }
                    break;
                case GamePadTrigger.GamePadTrigger_Right:
                    if (GamePad.GetState(Player).Triggers.Right >= TriggerDepth)
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
        }

        public override string ToString()
        {
            return String.Concat("GamePadTriggerAction: ", Enum.GetName(typeof(GamePadTrigger), Trigger), ", Depth: ", TriggerDepth);
        }
    }
}
