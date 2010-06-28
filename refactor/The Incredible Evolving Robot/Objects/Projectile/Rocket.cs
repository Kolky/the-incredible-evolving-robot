using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Misc;
using Tier.Handlers;

namespace Tier.Objects.Destroyable.Projectile
{
    class Rocket : Tier.Objects.Projectile
    {
        public Rocket(Game game, Position sourcePos)
            : base(game, true, sourcePos, 10)
        {
            this.TimeToLive = 2000;
            this.ModelName = "Rocket";
            this.ModelMeta = TierGame.ContentHandler.GetModelMeta(this.ModelName);
            this.Model = this.ModelMeta.Model;
            this.Scale = 0.025f;
            //this.Sort = SortFilter.OtherInstanced;
            //this.IsInstanced = true;
            this.addBoundingShere(0.2f, new Vector3(0f, 0f, 0.8f));

            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            //this.RotationFix = Matrix.CreateRotationY(-MathHelper.PiOver2);
            this.RotationFix = Matrix.CreateRotationX(MathHelper.PiOver2);
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateBoundingObjects();
            base.Update(gameTime);
        }
    }
}
