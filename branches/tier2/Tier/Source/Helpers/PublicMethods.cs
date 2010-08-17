using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Globalization;
using Tier.Source.Objects;
using Tier.Source.Misc;

namespace Tier.Source.Helpers
{
  class PublicMethods
  {
    public static float GetShortestAngleBetweenVectors(ref Vector3 v1, ref Vector3 v2)
    {
      double angle;
      float dot1, dot2;
      Vector3 invVector = -v1;

      Vector3.Dot(ref invVector, ref v2, out dot1);
      Vector3.Dot(ref v1, ref v2, out dot2);

      if (dot1 > dot2)
      {
        angle = Math.Acos(dot1);
      }
      else
      {
        angle = Math.Acos(dot2);
      }

      return (float)angle;
    }

    public static Quaternion GetShortestRotationQuaternionBetweenVectors(ref Vector3 v1, ref Vector3 v2)
    {
      float angle = GetShortestAngleBetweenVectors(ref v1, ref v2);
      Vector3 cross;// = Vector3.Zero;

      Vector3.Cross(ref v1, ref v2, out cross);

      return Quaternion.CreateFromAxisAngle(cross, angle);
    }

    public static Matrix GetShortestRotationMatrixBetweenVectors(ref Vector3 v1, ref Vector3 v2)
    {
      float angle = GetShortestAngleBetweenVectors(ref v1, ref v2);
      Vector3 cross;// = Vector3.Zero;

      Vector3.Cross(ref v1, ref v2, out cross);

      return Matrix.CreateFromAxisAngle(cross, angle);
    }

    /// <summary>
    /// Checks wether this object is displayed in the frustum, uses a defeault frustrum
    /// new BoundingFrustum( serv.GameState.View * serv.GameState.Projection)
    /// </summary>
    /// <param name="isFullObject">Wether the full object is taken in account, or partial object is also valid as in frustrum</param>
    /// <returns></returns>
    public static bool isInFrustum(BoundingBox box, bool isFullObjectInFrustrum, Matrix view, Matrix projection)
    {
      return isInFrustum(box, new BoundingFrustum(view * projection), isFullObjectInFrustrum);
    }

    /// <summary>
    /// Checks wether this object is displayed in the frustum
    /// </summary>
    /// <param name="bFrustum">new BoundingFrustum( serv.GameState.View * serv.GameState.Projection)</param>
    /// <param name="isFullObject">Wether the full object is taken in account, or partial object is also valid as in frustrum</param>
    /// <returns></returns>
    public static bool isInFrustum(BoundingBox box, BoundingFrustum bFrustum, bool isFullObjectInFrustrum)
    {
      switch (bFrustum.Contains(box))
      {
        case ContainmentType.Disjoint:	// The sphere is not in the frustum
          return false;

        case ContainmentType.Intersects:	// The sphere intersects with the frustum
          return !isFullObjectInFrustrum;

        case ContainmentType.Contains:	// The sphere is in the frustum
          return true;

        default:
          return true;
      }
    }

    /// <summary>
    /// Checks wether this object is displayed in the frustum
    /// </summary>
    /// <param name="bFrustum">new BoundingFrustum( serv.GameState.View * serv.GameState.Projection)</param>
    /// <param name="isFullObject">Wether the full object is taken in account, or partial object is also valid as in frustrum</param>
    /// <returns></returns>
    public static bool isInFrustum(Vector3 position, BoundingFrustum bFrustum, bool isFullObjectInFrustrum)
    {
      switch (bFrustum.Contains(position))
      {
        case ContainmentType.Disjoint:	// The sphere is not in the frustum
          return false;

        case ContainmentType.Intersects:	// The sphere intersects with the frustum
          return !isFullObjectInFrustrum;

        case ContainmentType.Contains:	// The sphere is in the frustum
          return true;

        default:
          return true;
      }
    }

    /// <summary>
    /// Retrieves yaw, pitch and roll from a rotation matrix
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="rotation"></param>
    public static void YawPitchRollFromMatrix(ref Matrix mat, out Vector3 rotation)
    {
      rotation = Vector3.Zero;
      double x, y = 0, z = 0;

      x = Math.Atan2(mat.M22, mat.M32);
      y = Math.Acos(mat.M33);
      z = -Math.Atan2(mat.M13, mat.M23);

      rotation = new Vector3((float)x, (float)y, (float)z);
    }

    static public void DecomposeRollPitchYawZXYMatrix(Matrix mx, out Vector3 rotation)
    {
      rotation = Vector3.Zero;
      rotation.X = (float)Math.Asin(-mx.M32);

      double threshold = 0.001; // Hardcoded constant - burn him, he's a witch      

      if (Math.Cos(rotation.X) > threshold)
      {
        rotation.Z = (float)Math.Atan2(mx.M12, mx.M22);
        rotation.Y = (float)Math.Atan2(mx.M31, mx.M33);
      }

      else
      {

        rotation.Z = (float)Math.Atan2(-mx.M21, mx.M11);
        rotation.Y = 0.0f;

      }

    }

    /// <summary>
    /// Determine if the supplied Ray collides with one of the objects in the scene.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsRayIntersection(TierGame game, ref Ray ray, out GameObject obj)
    {
      obj = null;
      float closestdist = float.MaxValue;
      GameObject closestObj = null;

      if (game.GameHandler.Octree.Segment.Segments != null)
      {        
        List<CollisionDistance> collisiondistances = new List<CollisionDistance>();
        
        foreach (GameObjectSegment segment in game.GameHandler.Octree.Segment.Segments)
        {
          float? dist = ray.Intersects(segment.BoundingBox);

          // If a segment has been intersected, add it to the list of to be checked segments
          if (dist != null)
            collisiondistances.Add(new CollisionDistance((float)dist, segment));
        }
        
        collisiondistances.Sort();

        // Now loop through the segments, which are sorted on distance.
        // When a segment is checked and object(s) are found, the one with the closest distance is hit.            
        foreach (CollisionDistance cd in collisiondistances)
        {          
          closestdist = float.MaxValue;
          closestObj = GetClosestObject(ray, cd.segment, ref closestdist);

          // Closest object found
          if (closestObj != null)
          {
            obj = closestObj;
            // Done            
            return true;
          }
        }
      }

      // No object found in sub segments, try main segment      
      closestObj = GetClosestObject(ray, game.GameHandler.Octree.Segment, ref closestdist);
      if (closestObj != null)
      {
        obj = closestObj;
        return true;
      }

      return false;
    }

    public static bool IsRayIntersection(TierGame game, ref Ray ray, out GameObject obj, out float distance)
    {
      obj = null;
      distance = float.MaxValue;
      float closestdist = float.MaxValue;
      GameObject closestObj = null;

      if (game.GameHandler.Octree.Segment.Segments != null)
      {
        List<CollisionDistance> collisiondistances = new List<CollisionDistance>();

        foreach (GameObjectSegment segment in game.GameHandler.Octree.Segment.Segments)
        {
          float? dist = ray.Intersects(segment.BoundingBox);

          // If a segment has been intersected, add it to the list of to be checked segments
          if (dist != null)
            collisiondistances.Add(new CollisionDistance((float)dist, segment));
        }

        collisiondistances.Sort();

        // Now loop through the segments, which are sorted on distance.
        // When a segment is checked and object(s) are found, the one with the closest distance is hit.            
        foreach (CollisionDistance cd in collisiondistances)
        {
          closestdist = float.MaxValue;
          closestObj = GetClosestObject(ray, cd.segment, ref closestdist);

          // Closest object found
          if (closestObj != null)
          {
            obj = closestObj;
            distance = closestdist;
            // Done            
            return true;
          }
        }
      }

      // No object found in sub segments, try main segment      
      closestObj = GetClosestObject(ray, game.GameHandler.Octree.Segment, ref closestdist);
      if (closestObj != null)
      {
        obj = closestObj;
        distance = closestdist;
        return true;
      }

      return false;
    }

    private static GameObject GetClosestObject(Ray r, GameObjectSegment segment, ref float closestDist)
    {
      GameObject closestObj = null;

      foreach (GameObject segmentObject in segment.Objects)
      {
        float? dist;
        segmentObject.CollisionModifier.CheckCollision(r, out dist);

        if (dist != null && dist < closestDist)
        {
          closestObj = segmentObject;
          closestDist = (float)dist;
        }
      }

      return closestObj;
    }

    public static Vector3 RoundVector3(Vector3 v)
    {
      if (v.X > 0 && v.X <= 0.0001f)
      {
        v.X = 0;
      }
      if (v.X < 0 && v.X >= -0.0001f)
      {
        v.X = 0;
      }

      if (v.Y > 0 && v.Y <= 0.0001f)
      {
        v.Y = 0;
      }
      if (v.Y < 0 && v.Y >= -0.0001f)
      {
        v.Y = 0;
      }

      if (v.Z > 0 && v.Z <= 0.0001f)
      {
        v.Z = 0;
      }
      if (v.Z < 0 && v.Z >= -0.0001f)
      {
        v.Z = 0;
      }

      return v;
    }

    public static Vector3 handleVector3(XmlNode xmlVector3)
    {
      Vector3 vector = Vector3.Zero;

      foreach (XmlAttribute xmlAtt in xmlVector3.Attributes)
      {
        switch (xmlAtt.Name.ToLower())
        {
          case "x":
            vector.X = float.Parse(xmlAtt.FirstChild.Value, CultureInfo.CreateSpecificCulture("en-us"));
            break;
          case "y":
            vector.Y = float.Parse(xmlAtt.FirstChild.Value, CultureInfo.CreateSpecificCulture("en-us"));
            break;
          case "z":
            vector.Z = float.Parse(xmlAtt.FirstChild.Value, CultureInfo.CreateSpecificCulture("en-us"));
            break;
        }
      }

      return vector;
    }
  }
}
