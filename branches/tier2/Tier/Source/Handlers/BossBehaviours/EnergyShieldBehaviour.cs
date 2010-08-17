using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class EnergyShieldBehaviour : BossBehaviour
  {
    private Vector3 projectileBlockDirection;
    private float timeElapsed;
    private EnergyShield shield;

    public EnergyShieldBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_ENERGYSHIELD)
    {
      
    }

    public override void Enable()
    {
      this.IsEnabled = true;
      this.timeElapsed = 0;

      if (shield == null)
      {
        shield = new EnergyShield(this.game);
        this.game.ObjectHandler.InitializeFromBlueprint<EnergyShield>(shield, "EnergyShield");
      }

      shield.Scale = new Vector3(10);

      Vector3 playerForward = Vector3.Transform(Vector3.Forward, this.game.GameHandler.Player.Rotation);

      float angle = (float)Math.Acos(Vector3.Dot(Vector3.Forward, playerForward));
      Vector3 cross = Vector3.Cross(Vector3.Forward, playerForward);

      shield.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver2) *
                        Quaternion.CreateFromAxisAngle(cross, angle);

      this.game.GameHandler.AddObject(shield);
    }

    /// <summary>
    /// Determines if the energy shield blocks the incoming projectile.
    /// </summary>
    /// <param name="direction">The direction of the projectile.</param>
    /// <returns></returns>
    public bool IsBlockProjectile(Vector3 direction)
    {
      float dot = Vector3.Dot(direction, projectileBlockDirection);
      Vector3 cross = Vector3.Cross(direction, projectileBlockDirection);

      return dot <= 0;
    }

    public override void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (this.timeElapsed >= this.game.Options.EnergyShield_Duration)
      {
        this.IsEnabled = false;
        this.game.GameHandler.RemoveObject(shield, GameHandler.ObjectType.Transparent);
      }
    }
  }
}
