using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Attachable;
using Tier.Objects.Destroyable;

namespace Tier.Misc
{
  public struct GrowthPatternBlock
  {
    public bool Required;
    public string Name;
    public int SourceConnector;
    public int BlockConnector;
  };

  public struct GrowthPattern
  {
    public string Name;
    public List<GrowthPatternBlock> Blocks;
  };

  public class BossGrowth
  {
    #region Properties
    private Hashtable growthpatterns;

    public Hashtable GrowthPatterns
    {
      get { return growthpatterns; }
    }
    #endregion

    public BossGrowth()
    {
      this.growthpatterns = new Hashtable();
    }
  }

  public class BossGrower
  {
    #region Properties
    private bool growing;

    public bool Growing
    {
      get { return growing; }
      set { growing = value; }
    }
	
    private Enemy parent;
    public Enemy Parent
    {
      get { return parent; }
      set { parent = value; }
    }	
    #endregion

    public void Grow()
    {
      this.growing = true;

      // Remove all weapons from outer connections
      this.RemoveWeapons(this.Parent, this.Parent);

      if (!this.Parent.Grown)
        this.Parent.Grow((GrowthPattern)this.Parent.GrowthPattern.GrowthPatterns[this.Parent.Name]);
      else
        this.GrowChilderen(this.Parent, this.Parent);

      // Attach all new blocks
      foreach (BlockPiece p in this.Parent.Pieces)
      {
        this.GrowChild(p);
      }
      this.Parent.Pieces.Clear();

      this.Parent.Spawn();
      this.growing = false;
    }

    private void RemoveWeapons(BlockPiece piece, BlockPiece parent)
    {
      foreach (Connector conn in piece.Connectors)
      {
        if (conn.ConnectedTo != null)
        {
          // Remove weapons all connected pieces
          if (conn.ConnectedTo.GetType() == typeof(BlockPiece))
          {
            BlockPiece newpiece = (BlockPiece)conn.ConnectedTo;

            // Don't jump back to parent to take care of Stack overflow
            if (parent != conn.ConnectedTo)
            {
              RemoveWeapons(newpiece, piece);
            }
          }
          else if (conn.ConnectedTo.GetType() == typeof(TurretHolder))
          {
            GameHandler.ObjectHandler.RemoveObject(((TurretHolder)conn.ConnectedTo).Weapon);
            GameHandler.ObjectHandler.RemoveObject(conn.ConnectedTo);
            conn.ConnectedTo = null;
          }
        }
      }
    }

    private void GrowChilderen(BlockPiece piece, BlockPiece parent)
    {
      foreach (Connector conn in piece.Connectors)
      {
        if (conn.ConnectedTo != null && conn.ConnectedTo.GetType() == typeof(BlockPiece))
        {
          BlockPiece newpiece = (BlockPiece)conn.ConnectedTo;

          if (newpiece.Grown)
          {
            if (parent != conn.ConnectedTo)
              this.GrowChilderen(newpiece, piece);
          }
          else
            this.Parent.Pieces.Add(newpiece);
        }
      }
    }

    private void GrowChild(BlockPiece piece)
    {
      piece.Grow((GrowthPattern)this.Parent.GrowthPattern.GrowthPatterns[piece.Name]);
    }
  }
}
