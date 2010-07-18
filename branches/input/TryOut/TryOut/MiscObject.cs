using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TryOut.Input;

namespace TryOut
{
    class MiscObject
    {
        public MiscObject()
        {
            TierGame.AddPlayerAction(ActionType.ActionType_Run, BaseAction.Create(PlayerIndex.One, ActionHandler, Buttons.X, ActionState.ActionState_Held));
        }

        private void ActionHandler(ActionType type, BaseAction caller)
        {
            Console.WriteLine(String.Concat("MiscObject> ", caller));
        }
    }
}
