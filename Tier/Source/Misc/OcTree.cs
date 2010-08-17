using System;
using System.Collections.Generic;
using System.Text;
using pjEngine.Helpers;
using Tier.Source.Objects;
using Microsoft.Xna.Framework;

namespace Tier.Source.Misc
{
  public class GameObjectSegment : OcTreeSegment<GameObject>
  {
    public GameObjectSegment()
      : base(Vector3.Zero, Vector3.Zero)
    { }

    public override IOcTreeSegment<GameObject> CreateInstance(Vector3 min, Vector3 max)
    {
      GameObjectSegment segment = new GameObjectSegment();
      segment.BoundingBox = new BoundingBox(min, max);

      return segment;
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
    }

    /// <summary>
    /// Check whetever GameObject can be contained in this segment.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override Microsoft.Xna.Framework.ContainmentType IsInSegment(GameObject obj)
    {
      return obj.CollisionModifier.CheckCollision(this.BoundingBox);
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {      
    }
  }
  
  public class OcTree : pjEngine.Helpers.OcTree<GameObject, GameObjectSegment>
  {
    public OcTree(Vector3 min, Vector3 max, int maxDepth, GameObjectSegment baseSegment)
      : base(min, max, maxDepth, baseSegment)
    {         
    }
  }
}
