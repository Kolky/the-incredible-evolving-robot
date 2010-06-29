using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects
{
    public class Movement
    {
        #region Properties
        public Quaternion Rotation { get; set; }
        public Vector3 Velocity { get; set; }
        #endregion

        public Movement()
        {
            Rotation = Quaternion.Identity;
            Velocity = Vector3.Zero;
        }
    }
}
