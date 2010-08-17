using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers.Cameras
{
  /// <summary>
  /// Camera designed to transition
  /// </summary>
  public class BlendingCamera : Camera
  {
    private Matrix viewFirstCamera, viewSecondCamera;
    private int timeToBlend;
    private float amount;

    public BlendingCamera(TierGame game) : base(game)
    {

    }

    public void Start(Camera first, Camera second, int timeToBlend)
    {
      this.viewFirstCamera = first.LookAtMatrix;
      this.viewSecondCamera = second.LookAtMatrix;
      this.timeToBlend = timeToBlend;
      amount = 0;
    }

    public override void Update(GameTime gameTime)
    {
      amount += gameTime.ElapsedGameTime.Milliseconds / (float)timeToBlend;

      if (amount > 1.0f)
      {
        amount = 1.0f;
      }

      CreateLookatMatrix();
    }

    public override void CreateLookatMatrix()
    {
      this.lookat = Matrix.Lerp(viewFirstCamera, viewSecondCamera, amount);
    }
  }
}
