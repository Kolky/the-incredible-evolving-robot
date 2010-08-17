using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Tier.Source.Objects
{
  public class DecorationObject : GameObject
  {
    private Texture2D diffuseMap, normalMap, specularMap;

    public DecorationObject(TierGame game)
      : base(game)
    { }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);

      ((DecorationObject)obj).Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      ((DecorationObject)obj).Effect.Parameters["colorOverlay"].SetValue(new Vector4(1));      
      ((DecorationObject)obj).diffuseMap = this.diffuseMap;
      ((DecorationObject)obj).normalMap = this.normalMap;
      ((DecorationObject)obj).specularMap = this.specularMap;
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);
      this.Effect.Parameters["Texture"].SetValue(diffuseMap);
      this.Effect.Parameters["SpecularMap"].SetValue(specularMap);
      this.Effect.Parameters["NormalMap"].SetValue(normalMap);

      this.Effect.Begin();

      foreach (ModelMesh mesh in Model.Meshes)
      {
        this.Effect.Parameters["matWorld"].SetValue(
            this.Game.GameHandler.World *
            this.transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.Rotation) *
            Matrix.CreateScale(this.Scale) *
            Matrix.CreateTranslation(this.Position));

        this.Effect.CurrentTechnique.Passes[0].Begin();

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

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "normal_map":
            normalMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
          case "diffuse_map":
            diffuseMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
          case "specular_map":
            specularMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
        }
      }
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);
    }
  }
}
