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
    private String name;
    private bool grown;
    private Effect effect;   
	
    public new Effect Effect
    {
      get { return effect; }
      set { effect = value; }
    }

    public bool Grown
    {
      get { return grown; }
      set { grown = value; }
    }

    public String Name
    {
      get { return name; }
      set { name = value; }
    }

    protected Matrix[] transforms;
    #endregion    

    public BlockPiece(Game game)
      : base(game)
    {
    }

    public void Grow(GrowthPattern pattern)
    {
      this.Exploded = false;

      if (this.Connectors.Count <= 0)
      {
        return;
      }

      foreach (GrowthPatternBlock block in pattern.Blocks)
      {
        // Check if block is required & if the connector is unused
        if (block.Required && this.Connectors[block.SourceConnector].ConnectedTo == null)
        {
          // Create new block
          BlockPiece piece = GameHandler.BossPieceHandler.GetPiece(block.Name);
          
          // Check if connector on new block is unused
          if (piece.Connectors[block.BlockConnector].ConnectedTo == null)
          {
            // Check if attaching went oke?
            if (this.Attach(block.SourceConnector, block.BlockConnector, piece))
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
                      //t.Weapon = new DoubleBulletTurret(GameHandler.Game, t);
                      break;
                    case 3:
                      t.Weapon = new RocketTurret(GameHandler.Game, t);
                      //t.Weapon = new LaserbeamTurret(GameHandler.Game, t);
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
      this.Grown = true;
    }

    #region AttachableObject overrides
    public override void Initialize()
    {
      base.Initialize();

      this.Effect = TierGame.ContentHandler.getEffect("BlockEffect");
      this.Effect.Parameters["lineColor"].SetValue(new Vector3(1, 1, 0));
      this.Effect.Parameters["blockColor"].SetValue(Options.Colors.Boss.BlockColor);
     
      transforms = new Matrix[this.Model.Bones.Count];
      this.Model.CopyAbsoluteBoneTransformsTo(transforms);
      this.spawnMatrix = Matrix.Identity;

#if DEBUG
      this.CreateLines();
#endif
    }

    public override void Update(GameTime gameTime)
    {
      this.UpdateBoundingObjects();
      base.Update(gameTime);
    }
   
    public override void Draw(GameTime gameTime)
    {
      if (this.Exploded)
        return;

      this.Effect.Begin();
      this.Effect.Parameters["matView"].SetValue(GameHandler.Camera.View);
      this.Effect.Parameters["matProjection"].SetValue(GameHandler.Camera.Projection);

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        this.GraphicsDevice.Indices = mesh.IndexBuffer;
        this.effect.Parameters["matWorld"].SetValue(          
          transforms[mesh.ParentBone.Index] *
          Matrix.CreateFromQuaternion(this.Position.Front) *
          this.spawnMatrix * 
          Matrix.CreateTranslation(this.Position.Coordinate));

        foreach (ModelMeshPart part in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);

          foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
          {
            pass.Begin();
              this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0,
                part.NumVertices, part.StartIndex, part.PrimitiveCount);
            pass.End();
          }
        }
      }

      this.Effect.End();
      this.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
    }
    #endregion
  }
}