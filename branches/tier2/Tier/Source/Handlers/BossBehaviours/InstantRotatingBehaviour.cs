using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class InstantRotatingBehaviour : BossBehaviour
  {
    #region Privates
    private Matrix rotateTo;
    private Matrix rotateFrom;
    private float timeElapsed;
    #endregion

    public InstantRotatingBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_INSTANTROTATING)
    {
      this.rotateFrom = Matrix.Identity;
    }

    public override void Enable()
    {
      this.IsEnabled = true;
      this.timeElapsed = 0;
      CreateRotationMatrix();
    }

    private void CreateRotationMatrix()
    {
      Random r = new Random(DateTime.Now.Millisecond);
      Vector3 axis = Vector3.Zero;
      float angle = 0;

      switch (r.Next(2))
      {
        case 0:
          angle = MathHelper.PiOver2;
          break;
        case 1:
          angle = MathHelper.Pi;
          break;
      }

      switch (r.Next(3))
      {
        case 0:
          axis = Vector3.Up;
          break;
        case 1:
          axis = Vector3.Left;
          break;
        case 2:
          axis = Vector3.Forward;
          break;
      }

      this.rotateTo = this.rotateFrom *
        Matrix.CreateFromAxisAngle(axis, angle);
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      if (timeElapsed > this.game.Options.Rotating_Duration)
      {
        this.IsEnabled = false;
        // Done rotating, set rotateFrom to the current rotateTo so future rotating wil be handled correctly
        this.Transform = this.rotateFrom = this.rotateTo;
        handler.EnableBehaviour(BossBehaviourType.BBT_ROTATING);        
        return;
      }

      this.Transform = Matrix.Lerp(this.rotateFrom, this.rotateTo,
        this.timeElapsed / this.game.Options.Rotating_Duration);
    }
  }
}
