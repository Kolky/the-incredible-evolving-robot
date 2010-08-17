using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tier.Source.Objects.Projectiles
{
  public class ShotgunProjectile : Projectile
  {
    public ShotgunProjectile(TierGame game)
      : base(game)
    { }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);

      float length = ((Vector3)(this.Game.GameHandler.Player.Position - this.Position)).Length();

      if (length <= 1.0f)
      {
        this.HandlerElement.Stop();

        this.Game.GameHandler.Player.DestroyableModifier.IsHit(this.Damage);
      }
    }
  }
}
