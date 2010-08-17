using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.ObjectModifiers;
using System.Xml;
using Tier.Source.Helpers;
using Tier.Source.Objects.ObjectModifiers;
using Tier.Source.Handlers;

namespace Tier.Source.Objects
{
  public abstract class GameObject : DrawableGameComponent, IDisposable
  {
    #region Properties
    private Effect effect;
    private Model model;
    protected List<ObjectModifier> objectModifiers;
    private AttachableModifier attachableModifier;
    private Vector3 position;
    private Quaternion rotation;
    private TierGame game;
    private Color color;
    protected Matrix[] transforms;
    private Vector3 scale;
    private MovableModifier movableModifier;
    private TemporaryModifier temporaryModifier;
    private CollisionModifier collisionModifier;
    private DestroyableModifier destroyableModifier;
    private GrowableModifier growableModifier;
    private bool isVisible;
    private GameHandler.ObjectType type;
    private FadingModifier fadeModifier;

    public GameHandler.ObjectType Type
    {
      get { return type; }
      set { type = value; }
    }
	
    public bool IsVisible
    {
      get { return isVisible; }
      set { isVisible = value; }
    }

    public CollisionModifier CollisionModifier
    {
      get { return collisionModifier; }
      set { collisionModifier = value; }
    }

    public DestroyableModifier DestroyableModifier
    {
      get { return destroyableModifier; }
      set { destroyableModifier = value; }
    }

    public FadingModifier FadingModifier
    {
      get { return fadeModifier; }
      set { fadeModifier = value; }
    }

    public GrowableModifier GrowableModifier
    {
      get { return growableModifier; }
      set { growableModifier = value; }
    }

    public TemporaryModifier TemporaryModifier
    {
      get { return temporaryModifier; }
      set { temporaryModifier = value; }
    }
	
    public MovableModifier MovableModifier
    {
      get { return movableModifier; }
      set { movableModifier = value; }
    }
	
    public Vector3 Scale
    {
      get { return scale; }
      set { scale = value; }
    }

    public Color Color
    {
      get { return color; }
      set { color = value; }
    }
	
    public new TierGame Game
    {
      get { return game; }
      set { game = value; }
    }

    public Quaternion Rotation
    {
      get { return rotation; }
      set 
      { 
        rotation = value;
      }
    }
	
    public Vector3 Position
    {
      get { return position; }
      set 
      { 
        position = value;

        // Update all connected pieces
        if (this.attachableModifier != null && this.movableModifier != null && this.movableModifier.Velocity != Vector3.Zero)
          this.attachableModifier.UpdateFromVelocity(this.movableModifier.Velocity, this);
      }
    }
	
    public AttachableModifier AttachableModifier
    {
      get { return attachableModifier; }
      set { attachableModifier = value; }
    }
	
    public Model Model
    {
      get { return model; }
      set { model = value; }
    }
	
    public Effect Effect
    {
      get { return effect; }
      set { effect = value; }
    }
    #endregion

    protected bool isDisposed;

    public GameObject(Game game)
      : base(game)
    {
      this.game = (TierGame)game;
      this.objectModifiers = new List<ObjectModifier>();
      this.scale = new Vector3(1);
      this.isVisible = true;
      this.rotation = Quaternion.Identity;
      this.color = Color.White;
    }

    public override void Initialize()
    {
      base.Initialize();

      if (this.model != null)
      {
        this.transforms = new Matrix[this.Model.Bones.Count];
        this.Model.CopyAbsoluteBoneTransformsTo(this.transforms);
      }
    }

    public void AddObjectModifier(ObjectModifier objmod)
    {
      this.objectModifiers.Add(objmod);
    }
    
    /// <summary>
    /// Create a clone of this GameObject in the supplied argument.
    /// </summary>
    /// <param name="obj"></param>
    public virtual void Clone(GameObject obj)
    {
      obj.Model = this.Model;
      obj.Effect = this.effect;//.Clone(this.game.GraphicsDevice);
      obj.Color = this.color;
      obj.scale = this.scale;
      obj.rotation = this.rotation;
      obj.Type = this.Type;

      foreach (ObjectModifier objmod in this.objectModifiers)
      {
        ObjectModifier newobjmod = null;
        objmod.Clone(out newobjmod, obj);

        obj.AddObjectModifier(newobjmod);
      }
    }
  
    public override void Draw(GameTime gameTime)
    {
      //this.GraphicsDevice.RenderState.AlphaBlendEnable = false;

      if (!this.isVisible)
        return;

      this.effect.Begin();

      foreach (ModelMesh mesh in this.model.Meshes)
      {        
        this.effect.Parameters["matWorld"].SetValue(
            this.Game.GameHandler.World *         
            this.transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.rotation) * 
            Matrix.CreateScale(this.scale) *
            Matrix.CreateTranslation(this.position));
        this.GraphicsDevice.Indices = mesh.IndexBuffer;

        foreach (ModelMeshPart part in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;          
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, 0, part.VertexStride);

          foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
          {
            pass.Begin();
            this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0,
              part.NumVertices, part.StartIndex, part.PrimitiveCount);
            pass.End();
          }
        }
      }

      this.effect.End();
    }

    public void RemoveObjectModifier(ObjectModifier objMod)
    {
      this.objectModifiers.Remove(objMod);
    }

    public override void Update(GameTime gameTime)
    {
      if (isDisposed)
        return;

      foreach (ObjectModifier mod in this.objectModifiers)
      {
        mod.Update(gameTime);
      }

      base.Update(gameTime);
    }



    #region XML parsing

    private void handleEffectParameters(XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "colors":
            handleEffectColors(node.ChildNodes);
            break;
        }
      }
    }

    private void handleEffectColors(XmlNodeList xmlNodeList)
    {
      List<Vector3> colors = new List<Vector3>();

      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "color":
            colors.Add(PublicMethods.handleVector3(node));
            break;
        }
      }

      Vector3[] colorarray = new Vector3[colors.Count];
      for (int i = 0; i < colors.Count; i++)
      {
        colorarray[i] = colors[i];
      }

      //this.effect.Parameters["color"].SetArrayRange(0, 2);
      this.effect.Parameters["color"].SetValue(colorarray[0]);
    }

    /// <summary>
    /// Read global object properties from XML file.
    /// </summary>
    /// <param name="xmlNodeList"></param>
    public void ReadFromXml(XmlNodeList xmlNodeList)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "specific":
            // A object specific tag has been detected, the ReadFromXml from the inherited class will now be run
            this.ReadSpecificsFromXml(xmlNode.ChildNodes);
            return;
          case "color":
            this.color = new Color(PublicMethods.handleVector3(xmlNode));
            break;
          case "model":
            this.Model = this.game.ContentHandler.GetAsset<Model>(xmlNode.Attributes["name"].Value);
            break;
          case "effect":

            switch (xmlNode.Attributes["name"].Value)
            {
              case "AlphaBlend":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.AlphaBlend;
                break;
              case "DefaultTextured":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.DefaultTextured;
                break;
              case "Transparent":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.Transparent;
                break;
              case "Skybox":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.Skybox;
                break;
              case "RenderGBuffeR":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.Deferred;
                break;
              case "Default":
                this.type = Tier.Source.Handlers.GameHandler.ObjectType.Default;
                break;
              default:
                break;
            }
            
            this.Effect = this.game.ContentHandler.GetAsset<Effect>(xmlNode.Attributes["name"].Value);

            // Determine if a different technique needs to be used in drawing this object
            if (xmlNode.Attributes["technique"] != null)
            {
              this.Effect.CurrentTechnique = this.Effect.Techniques[xmlNode.Attributes["technique"].Value];
            }

            if (xmlNode.HasChildNodes)
            {
              handleEffectParameters(xmlNode.ChildNodes);
            }

            break;
          case "rotation":
            Vector3 rot = PublicMethods.handleVector3(xmlNode);
            rot *= MathHelper.Pi / 180;
            this.rotation = Quaternion.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z);

            break;
          case "scale":
            this.scale = PublicMethods.handleVector3(xmlNode);
            break;
          case "attachable":
            AttachableModifier attmod = new AttachableModifier(this);

            handleAttachable(xmlNode.ChildNodes, attmod);
            
            this.attachableModifier = attmod;
            this.objectModifiers.Add(attmod);
            break;
          case "movable":
            MovableModifier movmod = new MovableModifier(this);

            this.objectModifiers.Add(movmod);
            break;
          case "growable":
            GrowableModifier growmod = new GrowableModifier(this);
            handleGrowable(xmlNode.ChildNodes, growmod);
            this.objectModifiers.Add(growmod);
            break;
          case "temporary":
            TemporaryModifier tempmod = new TemporaryModifier(this);

            handleTemporary(xmlNode.ChildNodes, tempmod);

            this.objectModifiers.Add(tempmod);
            break;
          case "collision":
            CollisionModifier colmod = new CollisionModifier(this);
            handleCollision(xmlNode.ChildNodes, colmod);
            this.objectModifiers.Add(colmod);
            break;
          case "destroyable":
            DestroyableModifier destmod = new DestroyableModifier(this);
            handleDestroyable(xmlNode.ChildNodes, destmod);

            this.objectModifiers.Add(destmod);
            break;
        }
      }
    }

    #region Destroyable modifier XML parsing
    /// <summary>
    /// Handles XML parsing of DestroyableModifier.
    /// </summary>
    /// <param name="xmlNodeList"></param>
    /// <param name="destmod"></param>
    private void handleDestroyable(XmlNodeList xmlNodeList, DestroyableModifier destmod)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "health":
            destmod.Health = int.Parse(xmlNode.InnerXml);
            break;
        }
      }
    }
    #endregion

    #region Growable modifier XML parsing
    private void handleGrowable(XmlNodeList xmlNodeList, GrowableModifier destmod)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "growth_templates":
            string id = xmlNode.Attributes["id"].Value;
            handleGrowthTemplates(xmlNode.ChildNodes, id, destmod);
            break;
        }
      }
    }

    private void handleGrowthTemplates(XmlNodeList xmlNodeList, string id,  GrowableModifier destmod)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "growth_template":            
            handleGrowthTemplate(xmlNode, id,  destmod);
            break;
        }
      }
    }

    private void handleGrowthTemplate(XmlNode xmlNode, string bossTemplate, GrowableModifier destmod)
    {      
      GrowableModifierRule rule = new GrowableModifierRule();
      rule.ruletext = xmlNode.InnerXml;
      
      destmod.AddRule(bossTemplate, rule);
    }
    #endregion

    #region Collision modifier XML parsing
    /// <summary>
    /// Read specific object properties from XML file.
    /// </summary>
    /// <param name="xmlNodeList"></param>
    public abstract void ReadSpecificsFromXml(XmlNodeList xmlNodeList);

    private void handleCollision(XmlNodeList xmlNodeList, CollisionModifier colmod)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "boundingbox":
            handleBoundingBox(xmlNode, colmod);
            break;
        }
      }
    }

    private void handleBoundingBox(XmlNode xmlNodeList, CollisionModifier colmod)
    {
      BoundingBox box = new BoundingBox();
      Vector3 position = Vector3.Zero;
      Vector3 min = Vector3.Zero, max = Vector3.Zero;

      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "position":
            position = PublicMethods.handleVector3(xmlNode);
            break;
          case "bottomleft":
            min = PublicMethods.handleVector3(xmlNode);
            break;
          case "topright":
            max = PublicMethods.handleVector3(xmlNode);
            break;
        }
      }

      box.Min = Vector3.Add(min, position);
      box.Max = Vector3.Add(max, position);
      colmod.AddBoundingBox(box);
    }
    #endregion

    #region Temporary modifier XML parsing
    /// <summary>
    /// Handles TemporaryModifier's XML parsing
    /// </summary>
    /// <param name="xmlNodeList"></param>
    /// <param name="attmod"></param>
    /// <param name="conntype"></param>
    private void handleTemporary(XmlNodeList xmlNodeList, TemporaryModifier tempmod)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "ttl":
            tempmod.TTL = int.Parse(node.InnerXml);
            break;
        }
      }
    }
    #endregion

    #region Attachable modifier XML parsing
    /// <summary>
    /// Handles AttachableModifier's connector XML parsing
    /// </summary>
    /// <param name="xmlNodeList"></param>
    /// <param name="attmod"></param>
    /// <param name="conntype"></param>
    private void handleAttachableConnection(XmlNodeList xmlNodeList, AttachableModifier attmod, ConnectorType conntype)
    {      
      Vector3 position = Vector3.Zero;
      Vector3 pivot = Vector3.Zero;
      Vector3 scale = new Vector3(1);

      foreach (XmlNode connectionNode in xmlNodeList)
      {
        switch (connectionNode.Name.ToLower())
        {
          case "position":
            position = PublicMethods.handleVector3(connectionNode);
            break;
          case "pivot":
            pivot = PublicMethods.handleVector3(connectionNode);
            break;
          case "scale":
            scale = PublicMethods.handleVector3(connectionNode);
            break;
        }
      }

      Connector conn = new Connector(position, pivot, null, conntype);
      conn.Scale = scale;
      attmod.Connectors.Add(conn);
    }

    /// <summary>
    /// Handles XML parsing of AttachableModifier.
    /// </summary>
    /// <param name="xmlNodeList"></param>
    /// <param name="attmod"></param>
    private void handleAttachable(XmlNodeList xmlNodeList, AttachableModifier attmod)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        switch (xmlNode.Name.ToLower())
        {
          case "weaponconnection":            
            handleAttachableConnection(xmlNode.ChildNodes, attmod, ConnectorType.Weapon);              
            break;
          case "connection":
            handleAttachableConnection(xmlNode.ChildNodes, attmod, ConnectorType.BossPiece);
            break;
        }
      }
    }
    #endregion
    #endregion

    #region IDisposable Members

    void IDisposable.Dispose()
    {
      isDisposed = true;
    }

    #endregion
  }
}