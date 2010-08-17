using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;

namespace Tier.Source.ObjectModifiers
{
  public class AttachableModifier : ObjectModifier
  {
    #region Properties
    private List<GameObject> connectedTo;
    private List<Connector> connectors;
    private bool rotationUpdate;
    private int level;

    public int Level
    {
      get { return level; }
      set { level = value; }
    }

    public List<GameObject> ConnectedTo
    {
      get { return connectedTo; }
    }
	
    public bool RotationUpdated
    {
      get { return rotationUpdate; }
      set { rotationUpdate = value; }
    }
	
    public List<Connector> Connectors
    {
      get { return connectors; }
      set { connectors = value; }
    }
    #endregion

    public AttachableModifier(GameObject obj)
      : base(obj)
    {
      this.connectors = new List<Connector>();
      obj.AttachableModifier = this;
      connectedTo = new List<GameObject>();
    }

    public override void Update(GameTime gameTime)
    {
      this.RotationUpdated = false;
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new AttachableModifier(parent);

      foreach (Connector conn in this.connectors)
      {
        Connector newconn = new Connector(conn.Position, conn.Pivot, parent, conn.Type);
        newconn.Scale = conn.Scale;

        ((AttachableModifier)objmod).AddConnection(newconn);
      }
    }

    /// <summary>
    /// Clears all connections made on current connectors.
    /// </summary>
    public void Clear()
    {
      foreach (Connector conn in this.connectors)
      {
        conn.Clear();
      }
    }

    public void AddConnection(Connector conn)
    {
      this.connectors.Add(conn);
    }

    /// <summary>
    /// Attaches two objects (with AttachableModifiers) to each other using the supplied indices.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="sourceIndex"></param>
    /// <param name="childIndex"></param>
    public bool Attach(GameObject obj, int sourceIndex, int childIndex)
    {
      if (obj.GetType() == typeof(BossPiece))
      {
        ((BossPiece)this.Parent).Depth = ((BossPiece)this.Parent).Depth + 1;
      }

      if (this.Connectors[sourceIndex].ConnectedTo == null)
      {
        obj.AttachableModifier.Level = this.level + 1;

        return this.connectors[sourceIndex].Connect(
          obj,
          obj.AttachableModifier.Connectors[childIndex]);        
      }

      return false;
    }

    /// <summary>
    /// Parent object has moved, update all connected objects
    /// </summary>    
    public void UpdateFromVelocity(Vector3 velo, GameObject parent)
    {
      foreach (Connector conn in this.connectors)
      {
        if (conn.ConnectedTo != null && conn.ConnectedTo != parent)
        {
          conn.ConnectedTo.Position = conn.Parent.Position + conn.Position;
          conn.ConnectedTo.AttachableModifier.UpdateFromVelocity(velo, conn.Parent);
        }
      }
    }

    /// <summary>
    /// Parent object has rotated, rotate all connected objects
    /// </summary>    
    public void UpdateFromRotation(Quaternion rotation, GameObject parent)
    {
      foreach (Connector conn in this.connectors)
      {
        if (conn.ConnectedTo != null && conn.ConnectedTo != parent)
        {
          this.RotationUpdated = true;
          conn.ConnectedTo.Rotation *= rotation;
          conn.ConnectedTo.AttachableModifier.UpdateFromRotation(rotation, conn.Parent);
        }
      }
    }
  }
}
