using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects;
using Tier.Source.Objects.Turrets;
using Microsoft.Xna.Framework;

namespace Tier.Source.Handlers
{
  public class BossCompositionHandler
  {
    #region Properties
    private TierGame game;
    private List<BossPiece> bosspieces;
    private List<Turret> turrets;
    private BoundingBox box;
    private bool isCompositionChanged;

    public BoundingBox BoundingBox
    {
      get { return box;}
    }
    #endregion

    public BossCompositionHandler(TierGame game)
    {
      this.game = game;
      this.bosspieces = new List<BossPiece>();
      this.turrets = new List<Turret>();
      this.isCompositionChanged = true;
    }

    public void AddBossPiece(BossPiece piece)
    {
      this.bosspieces.Add(piece);
      this.isCompositionChanged = true;
    }

    public void AddTurret(Turret turret)
    {
      this.turrets.Add(turret);
      this.isCompositionChanged = true;
    }

    public void Reset()
    {
      this.bosspieces.Clear();
      this.turrets.Clear();
    }

    /// <summary>
    /// Determine global boundingbox of all BossPieces contained in boss.
    /// </summary>
    /// <returns></returns>
    public void DetermineBossPiecesBoundingBox()
    {
      if (!this.isCompositionChanged)
        return;

      Vector3 min = Vector3.Zero, max = Vector3.Zero;

      foreach (BossPiece piece in this.bosspieces)
      {
        if (piece.FadingModifier == null ||
            piece.FadingModifier.FadeAmount < 1)
          continue;

        foreach (BoundingBox b in piece.CollisionModifier.BoundingBoxes)
        {
          BoundingBox box = piece.CollisionModifier.TransformBoundingbox(b);

          if (box.Min.X < min.X)
          {
            min.X = box.Min.X;
          }
          if (box.Max.X < min.X)
          {
            min.X = box.Max.X;
          }

          if (box.Min.Y < min.Y)
          {
            min.Y = box.Min.Y;
          }
          if (box.Max.Y < min.Y)
          {
            min.X = box.Max.Y;
          }

          if (box.Min.Z < min.Z)
          {
            min.Z = box.Min.Z;
          }
          if (box.Max.Z < min.Z)
          {
            min.Z = box.Max.Z;
          }

          if (box.Max.X > max.X)
          {
            max.X = box.Max.X;
          }
          if (box.Min.X > max.X)
          {
            max.X = box.Min.X;
          }

          if (box.Max.Y > max.Y)
          {
            max.Y = box.Max.Y;
          }
          if (box.Min.Y > max.Y)
          {
            max.Y = box.Min.Y;
          }

          if (box.Max.Z > max.Z)
          {
            max.Z = box.Max.Z;
          }
          if (box.Min.Z > max.Y)
          {
            max.Y = box.Min.Z;
          }
        }
      }

      this.isCompositionChanged = false;
      this.box = new BoundingBox(min, max);
    }

    public float GetLength()
    {
      float length = 0;

      // Determine distance from boss
      BoundingBox bb = this.BoundingBox;
      Vector3 diff = bb.Max - bb.Min;
      length = bb.Max.Length();
      if (bb.Min.Length() > length)
      {
        length = bb.Min.Length();
      }

      return length;
    }
  }
}
