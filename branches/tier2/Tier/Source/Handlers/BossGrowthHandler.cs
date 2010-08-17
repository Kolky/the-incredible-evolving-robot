using System.Collections.Generic;
using Tier.Source.Objects;
using System.Collections;

namespace Tier.Source.Handlers
{
  /// <summary>
  /// Makes the actual growing of the boss work.
  /// </summary>
  public class BossGrowthHandler
  {
    #region Properties
    private TierGame game;
    private Hashtable behaviourTrees;    
    private List<GameObject> objects;
    private BossPiece core;
    private bool isInitialized;

    public bool IsInitialized
    {
      get { return isInitialized; }
      set { isInitialized = value; }
    }
	
    public BossPiece Core
    {
      get { return core; }
      set { core = value; }
    }
    #endregion

    public BossGrowthHandler(TierGame game)
    {
      this.game = game;
      this.behaviourTrees = new Hashtable();      
      objects = new List<GameObject>();
    }

    public void Initialize()
    {
      this.isInitialized = true; 
    }

    public void Start()
    {
      if (core == null)
      {
        core = new BossPiece(this.game);
        this.game.ObjectHandler.InitializeFromBlueprint<BossPiece>(core, "Core");
      }
      
      this.game.GameHandler.AddObject(core);
    }

    public void Stop()
    {
      if (core != null)
      {
        this.game.GameHandler.RemoveObject(core, GameHandler.ObjectType.DefaultTextured);
        core = null;
      }
    }

    /// <summary>
    /// Grow a number of pieces according to level
    /// </summary>
    public void Grow()
    {
      if (core == null)
        return;

      int count = this.game.GameHandler.GetBlockGrowthCount();

      for (int i = 0; i < count; i++)
      {
        int growcount = 1;
        core.GrowableModifier.Grow(ref growcount, core);
      }
    }
  }
}