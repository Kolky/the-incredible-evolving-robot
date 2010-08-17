using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Handlers
{
  public class ResolveTargetHandler
  {
    private ResolveTexture2D firstRenderTarget;
    private ResolveTexture2D secondRenderTarget;
    private ResolveTexture2D bottomScreenRenderTarget;
    private ResolveTexture2D topScreenRenderTarget;

    public ResolveTexture2D TopScreenTarget
    {
      get { return topScreenRenderTarget; }
      set { topScreenRenderTarget = value; }
    }
	
    public ResolveTexture2D BottomScreenTarget
    {
      get { return bottomScreenRenderTarget; }
      set { bottomScreenRenderTarget = value; }
    }
	
    public ResolveTexture2D SecondRenderTarget
    {
      get { return secondRenderTarget; }
      set { secondRenderTarget = value; }
    }
	
    public ResolveTexture2D FirstRenderTarget
    {
      get { return firstRenderTarget; }
      set { firstRenderTarget = value; }
    }
	
  }
}
