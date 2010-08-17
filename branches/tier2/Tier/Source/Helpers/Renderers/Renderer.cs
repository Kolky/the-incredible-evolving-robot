using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers.Renderers
{
  public abstract class Renderer
  {
    protected TierGame game;
    protected Matrix cameraLeft, cameraRight;

    public Matrix CameraLeftScreen 
    {
      set { cameraLeft = value; }
    }
    
    public Matrix CameraRightScreen
    {
      set { cameraRight = value; }
    }

    public Renderer(TierGame game)
    {
      this.game = game;      
    }

    public abstract void Draw(GameTime gameTime);

    public abstract void DisableSplitScreen();

    public abstract void EnableSplitScreen();

    public abstract void Initialize(int width, int height);
  }
}
