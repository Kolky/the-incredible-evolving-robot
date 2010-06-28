using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Tier.Objects;
using Tier.Handlers;
using Tier.Misc;
using Microsoft.Xna.Framework.Graphics;
using Tier.Objects.Attachable;

namespace Tier.Objects.Destroyable.Projectile
{
  public class Laser : Tier.Objects.Projectile
  {
    private Matrix transform;

    public Laser(Game game, Position sourcePos)
      : base(game, sourcePos,40)
    {
      this.TimeToLive = 750;
      this.ModelName = "Laser";
      this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
      this.Scale = 0.075f;
      this.Sort = SortFilter.Other;

      this.addBoundingShere(0.1f, Vector3.Zero);
      this.Initialize();
    }

    public override void Initialize()
    {      
      base.Initialize();

      this.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2 + MathHelper.Pi);
    }

    public override void Update(GameTime gameTime)
    {
      transform = Matrix.CreateScale(this.Scale) *
              RotationFix *
              Matrix.CreateFromQuaternion(this.Position.Front) *
              Matrix.CreateTranslation(this.Position.Coordinate);


      this.UpdateBoundingObjects();
      base.Update(gameTime);

      /*
      foreach (BasicObject o in GameHandler.ObjectHandler.Objects)
      {
        if (o.GetType().IsSubclassOf(typeof(BlockPiece)))
        {
          BlockPiece p = (BlockPiece)o;

          if (!p.Exploded && p.DetectCollision(this))
          {
            this.TimeToLive = 0;
            p.Explode();
          }
        }
      }
      */
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Model != null)
      {
        for (int i = 0; i < this.Model.Meshes.Count; i++)
        {
          foreach (BasicEffect effect in this.Model.Meshes[i].Effects)
          {
            effect.World = transform;
            effect.View = GameHandler.Camera.View;
            effect.Projection = GameHandler.Camera.Projection;
          }
          this.Model.Meshes[i].Draw();
        }
      }
    }
  }
}
