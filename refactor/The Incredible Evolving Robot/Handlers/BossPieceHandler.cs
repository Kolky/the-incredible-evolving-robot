using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Tier.Objects.Attachable;
using Microsoft.Xna.Framework.Graphics;
using Tier.Objects.Destroyable;
using Microsoft.Xna.Framework;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Tier.Handlers
{
  public class BossPieceHandler : GameComponent
  {
    #region Properties
    private Hashtable pieces;

    public Hashtable Pieces
    {
      get { return pieces; }
    }
    #endregion

    public BossPieceHandler(Game game)
      : base(game)
    {
      this.pieces = new Hashtable();
    }

    public void AddPiece(String name, Model model)
    {
      this.pieces.Add(name, this.Load(name, model));
    }

    public Enemy GetEnemy(String name)
    {
      Enemy boss = new Enemy(this.Game);
      BlockPiece blueprintPiece = (BlockPiece)this.pieces[name];

      boss.Name = blueprintPiece.Name;

      foreach (Connector conn in blueprintPiece.Connectors)
      {
        boss.Connectors.Add(new Connector(conn.Position, conn.Pivot, boss));
      }

      for (int i = 0; i < blueprintPiece.BoundingBoxMetas.Count; i++)
        boss.BoundingBoxMetas.Add(blueprintPiece.BoundingBoxMetas[i]);

      for (int i = 0; i < blueprintPiece.BoundingSphereMetas.Count; i++)
        boss.BoundingSphereMetas.Add(blueprintPiece.BoundingSphereMetas[i]);

      boss.Model = blueprintPiece.Model;
      boss.ModelMeta = blueprintPiece.ModelMeta;
      boss.IsCollidable = blueprintPiece.IsCollidable;
      boss.Initialize();

      return boss;
    }

    public BlockPiece GetPiece(String name)
    {
      BlockPiece newPiece = new BlockPiece(this.Game);
      BlockPiece blueprintPiece = (BlockPiece)this.pieces[name];

      newPiece.Name = blueprintPiece.Name;

      foreach (Connector conn in blueprintPiece.Connectors)
      {
        newPiece.Connectors.Add(new Connector(conn.Position, conn.Pivot, newPiece));
      }

      for (int i = 0; i < blueprintPiece.BoundingBoxMetas.Count; i++)
        newPiece.BoundingBoxMetas.Add(blueprintPiece.BoundingBoxMetas[i]);

      for (int i = 0; i < blueprintPiece.BoundingSphereMetas.Count; i++)
        newPiece.BoundingSphereMetas.Add(blueprintPiece.BoundingSphereMetas[i]);

      for (int i = 0; i < blueprintPiece.BoundingBarMetas.Count; i++)
        newPiece.BoundingBarMetas.Add(blueprintPiece.BoundingBarMetas[i]);

      newPiece.Model = blueprintPiece.Model;
      newPiece.ModelMeta = blueprintPiece.ModelMeta;
      newPiece.IsCollidable = blueprintPiece.IsCollidable;
      newPiece.Initialize();

      return newPiece;
    }

    private BlockPiece Load(String name, Model model)
    {
      BlockPiece piece = new BlockPiece(this.Game);

      // Validate the XML file & Load
      using (FileStream fs = File.Open(String.Format("Xml//Pieces//{0}.xml", name),
          FileMode.Open, FileAccess.Read))
      {
        XmlDocument XmlDocument = new XmlDocument();
        XmlReader reader = XmlReader.Create(fs);
        XmlDocument.Load(reader);
        fs.Close();

        // Handles child XmlNodes
        if (XmlDocument.HasChildNodes)
          this.handleChildNodes(XmlDocument.ChildNodes, piece);
      }

      piece.Model = model;

      return piece;
    }

    private BlockPiece handleChildNodes(XmlNodeList xmlNodeList, BlockPiece piece)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "name":
            piece.Name = xmlNode.FirstChild.Value;
            break;
          case "connection":
            Vector3 position = Vector3.Zero;
            Vector3 pivot = Vector3.Zero;

            foreach (XmlNode connectionNode in xmlNode.ChildNodes)
            {
              switch (connectionNode.Name.ToLower())
              {
                case "position":
                  position = this.handleVector3(connectionNode);
                  break;
                case "pivot":
                  pivot = this.handleVector3(connectionNode);
                  break;
              }
            }

            piece.Connectors.Add(new Connector(position, pivot, null));

            break;
          case "collisionvolumes":
            if (xmlNode.HasChildNodes)
              this.handleCollisionVolumes(piece, xmlNode);
            break;
          default:
            if (xmlNode.HasChildNodes)
              this.handleChildNodes(xmlNode.ChildNodes, piece);
            break;
        }
      }

      return piece;
    }

    private void handleCollisionVolumes(BlockPiece piece, XmlNode xmlNode)
    {
      foreach (XmlNode collisionNode in xmlNode.ChildNodes)
      {
        Vector3 offset = Vector3.Zero;

        switch (collisionNode.Name.ToLower())
        {
          case "bar":
            Vector3 boundsLeft = Vector3.Zero;
            Vector3 boundsRight = Vector3.Zero;

            foreach (XmlNode boxNode in collisionNode.ChildNodes)
            {
              switch (boxNode.Name.ToLower())
              {
                case "boundsleft":
                  boundsLeft = this.handleVector3(boxNode);
                  break;
                case "boundsright":
                  boundsRight = this.handleVector3(boxNode);
                  break;
                case "offset":
                  offset = this.handleVector3(boxNode);
                  break;
              }
            }

            piece.addBoundingBar(boundsLeft, boundsRight, offset);
            piece.ModelMeta = new ModelMeta(piece.Model);
            piece.IsCollidable = true;
            break;
          case "box":            
            Vector3 bounds = Vector3.Zero;            

            foreach (XmlNode boxNode in collisionNode.ChildNodes)
            {
              switch (boxNode.Name.ToLower())
              {                
                case "bounds":
                  bounds = this.handleVector3(boxNode);
                  break;
                case "offset":
                  offset = this.handleVector3(boxNode);
                  break;
              }
            }

            piece.addBoundingBox(bounds, offset);
            piece.ModelMeta = new ModelMeta(piece.Model);
            piece.IsCollidable = true;
            break;
          case "sphere":
            float radius = 0f;

            foreach (XmlNode boxNode in collisionNode.ChildNodes)
            {
              switch (boxNode.Name.ToLower())
              {
                case "radius":
                  radius = float.Parse(boxNode.FirstChild.Value, CultureInfo.CreateSpecificCulture("en-us"));
                  break;
                case "offset":
                  offset = this.handleVector3(boxNode);
                  break;
              }
            }

            piece.addBoundingShere(radius, offset);
            piece.ModelMeta = new ModelMeta(piece.Model);
            piece.IsCollidable = true;
            break;
        }
      }
    }

    private Vector3 handleVector3(XmlNode xmlVector3)
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