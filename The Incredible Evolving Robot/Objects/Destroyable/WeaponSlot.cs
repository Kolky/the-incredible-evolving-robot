using System;
using System.Collections.Generic;
using System.Text;

namespace Tier.Objects.Destroyable
{
  public class WeaponSlot
  {
    #region Properties
    private BlockPiece parent;
    public BlockPiece Parent
    {
      get { return parent; }
      set { parent = value; }
    }

    private Weapon weapon;
    public Weapon Weapon
    {
      get { return weapon; }
      set { weapon = value; }
    }	
    #endregion
  }
}
