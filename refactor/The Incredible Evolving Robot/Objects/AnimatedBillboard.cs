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
            public Vector2 TextureCoordinate { get; set; }
            public Vector3 Position { get; set; }

            public MyVertexPositionTexture(Vector3 pos, Vector2 texcoord)
                : this()
            {
                Position = pos;
                TextureCoordinate = texcoord;
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
        public String Name { get; set; }
        public int TimePerFrame { get; set; }
        public float DistanceToPlayerSquared { get; set; }
        #endregion

        public AnimatedBillboard(Game game, String name, bool isLooped, Vector3 pos, float scale, int timePerFrame)
            : base(game, false)
        {
            Name = name;
            isLooping = isLooped;
            Sort = SortFilter.AlphaBlendedBillboard;
            Scale = scale;
            Position.Coordinate = pos;
            TimePerFrame = timePerFrame;
            DistanceToPlayerSquared = (GameHandler.Player.Position.Coordinate - Position.Coordinate).LengthSquared();
            Initialize();
        }

        /// <summary>
        /// create an animated billboard that is not looping
        /// </summary>
        /// <param name="game"></param>
        /// <param name="filepath"></param>
        public AnimatedBillboard(Game game, String filepath)
            : this(game, filepath, false, Vector3.Zero, 1, 50)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //vd = new VertexDeclaration(this.GraphicsDevice, MyVertexPositionTexture.VertexElements);
            this.Effect = (BasicEffect)TierGame.ContentHandler.getEffect("AnimatedBillboard");

            // Initialise vertex buffer
            vb = new VertexBuffer(this.GraphicsDevice, MyVertexPositionTexture.SizeInBytes * 4, BufferUsage.Points);
            MyVertexPositionTexture[] verts = new MyVertexPositionTexture[]
            {
                new MyVertexPositionTexture(
                    new Vector3(-1,-1,0),
                    new Vector2(0,0)
                ),
                new MyVertexPositionTexture(
                    new Vector3(1,-1,0),
                    new Vector2(1,0)
                ),
                new MyVertexPositionTexture(
                    new Vector3(1,1,0),
                    new Vector2(1,1)
                ),
                new MyVertexPositionTexture(
                    new Vector3(-1,1,0),
                    new Vector2(0,1)
                )
            };
            vb.SetData<MyVertexPositionTexture>(verts);

            // Initialise index buffer
            short[] indices = new short[] { 0, 1, 2, 2, 3, 0 };
            ib = new IndexBuffer(this.GraphicsDevice, sizeof(short) * 6, BufferUsage.WriteOnly, IndexElementSize.SixteenBits);
            ib.SetData<short>(indices);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedMillis += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedMillis >= TimePerFrame)
            {
                currentTexture += (elapsedMillis / TimePerFrame);
                elapsedMillis = 0;

                if (currentTexture >= TierGame.ContentHandler.getAnimatedTextureCount(Name))
                {
                    currentTexture = 0;
                    if (!isLooping)
                        GameHandler.ObjectHandler.RemoveObject(this);
                    return;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Effect.Texture = TierGame.ContentHandler.getAnimatedTexture(Name, currentTexture);

            Effect.World = Matrix.CreateScale(Scale) * Matrix.CreateBillboard(Position.Coordinate, GameHandler.Camera.RealPosition, GameHandler.Camera.Up, GameHandler.Camera.Forward);
            Effect.View = GameHandler.Camera.View;
            Effect.Projection = GameHandler.Camera.Projection;
            GraphicsDevice.VertexDeclaration = AnimatedBillboard.vd;
            GraphicsDevice.Indices = ib;
            GraphicsDevice.Vertices[0].SetSource(vb, 0, MyVertexPositionTexture.SizeInBytes);

            Effect.Begin();
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                pass.End();
            }
            Effect.End();

            GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        public override int CompareTo(object obj)
        {
            if (!obj.GetType().Equals(typeof(AnimatedBillboard)))
                return base.CompareTo(obj);

            AnimatedBillboard otherObject = (AnimatedBillboard)obj;

            if (otherObject.Sort.Equals(this.Sort))
            {
                if (DistanceToPlayerSquared > otherObject.DistanceToPlayerSquared)
                    return -1;
                else return 1;
            }
            return base.CompareTo(obj);
        }
    }
}
