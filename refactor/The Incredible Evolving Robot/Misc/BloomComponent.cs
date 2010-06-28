using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Misc
{
    public class BloomComponent : DrawableGameComponent
    {
        #region Properties
        private SpriteBatch spriteBatch;

        private Effect bloomExtractEffect;
        private Effect bloomCombineEffect;
        private Effect gaussianBlurEffect;

        private ResolveTexture2D resolveTarget;
        private RenderTarget2D renderTarget1;
        private RenderTarget2D renderTarget2;

        // Choose what display settings the bloom should use.
        private BloomSettings settings = BloomSettings.PresetSettings[0];
        public BloomSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        // Optionally displays one of the intermediate buffers used
        // by the bloom postprocess, so you can see exactly what is
        // being drawn into each rendertarget.
        public enum IntermediateBuffer
        {
            PreBloom,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult,
        }

        private IntermediateBuffer showBuffer = IntermediateBuffer.FinalResult;
        public IntermediateBuffer ShowBuffer
        {
            get { return showBuffer; }
            set { showBuffer = value; }
        }
        #endregion

        #region Initialization
        public BloomComponent(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        [Obsolete]
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

                this.bloomExtractEffect = TierGame.Instance.Content.Load<Effect>("Content/Effects/BloomExtract");
                this.bloomCombineEffect = TierGame.Instance.Content.Load<Effect>("Content/Effects/BloomCombine");
                this.gaussianBlurEffect = TierGame.Instance.Content.Load<Effect>("Content/Effects/GaussianBlur");
            }

            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for reading back the backbuffer contents.
            this.resolveTarget = new ResolveTexture2D(this.GraphicsDevice, width, height, 1, format);

            // Create two rendertargets for the bloom processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because we are going
            // to be blurring the bloom images in any case.
            width = (int)(width * 0.5);
            height = (int)(height * 0.5);

            this.renderTarget1 = new RenderTarget2D(this.GraphicsDevice, width, height, 1, format);
            this.renderTarget2 = new RenderTarget2D(this.GraphicsDevice, width, height, 1, format);
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        [Obsolete]
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            this.resolveTarget.Dispose();
            this.renderTarget1.Dispose();
            this.renderTarget2.Dispose();
        }
        #endregion

        #region Draw
        private void DrawBloomEffect()
        {
            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            this.bloomExtractEffect.Parameters["BloomThreshold"].SetValue(this.Settings.BloomThreshold);

            this.DrawFullscreenQuad(this.resolveTarget, this.renderTarget1,
                               this.bloomExtractEffect, IntermediateBuffer.PreBloom);

            // Pass 2: draw from rendertarget 1 into rendertarget 2,
            // using a shader to apply a horizontal gaussian blur filter.
            this.SetBlurEffectParameters(1.0f / (float)this.renderTarget1.Width, 0);
            this.DrawFullscreenQuad(this.renderTarget1.GetTexture(), this.renderTarget2,
                               this.gaussianBlurEffect, IntermediateBuffer.BlurredBothWays);

            // Pass 3: draw from rendertarget 2 back into rendertarget 1,
            // using a shader to apply a vertical gaussian blur filter.
            this.SetBlurEffectParameters(0, 1.0f / (float)this.renderTarget1.Height);

            this.DrawFullscreenQuad(this.renderTarget2.GetTexture(), this.renderTarget1,
                               this.gaussianBlurEffect, IntermediateBuffer.BlurredBothWays);

            // Pass 4: draw both rendertarget 1 and the original scene
            // image back into the main backbuffer, using a shader that
            // combines them to produce the final bloomed result.
            this.GraphicsDevice.SetRenderTarget(0, null);

            EffectParameterCollection parameters = this.bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(this.Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(this.Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(this.Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(this.Settings.BaseSaturation);

            this.GraphicsDevice.Textures[1] = this.resolveTarget;

            Viewport viewport = this.GraphicsDevice.Viewport;

            this.DrawFullscreenQuad(this.renderTarget1.GetTexture(), viewport.Width, viewport.Height,
                               this.bloomCombineEffect, IntermediateBuffer.FinalResult);
        }

        public void Draw(GameTime gameTime, ResolveTexture2D target)
        {
            Viewport viewport = this.GraphicsDevice.Viewport;

            // Temporarily disable the depth stencil buffer.
            DepthStencilBuffer previousDepthStencil = this.GraphicsDevice.DepthStencilBuffer;
            this.GraphicsDevice.DepthStencilBuffer = null;

            this.resolveTarget = target;

            this.DrawBloomEffect();

            this.GraphicsDevice.SetRenderTarget(0, null);
            this.DrawFullscreenQuad(target, viewport.Width, viewport.Height, this.bloomCombineEffect, IntermediateBuffer.FinalResult);

            // Restore the original depth stencil buffer.
            this.GraphicsDevice.DepthStencilBuffer = previousDepthStencil;
        }

        /// <summary>
        /// This is where it all happens. Grabs a scene that has already been rendered,
        /// and uses postprocess magic to add a glowing bloom effect over the top of it.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Temporarily disable the depth stencil buffer.
            DepthStencilBuffer previousDepthStencil = this.GraphicsDevice.DepthStencilBuffer;
            CullMode prevCullMode = this.GraphicsDevice.RenderState.CullMode;
            bool prevDepthBuffer = this.GraphicsDevice.RenderState.DepthBufferEnable;

            this.GraphicsDevice.DepthStencilBuffer = null;

            // Resolve the scene into a texture, so we can
            // use it as input data for the bloom processing.
            this.GraphicsDevice.ResolveBackBuffer(this.resolveTarget);

            this.DrawBloomEffect();

            // Restore the original depth stencil buffer.
            this.GraphicsDevice.DepthStencilBuffer = previousDepthStencil;
            // The bloom effect modifies some Graphics Device RenderState values, we need to reset them
            this.GraphicsDevice.RenderState.CullMode = prevCullMode;
            this.GraphicsDevice.RenderState.DepthBufferEnable = prevDepthBuffer;
        }


        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect, IntermediateBuffer currentBuffer)
        {
            this.GraphicsDevice.SetRenderTarget(0, renderTarget);

            this.DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect, currentBuffer);

            //this.GraphicsDevice.ResolveRenderTarget(0);
        }

        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect, IntermediateBuffer currentBuffer)
        {
            this.spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);

            // Begin the custom effect, if it is currently enabled. If the user
            // has selected one of the show intermediate buffer options, we still
            // draw the quad to make sure the image will end up on the screen,
            // but might need to skip applying the custom pixel shader.
            if (this.showBuffer >= currentBuffer)
            {
                effect.Begin();
                effect.CurrentTechnique.Passes[0].Begin();
            }

            // Draw the quad.
            this.spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            this.spriteBatch.End();

            // End the custom effect.
            if (this.showBuffer >= currentBuffer)
            {
                effect.CurrentTechnique.Passes[0].End();
                effect.End();
            }
        }


        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = this.gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = this.gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = this.ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = this.ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeGaussian(float n)
        {
            float theta = this.Settings.BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) * Math.Exp(-(n * n) / (2 * theta * theta)));
        }
        #endregion
    }
}