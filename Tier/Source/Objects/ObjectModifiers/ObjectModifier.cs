using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;

namespace Tier.Source.ObjectModifiers
{
  public abstract class ObjectModifier
  {
    #region Properties
    private GameObject parent;

    public GameObject Parent
    {
      get { return parent; }
      set { parent = value; }
    }
    #endregion

    public ObjectModifier(GameObject parent)
    {
      this.parent = parent;      
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Clone(out ObjectModifier objmod, GameObject parent);
  }
}
