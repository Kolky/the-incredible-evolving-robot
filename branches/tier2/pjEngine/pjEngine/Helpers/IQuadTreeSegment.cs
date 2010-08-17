using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pjEngine.Helpers
{
  public interface IOcTreeSegment<T>
  {
    /// <summary>
    /// Determine if object is in this OcTreeSegment.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    ContainmentType IsInSegment(T obj);

    /// <summary>
    /// Add object to this OcTreeSegment.
    /// </summary>
    /// <param name="obj"></param>
    IOcTreeSegment<T> AddObject(T obj);

    /// <summary>
    /// Remove object from this OcTreeSegment.
    /// </summary>
    /// <param name="obj"></param>
    bool RemoveObject(T obj);

    /// <summary>
    /// Creates the BoundingBox of this segment using the two Vector3 arguments.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    void CreateSegment(Vector3 vMin, Vector3 vMax);

    IOcTreeSegment<T> GetSegment(T obj);
    
    void SetMaximumDepth(int value);

    IOcTreeSegment<T> CreateInstance(Vector3 min, Vector3 max);

    void TransformBoundingBox(Matrix transform);
  }
}
