using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;
using Tier.Objects.Attachable;
using Tier.Objects.Destroyable;
using Tier.Objects.Basic;

namespace Tier.Objects
{
    public class AttachableObject : DestroyableObject
    {
        #region Properties
        protected Matrix spawnMatrix;
        private AttachableObject parent;

        public AttachableObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Matrix SpawnMatrix
        {
            get { return spawnMatrix; }
            set { spawnMatrix = value; }
        }

        protected List<Connector> connectors;
        public List<Connector> Connectors
        {
            get { return connectors; }
            set { connectors = value; }
        }
        // Used for drawing the Pivot lines
        protected VertexPositionColor[] lines;
        protected BasicEffect linesEffect;

        protected VertexDeclaration vertexDecla;
        #endregion

        public AttachableObject(Game game)
            : this(game, false, null)
        {
        }

        public AttachableObject(Game game, Boolean isCollidable, AttachableObject parent)
            : base(game, isCollidable)
        {
            this.Connectors = new List<Connector>();
            this.lines = new VertexPositionColor[0];
            this.parent = parent;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.vertexDecla = new VertexDeclaration(this.GraphicsDevice, VertexPositionColor.VertexElements);
        }

        public void AddConnection(Vector3 pos, Vector3 pivot)
        {
            this.Connectors.Add(new Connector(pos, pivot, this));
        }

        /// Create a line for each Connector associated with this BlockPiece. Note: Only used in debug mode.
        /// </summary>
        public void CreateLines()
        {
            lock (lines)
            {
                this.lines = new VertexPositionColor[2 * this.Connectors.Count];

                int i = 0;

                foreach (Connector conn in this.Connectors)
                {
                    Color lineColor = Color.White;

                    if (conn.ConnectedTo != null)
                    {
                        lineColor = Color.Red;
                    }
                    this.lines[i++] = new VertexPositionColor(conn.Position, lineColor);
                    this.lines[i++] = new VertexPositionColor(conn.Position + conn.Pivot, lineColor);
                }
            }
        }

        /// <summary>
        /// Draw lines created for highlighting the pivot points.
        /// </summary>
        /// <param name="gameTime"></param>
        public void DrawLines(GameTime gameTime)
        {
            lock (lines)
            {
                if (this.Connectors.Count > 0)
                {
                    this.GraphicsDevice.VertexDeclaration = this.vertexDecla;

                    this.linesEffect.Begin();
                    this.linesEffect.VertexColorEnabled = true;
                    this.linesEffect.LightingEnabled = false;
                    this.linesEffect.View = GameHandler.Camera.View;
                    this.linesEffect.Projection = GameHandler.Camera.Projection;
                    this.linesEffect.World =
                        Matrix.CreateFromQuaternion(this.Position.Front) *
                        Matrix.CreateTranslation(this.Position.Coordinate);

                    foreach (EffectPass pass in this.linesEffect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                            PrimitiveType.LineList, lines, 0, this.Connectors.Count);
                        pass.End();
                    }

                    this.linesEffect.LightingEnabled = true;
                    this.linesEffect.End();
                }
            }
        }

        public Boolean Attach(int sourceConnectorIndex, int newObjectConnectorIndex, AttachableObject newObject)
        {
            newObject.Parent = this;

            if (sourceConnectorIndex < this.Connectors.Count && sourceConnectorIndex >= 0)
            {
                if (newObjectConnectorIndex < newObject.Connectors.Count && newObjectConnectorIndex >= 0)
                {
                    this.Connectors[sourceConnectorIndex].Connect(newObject, newObject.Connectors[newObjectConnectorIndex]);
                    return true;
                }
            }
            return false;
        }

        public override void Explode(BasicObject parent)
        {
            base.Explode(parent);

            foreach (Connector conn in this.Connectors)
            {
                if (conn.ConnectedTo != null
                  && !conn.ConnectedTo.Exploded
                  && conn.ConnectedTo != this
                  && conn.ConnectedTo != parent)
                {
                    conn.ConnectedTo.Explode(this);
                }
            }
        }

        public virtual void UpdateVelocity(AttachableObject parent, Vector3 velocity)
        {
            foreach (Connector conn in Connectors)
            {
                if (conn.ConnectedTo != null
                    && conn.ConnectedTo != this
                    && conn.ConnectedTo != parent)
                {
                    conn.ConnectedTo.UpdateVelocity(this, velocity);
                }
            }

            spawnMatrix *= Matrix.CreateTranslation(velocity);
        }

        public virtual void SetPosition(AttachableObject parent, Matrix translation)
        {
            foreach (Connector conn in Connectors)
            {
                if (conn.ConnectedTo != null
                    && conn.ConnectedTo != this
                    && conn.ConnectedTo != parent)
                {
                    conn.ConnectedTo.SetPosition(this, translation);
                }
            }

            spawnMatrix = translation;
        }

        public virtual int UpdateHealth(AttachableObject parent)
        {
            int totalHealth = 0;
            foreach (Connector conn in Connectors)
            {
                if (conn.ConnectedTo != null
                    && conn.ConnectedTo != this
                    && conn.ConnectedTo != parent)
                {
                    totalHealth += conn.ConnectedTo.UpdateHealth(this);
                }
            }

            Health = MaxHealth;
            Exploded = false;
            return totalHealth + Health;
        }

        public override void UpdateBoundingObjects()
        {
            for (int i = 0; i < BoundingSphereMetas.Count; i++)
                BoundingSphereMetas[i].Center = Position.Coordinate + Vector3.Transform(BoundingSphereMetas[i].Offset, (Matrix.CreateFromQuaternion(Position.Front) * SpawnMatrix));
            for (int i = 0; i < BoundingBoxMetas.Count; i++)
                BoundingBoxMetas[i].Center = Position.Coordinate + Vector3.Transform(BoundingBoxMetas[i].Offset, (Matrix.CreateFromQuaternion(Position.Front) * SpawnMatrix));
        }

        public override void Update(GameTime gameTime)
        {
            if (Health < 0 && !Exploded)
                Explode(Parent);

            base.Update(gameTime);
        }
    }
}