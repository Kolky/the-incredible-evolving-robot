using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers.Cameras
{
  public class PositionalCamera : Camera
  {
    public PositionalCamera(TierGame game)
      : base(game)
    {      
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      CreateLookatMatrix();
    }
  }
}
