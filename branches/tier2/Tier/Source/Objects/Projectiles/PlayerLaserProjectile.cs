using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Misc;
using System.Collections;
using Tier.Source.Helpers;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Handlers;
using Tier.Source.Handlers.BossBehaviours;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Tier.Source.Objects
{
  /// <summary>
  /// Determines the distance of a GameObjectSegment.
  /// </summary>
  struct CollisionDistance : IComparable
  {
    public float distance;
    public GameObjectSegment segment;

    public CollisionDistance(float distance, GameObjectSegment segment)
    {
      this.distance = distance; this.segment = segment;
    }

    #region IComparable Members

    public int CompareTo(object obj)
    {
      CollisionDistance cd = (CollisionDistance)obj;

      if (this.distance < cd.distance)
      {
        return -1;
      }
      else if (this.distance > cd.distance)
      {
        return 1;
      }

      return 0;
    }

    #endregion
  }

  class PlayerLaserProjectile : Projectile
  {
    #region Properties
    private Vector3 direction;
    private float speed;
    private float damage;

    public float Damage
    {
      get { return damage; }
      set { damage = value; }
    }
	
    public float Speed
    {
      get { return speed; }
      set { speed = value; }
    }
	
    public Vector3 Direction
    {
      get { return direction; }
      set { direction = value; }
    }
    #endregion

    public PlayerLaserProjectile(TierGame game)
      : base(game)
    {      
      this.TextureName = "PlayerPlasmaProjectile";      
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      ((PlayerLaserProjectile)obj).speed = this.speed;
      ((PlayerLaserProjectile)obj).damage = this.damage;
      
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      base.ReadSpecificsFromXml(xmlNodeList);
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "speed":
            this.speed = float.Parse(node.InnerXml);
            break;
          case "damage":
            this.damage = int.Parse(node.InnerXml);
            break;
        }
      }
    }    
  }
}
