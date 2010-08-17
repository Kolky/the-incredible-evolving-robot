using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects.ObjectModifiers;
using System.Xml;
using Tier.Source.ObjectModifiers;
using Tier.Source.Handlers;
using Tier.Source.Objects.Powerups;

namespace Tier.Source.Objects
{
  public class BossPiece : GameObject
  {
    #region Properties
    private string name;
    private bool isSelected;
    private bool isCore;
    private Texture2D specularMap;
    private Texture2D diffuseMap;
    private Texture2D normalMap;
    private bool isGrown;
    private bool isInTemplate;
    private int  depth;

    public bool IsInTemplate
    {
      get { return isInTemplate; }
      set { isInTemplate = value; }
    }
	
    public int  Depth
    {
      get { return depth; }
      set { depth = value; }
    }
	
    public bool IsGrown
    {
      get { return isGrown; }
      set { isGrown = value; }
    }
	
    public Texture2D NormalMap
    {
      get { return normalMap; }
      set { normalMap = value; }
    }
	
    public Texture2D DiffuseMap
    {
      get { return diffuseMap; }
      set { diffuseMap = value; }
    }

    public Texture2D SpecularMap
    {
      get { return specularMap; }
      set { specularMap = value; }
    }
	
    public bool IsCore
    {
      get { return isCore; }
      set { isCore = value; }
    }
	
    public bool IsSelected
    {
      get { return isSelected; }
      set { isSelected = value; }
    }
	
    public string Name
    {
      get { return name; }
      set { name = value; }
    }
    #endregion

    private bool isColorLocked;
    private readonly Vector4 colorHit, colorCritical, colorNormal;
    private Matrix transform;
    public FadingModifier fadeMod;

    public BossPiece(Game game)
      : base(game)
    {
      Rotation = Quaternion.Identity;
      colorHit = new Vector4(1, 1, 0, 1);
      colorNormal = Vector4.One;
      colorCritical = new Vector4(1, 0, 0, 1);
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);

      ((BossPiece)obj).IsCore = this.isCore;
      ((BossPiece)obj).Name = this.name;

      obj.Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      obj.Effect.Parameters["Texture"].SetValue(diffuseMap);
      fadeMod = new FadingModifier(this, 5000);
      ((BossPiece)obj).fadeMod = fadeMod;
      obj.AddObjectModifier(fadeMod);
      obj.FadingModifier = fadeMod;

      if (((BossPiece)obj).IsCore)
      {
        fadeMod.IncreaseTimeElapsed(5000);
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if( !this.IsVisible)
        return;

      this.Effect.Begin();

      this.Effect.CurrentTechnique.Passes[0].Begin();
      foreach (ModelMesh mesh in Model.Meshes)
      {
        Matrix m1 =
          this.Game.GameHandler.World *
          this.transforms[mesh.ParentBone.Index] *
          Matrix.CreateScale(this.Scale) *
          Matrix.CreateFromQuaternion(this.Rotation) *
          Matrix.CreateTranslation(this.Position);

        Matrix m2 = this.Game.BehaviourHandler.Transform;

        this.Effect.Parameters["matWorld"].SetValue(m1 * m2);

        foreach (ModelMeshPart meshPart in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);
          this.GraphicsDevice.Indices = mesh.IndexBuffer;
          this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
            meshPart.BaseVertex, 0,
            meshPart.NumVertices,
            meshPart.StartIndex,
            meshPart.PrimitiveCount);
        }
      }

      this.Effect.CurrentTechnique.Passes[0].End();
      this.Effect.End();
    }

    public void Reset()
    {
      this.AttachableModifier.Clear();
      this.IsGrown = false;
      transform = Matrix.Identity;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      this.Effect.Parameters["transparentAmount"].SetValue(fadeMod.FadeAmount);

      if (fadeMod.FadeAmount >= 1.0f &&
          this.GrowableModifier != null)
      {
        // Grow                
        this.GrowableModifier.Grow();
      }
    }

    public void GrowChildren(int damage, GameObject parent)
    {
      if (this.AttachableModifier != null)
      {
        // All attached children will grow too :)
        foreach (Connector conn in this.AttachableModifier.Connectors)
        {
          if (conn.ConnectedTo != null &&
            conn.ConnectedTo != parent &&
            conn.ConnectedTo.AttachableModifier.Level > parent.AttachableModifier.Level &&
            conn.ConnectedTo.GetType() == typeof(BossPiece))
          {
            BossPiece piece = (BossPiece)conn.ConnectedTo;

            piece.GrowChildren(damage, this);
          }
        }
      }

      if (this.FadingModifier != null)
        this.FadingModifier.IncreaseTimeElapsed(damage);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);

      if (this.Effect != null)
        this.Effect.Dispose();
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "name":
            name = node.InnerText;
            break;
          case "normal_map":
            normalMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
          case "diffuse_map":
            diffuseMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
          case "specular_map":
            specularMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
          case "core":
            this.isCore = true;
            break;
        }
      }
    }

    public void SetBloomTechnique()
    {
      // Update overlay in effect and enable blooming
      this.Effect.Parameters["colorOverlay"].SetValue(Color.Red.ToVector4());
      this.Effect.CurrentTechnique = this.Effect.Techniques["Bloom"];
      this.isColorLocked = true;
    }

    public void SetDefaultTechnique()
    {
      this.Effect.Parameters["colorOverlay"].SetValue(Vector4.One);
      this.Effect.CurrentTechnique = this.Effect.Techniques["Default"];
      this.isColorLocked = false;
    }
  }
}