using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Destroyable;
using Tier.Objects.Attachable.Weapons;
using Tier.Objects.Basic;

namespace Tier.Objects.Attachable
{
    public class BlockPiece : AttachableObject
    {
        #region Properties
        public new Effect Effect { get; set; }
        public bool Grown { get; set; }
        public String Name { get; set; }

        protected Matrix[] transforms;
        #endregion

        public BlockPiece(Game game)
            : base(game)
        {
        }

        public void Grow(GrowthPattern pattern)
        {
            Exploded = false;

            if (Connectors.Count <= 0)
            {
                return;
            }

            foreach (GrowthPatternBlock block in pattern.Blocks)
            {
                // Check if block is required & if the connector is unused
                if (block.Required && Connectors[block.SourceConnector].ConnectedTo == null)
                {
                    // Create new block
                    BlockPiece piece = GameHandler.BossPieceHandler.GetPiece(block.Name);

                    // Check if connector on new block is unused
                    if (piece.Connectors[block.BlockConnector].ConnectedTo == null)
                    {
                        // Check if attaching went oke?
                        if (Attach(block.SourceConnector, block.BlockConnector, piece))
                        {
                            // Add to the block to the game
                            GameHandler.ObjectHandler.AddObject(piece);

                            // Attach weapons to all available piece connections
                            for (int j = 0; j < piece.Connectors.Count; j++)
                            {
                                if (piece.Connectors[j].ConnectedTo == null)
                                {
                                    TurretHolder t = new TurretHolder(GameHandler.Game);

                                    switch (Options.Random.Next(4))
                                    {
                                        case 0:
                                            t.Weapon = new DoubleBulletTurret(GameHandler.Game, t);
                                            break;
                                        case 1:
                                            t.Weapon = new LaserbeamTurret(GameHandler.Game, t);
                                            break;
                                        case 2:
                                            t.Weapon = new HomingRocketTurret(GameHandler.Game, t);
                                            break;
                                        case 3:
                                            t.Weapon = new RocketTurret(GameHandler.Game, t);
                                            break;
                                    }

                                    if (piece.Attach(j, 0, t))
                                    {
                                        // Add turret to game
                                        GameHandler.ObjectHandler.AddObject(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Grown = true;
        }

        #region AttachableObject overrides
        public override void Initialize()
        {
            base.Initialize();

            Effect = TierGame.ContentHandler.getEffect("BlockEffect");
            Effect.Parameters["lineColor"].SetValue(new Vector3(1, 1, 0));
            Effect.Parameters["blockColor"].SetValue(Options.Colors.Boss.BlockColor);

            transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);
            spawnMatrix = Matrix.Identity;

#if DEBUG
            CreateLines();
#endif
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateBoundingObjects();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Exploded)
                return;

            Effect.Begin();
            Effect.Parameters["matView"].SetValue(GameHandler.Camera.View);
            Effect.Parameters["matProjection"].SetValue(GameHandler.Camera.Projection);

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                GraphicsDevice.Indices = mesh.IndexBuffer;
                Effect.Parameters["matWorld"].SetValue(transforms[mesh.ParentBone.Index] *
                    Matrix.CreateFromQuaternion(Position.Front) *
                    spawnMatrix *
                    Matrix.CreateTranslation(Position.Coordinate));

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                    GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);

                    foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0,
                          part.NumVertices, part.StartIndex, part.PrimitiveCount);
                        pass.End();
                    }
                }
            }

            Effect.End();
            GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        }
        #endregion
    }
}