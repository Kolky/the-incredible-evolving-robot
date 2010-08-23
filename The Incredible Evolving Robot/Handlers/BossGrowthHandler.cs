using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tier.Misc;
using System.IO;

namespace Tier.Handlers
{
  public class BossGrowthHandler
  {
    public BossGrowth Load(string name)
    {
      BossGrowth pattern = new BossGrowth();

      // Validate the XML file & Load
      using ( FileStream fs = File.Open(String.Format("Content//Xml//BossGrowth//{0}.xml", name),
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

            pattern.GrowthPatterns.Add(name, handleGrowthPattern(xmlNode.ChildNodes));
            break;
        }
      }
    }

    private GrowthPattern handleGrowthPattern(XmlNodeList xmlNodeList)
    {
      GrowthPattern pattern = new GrowthPattern();
      pattern.Blocks = new List<GrowthPatternBlock>();

      foreach (XmlNode xmlNode in xmlNodeList)
	    {
        switch (xmlNode.Name.ToLower())
        {
          case "block":
            pattern.Blocks.Add(handleGrowthPatternBlock(xmlNode));            
            break;
        }
	    }

      return pattern;
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
          case "required":
            block.Required = bool.Parse(att.Value);
            break;
          case "sourceconnector":
            block.SourceConnector = int.Parse(att.Value);
            break;
          case "blockconnector":
            block.BlockConnector = int.Parse(att.Value);
            break;
        }
      }

      return block;
    }
  }
}
