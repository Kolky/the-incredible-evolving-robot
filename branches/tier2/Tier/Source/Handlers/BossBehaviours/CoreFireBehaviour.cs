using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class CoreFireBehaviour : BossBehaviour
  {
    private enum CoreFireBehaviourType
    {
      CFBT_360, CFBT_AIMED, CFBT_IDLE
    };
    
    private float timeElapsed;    
    private int bulletsFired;
    private CoreFireBehaviourType type;

    public CoreFireBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_COREFIRE)
    {
      this.type = CoreFireBehaviourType.CFBT_IDLE;
    }

    // Spawn projectiles and shoot in a 360 degree radius
    private void Do360()
    {      
      for (int i = 0; i < 50; i++)
      {
        LaserProjectile pro = new LaserProjectile(this.game);
        Vector3 dir = this.game.GameHandler.Player.Position;
        dir.Normalize();

        if (this.game.ObjectHandler.InitializeFromBlueprint<LaserProjectile>(pro, "LaserProjectile"))
        {
          pro.Position = Vector3.Zero;
          pro.MovableModifier.Velocity = Vector3.Transform(
            dir,
            Quaternion.CreateFromAxisAngle(Vector3.Up, (float)((Math.PI * 2) * (i / 50.0f)))
          );
          pro.MovableModifier.Velocity *= pro.Speed;

          this.game.GameHandler.AddObject(pro);
        }
      }

      this.type = CoreFireBehaviourType.CFBT_IDLE;
    }

    // Quickly fire some projectiles at player
    private void DoAimed(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (bulletsFired >= 100)
      {
        this.type = CoreFireBehaviourType.CFBT_IDLE;
        bulletsFired = 0;
        timeElapsed = 0;
        return;
      }

      if (timeElapsed > 20)
      {
        timeElapsed = 0;
        bulletsFired++;        
        LaserProjectile pro = new LaserProjectile(this.game);

        Vector3 dir = this.game.GameHandler.Player.Position;
        dir.Normalize();

        if (this.game.ObjectHandler.InitializeFromBlueprint<LaserProjectile>(pro, "LaserProjectile"))
        {
          pro.Position = Vector3.Zero;
          pro.MovableModifier.Velocity = dir;
          pro.MovableModifier.Velocity *= pro.Speed * 2;

          this.game.GameHandler.AddObject(pro);
        }
      }
    }

    private void DetermineNewType()
    {
      // Determine new fire behaviour
      switch (this.game.Random.Next(2))
      {
        case 0:
          this.type = CoreFireBehaviourType.CFBT_360;
          break;
        case 1:
          this.type = CoreFireBehaviourType.CFBT_AIMED;
          break;
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.type == CoreFireBehaviourType.CFBT_IDLE)
      {
        timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

        if (timeElapsed >= 1000)
        {
          timeElapsed = 0;
          DetermineNewType();
        }
      }

      switch (this.type)
      {
        case CoreFireBehaviourType.CFBT_360:
          Do360();
          break;
        case CoreFireBehaviourType.CFBT_AIMED:
          DoAimed(gameTime);
          break;
      }
    }

    public override void Enable()
    {
      this.IsEnabled = true;
    }
  }
}