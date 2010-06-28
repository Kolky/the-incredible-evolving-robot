using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Tier.Objects;
using Tier.Handlers;
using Tier.Misc;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects.Destroyable.Projectile
{
  public class Laserbeam : Tier.Objects.Projectile
  {

    public Laserbeam(Game game, Position sourcePos)
      : base(game, true, sourcePos, 0)
    {
      this.TimeToLive = 5000;

      this.Model = TierGame.ContentHandler.GetModel("Laserbeam");

      this.ModelMeta = new ModelMeta(this.Model);

      this.Scale = 0.5f;
      //this.Sort = SortFilter.Bloom;
      this.Initialize();
    }

    public override void Initialize()
    {
      this.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.Pi);

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          if (mesh.Name.Equals("mesh_shell"))
          {
            effect.Alpha = 0.40f;
            effect.EmissiveColor = new Vector3(1.0f, 0f, 0f);
          }
          else
          {
            effect.EmissiveColor = new Vector3(1f);
          }
        }
      }
      base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Model != null)
      {
        this.GraphicsDevice.RenderState.AlphaBlendEnable = true;
        this.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
        this.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
        this.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;

        float s;

        if (this.CurrentTime < 1000)
          s = this.Scale * this.CurrentTime / 1000;
        else if (this.CurrentTime > this.TimeToLive - 1000)
          s = this.Scale * (1000 - (this.CurrentTime - (this.TimeToLive - 1000))) / 1000;
        else s = this.Scale;
        

        Matrix world = Matrix.CreateScale(new Vector3(s, s, 20)) *
              RotationFix *
              Matrix.CreateFromQuaternion(this.Position.Front) *
              Matrix.CreateTranslation(this.Position.Coordinate);


        foreach (ModelMesh mesh in this.Model.Meshes)
        {
          foreach (BasicEffect effect in mesh.Effects)
          {
            effect.World = world;
            effect.View = GameHandler.Camera.View;
            effect.Projection = GameHandler.Camera.Projection;
          }
          mesh.Draw();
        }
      }
      this.GraphicsDevice.RenderState.AlphaBlendEnable = false;
    }
  }
}

