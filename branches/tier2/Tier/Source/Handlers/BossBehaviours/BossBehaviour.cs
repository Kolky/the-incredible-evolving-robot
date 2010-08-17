using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers.BossBehaviours
{
  public abstract class BossBehaviour : IComparable
  {
    #region Properties
    private Matrix transform;
    private int ttl;
    private BossBehaviourType type;
    protected TierGame game;
    private bool isEnabled;
    protected BossBehaviourHandler handler;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; }
    }

    public BossBehaviourType Type
    {
      get { return type; }
      set { type = value; }
    }

    public int TTL
    {
      get { return ttl; }
      set { ttl = value; }
    }

    public Matrix Transform
    {
      get { return transform; }
      set { transform = value; }
    }
    #endregion

    public BossBehaviour(TierGame game, BossBehaviourHandler handler, BossBehaviourType type)
    {
      this.type = type;
      this.Transform = Matrix.Identity;
      this.game = game;
      this.isEnabled = false;
      this.handler = handler;
    }

    public abstract void Update(GameTime gameTime);

    public virtual void Draw(GameTime gameTime)
    { }

    public virtual void Disable()
    {
      this.isEnabled = false;
    }

    public abstract void Enable();
    #region IComparable Members

    public int CompareTo(object obj)
    {
      BossBehaviour b = (BossBehaviour)obj;

      if (this.TTL < b.TTL)
      {
        return -1;
      }
      else if (this.TTL > b.TTL)
      {
        return 1;
      }

      return 0;
    }

    #endregion
  }
}
