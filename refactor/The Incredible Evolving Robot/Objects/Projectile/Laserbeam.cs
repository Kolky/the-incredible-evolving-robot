using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Handlers;

namespace Tier.Objects.Destroyable.Projectile
{
    public class Laserbeam : Tier.Objects.Projectile
    {
        public Laserbeam(Game game, Position sourcePos)
            : base(game, true, sourcePos, 0)
        {
            TimeToLive = 5000;
            Model = TierGame.ContentHandler.GetModel("Laserbeam");
            ModelMeta = new ModelMeta(Model);

            Scale = 0.5f;
            //this.Sort = SortFilter.Bloom;
            Initialize();
        }

        public override void Initialize()
        {
            RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.Pi);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Equals("mesh_shell"))
                    {
                        effect.Alpha = 0.40f;
                        effect.EmissiveColor = new Vector3(1.0f, 0f, 0f);
                    }
                    else
                    {
                        effect.EmissiveColor = new Vector3(1f);
                    }
                }
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Model != null)
            {
                GraphicsDevice.RenderState.AlphaBlendEnable = true;
                GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;

                float s;

                if (CurrentTime < 1000)
                    s = Scale * CurrentTime / 1000;
                else if (CurrentTime > TimeToLive - 1000)
                    s = Scale * (1000 - (CurrentTime - (TimeToLive - 1000))) / 1000;
                else s = Scale;


                Matrix world = Matrix.CreateScale(new Vector3(s, s, 20)) *
                      RotationFix *
                      Matrix.CreateFromQuaternion(Position.Front) *
                      Matrix.CreateTranslation(Position.Coordinate);


                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = world;
                        effect.View = GameHandler.Camera.View;
                        effect.Projection = GameHandler.Camera.Projection;
                    }
                    mesh.Draw();
                }
            }
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }
    }
}

