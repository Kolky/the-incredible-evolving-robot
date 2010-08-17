using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class RotatingBehaviour : BossBehaviour
  {
    private Random r;
    private int timeToNext, timeElapsed;
    private Matrix rotation;

    public RotatingBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_ROTATING)
    {
      r = new Random(System.DateTime.Now.Second * System.DateTime.Now.Millisecond);
      timeToNext = 0;// r.Next(10000);
      rotation = Matrix.Identity;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (timeElapsed >= timeToNext)
      {
        DetermineNewValues();
      }

      this.Transform *= rotation;
    }

    public override void Enable()
    {
      this.IsEnabled = true;
    }

    private void DetermineNewValues()
    {
      timeToNext = 2000;//r.Next(10000);
      timeElapsed = 0;

      float angle = (float)(r.NextDouble() * Math.PI / 100);

      switch (r.Next(3))
      {
        case 0:
          rotation = Matrix.CreateFromAxisAngle(Vector3.Up, angle);
          break;
        case 1:
          rotation = Matrix.CreateFromAxisAngle(Vector3.Right, angle);
          break;
        case 2:
          rotation = Matrix.CreateFromAxisAngle(Vector3.Forward, angle);
          break;
      }
    }
  }
}
