using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Handlers
{
  public class EffectParameterHandler
  {
    private struct AmbientLight
    {
      public Vector4 intensity;
      public Vector4 color;
    };

    private struct DirectionalLight
    {
      public Vector4 direction;
      public Vector4 color;
    };

    private AmbientLight ambient;
    private DirectionalLight[] directionalLights;
    private TierGame game;

    public EffectParameterHandler(TierGame game)
    {
      this.game = game;

      ambient = new AmbientLight();
      ambient.color = Vector4.One;
      ambient.intensity = new Vector4(0.45f);

      directionalLights = new DirectionalLight[2];
      directionalLights[0] = new DirectionalLight();
    }

    public void Update(GameTime gameTime)
    {
      if (this.game.GameHandler.Camera == null)
        return;
      
      directionalLights[0].color = Color.White.ToVector4();
      directionalLights[0].direction = new Vector4(this.game.GameHandler.Camera.Position, 1.0f);
      directionalLights[0].direction.Normalize();

      Effect ef = this.game.ContentHandler.GetAsset<Effect>("DefaultTextured");
      ef.Parameters["ambientLight"].StructureMembers["intensity"].SetValue(ambient.intensity);
      ef.Parameters["ambientLight"].StructureMembers["color"].SetValue(ambient.color);
      ef.Parameters["directionalLights"].Elements[0].StructureMembers["direction"].SetValue(
        directionalLights[0].direction);
      ef.Parameters["directionalLights"].Elements[0].StructureMembers["color"].SetValue(
        directionalLights[0].color);
      ef.Parameters["matProj"].SetValue(this.game.GameHandler.Projection);
      ef.Parameters["matView"].SetValue(this.game.GameHandler.View);
    }
  }
}
