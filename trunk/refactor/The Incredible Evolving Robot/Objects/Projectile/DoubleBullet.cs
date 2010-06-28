using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Objects;
using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Attachable;


namespace Tier.Objects.Destroyable.Projectile
{
    public class DoubleBullet : Tier.Objects.Projectile
    {
        public DoubleBullet(Game game, Position sourcePos)
            : base(game, sourcePos, 30)
        {
            this.TimeToLive = 1500;
            this.ModelName = "DoubleBullet";
            this.ModelMeta = TierGame.ContentHandler.GetModelMeta(this.ModelName);
            this.Model = this.ModelMeta.Model;
            this.Scale = 0.5f;
            //this.Sort = SortFilter.OtherInstanced;
            //this.IsInstanced = true;
            this.RotationFix = Matrix.CreateRotationX(-MathHelper.PiOver2);
            this.Initialize();
        }
    }
}
