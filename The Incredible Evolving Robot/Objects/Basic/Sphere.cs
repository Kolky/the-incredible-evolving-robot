using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects.Basic
{
  public class Sphere : BasicObject
  {
    public Sphere(Game game, float scale)
      : base(game, false)
    {
      this.Model = TierGame.ContentHandler.GetModel("Globe");
      this.Scale = scale;

      this.Initialize();
    }

    public float getRadius()
    {
      float radius = 0.0f;

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        radius = mesh.BoundingSphere.Radius;
      }

      return radius * this.Scale;
    }

    public override void Initialize()
    {
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          effect.DiffuseColor = Color.Blue.ToVector3();
        }
      }
      base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
      TierGame.Device.RenderState.FillMode = FillMode.WireFrame;
      CullMode previousCull = TierGame.Device.RenderState.CullMode;
      TierGame.Device.RenderState.CullMode = CullMode.CullClockwiseFace;

      base.Draw(gameTime);

      TierGame.Device.RenderState.FillMode = FillMode.Solid;
      TierGame.Device.RenderState.CullMode = previousCull;
    }
  }
}
