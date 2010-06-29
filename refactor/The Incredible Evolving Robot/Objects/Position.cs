using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects
{
    public class Position
    {
        #region Properties
        public Vector3 Coordinate { get; set; }
        public Quaternion Front { get; set; }
        #endregion

        public Position()
        {
            Coordinate = Vector3.Zero;
            Front = Quaternion.Identity;
        }

        public Position(Position p)
        {
            Coordinate = p.Coordinate;
            Front = p.Front;
        }
    }
}
