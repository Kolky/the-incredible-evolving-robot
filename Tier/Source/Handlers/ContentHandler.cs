#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
#endregion

namespace pjEngine.Content
{
  public enum RecordType
  {
    Model, Effect, Texture
  };

  public struct DataCollectionRecord
  {
    public string Name;
    public RecordType Type;
  }

  public class DataCollection
  {     
    #region properties
    private List<DataCollectionRecord> records;
    private string name;
    private string rootFolder;
    private ContentManager content;

    public ContentManager ContentManager
    {
      get { return content; }
      set { content = value; }
    }
	
    public string RootFolder
    {
      get { return rootFolder; }
      set { rootFolder = value; }
    }
	
    public string Name
    {
      get { return name; }
      set { name = value; }
    }
	
    public List<DataCollectionRecord> Records
    {
      get { return records;}		  
    }
    #endregion

    public DataCollection(Game game)
    {
      this.records = new List<DataCollectionRecord>();
      this.content = new ContentManager(game.Services);
    }
  }

  public class ContentHandler
  {
    #region Properties
    private Hashtable effects;
    private Hashtable models;
    private Hashtable fonts;
    private Game game;
    private Hashtable textures;
    private List<DataCollection> dataCollections;
    private ContentManager content;    

    public ContentManager ContentManager
    {
      get { return content; }
      set { content = value; }
    }
	
    #endregion

    public ContentHandler(Game game)
    {
      this.game = game;
      this.effects = new Hashtable();
      this.models = new Hashtable();
      this.textures = new Hashtable();
      this.fonts = new Hashtable();
      this.dataCollections = new List<DataCollection>();      
    }

    /// <summary>
    /// Load the root tag from the xml file.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="list"></param>
    private void LoadRoot(DataCollection dc, XmlNodeList list)
    {
      foreach (XmlNode xmlNode in list)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "datacollection":                        
            dc.Name = xmlNode.Attributes["name"].Value;            
            dc.RootFolder = xmlNode.Attributes["rootfolder"].Value;            
            LoadDataCollection(dc, xmlNode.ChildNodes);
            break;
        }
      }
    }

    /// <summary>
    /// Loads the contents of a DataCollection.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="list"></param>
    private void LoadDataCollection(DataCollection dc, XmlNodeList list)
    {
      dc.ContentManager.RootDirectory = dc.RootFolder;

      foreach (XmlNode xmlNode in list)
      {
        string filepath = "";

        if(xmlNode.Attributes["rootfolder"] != null)
          filepath = xmlNode.Attributes["rootfolder"].Value;

        switch (xmlNode.Name.ToLower())
        {
          case "models":
            ParseDirectory<Model>(dc,
              String.Format("{0}//{1}", dc.ContentManager.RootDirectory, filepath), filepath);
            break;
          case "textures":
            ParseDirectory<Texture2D>(dc,
              String.Format("{0}//{1}", dc.ContentManager.RootDirectory, filepath), filepath);
            break;
          case "effects":
            ParseDirectory<Effect>(dc,
              String.Format("{0}//{1}", dc.ContentManager.RootDirectory, filepath), filepath); 
            break;
          case "fonts":
            ParseDirectory<SpriteFont>(dc,
              String.Format("{0}//{1}", dc.ContentManager.RootDirectory, filepath), filepath);
            break;
        }
      }
    }

    private void ParseDirectory<T>(DataCollection dc, string directory, string xnadir)
    {
      if (Directory.Exists(directory))
      {
        foreach (string file in Directory.GetFiles(directory))
        {
          string assetname = ""; 

          using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read))
          {           
            assetname = file.Replace(directory, "").Replace(".xnb", "");
            fs.Close();
          }

          LoadAsset<T>(dc, assetname, xnadir);
        }
      }
    }

    /// <summary>
    /// Load asset of type T using a XNA ContentManager. This asset will be added to the ContentHandler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dc"></param>
    /// <param name="node"></param>
    /// <param name="rootfolder"></param>
    private void LoadAsset<T>(DataCollection dc, string assetname, string xnadir)
    {
      T asset;
      string filename = String.Format("{0}{1}", xnadir.Replace("//", "/"), assetname);
      
      try
      {
        asset = dc.ContentManager.Load<T>(filename);
        this.AddAsset<T>(assetname, asset);  
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);        
      }    
    }
   
    /// <summary>
    /// Parses a XML file containing assets. 
    /// All assets contained in the XML file will be loaded and assigned to the DataCollection name.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="datacollectionname"></param>
    public void Load(string filepath, string datacollectionname)
    {
      DataCollection dc = new DataCollection(this.game);
      dc.ContentManager = new ContentManager(this.game.Services);

      // Validate the XML file & Load
      using ( FileStream fs = File.Open(filepath,FileMode.Open, FileAccess.Read))
      {
        XmlDocument XmlDocument = new XmlDocument();
        XmlReader reader = XmlReader.Create(fs);
        XmlDocument.Load(reader);
        fs.Close();

        // Handles child XmlNodes
        if (XmlDocument.HasChildNodes)
        {
          this.LoadRoot(dc, XmlDocument.ChildNodes);
          this.dataCollections.Add(dc);
        }                             
      }      
    }

    /// <summary>
    /// Unload all assets assigned to a certain DataCollection name.
    /// </summary>
    /// <param name="datacollectionname"></param>
    public void UnLoad(string datacollectionname)
    {
      DataCollection toBeRemoved = null;

      foreach (DataCollection coll in this.dataCollections)
      {
        if (coll.Name.Equals(datacollectionname))
        {
          coll.ContentManager.Unload();
        }

        toBeRemoved = coll;
      }

      this.dataCollections.Remove(toBeRemoved);
    }

    #region Asset Management
    /// <summary>
    /// Add an asset to this contenthandler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="asset"></param>
    public void AddAsset<T>(string name, T asset)
    {
      if(asset.GetType() == typeof(Model))
      {
        this.models.Add(name, asset);
      }
      else if (asset.GetType() == typeof(Effect))
      {
        this.effects.Add(name, asset);
      }
      else if (asset.GetType() == typeof(Texture2D))
      {
        this.textures.Add(name, asset);
      }
      else if (asset.GetType() == typeof(SpriteFont))
      {
        this.fonts.Add(name, asset);
      }
    }   

    /// <summary>
    /// Retrieve an asset from this contenthandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetAsset<T>(string name)
    {
      if (typeof(T) == typeof(Model))
      {
        return (T)this.models[name];
      }
      else if (typeof(T) == typeof(Effect))
      {
        return (T)this.effects[name];
      }
      else if (typeof(T) == typeof(Texture2D))
      {
        return (T)this.textures[name];
      }
      else if (typeof(T) == typeof(SpriteFont))
      {
        return (T)this.fonts[name];
      }

      return default(T);
    }
    #endregion
  }
}
