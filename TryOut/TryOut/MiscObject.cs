using System;

using Microsoft.Xna.Framework.Input;

using TryOut.Input;

namespace TryOut
{
    class MiscObject
    {
        public MiscObject()
        {
            TierGame.Actions.Add(ActionType.ActionType_Run, BaseAction.Create(ActionHandler, Buttons.X, ActionState.ActionState_Held));
        }

        private void ActionHandler(BaseAction caller)
        {
            Console.WriteLine(String.Concat("MiscObject> ", caller));
        }
    }
}
