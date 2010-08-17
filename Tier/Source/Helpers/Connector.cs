using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;
using Tier.Source.Objects.Turrets;

namespace Tier.Source.Helpers
{
  public enum ConnectorType
  {
    BossPiece, Weapon
  };

  public class Connector
  {
    #region Properties
    private GameObject parent;
    private GameObject connectedTo;
    private Vector3 position;
    private Vector3 pivot;
    private ConnectorType type;
    private Vector3 scale;
    private float rotation;
    private Vector3 extraTranslation;
    private readonly Vector3 originalPosition;

    /// <summary>
    /// Extra translation after connecting
    /// </summary>
    public Vector3 ExtraTranslation
    {
      get { return extraTranslation; }
      set { extraTranslation = value; }
    }

    /// <summary>
    /// Extra rotation around pivot
    /// </summary>
    public float Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }
	
    public Vector3 Scale
    {
      get { return scale; }
      set { scale = value; }
    }
	
    public ConnectorType Type
    {
      get { return type; }
      set { type = value; }
    }
	
    public GameObject Parent
    {
      get { return parent; }
      set { parent = value; }
    }

    public GameObject ConnectedTo
    {
      get { return connectedTo; }
      set { connectedTo = value; }
    }

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
    #endregion

    public Connector(Vector3 position, Vector3 pivot, GameObject parent)
      : this(position, pivot, parent, ConnectorType.BossPiece)
    { }

    public Connector(Vector3 position, Vector3 pivot, GameObject parent, ConnectorType type)
    {
      this.originalPosition = this.position = position;
      this.pivot = pivot;
      this.parent = parent;
      this.type = type;
    }

    public bool Connect(GameObject obj, Connector connector)
    {
      ConnectConnectors(connector);  
      ManipulatePiece(obj, connector);
      
      // Object positioned correctly for attachement, check collision with other objects
      List<GameObject> objects;
      this.parent.Game.GameHandler.GetObjects(out objects, Handlers.GameHandler.ObjectType.DefaultTextured);

      foreach (GameObject o in objects)
      {
        if (
          o.GetType() != typeof(BossPiece) ||
          o.CollisionModifier == null ||
          o.CollisionModifier.CheckCollision(connector.Parent.CollisionModifier) == ContainmentType.Disjoint)
          continue;

        this.connectedTo = null;
        this.parent.Game.GameHandler.RemoveObject(connector.parent, Handlers.GameHandler.ObjectType.DefaultTextured);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Clears the current connection.
    /// </summary>
    public void Clear()
    {
      this.position = originalPosition;
      //this.parent = null;
      this.connectedTo = null;      
    }

    private void ConnectConnectors(Connector connector)
    {
      this.ConnectedTo = connector.Parent;
      connector.ConnectedTo = this.Parent;
    }

    private Vector3 RoundVector3(ref Vector3 vector)
    {
      if (vector.X <= 0.001f)
      {
        vector.X = 0;
      }
      if (vector.Y <= 0.001f)
      {
        vector.Y = 0;
      }
      if (vector.Z <= 0.001f)
      {
        vector.Z = 0;
      }

      return vector;
    }

    private void ManipulatePiece(GameObject piece, Connector connector)
    {
      Quaternion rot = Quaternion.Identity;
      this.pivot.Normalize();
      connector.pivot.Normalize();

      float angle = (float)Math.Acos(Vector3.Dot(this.pivot, connector.pivot));
      Vector3 cross = Vector3.Cross(this.pivot, connector.pivot);

      if (cross != Vector3.Zero)
      {
        cross.Normalize();
        rot *= Quaternion.CreateFromAxisAngle(cross, angle);
      }
      else if(this.pivot == connector.pivot)
      {
        // Pivots are the same, do a flip according to the axis
        if (this.pivot.X != 0)
        {
          rot *= Quaternion.CreateFromAxisAngle(Vector3.Up, -MathHelper.Pi);
        }
        else if (this.pivot.Y != 0)
        {
          rot *= Quaternion.CreateFromAxisAngle(Vector3.Forward, -MathHelper.Pi);
        }
        else if (this.pivot.Z != 0)
        {
          rot *= Quaternion.CreateFromAxisAngle(Vector3.Left, -MathHelper.Pi);
        }
      }

      rot *= Quaternion.CreateFromAxisAngle(this.pivot, this.rotation);

      if (connector.Parent.GetType() == typeof(Turret) ||
        connector.Parent.GetType().IsSubclassOf(typeof(Turret)))
      {
        ((Turret)connector.Parent).BaseRotation = rot;
      }

      foreach (Connector conn in connector.Parent.AttachableModifier.Connectors)
      {
        conn.Pivot = PublicMethods.RoundVector3(Vector3.Transform(conn.Pivot, rot));
        conn.Position = Vector3.Transform(conn.Position, rot);
      }

      // Translate child object according to the difference between the parent's and child connector (world)positions
      Vector3 parentConnectorPosition = this.Parent.Position + this.position;
      Vector3 childConnectorPosition = connector.Parent.Position + connector.Position;

      // Position and rotate to the connected piece
      piece.Position = parentConnectorPosition - childConnectorPosition;
      piece.Rotation = rot;
      // Transform bounding volumes
      Matrix transform =
        Matrix.CreateFromQuaternion(rot) *
        Matrix.CreateTranslation(piece.Position);
      piece.CollisionModifier.Transform = transform;
    }
  }
}
