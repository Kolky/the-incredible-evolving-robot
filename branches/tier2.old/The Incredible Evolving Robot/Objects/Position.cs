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
    private Vector3 coordinate;
    public Vector3 Coordinate
    {
      get { return coordinate; }
      set { coordinate = value; }
    }
		public float CoordinateZ
		{
			get { return coordinate.Z; }
			set { coordinate.Z = value; }
		}

    private Quaternion front;
    public Quaternion Front
    {
      get { return front; }
      set { front = value; }
    }	
    #endregion

    public Position()
    {
      this.Coordinate = Vector3.Zero;
      this.Front = Quaternion.Identity;
    }

    public Position(Position p)
    {
      this.Coordinate = p.Coordinate;
      this.Front = p.Front;
    }
  }
}
