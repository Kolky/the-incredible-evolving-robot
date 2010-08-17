using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class RoamingBehaviour : BossBehaviour
  {
    float timeElapsed;    
    Random r;
    Vector3 direction;

    public RoamingBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_ROAMING)
    {
      this.Transform = Matrix.Identity;
      this.TTL = 100000;
      timeElapsed = 0;
      r = new Random(DateTime.Now.Millisecond);      
    }

    public override void Enable()
    {
      this.IsEnabled = true;
      DetermineDirection();
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (timeElapsed >= 2500.0f)
      {
        direction = Vector3.Zero;
        
        for (int i = 0; i < r.Next(3) + 1; i++)
        {
          DetermineDirection();
        } 
        
        timeElapsed = 0;
      }

     this.Transform *= 
        Matrix.CreateTranslation(direction * this.game.Options.RoamSpeed);
    }

    private void DetermineDirection()
    {
      switch (r.Next(6))
      {
        case 0:
          direction = Vector3.Left;
          break;
        case 1:
          direction = Vector3.Right;
          break;
        case 2:
          direction = Vector3.Up;
          break;
        case 3:
          direction = Vector3.Down;
          break;
        case 4:
          direction = Vector3.Forward;
          break;
        case 5:
          direction = Vector3.Backward;
          break;
      }
    }
  }
}
