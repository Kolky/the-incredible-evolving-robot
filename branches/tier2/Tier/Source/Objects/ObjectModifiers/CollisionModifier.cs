using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;
using pjEngine.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.ObjectModifiers
{
  public class CollisionModifier : ObjectModifier
  {
    #region Properties
    private List<BoundingBox> boxes;
    private IOcTreeSegment<GameObject> segment;
    private Matrix transform;

    public Matrix Transform
    {
      get { return transform; }
      set { transform = value; }
    }
	
    /// <summary>
    /// The OcTree segment this modifier's parent is contained in
    /// </summary>
    public IOcTreeSegment<GameObject> OcTreeSegment
    {
      get { return segment; }
      set { segment = value; }
    }
	
    public List<BoundingBox> BoundingBoxes
    {
      get { return boxes; }
      set { boxes = value; }
    }
    #endregion

    public CollisionModifier(GameObject parent)
      : base(parent)
    {
      parent.CollisionModifier = this;
      this.boxes = new List<BoundingBox>();
      this.transform = Matrix.Identity;
    }

    public override void Update(GameTime gameTime)
    {
      /*
       * Absolete code, only one oc tree segment is used
       * if (segment != null &&
        segment.IsInSegment(this.Parent) == ContainmentType.Disjoint)
      {
        segment.RemoveObject(this.Parent);
        Parent.Game.GameHandler.AddObjectToOctree(Parent);
      }*/
    }

    public BoundingBox TransformBoundingbox(BoundingBox box)
    {
      box.Min = Vector3.Transform(box.Min, transform * Parent.Game.BehaviourHandler.Transform);
      box.Max = Vector3.Transform(box.Max, transform * Parent.Game.BehaviourHandler.Transform);

      return box;
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new CollisionModifier(parent);

      foreach (BoundingBox box in this.boxes)
      {
        ((CollisionModifier)objmod).AddBoundingBox(new BoundingBox(box.Min, box.Max));
      }
    }

    public void AddBoundingBox(BoundingBox box)
    {
      this.boxes.Add(box);
    }

    public ContainmentType CheckCollision(CollisionModifier colmod)
    {
      foreach (BoundingBox box in colmod.BoundingBoxes)
      {
        foreach (BoundingBox b in this.boxes)
        {
          BoundingBox b1 = colmod.TransformBoundingbox(box);
          BoundingBox b2 = TransformBoundingbox(b);

          ContainmentType ct = b1.Contains(b2);

          if (ct != ContainmentType.Disjoint)
          {
            return ct;
          }
        }
      }

      return ContainmentType.Disjoint;
    }

    public ContainmentType CheckCollision(BoundingBox box)
    {
      foreach (BoundingBox b in this.boxes)
      {
        BoundingBox _b = TransformBoundingbox(b);
        ContainmentType ct = box.Contains(_b);

        if (ct != ContainmentType.Disjoint)
        {          
          return ct;
        }
      }

      return ContainmentType.Disjoint;
    }

    public ContainmentType CheckCollision(BoundingSphere sphere)
    {
      foreach (BoundingBox b in this.boxes)
      {
        BoundingBox _b = TransformBoundingbox(b);
        ContainmentType ct = _b.Contains(sphere);

        if (ct != ContainmentType.Disjoint)
        {
          return ct;
        }
      }

      return ContainmentType.Disjoint;
    }

    public ContainmentType CheckCollision(Vector3 point)
    {
      foreach (BoundingBox b in this.boxes)
      {
        BoundingBox _b = TransformBoundingbox(b);
        ContainmentType ct = _b.Contains(point);
        
        if (ct != ContainmentType.Disjoint)
        {
          return ct;
        }
      }

      return ContainmentType.Disjoint;
    }

    public void CheckCollision(Ray ray, out float? distance)
    {
      distance = float.MaxValue;

      foreach (BoundingBox box in this.BoundingBoxes)
      {
        BoundingBox b = TransformBoundingbox(box);
        float? dist = ray.Intersects(b);

        if (dist != null && dist < distance)
        {
          distance = dist;
        }
      }
    }
  }
}
