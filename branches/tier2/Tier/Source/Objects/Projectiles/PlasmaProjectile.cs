using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Projectiles
{
  public class PlasmaProjectile : Projectile
  {
    private float roll;

    public PlasmaProjectile(TierGame game)
      : base(game)
    { }

    public override void Update(GameTime gameTime)
    {
      // Create a roll animation in the plasma projectile. Rotate 360 degrees every second
      this.roll += MathHelper.Pi * 2 / (250.0f / gameTime.ElapsedGameTime.Milliseconds);
      if (roll >= MathHelper.Pi * 2)
        roll = 0;

      // Determine difference to player and change velocity accoringly
      Vector3 diffPlayer = Vector3.Subtract(this.Game.GameHandler.Player.Position, this.Position);
      float length = diffPlayer.Length();
      diffPlayer.Normalize();
      diffPlayer *= this.Speed;
      this.MovableModifier.Velocity = 
        0.75f * this.MovableModifier.Velocity +
        0.25f * diffPlayer;
      
      base.Update(gameTime);

      if (length <= 2.0f)
      {
        if(this.HandlerElement != null)
          this.HandlerElement.Stop();
        
        this.Game.GameHandler.Player.DestroyableModifier.IsHit(this.Damage);
      }
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.Game.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;

      this.Effect.Begin();

      this.Effect.Parameters["coloroverlay"].SetValue(this.Color.ToVector4());
      this.Effect.Parameters["matWorld"].SetValue
      (
        Matrix.CreateScale(this.Scale) *
        Matrix.CreateFromAxisAngle(Vector3.Forward, roll) *
        Matrix.CreateBillboard(
          this.Position,
          this.Game.GameHandler.Camera.Position,
          this.Game.GameHandler.Camera.UpVector,
          this.Game.GameHandler.Camera.ForwardVector)
      );

      foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
      {
        pass.Begin();
        this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
          this.Game.BillboardVertices, 0, 2);
        pass.End();
      }

      this.Effect.End();
    }
  }
}
