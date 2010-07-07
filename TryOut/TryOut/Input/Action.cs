using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TryOut.Input
{
    public enum ActionType
    {
        ActionType_Fire,
        ActionType_AltFire,
        ActionType_MegaFire,
        ActionType_Run,
        ActionType_Exit
    }

    public delegate void ActionMethod(BaseAction caller);

    public enum ActionState
    {
        ActionState_Pressed,
        ActionState_Held,
        ActionState_Released
    }

    public abstract class BaseAction
    {
        public ActionMethod Method { get; private set; }
        public ActionState State { get; private set; }

        public BaseAction(ActionMethod method, ActionState state)
        {
            Method = method;
            State = state;
        }

        public static BaseAction Create(ActionMethod method, Keys key)
        {
            return new KeyboardAction(method, key);
        }
        public static BaseAction Create(ActionMethod method, Keys key, ActionState state)
        {
            return new KeyboardAction(method, key, state);
        }

        public static BaseAction Create(ActionMethod method, MouseButton button)
        {
            return new MouseButtonAction(method, button);
        }
        public static BaseAction Create(ActionMethod method, MouseButton button, ActionState state)
        {
            return new MouseButtonAction(method, button, state);
        }

        public static BaseAction Create(ActionMethod method, Buttons button)
        {
            return new GamePadButtonAction(method, button);
        }
        public static BaseAction Create(ActionMethod method, Buttons button, ActionState state)
        {
            return new GamePadButtonAction(method, button, state);
        }

        public static BaseAction Create(ActionMethod method, GamePadTrigger trigger, float triggerDepth)
        {
            return new GamePadTriggerAction(method, trigger, triggerDepth);
        }

        public void Execute()
        {
            Execute(PlayerIndex.One);
        }
        public abstract void Execute(PlayerIndex player);

        /// <summary>
        /// Update should be called after all Execute calls are done
        /// </summary>
        public void Update()
        {
            Update(PlayerIndex.One);
        }
        public abstract void Update(PlayerIndex player);

        public override string ToString()
        {
            return "BaseAction";
        }
    }
}
