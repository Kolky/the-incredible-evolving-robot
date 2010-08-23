using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Handlers;
using Tier.Misc;

namespace Tier.Objects.Basic
{
  class ExplosionCluster : BasicObject
  {
    private Vector3 size;
    private Vector3 pos;
    private float ttl;

    public ExplosionCluster(Game game, Vector3 size, Vector3 pos, float ttl)
      :base(game,false)
    { 
      this.size = size;
      this.pos = pos;
      this.ttl = ttl;

      for (int i = 0; i < 5; i++)
      {
        Vector3 randomSize = new Vector3(Options.Random.Next(-1000, 1000) * (size.X / 1000f),
                                       Options.Random.Next(-1000, 1000) * (size.Y / 1000f),
                                       Options.Random.Next(-1000, 1000) * (size.Z / 1000f));
        randomSize = pos + randomSize;

        GameHandler.ObjectHandler.AddObject(new AnimatedBillboard(
          GameHandler.Game, "Explosion", false,
          randomSize,
          Options.Random.Next(10, 800) / 400f,
          100)
          );
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.ttl -= gameTime.ElapsedGameTime.Milliseconds;

      if (this.ttl <= 0)
      {
        GameHandler.ObjectHandler.RemoveObject(this);
        return;
      }      
    }
  }
}
