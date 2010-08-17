using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace Tier.Source.Handlers
{
  #region Datastructures
  public enum BossGrowthTemplateLinkType
  {
    BOSSPIECE, WEAPON, BOSSPIECE_TEMPLATE
  };

  /// <summary>
  /// Creates a template for a series of BossPieces.   
  /// </summary>
  public struct BossGrowthTemplate
  {
    public string name;
    // The connector index which will be used in connecting this template to a parent object
    public int pieceIndex;
    // The parent piece which this template will start from
    public BossGrowthTemplatePiece piece;
  };

  public class BossGrowthTemplatePiece
  {
    // The name of the asset to be used representing this piece
    public string name;
    // A list of connectors determining which pieces/weapons are connected to this piece
    public List<BossGrowthTemplateLink> links;

    public BossGrowthTemplatePiece()
    {
      this.links = new List<BossGrowthTemplateLink>();
    }

    public void AddLink(ref BossGrowthTemplateLink link)
    {
      this.links.Add(link);
    }
  };

  public struct BossGrowthTemplateLink
  {
    // Determines if a piece or weapon will be connected
    public BossGrowthTemplateLinkType type;
    // Which connector will be used in parent piece 
    public int parentIndex;
    public int childIndex;
    // The connected object
    public BossGrowthTemplatePiece connectedObject;
    public float rotation;
    public Vector3 translation;
  };
  #endregion

  public class BossPieceTemplateHandler
  {
    #region Properties
    private TierGame game;
    private Hashtable templates;
    #endregion

    public BossPieceTemplateHandler(TierGame game)
    {
      this.game = game;
      this.templates = new Hashtable();
    }

    #region XML parsing

    public void LoadFromXml(string name)
    {
      // Validate the XML file & Load
      using (FileStream fs = File.Open(String.Format("Content//Xml//GrowthPatterns//{0}.xml", name),
          FileMode.Open, FileAccess.Read))
      {
        XmlDocument XmlDocument = new XmlDocument();
        XmlReader reader = XmlReader.Create(fs);
        XmlDocument.Load(reader);
        fs.Close();

        // Handles child XmlNodes
        if (XmlDocument.HasChildNodes)
          this.handleChildNodes(XmlDocument.ChildNodes);
      }
    }

    private void handleChildNodes(XmlNodeList nodelist)
    {
      foreach (XmlNode xmlNode in nodelist)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "boss":
            handleBoss(xmlNode.ChildNodes);
            break;
        }
      }
    }

    private void handleBoss(XmlNodeList nodelist)
    {
      foreach (XmlNode xmlNode in nodelist)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "templates":
            handleTemplates(xmlNode.ChildNodes);
            break;
        }
      }
    }

    private void handleTemplates(XmlNodeList nodelist)
    {
      foreach (XmlNode xmlNode in nodelist)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "template":
            handleTemplate(xmlNode);
            break;
        }
      }
    }

    private void handleTemplate(XmlNode node)
    {
      string value = node.FirstChild.InnerText;
      string name = node.Attributes["name"].Value;
      int pieceIndex = int.Parse(node.Attributes["pieceIndex"].Value);
      BossGrowthTemplate template;

      if(CreateTemplate(value, out template))
      {
        template.name = name;
        template.pieceIndex = pieceIndex;
        this.templates.Add(name, template);
      }
    }

    private bool CreateTemplate(string value, out BossGrowthTemplate template)
    {
      template = new BossGrowthTemplate();
      template.piece = new BossGrowthTemplatePiece();
      return CreateTemplatePiece(value, ref template.piece);
    }

    private bool CreateTemplatePiece(string value, ref BossGrowthTemplatePiece piece)
    {
      string pieceName = "";
      int i = 0;
      for (; i < value.Length; i++)
      {
        if (value[i] == '(')
        {
          pieceName = value.Substring(0, i++);
          break;
        }
      }

      // Save name
      piece.name = pieceName;
      for (int i2 = i; i2 < value.Length; i2++)
      {
        bool doParse = false;
        bool recursive = false;

        if (  value[i2] == ',' || 
              value[i2] == ')')
        {
          doParse = true;
        }
        else if (value[i2] == '(')
        {
          doParse = recursive = true;
        }

        if (doParse)
        {
          // Found a subnode
          string name = value.Substring(i, i2 - i);
          i = i2 + 1;
          BossGrowthTemplateLink link = new BossGrowthTemplateLink();

          if (CreateTemplateLink(name, ref link))
          {            
            // Check if this link has sublinks 
            if (recursive)
            {
              int i3;
              // Backtrack in value string to start of link
              for (i3 = i2; i3 > 0; i3--)
              {
                if (value[i3] == '+')
                {
                  // Found start, break
                  break;
                }
              }

              string subvalue = value.Substring(i3 + 1);

              CreateTemplatePiece(subvalue, ref link.connectedObject);
            }

            // Assume link is created succesfully
            piece.AddLink(ref link);
          }
        }
      }

      return true;
    }

    private bool CreateTemplateLink(string value, ref BossGrowthTemplateLink link)
    {
      link = new BossGrowthTemplateLink();
      string[] data = value.Split('_');

      if (data.Length <= 2)
      {
        return false;
      }

      // First and last element in data array are always present. 
      link.parentIndex = int.Parse(data[0]);
      link.childIndex = int.Parse(data[1]);
      link.connectedObject = new BossGrowthTemplatePiece();
      string name = data[data.Length - 1];
      switch (name[0])
      {
        case '+':
          link.type = BossGrowthTemplateLinkType.BOSSPIECE;
          break;
        case '*':
          link.type = BossGrowthTemplateLinkType.WEAPON;
          break;
      }
      
      link.connectedObject.name = name.Substring(1, name.Length - 1);

      // Between first and last element there can be number of optional modifiers
      for (int i = 1; i < data.Length - 1; i++)
      {
        Regex regRotation = new Regex("rot[0-9][0-9][0-9]");

        if (regRotation.Match(data[i]).Success)
        {
          // TODO: Rotatie van link opslaan.
          int degrees = int.Parse(data[i].Substring(3));

          link.rotation = MathHelper.ToRadians(degrees);
        }
      }
      return true;
    }
    #endregion

    public BossGrowthTemplate GetTemplate(string name)
    {
      return (BossGrowthTemplate)templates[name];
    }

    public BossPiece CreatePieceFromTemplate(ref BossGrowthTemplate template, GameObject parent, int parentIndex)
    {
      return CreateTemplateSubPiece(template.piece, parent, parentIndex, template.pieceIndex);
    }

    private BossPiece CreateTemplateSubPiece(BossGrowthTemplatePiece templatePiece,
      GameObject parent,
      int parentIndex,
      int childIndex)
    {
      BossPiece child = new BossPiece(this.game);

      if (this.game.ObjectHandler.InitializeFromBlueprint<BossPiece>(child, templatePiece.name))
      {
        child.IsInTemplate = true;
        // Connect to parent object
        parent.AttachableModifier.Attach(child, parentIndex, childIndex);
        // Make invisible
        if (child.GetType() == typeof(BossPiece))
        {
          ((BossPiece)child).IsVisible = false;
        }

        // Connect all child's childeren
        foreach (BossGrowthTemplateLink link in templatePiece.links)
        {
          if (child.GetType() == typeof(BossPiece))
          {
            ((BossPiece)child).IsGrown = true;            
          }

          child.AttachableModifier.Connectors[link.parentIndex].Rotation = link.rotation;
          CreateTemplateSubPiece(link.connectedObject, child, link.parentIndex, link.childIndex);
        }
      }

      //this.game.GameHandler.AddObject(child);
      return child;
    }
  }
}