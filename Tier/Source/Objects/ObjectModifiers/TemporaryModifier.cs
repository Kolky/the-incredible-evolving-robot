using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;

namespace Tier.Source.ObjectModifiers
{
  /// <summary>
  /// TemporaryModifier will enable the object it is assigned to, to be active in the game for a given duration.
  /// </summary>
  public class TemporaryModifier : ObjectModifier
  {
    #region Properties
    private int ttl;

    public int TTL
    {
      get { return ttl; }
      set { ttl = value; }
    }
    #endregion

    private int timeElapsed;

    public TemporaryModifier(GameObject parent)
      : base(parent)
    {
      parent.TemporaryModifier = this;
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new TemporaryModifier(parent);
      ((TemporaryModifier)objmod).TTL = this.ttl;      
    }

    public override void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (this.timeElapsed >= ttl)
      {
        this.Parent.Game.GameHandler.RemoveObject(this.Parent, this.Parent.Type);
        this.timeElapsed = 0;
      }
    }
  }
}
