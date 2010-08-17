using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Tier.Source.Objects;
using Tier;

namespace Tier.Source.Objects
{
  public class SkyBox : GameObject
  {
    #region Properties
    protected TextureCube textureCube;
    public TextureCube TextureCube
    {
      get { return textureCube; }
      set { textureCube = value; }
    }
    #endregion

    public SkyBox(TierGame game)
      : base(game)
    {
    }

    public override void Initialize()
    {     
      ContentManager man = new ContentManager(this.Game.Services);
      this.TextureCube = man.Load<TextureCube>("Content//Textures//g02_cube");

      // Assign the Skybox texture to effect
      this.Effect.Parameters["Texture0"].SetValue(TextureCube);
      // Assign the skybox effect to all mesh parts
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (ModelMeshPart part in mesh.MeshParts)
        {
          part.Effect = this.Effect;
        }
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.Model != null)
      {
        this.Effect.Parameters["matWorld"].SetValue(Matrix.Identity);
        this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
        this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);

        foreach (ModelMesh mesh in this.Model.Meshes)
        {
          mesh.Draw();
        }
      }
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      
    }
  }
}