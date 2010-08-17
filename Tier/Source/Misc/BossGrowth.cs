using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Misc
{
  public struct GrowthPatternWeapon
  {
    public string Name;
    public int SourceConnectorIndex;
  };

  public struct GrowthPatternBlock
  {
    public bool IsStartOfSequence;
    public bool IsPartOfSequence;
    public string Name;
    public int SourceConnectorIndex;
    public int BlockConnectorIndex;

    public List<GrowthPatternBlock> BlockSequence;
  };

  public struct GrowthPatternConnector
  {
    public int index;
    public List<GrowthPatternBlock> blocks;
  };

  public struct GrowthPatternWeaponConnector
  {
    public int index;
    public List<GrowthPatternWeapon> weapons;
  };

  public struct GrowthPattern
  {
    public string Name;    
    public bool IsCore;
    public List<GrowthPatternConnector> connectors;
    public List<GrowthPatternWeaponConnector> weaponconnectors;
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

    public GrowthPattern GetPattern(string name)
    {
      if(this.growthpatterns[name] != null)
      {
        return (GrowthPattern)this.growthpatterns[name];
      }
      else
      {        
        throw new ApplicationException(String.Format("{0} does not have a GrowthPattern specified in XML file.", name));
      }      
    }
  }
}
