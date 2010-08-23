using System;
using System.Collections.Generic;
using System.Text;

using Tier.Menus;

namespace Tier.Handlers
{
  class MenuHandler
  {
    #region Properties
    private MenuState menuState;
    public MenuState MenuState
    {
      get { return menuState; }
      set { menuState = value; }
    }	
    #endregion
  }
}