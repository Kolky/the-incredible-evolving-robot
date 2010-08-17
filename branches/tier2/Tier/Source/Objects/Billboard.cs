using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Tier.Source.Objects
{
  public enum BillboardType
  {
    Normal, AnimatedTexture
  };

  public class Billboard : GameObject
  {
    #region Properties
    protected BillboardType billBoardType;
    private int currentTexture;
    private int textureCount;
    private string textureName;
    private LinkedListNode<Billboard> billboardNode;
    private bool isAxisAligned;
    private Vector3 constrainAxis;
    private Vector3 min;
    private Vector3 max;
	
    public Vector3 Max
    {
      get { return max; }
      set { max = value; }
    }

    public Vector3 Min
    {
      get { return min; }
      set { min = value; }
    }


    /// <summary>
    /// The axis around which the axis aligned Billboard will be constrained
    /// </summary>
    public Vector3 ConstrainAxis
    {
      get { return constrainAxis; }
      set { constrainAxis = value; }
    }

    public bool IsAxisAligned
    {
      get { return isAxisAligned; }
      set { isAxisAligned = value; }
    }

    /// <summary>
    /// The node in which this billboard is saved in the billboards list
    /// </summary>
    public LinkedListNode<Billboard> Node
    {
      get { return billboardNode; }
      set { billboardNode = value; }
    }

    public string TextureName
    {
      get { return textureName; }
      set { textureName = value; }
    }

    public int TextureCount
    {
      get { return textureCount; }
      set { textureCount = value; }
    }
    #endregion

    public Billboard(TierGame game)
      : this(game, BillboardType.Normal, false)
    { }

    public Billboard(TierGame game, BillboardType type)
      : this(game, type, false)
    { }

    public Billboard(TierGame game, BillboardType type, bool isAxisAligned)
      : base(game)
    {
      this.billBoardType = type;
      this.isAxisAligned = isAxisAligned;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);

      switch (this.billBoardType)
      {
        case BillboardType.AnimatedTexture:
          if (++this.currentTexture > this.textureCount)
            this.currentTexture = 0;

          // Update texture in effect file
          Texture2D tex = this.Game.ContentHandler.GetAsset<Texture2D>
            (String.Format("{0}{1:0000}", this.TextureName, this.currentTexture));
          this.Effect.Parameters["Texture"].SetValue(tex);
          break;
        case BillboardType.Normal:

          break;
      }

      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);
      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
    }

    public override void Draw(GameTime gameTime)
    {
      this.Game.GraphicsDevice.VertexDeclaration = this.Game.BillboardVertexDeclaration;

      this.Effect.Begin();

      if (this.billBoardType == BillboardType.Normal)
        this.Effect.Parameters["Texture"].SetValue(this.Game.ContentHandler.GetAsset<Texture2D>(this.TextureName));

      this.Effect.Parameters["matWorld"].SetValue(
        Matrix.CreateScale(this.Scale) *
        Matrix.CreateBillboard(this.Position, this.Game.GameHandler.Camera.Position,
                this.Game.GameHandler.Camera.UpVector, this.Game.GameHandler.Camera.ForwardVector));
            
      foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
      {
        pass.Begin();
        this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
          this.Game.BillboardVertices, 0, 2);
        pass.End();
      }

      this.Effect.End();
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      ((Billboard)obj).Effect = this.Effect.Clone(obj.Game.GraphicsDevice);
      ((Billboard)obj).TextureCount = this.textureCount;
      ((Billboard)obj).TextureName = this.TextureName;

      // Assign texture in creation of Normal Billboard
      if (billBoardType == BillboardType.Normal && this.TextureName != null)
      {
        Texture2D tex = this.Game.ContentHandler.GetAsset<Texture2D>(this.textureName);
        obj.Effect.Parameters["Texture"].SetValue(tex);
      }
    }

    public override void ReadSpecificsFromXml(XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "texture":
            this.textureCount = 1;
            this.textureName = node.Attributes["assetname"].Value;
            break;
          case "animatedtexture":
            this.textureCount = int.Parse(node.Attributes["count"].Value);
            this.textureName = node.Attributes["assetname"].Value;
            break;
        }
      }
    }
  }
}