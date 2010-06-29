using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
#if WINDOWS
    public class ControlsSetPC
    {
        #region Properties
        private List<ActionSetPC> usedKeys;
        public List<ActionSetPC> UsedKeys
        {
            get { return usedKeys; }
            set { usedKeys = value; }
        }

        #region Keys
        private ActionSetPC fire;
        public ActionSetPC Fire
        {
            get { return fire; }
            set
            {
                if (this.changeKey(value))
                    fire = value;
            }
        }

        private ActionSetPC rollLeft;
        public ActionSetPC RollLeft
        {
            get { return rollLeft; }
            set
            {
                if (this.changeKey(value))
                    rollLeft = value;
            }
        }

        private ActionSetPC rollRight;
        public ActionSetPC RollRight
        {
            get { return rollRight; }
            set
            {
                if (this.changeKey(value))
                    rollRight = value;
            }
        }

        private ActionSetPC moveUp;
        public ActionSetPC MoveUp
        {
            get { return moveUp; }
            set
            {
                if (this.changeKey(value))
                    moveUp = value;
            }
        }

        private ActionSetPC moveDown;
        public ActionSetPC MoveDown
        {
            get { return moveDown; }
            set
            {
                if (this.changeKey(value))
                    moveDown = value;
            }
        }

        private ActionSetPC moveLeft;
        public ActionSetPC MoveLeft
        {
            get { return moveLeft; }
            set
            {
                if (this.changeKey(value))
                    moveLeft = value;
            }
        }

        private ActionSetPC moveRight;
        public ActionSetPC MoveRight
        {
            get { return moveRight; }
            set
            {
                if (this.changeKey(value))
                    moveRight = value;
            }
        }

        private ActionSetPC moveForward;
        public ActionSetPC MoveForward
        {
            get { return moveForward; }
            set
            {
                if (this.changeKey(value))
                    moveForward = value;
            }
        }

        private ActionSetPC moveBackward;
        public ActionSetPC MoveBackward
        {
            get { return moveBackward; }
            set
            {
                if (this.changeKey(value))
                    moveBackward = value;
            }
        }

        private ActionSetPC spreadIncrease;
        public ActionSetPC SpreadIncrease
        {
            get { return spreadIncrease; }
            set
            {
                if (this.changeKey(value))
                    spreadIncrease = value;
            }
        }

        private ActionSetPC spreadDecrease;
        public ActionSetPC SpreadDecrease
        {
            get { return spreadDecrease; }
            set
            {
                if (this.changeKey(value))
                    spreadDecrease = value;
            }
        }

        private ActionSetPC spreadLeft;
        public ActionSetPC SpreadLeft
        {
            get { return spreadLeft; }
            set
            {
                if (this.changeKey(value))
                    spreadLeft = value;
            }
        }

        private ActionSetPC spreadRight;
        public ActionSetPC SpreadRight
        {
            get { return spreadRight; }
            set
            {
                if (this.changeKey(value))
                    spreadRight = value;
            }
        }

        private ActionSetPC spreadUp;
        public ActionSetPC SpreadUp
        {
            get { return spreadUp; }
            set
            {
                if (this.changeKey(value))
                    spreadUp = value;
            }
        }

        private ActionSetPC spreadDown;
        public ActionSetPC SpreadDown
        {
            get { return spreadDown; }
            set
            {
                if (this.changeKey(value))
                    spreadDown = value;
            }
        }

        private ActionSetPC growBoss;
        public ActionSetPC GrowBoss
        {
            get { return growBoss; }
            set
            {
                if (this.changeKey(value))
                    growBoss = value;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ControlsSetPC()
        {
            this.UsedKeys = new List<ActionSetPC>();

            // initialize default keys
            this.Fire = new ActionSetPC(GameAction.FIRE, Keys.Space);
            this.RollLeft = new ActionSetPC(GameAction.ROLL_LEFT, Keys.PageUp);
            this.RollRight = new ActionSetPC(GameAction.ROLL_RIGHT, Keys.PageDown);
            this.MoveUp = new ActionSetPC(GameAction.MOVE_UP, Keys.Up);
            this.MoveDown = new ActionSetPC(GameAction.MOVE_DOWN, Keys.Down);
            this.MoveLeft = new ActionSetPC(GameAction.MOVE_LEFT, Keys.Left);
            this.MoveRight = new ActionSetPC(GameAction.MOVE_RIGHT, Keys.Right);
            this.MoveForward = new ActionSetPC(GameAction.MOVE_FORWARD, Keys.Z);
            this.MoveBackward = new ActionSetPC(GameAction.MOVE_BACKWARD, Keys.X);
            this.SpreadLeft = new ActionSetPC(GameAction.SPREAD_LEFT, Keys.A);
            this.SpreadRight = new ActionSetPC(GameAction.SPREAD_RIGHT, Keys.D);
            this.SpreadDown = new ActionSetPC(GameAction.SPREAD_DOWN, Keys.W);
            this.SpreadUp = new ActionSetPC(GameAction.SPREAD_UP, Keys.S);
            this.SpreadIncrease = new ActionSetPC(GameAction.SPREAD_INCREASE, Keys.E);
            this.SpreadDecrease = new ActionSetPC(GameAction.SPREAD_DECREASE, Keys.Q);
            this.GrowBoss = new ActionSetPC(GameAction.GROW_BOSS, Keys.LeftShift);
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="pcKeys"></param>
        public ControlsSetPC(ControlsSetPC pcKeys)
        {
            this.UsedKeys = new List<ActionSetPC>();

            // copy set
            this.Fire = pcKeys.Fire;
            this.RollLeft = pcKeys.RollLeft;
            this.RollRight = pcKeys.RollRight;
            this.MoveUp = pcKeys.MoveUp;
            this.MoveDown = pcKeys.MoveDown;
            this.MoveLeft = pcKeys.MoveLeft;
            this.MoveRight = pcKeys.MoveRight;
            this.MoveForward = pcKeys.MoveForward;
            this.MoveBackward = pcKeys.MoveBackward;
            this.SpreadLeft = pcKeys.SpreadLeft;
            this.SpreadRight = pcKeys.SpreadRight;
            this.SpreadDown = pcKeys.SpreadDown;
            this.SpreadUp = pcKeys.SpreadUp;
            this.SpreadIncrease = pcKeys.SpreadIncrease;
            this.SpreadDecrease = pcKeys.SpreadDecrease;
            this.GrowBoss = pcKeys.GrowBoss;
        }

        public ActionSetPC getSetByAction(GameAction action)
        {
            foreach (ActionSetPC actionSet in this.UsedKeys)
            {
                if (actionSet.Action == action)
                    return actionSet;
            }
            return null;
        }

        private void clearPCActionSet(ActionSetPC set)
        {
            set.IsKeyboard = false;
            set.IsMouse = false;
            set.Key = Keys.None;
            set.MouseKey = MouseKey.LEFTBUTTON;
        }

        private Boolean changeKey(ActionSetPC action)
        {
            Boolean keyFound = false;
            Boolean actionFound = false;
            ActionSetPC keySet = new ActionSetPC();
            ActionSetPC actionSet = new ActionSetPC();

            List<ActionSetPC>.Enumerator it = this.UsedKeys.GetEnumerator();
            while (it.MoveNext())
            {
                if (it.Current.IsKeyboard && action.IsKeyboard)
                {
                    if (it.Current.Key == action.Key)
                    {
                        keyFound = true;
                        keySet = it.Current;
                    }
                }
                if (it.Current.IsMouse && action.IsMouse)
                {
                    if (it.Current.MouseKey == action.MouseKey)
                    {
                        keyFound = true;
                        keySet = it.Current;
                    }
                }
                if (it.Current.Action == action.Action)
                {
                    actionFound = true;
                    actionSet = it.Current;
                }

                if (keyFound && actionFound)
                    break;

            }

            if (keyFound)
            {
                this.UsedKeys.Remove(keySet);

                if (actionFound)
                    this.UsedKeys.Remove(actionSet);

                switch (keySet.Action)
                {
                    case GameAction.FIRE:
                        clearPCActionSet(this.fire);
                        break;
                    case GameAction.ROLL_LEFT:
                        clearPCActionSet(this.RollLeft);
                        break;
                    case GameAction.ROLL_RIGHT:
                        clearPCActionSet(this.RollRight);
                        break;
                    case GameAction.MOVE_UP:
                        clearPCActionSet(this.MoveUp);
                        break;
                    case GameAction.MOVE_DOWN:
                        clearPCActionSet(this.MoveDown);
                        break;
                    case GameAction.MOVE_LEFT:
                        clearPCActionSet(this.MoveLeft);
                        break;
                    case GameAction.MOVE_RIGHT:
                        clearPCActionSet(this.MoveRight);
                        break;
                    case GameAction.MOVE_FORWARD:
                        clearPCActionSet(this.MoveForward);
                        break;
                    case GameAction.MOVE_BACKWARD:
                        clearPCActionSet(this.MoveBackward);
                        break;
                    case GameAction.SPREAD_DOWN:
                        clearPCActionSet(this.spreadDown);
                        break;
                    case GameAction.SPREAD_LEFT:
                        clearPCActionSet(this.SpreadLeft);
                        break;
                    case GameAction.SPREAD_RIGHT:
                        clearPCActionSet(this.SpreadRight);
                        break;
                    case GameAction.SPREAD_UP:
                        clearPCActionSet(this.SpreadUp);
                        break;
                    case GameAction.SPREAD_INCREASE:
                        clearPCActionSet(this.SpreadIncrease);
                        break;
                    case GameAction.SPREAD_DECREASE:
                        clearPCActionSet(this.SpreadDecrease);
                        break;
                    case GameAction.GROW_BOSS:
                        clearPCActionSet(this.GrowBoss);
                        break;
                }

                this.UsedKeys.Add(action);
                return true;
            }
            else if (!keyFound && actionFound)
            {
                this.UsedKeys.Remove(actionSet);
                this.UsedKeys.Add(action);
                return true;
            }
            else
            {
                this.UsedKeys.Add(action);
                return true;
            }
        }
    }
#endif
}
