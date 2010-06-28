using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Tier.Handlers;
using Tier.Misc;

namespace Tier.Objects
{
  class AnimatedBillboard : BasicObject
  {
    #region Properties
    public struct MyVertexPositionTexture
    {
      private Vector3 position;
      private Vector2 texcoord;

      public Vector2 TextureCoordinate
      {
        get { return texcoord; }
        set { texcoord = value; }
      }

      public Vector3 Position
      {
        get { return position; }
        set { position = value; }
      }

      public MyVertexPositionTexture(Vector3 position, Vector2 texcoord)
      {
        this.position = position;
        this.texcoord = texcoord;
      }

      public static readonly VertexElement[] VertexElements = new VertexElement[]		
      {
        new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
        new VertexElement(0, 12, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0)
      };

      public static int SizeInBytes
      {
        get { return 20; }
      }
    };

    private VertexBuffer vb;
    private IndexBuffer ib;
    private int currentTexture = 0;
    private int elapsedMillis = 0;
    private bool isLooping;
    public static VertexDeclaration vd;
    private String name;
    private int timePerFrame = 50;

    public int TimePerFrame
    {
      get { return timePerFrame; }
      set { timePerFrame = value; }
    }

    private float distanceToPlayerSquared;

    public float DistanceToPlayerSquared
    {
      get { return distanceToPlayerSquared; }
      set { distanceToPlayerSquared = value; }
    }
	  #endregion

    public AnimatedBillboard(Game game, String name, bool isLooped,Vector3 pos, float scale, int timePerFrame)
      : base(game, false)
    {
      this.name = name;
      this.isLooping = isLooped;
      this.Sort = SortFilter.AlphaBlendedBillboard;
      this.Scale = scale;
      this.Position.Coordinate = pos;
      this.TimePerFrame = timePerFrame;
      this.distanceToPlayerSquared = (GameHandler.Player.Position.Coordinate - this.Position.Coordinate).LengthSquared();
      this.Initialize();
    }

    /// <summary>
    /// create an animated billboard that is not looping
    /// </summary>
    /// <param name="game"></param>
    /// <param name="filepath"></param>
    public AnimatedBillboard(Game game, String filepath)
      : this(game,filepath,false,Vector3.Zero,1,50)
    {      
    }

    public override void Initialize()
    {
      base.Initialize();

      //vd = new VertexDeclaration(this.GraphicsDevice, MyVertexPositionTexture.VertexElements);
      this.Effect = (BasicEffect) TierGame.ContentHandler.getEffect("AnimatedBillboard");

      // Initialise vertex buffer
      vb = new VertexBuffer(this.GraphicsDevice, MyVertexPositionTexture.SizeInBytes * 4, ResourceUsage.None);
      MyVertexPositionTexture[] verts = new MyVertexPositionTexture[]
                      {
                          new MyVertexPositionTexture(
                              new Vector3(-1,-1,0),
                              new Vector2(0,0)),
                          new MyVertexPositionTexture(
                              new Vector3(1,-1,0),
                              new Vector2(1,0)),
                          new MyVertexPositionTexture(
                              new Vector3(1,1,0),
                              new Vector2(1,1)),
                          new MyVertexPositionTexture(
                              new Vector3(-1,1,0),
                              new Vector2(0,1))
                      };
      vb.SetData<MyVertexPositionTexture>(verts);

      // Initialise index buffer
      short[] indices = new short[] { 0, 1, 2, 2, 3, 0 };
      ib = new IndexBuffer(this.GraphicsDevice, sizeof(short) * 6, ResourceUsage.None, IndexElementSize.SixteenBits);
      ib.SetData<short>(indices);
    }

    public override void Update(GameTime gameTime)
    {
      elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;

      if (elapsedMillis >= this.timePerFrame)
      {
        currentTexture+=(elapsedMillis/this.timePerFrame);
        elapsedMillis = 0;

        if (currentTexture >= TierGame.ContentHandler.getAnimatedTextureCount(name))
        {
          currentTexture = 0;
          if(!isLooping)
            GameHandler.ObjectHandler.RemoveObject(this);
          return;
        }
      }
    }

    public override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.RenderState.AlphaBlendEnable = true;
      this.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
      this.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
      this.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
      this.Effect.Texture = TierGame.ContentHandler.getAnimatedTexture(name, currentTexture);
    
      this.Effect.World = Matrix.CreateScale(this.Scale) *
        Matrix.CreateBillboard(this.Position.Coordinate, GameHandler.Camera.RealPosition, GameHandler.Camera.Up, GameHandler.Camera.Forward);  
      this.Effect.View = GameHandler.Camera.View;
      this.Effect.Projection = GameHandler.Camera.Projection;
      this.GraphicsDevice.VertexDeclaration = AnimatedBillboard.vd;
      this.GraphicsDevice.Indices = ib;      
      this.GraphicsDevice.Vertices[0].SetSource(vb, 0, MyVertexPositionTexture.SizeInBytes);
      
      this.Effect.Begin();
      foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
      {
        pass.Begin();        
          this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
        pass.End();
      }
      this.Effect.End();
      
      this.GraphicsDevice.RenderState.AlphaBlendEnable = false;
    }

    public override int CompareTo(object obj)
    {
      if (!obj.GetType().Equals(typeof(AnimatedBillboard)))
        return base.CompareTo(obj);

      AnimatedBillboard otherObject = (AnimatedBillboard)obj;

      if(otherObject.Sort.Equals(this.Sort))
      {
        if (this.distanceToPlayerSquared > otherObject.distanceToPlayerSquared)
          return -1;
        else return 1;
      }
      return base.CompareTo(obj);
    }
  }
}
