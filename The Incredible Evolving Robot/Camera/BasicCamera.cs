using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Camera
{
  public class BasicCamera : Camera
  {
    public BasicCamera(Vector3 position, Vector3 target)
      : base(position, target)
    {
    }
  }
}
