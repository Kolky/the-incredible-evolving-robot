using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pjEngine.Helpers
{
  public class OcTree<T, U> where U : IOcTreeSegment<T>
  {
    #region Properties
    protected int maximumdepth;
    protected U segment;

    public U Segment
    {
      get { return segment; }
      set { segment = value; }
    }	

    public int MaximumDepth
    {
      get { return maximumdepth; }
      set { maximumdepth = value; }
    }
    #endregion

    public OcTree(Vector3 min, Vector3 max, int maxDepth, U segment)
    {
      this.maximumdepth = 1;

      this.segment = segment;
      this.segment.CreateSegment(min, max);
      this.segment.SetMaximumDepth(maxDepth);
    }

    /// <summary>
    /// Add object to the OcTree.
    /// </summary>
    /// <param name="obj"></param>
    public IOcTreeSegment<T> AddObject(T obj)
    {
      return segment.AddObject(obj);
    }

    /// <summary>
    /// Remove this object from the OcTree.
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(T obj)
    {
      segment.RemoveObject(obj);
    }

    /// <summary>
    /// Retrieves the OcTree segment this object is contained in.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public IOcTreeSegment<T> GetSegment(T obj)
    {
      return this.segment.GetSegment(obj);
    }
  }
}
