using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Globalization;
using Tier.Source.Objects;
using Tier.Source.Misc;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Objects.Turrets;
using Tier.Source.Objects.Powerups;

namespace Tier.Source.Handlers
{
  public class ObjectHandler : GameComponent
  {
    #region Properties
    private TierGame game;
    private Hashtable objects;

    public Hashtable Objects
    {
      get { return objects; }
      set { objects = value; }
    }
	
    #endregion

    public ObjectHandler(TierGame game)
      : base(game)
    {
      this.game = game;
      this.objects = new Hashtable();      
    }

    public void ClearObjects()
    {
      this.objects.Clear();
    }

    public void LoadObjects(string filepath)
    {
      if (Directory.Exists(filepath))
      {
        foreach (string file in Directory.GetFiles(filepath))
        {          
          using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read))
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
      }
    }

    public bool Exists(string name)
    {
      return (this.objects[name] != null);
    }

    private T GetObject<T>(string name)
    {
      return (T)this.objects[name];
    }

    public bool InitializeFromBlueprint<T>(T obj, string name) where T:GameObject
    {     
      if (this.Exists(name))
      {        
        T blueprint = this.game.ObjectHandler.GetObject<T>(name);        
        blueprint.Clone(obj);
        obj.Initialize();
        return true;
      }

      return false;
    }

    private void handleChildNodes(XmlNodeList xmlNodeList)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "object":
            GameObject obj = null;
            string name = xmlNode.Attributes["name"].Value;
            string type = "";

            if (xmlNode.Attributes["type"] != null)
            {
              type = xmlNode.Attributes["type"].Value;
            }

            switch (type)
            { 
              case "bosspiece":
                obj = new BossPiece(this.game);
                break;
              case "plasmaturret":
                obj = new PlasmaTurret(this.game);
                break;
              case "plasmaspreadturret":
                obj = new PlasmaSpreadTurret(this.game);
                break;
              case "shotgunturret":
                obj = new ShotgunTurret(this.game);
                break;
              case "laserbeamturret":
                obj = new LaserBeamTurret(this.game);
                break;
              case "laserturret":
                obj = new LaserTurret(this.game);
                break;
              case "player":
                obj = new Player(this.game);
                break;
              case "projectile":
                obj = new Projectile(this.game);
                break;
              case "poweruphealth":
                obj = new PowerupHealth(this.game);
                break;
              case "poweruplaser":
                obj = new PowerupLaser(this.game);
                break;
              case "laserbeamprojectile":
                obj = new LaserbeamProjectile(this.game);
                break;
              case "rocketprojectile":
                obj = new RocketProjectile(this.game);
                break;
              case "laserprojectile":
                obj = new LaserProjectile(this.game);
                break;
              case "playerlaserprojectile":
                obj = new PlayerLaserProjectile(this.game);
                break;
              case "billboard":
                obj = new Billboard(this.game);
                break;
              case "decoration":
                obj = new DecorationObject(this.game);
                break;
              case "skybox":
                obj = new SkyBox(this.game);
                break;
              case "energyshield":
                obj = new EnergyShield(this.game);
                break;
              default:
                throw new Exception(string.Format("Unknown object type {0}", type));
            }

            if (obj != null)
            {
              // Read object information from XML
              obj.ReadFromXml(xmlNode.ChildNodes);
              // Insert the object in the ObjectList
              this.objects.Add(name, obj);
            }
            break;
        }
      }
    }
  }
}
