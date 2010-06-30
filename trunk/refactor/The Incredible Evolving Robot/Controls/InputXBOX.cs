using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Controls
{
    public class InputXBOX : Input
    {
        private PlayerIndex playerIndex;
        private List<GamePadKey> lastState;
        private float lastTriggerLeft, lastTriggerRight;
        private float stickDeadzone, triggerDeadzone;

        public InputXBOX()
            : this(PlayerIndex.One)
        {
        }

        public InputXBOX(PlayerIndex player)
        {
            playerIndex = player;
            lastState = new List<GamePadKey>();
            lastTriggerLeft = 0.0f;
            lastTriggerRight = 0.0f;
            stickDeadzone = 0.15f;
            triggerDeadzone = 0.05f;
        }

        #region Vibration
        public Boolean setVibration(float left, float right)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
                return GamePad.SetVibration(playerIndex, left, right);
            else
                return false;
        }
        #endregion

        #region Trigger
        public Boolean checkTrigger(GamePadTrigger trigger)
        {
            return checkTrigger(trigger, false);
        }
        public Boolean checkTrigger(GamePadTrigger trigger, Boolean sticky)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            if (state.IsConnected)
            {
                switch (trigger)
                {
                    case GamePadTrigger.LEFT:
                        if (sticky)
                            return (state.Triggers.Left > triggerDeadzone ? true : false);
                        else
                        {
                            if (lastTriggerLeft < triggerDeadzone)
                                return (state.Triggers.Left > triggerDeadzone ? true : false);
                            else
                                return false;
                        }
                    case GamePadTrigger.RIGHT:
                        if (sticky)
                            return (state.Triggers.Right > triggerDeadzone ? true : false);
                        else
                        {
                            if (lastTriggerRight < triggerDeadzone)
                                return (state.Triggers.Right > triggerDeadzone ? true : false);
                            else
                                return false;
                        }
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        public float getTriggerDepth(GamePadTrigger trigger)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            if (state.IsConnected)
            {
                switch (trigger)
                {
                    case GamePadTrigger.LEFT:
                        return state.Triggers.Left;
                    case GamePadTrigger.RIGHT:
                        return state.Triggers.Right;
                    default:
                        return 0.0f;
                }
            }
            else
                return 0.0f;
        }
        #endregion

        #region Thumbstick
        private GamePadDirection getDirection(float x, float y)
        {
            if (x > stickDeadzone)
            {
                if (y > stickDeadzone)
                    return GamePadDirection.RIGHTUP;
                else if (y < -stickDeadzone)
                    return GamePadDirection.RIGHTDOWN;
                else if (y > -stickDeadzone && y < stickDeadzone)
                    return GamePadDirection.RIGHT;
                else
                    return GamePadDirection.NONE;
            }
            else if (x < -stickDeadzone)
            {
                if (y > stickDeadzone)
                    return GamePadDirection.LEFTUP;
                else if (y < -stickDeadzone)
                    return GamePadDirection.LEFTDOWN;
                else if (y > -stickDeadzone && y < stickDeadzone)
                    return GamePadDirection.LEFT;
                else
                    return GamePadDirection.NONE;
            }
            else if (x > -stickDeadzone && x < stickDeadzone)
            {
                if (y > stickDeadzone)
                    return GamePadDirection.UP;
                else if (y < -stickDeadzone)
                    return GamePadDirection.DOWN;
                else if (y > -stickDeadzone && y < stickDeadzone)
                    return GamePadDirection.NONE;
                else
                    return GamePadDirection.NONE;
            }
            else
                return GamePadDirection.NONE;
        }
        private GamePadDirection getStickDirection(GamePadStick stick)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            if (state.IsConnected)
            {
                switch (stick)
                {
                    case GamePadStick.LEFT:
                        return this.getDirection(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
                    case GamePadStick.RIGHT:
                        return this.getDirection(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
                    default:
                        return GamePadDirection.NONE;
                }
            }
            else
                return GamePadDirection.NONE;
        }
        public Vector2 getStickVector2(GamePadStick stick)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            if (state.IsConnected)
            {
                switch (stick)
                {
                    case GamePadStick.LEFT:
                        return state.ThumbSticks.Left;
                    case GamePadStick.RIGHT:
                        return state.ThumbSticks.Right;
                    default:
                        return Vector2.Zero;
                }
            }
            else
                return Vector2.Zero;
        }
        #endregion

        #region Buttons
        public override Boolean checkKey(GamePadKey key, Boolean sticky)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            if (state.IsConnected)
            {
                if (lastState.Contains(key))
                {
                    if (sticky)
                        return true;
                    else
                    {
                        switch (key)
                        {
                            case GamePadKey.A:
                                return (state.Buttons.A == ButtonState.Released);
                            case GamePadKey.B:
                                return (state.Buttons.B == ButtonState.Released);
                            case GamePadKey.BACK:
                                return (state.Buttons.Back == ButtonState.Released);
                            case GamePadKey.DPADDOWN:
                                return (state.DPad.Down == ButtonState.Released);
                            case GamePadKey.DPADLEFT:
                                return (state.DPad.Left == ButtonState.Released);
                            case GamePadKey.DPADRIGHT:
                                return (state.DPad.Right == ButtonState.Released);
                            case GamePadKey.DPADUP:
                                return (state.DPad.Up == ButtonState.Released);
                            case GamePadKey.LEFTSHOULDER:
                                return (state.Buttons.LeftShoulder == ButtonState.Released);
                            case GamePadKey.LEFTSTICK:
                                return (state.Buttons.LeftStick == ButtonState.Released);
                            case GamePadKey.RIGHTSHOULDER:
                                return (state.Buttons.RightShoulder == ButtonState.Released);
                            case GamePadKey.RIGHTSTICK:
                                return (state.Buttons.RightStick == ButtonState.Released);
                            case GamePadKey.START:
                                return (state.Buttons.Start == ButtonState.Released);
                            case GamePadKey.X:
                                return (state.Buttons.X == ButtonState.Released);
                            case GamePadKey.Y:
                                return (state.Buttons.Y == ButtonState.Released);
                            default:
                                return false;
                        }
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override Boolean checkKeyAction(GameAction action, Boolean sticky)
        {
            GamePadDirection gpd;

            switch (action)
            {
                case GameAction.MOVE_UP:
                    gpd = getStickDirection(GamePadStick.LEFT);
                    return ((gpd == GamePadDirection.UP) ||
                        (gpd == GamePadDirection.LEFTUP) ||
                        (gpd == GamePadDirection.RIGHTUP));
                case GameAction.MOVE_DOWN:
                    gpd = getStickDirection(GamePadStick.LEFT);
                    return (gpd == GamePadDirection.DOWN ||
                        (gpd == GamePadDirection.LEFTDOWN) ||
                        (gpd == GamePadDirection.RIGHTDOWN));
                case GameAction.MOVE_LEFT:
                    gpd = getStickDirection(GamePadStick.LEFT);
                    return (gpd == GamePadDirection.LEFT ||
                        (gpd == GamePadDirection.LEFTDOWN) ||
                        (gpd == GamePadDirection.LEFTUP));
                case GameAction.MOVE_RIGHT:
                    gpd = getStickDirection(GamePadStick.LEFT);
                    return (gpd == GamePadDirection.RIGHT ||
                        (gpd == GamePadDirection.RIGHTDOWN) ||
                        (gpd == GamePadDirection.RIGHTUP));
                case GameAction.ROLL_LEFT:
                    gpd = getStickDirection(GamePadStick.RIGHT);
                    return (gpd == GamePadDirection.UP ||
                        (gpd == GamePadDirection.LEFTUP) ||
                        (gpd == GamePadDirection.RIGHTUP));
                case GameAction.ROLL_RIGHT:
                    gpd = getStickDirection(GamePadStick.RIGHT);
                    return (gpd == GamePadDirection.DOWN ||
                        (gpd == GamePadDirection.LEFTDOWN) ||
                        (gpd == GamePadDirection.RIGHTDOWN));
                case GameAction.FIRE:
                    return checkKey(GamePadKey.A);
                case GameAction.SPREAD_DOWN:
                    return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
                case GameAction.SPREAD_UP:
                    return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
                case GameAction.SPREAD_LEFT:
                    return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
                case GameAction.SPREAD_RIGHT:
                    return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
                case GameAction.SPREAD_INCREASE:
                    return checkKey(Tier.Controls.GamePadKey.RIGHTSHOULDER, sticky);
                case GameAction.SPREAD_DECREASE:
                    return checkKey(Tier.Controls.GamePadKey.LEFTSHOULDER, sticky);
            }
            return false;
        }
        #endregion

        public override void Update()
        {
            GamePadState state = GamePad.GetState(playerIndex);

            lastTriggerLeft = state.Triggers.Left;
            lastTriggerRight = state.Triggers.Right;

            lastState.Clear();

            if (state.Buttons.A == ButtonState.Pressed)
                lastState.Add(GamePadKey.A);
            if (state.Buttons.B == ButtonState.Pressed)
                lastState.Add(GamePadKey.B);
            if (state.Buttons.Back == ButtonState.Pressed)
                lastState.Add(GamePadKey.BACK);
            if (state.DPad.Down == ButtonState.Pressed)
                lastState.Add(GamePadKey.DPADDOWN);
            if (state.DPad.Left == ButtonState.Pressed)
                lastState.Add(GamePadKey.DPADLEFT);
            if (state.DPad.Right == ButtonState.Pressed)
                lastState.Add(GamePadKey.DPADRIGHT);
            if (state.DPad.Up == ButtonState.Pressed)
                lastState.Add(GamePadKey.DPADUP);
            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                lastState.Add(GamePadKey.LEFTSHOULDER);
            if (state.Buttons.LeftStick == ButtonState.Pressed)
                lastState.Add(GamePadKey.LEFTSTICK);
            if (state.Buttons.RightShoulder == ButtonState.Pressed)
                lastState.Add(GamePadKey.RIGHTSHOULDER);
            if (state.Buttons.RightStick == ButtonState.Pressed)
                lastState.Add(GamePadKey.RIGHTSTICK);
            if (state.Buttons.Start == ButtonState.Pressed)
                lastState.Add(GamePadKey.START);
            if (state.Buttons.X == ButtonState.Pressed)
                lastState.Add(GamePadKey.X);
            if (state.Buttons.Y == ButtonState.Pressed)
                lastState.Add(GamePadKey.Y);
        }
    }
}
