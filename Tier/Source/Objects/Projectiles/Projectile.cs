using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Handlers;

namespace Tier.Source.Objects.Projectiles
{
  public class Projectile : GameObject
  {
    #region Properties
    private float speed;
    private int damage;
    private string textureName;
    private ProjectileHandlerElement element;

    public ProjectileHandlerElement HandlerElement
    {
      get { return this.element; }
      set { this.element = value; }
    }

    public string TextureName 
    { 
      get {return this.textureName;} 
      set {this.textureName=value;} 
    }
    public int Damage
    {
      get { return damage; }
      set { damage = value; }
    }
	
    public float Speed
    {
      get { return speed; }
      set { speed = value; }
    }
    #endregion

    public Projectile(TierGame game)
      : base(game)
    {
      textureName = "Plasma";
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      obj.Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      
      ((Projectile)obj).Speed = this.Speed;
      ((Projectile)obj).Damage = this.Damage;
 
    }

    public void SetEffectParameters()
    {
      this.Effect.Parameters["Texture"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>(textureName));
    }

    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.Game.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;

      this.Effect.Begin();

      this.Effect.Parameters["coloroverlay"].SetValue(this.Color.ToVector4());
      this.Effect.Parameters["matWorld"].SetValue
      (
        Matrix.CreateScale(this.Scale) *
        Matrix.CreateBillboard(
          this.Position,
          this.Game.GameHandler.Camera.Position,
          this.Game.GameHandler.Camera.UpVector,
          this.Game.GameHandler.Camera.ForwardVector)
      );

      this.Effect.CurrentTechnique.Passes[0].Begin();
      this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
        this.Game.BillboardVertices, 0, 2);
      this.Effect.CurrentTechnique.Passes[0].End();

      this.Effect.End();
    }
    
    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "speed":
            this.speed = float.Parse(node.InnerXml);
            break;
          case "damage":
            this.damage = int.Parse(node.InnerXml);
            break;
        }
      }
    }
  }
}
