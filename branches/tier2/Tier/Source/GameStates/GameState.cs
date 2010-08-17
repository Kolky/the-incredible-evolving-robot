using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Tier;

namespace Tier.Source.GameStates
{
  public abstract class GameState
  {
    #region Properties
    protected bool isInitialized;
    protected TierGame game;
    protected KeyboardState previousKeyboardState;

    public TierGame Game
    {
      get { return game; }
      set { game = value; }
    }
    #endregion

    public GameState(TierGame game)
    {
      this.Game = game;
    }

    public virtual void Enter(GameState previousState)
    {
      if (!isInitialized)
      {
        Initialize();
      }
    }

    public virtual void Initialize()
    {
      this.isInitialized = true;
    }

    public abstract void Leave();

    public abstract void Update(GameTime gameTime);
  }
}
