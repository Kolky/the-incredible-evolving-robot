using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Objects.Destroyable
{
  public class Connector
  {
    #region Properties
    private AttachableObject parent;
    private AttachableObject connectedTo;

    public AttachableObject ConnectedTo
    {
      get { return connectedTo; }
      set { connectedTo = value; }
    }

    private Vector3 position;
    private Vector3 pivot;

    public Vector3 Pivot
    {
      get { return pivot; }
      set { pivot = value; }
    }

    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }

    public AttachableObject Parent
    {
      get { return parent; }
      set { parent = value; }
    }
    #endregion

    public Connector(Vector3 position, Vector3 pivot, AttachableObject parent)
    {
      this.position = position;
      this.pivot = pivot;
      this.parent = parent;
    }

    public void Connect(AttachableObject obj, Connector connector)
    {
      ConnectConnectors(connector);
      ManipulatePiece(obj, connector);      
    }

    private void ConnectConnectors(Connector connector)
    {
      this.ConnectedTo = connector.Parent;
      connector.ConnectedTo = this.Parent;
    }

    private void ManipulatePiece(AttachableObject piece, Connector connector)
    {
      Vector3 newpivot = Vector3.Cross(this.pivot, connector.Pivot);
      Quaternion rotation = Quaternion.Identity;

      if (newpivot == Vector3.Zero && (this.pivot == connector.pivot))
      {
          newpivot = -connector.pivot;

          if (Math.Abs(pivot.Z) == 1)
          {
              rotation = Quaternion.CreateFromYawPitchRoll(0, MathHelper.Pi * newpivot.Z, 0);
          }
          else
          {              
              rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.Pi * newpivot.X,
                        MathHelper.Pi * newpivot.Y, MathHelper.Pi * newpivot.Z);
          }
      }
      else
      {
        rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.PiOver2 * newpivot.Y,
                  MathHelper.PiOver2 * newpivot.X, MathHelper.PiOver2 * newpivot.Z);
      }

      Vector3 rotatedConnectorPosition = Vector3.Transform(connector.Position, rotation);
      Vector3 newConnectorPosition = connector.Parent.Position.Coordinate + rotatedConnectorPosition;
      Vector3 parentConnectorPosition = this.Position + this.Parent.Position.Coordinate;
   
      // Verschil vector opstellen
      Vector3 diff = -(newConnectorPosition - parentConnectorPosition);
      // Blokje positioneren tegen connectiepunt
      Vector3 temp = Vector3.Add(connector.Parent.Position.Coordinate, diff);
      connector.Parent.Position.Coordinate = RoundVector3(temp);
      // Rotatie van blokje goed zetten
      connector.Parent.Position.Front = rotation;
      // Ook posities van pivots aanpassen aan nieuwe rotatie
      foreach (Connector conn in connector.Parent.Connectors)
      {
        conn.Pivot = Vector3.Transform(conn.Pivot, rotation);        
        conn.Position = Vector3.Transform(conn.Position, rotation);

        conn.Pivot = RoundVector3(conn.Pivot);
        conn.Position = RoundVector3(conn.Position);
      }
    }

    private Vector3 RoundVector3(Vector3 vector)
    {
      vector.X = (float)Math.Round(vector.X, 1);
      vector.Y = (float)Math.Round(vector.Y, 1);
      vector.Z = (float)Math.Round(vector.Z, 1);

      return vector;
    }
  }
}
