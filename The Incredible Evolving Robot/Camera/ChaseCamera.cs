using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Objects.Destroyable;

namespace Tier.Camera
{
    public class ChaseCamera : Camera
    {
        #region Properties
        private Player source;
        public Player Source
        {
            get { return source; }
            set { source = value; }
        }

        private Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        #endregion

        public ChaseCamera(Vector3 position, Vector3 target, Player source)
            : this(position, target, source, Vector3.Zero)
        {
        }

        public ChaseCamera(Vector3 position, Vector3 target, Player source, Vector3 offset)
            : base(position, target)
        {
            this.Source = source;
        }

        public override void Update(GameTime gameTime)
        {
            this.Rotation = this.Source.Position.Front;
            this.Target = this.Source.Position.Coordinate + Vector3.Transform(this.Offset, this.Rotation);


            base.Update(gameTime);
        }
    }
}
