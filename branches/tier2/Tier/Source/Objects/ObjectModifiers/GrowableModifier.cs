using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using System.Collections;
using System.Text.RegularExpressions;
using Tier.Source.Helpers;
using Tier.Source.Objects.Turrets;
using Tier.Source.GameStates;

namespace Tier.Source.ObjectModifiers
{
  public class GrowableModifierRule
  {
    public string ruletext;
    public bool isDone;

    public GrowableModifierRule()
    {
      ruletext = "";
      isDone = false;
    }
  };

  public class GrowableModifier : ObjectModifier
  {
    #region Properties
    private Hashtable rules;
    private bool isGrown;
    private string currentBossTemplate;
    private int connectorToBeGrown;
    private List<int> growOrder; 

    public string CurrentBossTemplate
    {
      get { return currentBossTemplate; }
      set { currentBossTemplate = value; }
    }
	
    public bool IsGrown
    {
      get { return isGrown; }
      set { isGrown = value; }
    }
    #endregion

    public GrowableModifier(GameObject parent)
      : base(parent)
    {
      this.growOrder = new List<int>();
      this.rules = new Hashtable();
      connectorToBeGrown = 0;
      currentBossTemplate = "testboss";
    }

    public void AddRule(string id, GrowableModifierRule rule)
    {
      if (!this.rules.ContainsKey(id))
      {
        this.rules.Add(id, new List<GrowableModifierRule>());
      }

      List<GrowableModifierRule> list = (List<GrowableModifierRule>)this.rules[id];
      list.Add(rule);
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = new GrowableModifier(parent);
      parent.GrowableModifier = (GrowableModifier)objmod;

      foreach (string bossTemplate in this.rules.Keys)
      {
        foreach (GrowableModifierRule rule in (List<GrowableModifierRule>)this.rules[bossTemplate])
        {
          GrowableModifierRule newrule = new GrowableModifierRule();
          newrule.isDone = false;
          newrule.ruletext = rule.ruletext;

          ((GrowableModifier)objmod).AddRule(bossTemplate, newrule);
        }        
      }

      ((GrowableModifier)objmod).Initialize();
    }

    private string GetRandomObjectName<T>() where T : GameObject
    {
      List<string> names = new List<string>();      

      IDictionaryEnumerator dict = this.Parent.Game.ObjectHandler.Objects.GetEnumerator();
      while (dict.MoveNext())
      {
        if (dict.Value.GetType() == typeof(T))
        {
          names.Add((string)dict.Key);
        }
      }
            
      Random r = new Random(
        DateTime.Now.Millisecond * names.Count);
      string randomName = names[r.Next(names.Count)];

      return randomName;
    }

    /// <summary>
    /// Grow this and a number of child objects attached to this object
    /// </summary>
    /// <param name="childGrownCount">The number of objects which will be grown</param>
    public void Grow(ref int childGrownCount, GameObject parent)
    {
      //foreach (GrowableModifierRule rule in (List<GrowableModifierRule>)this.rules[currentBossTemplate])
      foreach(int i in this.growOrder)
      {
        GrowableModifierRule rule = ((List<GrowableModifierRule>)this.rules[currentBossTemplate])[i];
        if (rule.isDone)
          continue;

        if (rule.ruletext.Contains("+"))
        {
          // Grow block  
          if (GrowBlock(rule))
          {
            childGrownCount--;
          }
        }
        else if (rule.ruletext.Contains("-"))
        {
          // Grow weapon
          if (GrowWeapon(rule))
          {
            childGrownCount--;
          }
        }
        else if (rule.ruletext.Contains("~"))
        {
          // Grow blocktemplate
        }

        // Check for optional arguments
        // Rotation:
        // Regex: rot[0-9]

        if(childGrownCount <= 0)
          break;
      }

      if (childGrownCount > 0 && this.Parent.AttachableModifier != null)
      {
        GrowChildren(ref childGrownCount, parent);        
      }
    }

    /// <summary>
    /// Grow block according to information saved in the growth rules
    /// </summary>
    public void Grow()
    {
      if (this.isGrown)
        return;

      foreach (int i in this.growOrder)
      {
        GrowableModifierRule rule = ((List<GrowableModifierRule>)this.rules[currentBossTemplate])[i];

        if (rule.ruletext.Contains("+"))
        {
          // Grow block  
          if (GrowBlock(rule))
          {
            //childGrownCount--;
          }
        }
        else if (rule.ruletext.Contains("-"))
        {
          // Grow weapon
          if (GrowWeapon(rule))
          {
            //childGrownCount--;
          }
        }
        else if (rule.ruletext.Contains("~"))
        {
          // Grow blocktemplate
        }

        rule.isDone = true;
      }

      this.isGrown = true;
    }

    private void GrowChildren(ref int childGrownCount, GameObject parent)
    {
      for (int i = 0;i < this.Parent.AttachableModifier.Connectors.Count; i++)
      {
        if (this.connectorToBeGrown >= this.Parent.AttachableModifier.Connectors.Count)
        {
          this.connectorToBeGrown = 0;
        }

        Connector conn = this.Parent.AttachableModifier.Connectors[this.connectorToBeGrown];
        this.connectorToBeGrown++;

        if (conn.ConnectedTo != null &&
          conn.ConnectedTo != parent &&
          conn.ConnectedTo.GrowableModifier != null)
        {
          conn.ConnectedTo.GrowableModifier.Grow(ref childGrownCount, this.Parent);

          if (childGrownCount <= 0)
            break;
        }
      }
    }

    private bool GrowObject<T>(T obj, GrowableModifierRule rule, string[] data) where T : GameObject
    {
      bool result = false;

      // Eerst deel beschrijft verbinding voor Connector
      string[] connection = data[0].Split('_');
      int parentConnector = int.Parse(connection[0]);
      int childConnector = int.Parse(connection[1]);

      // Tweede deel is naam blok
      string name = data[1];
      if (name == "Random")
      {
        name = GetRandomObjectName<T>();
      }

      if (this.Parent.Game.ObjectHandler.InitializeFromBlueprint<T>(obj, name))
      {
        rule.isDone = true;

        if (this.Parent.AttachableModifier.Attach(obj,
          parentConnector,
          childConnector))
        {
          //this.Parent.Game.GameHandler.AddObjectInstantly(obj);
          this.Parent.Game.GameHandler.AddObject(obj);
          result = true;
        }
        else
        {
          obj.Dispose();
        }
      }

      return result;
    }

    private bool GrowBlock(GrowableModifierRule rule)
    {
      // Split string op +
      bool result = false;
      string[] data = rule.ruletext.Split('+');

      if (data.Length > 1)
      {
        BossPiece p = new BossPiece(this.Parent.Game);
        result = GrowObject<BossPiece>(p, rule, data);
      }

      return result;
    }

    private bool GrowWeapon(GrowableModifierRule rule)
    {
      // Split string op +
      bool result = false;
      string[] data = rule.ruletext.Split('-');

      if (data.Length > 1)
      {
        Turret t = null;
        TierGame game = this.Parent.Game;

        if (game.GameState.GetType() != typeof(MainGameState))
          return false;

        switch (game.Random.Next(3))
        {
          case 0:
            t = new ShotgunTurret(game);
            game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "ShotgunTurret");
            break;
          case 1:
            t = new PlasmaTurret(game);
            game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "PlasmaTurret");
            break;
          case 2:
            t = new LaserBeamTurret(game);
            game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "LaserBeamTurret");
            break;
          /*case 2:
            t = new LaserTurret(game);
            game.ObjectHandler.InitializeFromBlueprint<Turret>(t, "LaserTurret");
            break;*/
        }
        
        if(t != null)
          result = GrowObject<Turret>(t, rule, data);
      }

      return result;
    }

    private void GrowBlockTemplate(string rule)
    { }

    public void Reset()
    {
      foreach (int i in this.growOrder)
      {
        ((List<GrowableModifierRule>)this.rules[currentBossTemplate])[i].isDone = false;        
      }
    }

    public void Initialize()
    {
      // Create random order in which the rules will be handled
      int count = ((List<GrowableModifierRule>)this.rules[currentBossTemplate]).Count;
      Random r = new Random(
        DateTime.Now.Second +
        DateTime.Now.Millisecond * count);

      while (this.growOrder.Count < count)
      {
        int number = r.Next(count);

        if (!this.growOrder.Contains(number))
        {
          this.growOrder.Add(number);
        }
      }
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      
    }
  }
}