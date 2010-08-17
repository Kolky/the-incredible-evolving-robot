using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pjEngine.Helpers
{
  public abstract class OcTreeSegment<T> : IOcTreeSegment<T>
  {
    #region Properties
    protected List<T> objects;
    protected BoundingBox bb;
    protected OcTreeSegment<T>[] segments;
    protected OcTreeSegment<T> parent;
    protected Vector3 min, max;
    protected int currentDepth;
    protected int maximumDepth;

    public int MaximumDepth
    {
      get { return maximumDepth; }
      set { maximumDepth = value; }
    }
	
    public int  CurrentDepth
    {
      get { return currentDepth; }
      set { currentDepth = value; }
    }
	
    public OcTreeSegment<T> Parent
    {
      get { return parent; }
      set { parent = value; }
    }
	
    public BoundingBox BoundingBox
    {
      get { return bb; }
      set { bb = value; }
    }

    public List<T> Objects
    {
      get { return objects; }
      set { objects = value; }
    }

    public OcTreeSegment<T>[] Segments
    {
      get { return segments; }
    }
    #endregion

    public OcTreeSegment(Vector3 min, Vector3 max)
    {
      this.segments = new OcTreeSegment<T>[0];
      this.objects = new List<T>();
      min = min;
      max = max;
    }

    /// <summary>
    /// Add object to this QuadTreeSegment.
    /// </summary>
    /// <param name="obj"></param>
    public IOcTreeSegment<T> AddObject(T obj)
    {
      switch (this.IsInSegment(obj))
      {
        case ContainmentType.Contains:
          // Create subsegments whenever they don't exist yet
          if (this.segments.Length == 0 && this.currentDepth < this.maximumDepth)
          {
            this.segments = new OcTreeSegment<T>[8];

            // Create the subsegments
            for (int i = 0; i < 8; i++)
            {              
              Vector3 min = Vector3.Zero, max = Vector3.Zero;
              float diff = Math.Abs(this.min.X - this.max.X); 

              switch (i)
              {
                // Upper left z = 0
                case 0:
                  min = new Vector3(this.min.X, this.min.Y + diff/2, this.min.Z);
                  max = new Vector3(this.min.X + diff/2, this.max.Y, this.min.Z + diff/2);
                  break;
                // Upper right z = 0
                case 1:
                  min = new Vector3(this.min.X + diff / 2, this.min.Y + diff / 2, this.min.Z);
                  max = new Vector3(this.max.X, this.max.Y, this.min.Z + diff / 2);
                  break;
                // Lower left z = 0
                case 2:
                  min = new Vector3(this.min.X, this.min.Y, this.min.Z);
                  max = new Vector3(this.min.X + diff / 2, this.min.Y + diff / 2, this.min.Z + diff / 2);
                  break;
                // Lower right z = 0
                case 3:
                  min = new Vector3(this.min.X + diff / 2, this.min.Y, this.min.Z);
                  max = new Vector3(this.max.X, this.min.Y + diff / 2, this.min.Z + diff / 2);
                  break;
                // Upper left z = 1
                case 4:
                  min = new Vector3(this.min.X, this.min.Y + diff / 2, this.min.Z + diff / 2);
                  max = new Vector3(this.min.X + diff / 2, this.max.Y, this.max.Z);
                  break;
                // Upper right z = 1
                case 5:
                  min = new Vector3(this.min.X + diff / 2, this.min.Y + diff / 2, this.min.Z + diff / 2);
                  max = new Vector3(this.max.X, this.max.Y, this.max.Z);
                  break;
                // Lower left z = 1
                case 6:
                  min = new Vector3(this.min.X, this.min.Y, this.min.Z + diff / 2);
                  max = new Vector3(this.min.X + diff / 2, this.min.Y + diff / 2, this.max.Z);
                  break;
                // Lower right z = 1
                case 7:
                  min = new Vector3(this.min.X + diff / 2, this.min.Y, this.min.Z + diff / 2);
                  max = new Vector3(this.max.X, this.min.Y + diff / 2, this.max.Z);
                  break;
              }

              this.segments[i] = (OcTreeSegment<T>)CreateInstance(min, max);
              this.segments[i].min = min;
              this.segments[i].max = max;
              this.segments[i].currentDepth = this.currentDepth + 1;
              this.segments[i].MaximumDepth = this.MaximumDepth;
              // Parent segment
              this.segments[i].parent = this;
            }
          }

          if (this.currentDepth < this.MaximumDepth)
          {
            // Now check the new segment for further partitioning
            foreach (OcTreeSegment<T> segment in this.segments)
            {
              if (segment.AddObject(obj) != null)
              {
                segment.Objects.Add(obj);
                return segment;
              }
            }
          }

          this.Objects.Add(obj);
          return this;
          break;
        case ContainmentType.Intersects:
          return this;
          break;
      }

      return null;
    }

    public bool RemoveObject(T obj)
    {
      switch (this.IsInSegment(obj))
      {
        case ContainmentType.Contains:
          this.Objects.Remove(obj);
          return true;
        case ContainmentType.Intersects:
          foreach (OcTreeSegment<T> segment in this.segments)
          {
            if (RemoveObject(obj))
            {
              segment.Objects.Remove(obj);
              return true;
            }
          }
          break;
        default:
          break;
      }

      return false;
    }

    public void SetMaximumDepth(int value)
    {
      this.MaximumDepth = value;
    }

    public void CreateSegment(Vector3 vMin, Vector3 vMax)
    {
      min = vMin;
      max = vMax;
      this.bb = new BoundingBox(min, max);
    }

    public abstract IOcTreeSegment<T> CreateInstance(Vector3 min, Vector3 max);

    /// <summary>
    /// Determine if object is in this QuadTreeSegment.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public abstract ContainmentType IsInSegment(T obj);

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);

    /// <summary>
    /// Transforms the bounding box of this segment with the supplied matrix
    /// </summary>
    /// <param name="transform"></param>
    public void TransformBoundingBox(Matrix transform)
    {
      this.bb.Min = Vector3.Transform(min, transform);
      this.bb.Max = Vector3.Transform(max, transform);
    }

    public IOcTreeSegment<T> GetSegment(T obj)
    {
      switch (this.IsInSegment(obj))
      {
        case ContainmentType.Contains:
          // Whenever object is contained completely, check if it can be placed in one the child segments          
          foreach (IOcTreeSegment<T> segment in this.segments)
          {
            IOcTreeSegment<T> s = segment.GetSegment(obj);

            if (s != null)
            {
              return s;
            }
          }
          return this;
        case ContainmentType.Disjoint:
          return null;
        case ContainmentType.Intersects:
          if (this.parent != null)
            return this.parent;
          else
            return this;
      }

      return null;
    }
  }
}