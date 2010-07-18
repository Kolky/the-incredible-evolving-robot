using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TryOut.Input
{
    public enum ActionType
    {
        ActionType_JoinGame,
        ActionType_LeaveGame,
        ActionType_Fire,
        ActionType_AltFire,
        ActionType_MegaFire,
        ActionType_Run,
        ActionType_Exit
    }

    public delegate void ActionMethod(ActionType type, BaseAction caller);

    public enum ActionState
    {
        ActionState_Pressed,
        ActionState_Held,
        ActionState_Released
    }

    public abstract class BaseAction
    {
        public PlayerIndex Player { get; private set; }
        public ActionMethod Method { get; private set; }
        public ActionState State { get; private set; }

        public BaseAction(PlayerIndex player, ActionMethod method, ActionState state)
        {
            Player = player;
            Method = method;
            State = state;
        }

        public static BaseAction Create(PlayerIndex player, ActionMethod method, Keys key)
        {
            return new KeyboardAction(player, method, key);
        }
        public static BaseAction Create(PlayerIndex player, ActionMethod method, Keys key, ActionState state)
        {
            return new KeyboardAction(player, method, key, state);
        }

        public static BaseAction Create(PlayerIndex player, ActionMethod method, MouseButton button)
        {
            return new MouseButtonAction(player, method, button);
        }
        public static BaseAction Create(PlayerIndex player, ActionMethod method, MouseButton button, ActionState state)
        {
            return new MouseButtonAction(player, method, button, state);
        }

        public static BaseAction Create(PlayerIndex player, ActionMethod method, Buttons button)
        {
            return new GamePadButtonAction(player, method, button);
        }
        public static BaseAction Create(PlayerIndex player, ActionMethod method, Buttons button, ActionState state)
        {
            return new GamePadButtonAction(player, method, button, state);
        }

        public static BaseAction Create(PlayerIndex player, ActionMethod method, GamePadTrigger trigger, float triggerDepth)
        {
            return new GamePadTriggerAction(player, method, trigger, triggerDepth);
        }

        public abstract void Execute(ActionType type);
        public abstract void Update();

        public override string ToString()
        {
            return "BaseAction";
        }
    }
}
