using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tier.Source.Misc;
using System.IO;

namespace Tier.Source.Handlers
{
  public class BossGrowthPatternHandler
  {
    public BossGrowth Load(string name)
    {
      BossGrowth pattern = new BossGrowth();

      // Validate the XML file & Load
      using ( FileStream fs = File.Open(String.Format("Content//Xml//GrowthPatterns//{0}.xml", name),
          FileMode.Open, FileAccess.Read))
      {
        XmlDocument XmlDocument = new XmlDocument();
        XmlReader reader = XmlReader.Create(fs);
        XmlDocument.Load(reader);
        fs.Close();

        // Handles child XmlNodes
        if (XmlDocument.HasChildNodes)
          this.handleChildNodes(XmlDocument.ChildNodes, pattern);
      }

      return pattern;
    }

    private void handleChildNodes(XmlNodeList xmlNodeList, BossGrowth pattern)
    {
        foreach (XmlNode xmlNode in xmlNodeList)
        {
          switch (xmlNode.Name.ToLower())
          {
            case "boss":
              handleBoss(xmlNode.ChildNodes, pattern);
              break;
          }
        }
    }

    private void handleBoss(XmlNodeList xmlNodeList, BossGrowth pattern)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "name":
            break;
          case "model":
            break;
          case "growthpattern":
            string name = xmlNode.Attributes["base"].Value;
            
            GrowthPattern p = handleGrowthPattern(xmlNode.ChildNodes);
            p.Name = name;            

            if (xmlNode.Attributes["iscore"] != null)
            {
              p.IsCore = bool.Parse(xmlNode.Attributes["iscore"].Value);
            }

            pattern.GrowthPatterns.Add(name, p);
            break;
        }
      }
    }

    private GrowthPattern handleGrowthPattern(XmlNodeList xmlNodeList)
    {
      GrowthPattern pattern = new GrowthPattern();
      pattern.connectors = new List<GrowthPatternConnector>();
      pattern.weaponconnectors = new List<GrowthPatternWeaponConnector>();

      foreach (XmlNode xmlNode in xmlNodeList)
	    {
        switch (xmlNode.Name.ToLower())
        {
          case "connector":
            GrowthPatternConnector connector = new GrowthPatternConnector();
            connector.blocks = new List<GrowthPatternBlock>();
            connector.index = int.Parse(xmlNode.Attributes["index"].Value);
                        
            handleConnector(xmlNode.ChildNodes, ref connector, pattern);
            pattern.connectors.Add(connector);
            break;
          case "weaponconnector":
            GrowthPatternWeaponConnector weaponconnector = new GrowthPatternWeaponConnector();
            weaponconnector.weapons = new List<GrowthPatternWeapon>();
            weaponconnector.index = int.Parse(xmlNode.Attributes["index"].Value);
            
            handleWeaponConnector(xmlNode.ChildNodes, ref weaponconnector, pattern);            
            pattern.weaponconnectors.Add(weaponconnector);
            break;
        }
	    }

      return pattern;
    }

    private void handleConnector(XmlNodeList xmlNodeList, ref GrowthPatternConnector connector, GrowthPattern pattern)
    {
      GrowthPatternBlock block;

      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "block":
            block = handleGrowthPatternBlock(xmlNode);
            block.SourceConnectorIndex = connector.index;
            connector.blocks.Add(block);
            break;
          case "blocksequence":
            block = handleGrowthPatternBlockSequence(xmlNode);
            connector.blocks.Add(block);
            break;
        }
      }
    }

    private void handleWeaponConnector(XmlNodeList xmlNodeList, ref GrowthPatternWeaponConnector connector, GrowthPattern pattern)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "weapon":
            GrowthPatternWeapon weapon = handleGrowthPatternWeapon(xmlNode);
            weapon.SourceConnectorIndex = connector.index;
            connector.weapons.Add(weapon);
            break;
        }
      }
    }

    private GrowthPatternWeapon handleGrowthPatternWeapon(XmlNode xmlNode)
    {
      GrowthPatternWeapon weapon = new GrowthPatternWeapon();

      foreach (XmlAttribute att in xmlNode.Attributes)
      {
        switch (att.Name)
        {
          case "name":
            weapon.Name = att.Value;
            break;
        }
      }

      return weapon;
    }

    private GrowthPatternBlock handleGrowthPatternBlockSequence(XmlNode xmlNode)
    {
      GrowthPatternBlock block = new GrowthPatternBlock();
      block.IsStartOfSequence = true;
      block.Name = "";
      block.BlockSequence = new List<GrowthPatternBlock>();

      foreach (XmlNode node in xmlNode.ChildNodes)
      {
        switch (node.Name.ToLower())
        {
          case "block":
            GrowthPatternBlock b = handleGrowthPatternBlock(node);
            b.IsPartOfSequence = true;
            block.BlockSequence.Add(b);
            
            if (block.Name.Equals(""))
            {
              block.Name = b.Name;
            }
            break;
        }
      }

      return block;
    }

    private GrowthPatternBlock handleGrowthPatternBlock(XmlNode xmlNode)
    {
      GrowthPatternBlock block = new GrowthPatternBlock();

      foreach (XmlAttribute att in xmlNode.Attributes)
      {
        switch (att.Name)
        {
          case "name":
            block.Name = att.Value;
            break;
          case "parentconnector":
            block.SourceConnectorIndex = int.Parse(att.Value);
            break;
          case "blockconnector":
            block.BlockConnectorIndex = int.Parse(att.Value);
            break;
        }
      }

      return block;
    }    
  }
}
