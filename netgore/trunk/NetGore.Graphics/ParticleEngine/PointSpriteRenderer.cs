using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    public sealed class PointSpriteRenderer : IParticleRenderer
    {
        readonly GraphicsDevice _graphicsDevice;
        readonly VertexDeclaration _vertexDeclaration;
        readonly Effect _pointSpriteEffect;
        readonly EffectParameter _textureParameter;
        readonly EffectParameter _wvpParameter;

        Matrix _screenSpaceMatrix;
        SpriteBlendMode _blendMode = SpriteBlendMode.Additive;
        
        /// <summary>
        /// Gets the <see cref="Effect"/> describing how to render the sprites.
        /// </summary>
        public Effect Effect { get { return _pointSpriteEffect; } }

        /// <summary>
        /// Gets or sets the <see cref="SpriteBlendMode"/> to use when rendering the particles.
        /// </summary>
        public SpriteBlendMode BlendMode { get { return _blendMode; } set { _blendMode = value; } }

        public PointSpriteRenderer(GraphicsDevice graphicsDevice, Effect effect)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (effect == null)
                throw new ArgumentNullException("effect");

            _graphicsDevice = graphicsDevice;
            _pointSpriteEffect = effect;

            _textureParameter = Effect.Parameters["SpriteTexture"];
            _wvpParameter = Effect.Parameters["WVPMatrix"];

            _vertexDeclaration = new VertexDeclaration(graphicsDevice, Particle.VertexElements);

            Matrix world = Matrix.Identity;

            Matrix view = new Matrix(
                    1.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, -1.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, -1.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 1.0f);

            var viewport = graphicsDevice.Viewport;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, -viewport.Height, 0, 0, 1);

            _screenSpaceMatrix = world * view * projection;
        }

        bool _isDisposed = false;

        /// <summary>
        /// Gets if this <see cref="PointSpriteRenderer"/> has been disposed.
        /// </summary>
        public bool IsDisposed { get { return _isDisposed; } }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            _vertexDeclaration.Dispose();

            if (_pointSpriteEffect != null)
                _pointSpriteEffect.Dispose();
        }

        public void RenderEmitter(ParticleEmitter emitter)
        {
            if (emitter.ParticleTexture == null || emitter.ActiveParticles <= 0)
                return;

            Matrix transform = Matrix.Identity;

            // Set the screen space transformation matrix
            Matrix tranformationMatrix;
            Matrix.Multiply(ref _screenSpaceMatrix, ref transform, out tranformationMatrix);

            _wvpParameter.SetValue(tranformationMatrix);

            // Set the particle texture
            _textureParameter.SetValue(emitter.ParticleTexture);

            // Set graphics device properties for rendering
            GraphicsDevice device = _graphicsDevice;
            device.VertexDeclaration = _vertexDeclaration;
            device.RenderState.PointSpriteEnable = true;
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.DepthBufferWriteEnable = false;

            // Set the blending mode
            switch (BlendMode)
            {
                case SpriteBlendMode.None:
                    return;

                case SpriteBlendMode.Additive:
                    device.RenderState.SourceBlend = Blend.SourceAlpha;
                    device.RenderState.DestinationBlend = Blend.One;
                    break;

                case SpriteBlendMode.AlphaBlend:
                    device.RenderState.SourceBlend = Blend.SourceAlpha;
                    device.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                    break;

                default:
                    Debug.Fail(string.Format("Invalid BlendMode `{0}`.", BlendMode));
                    return;
            }

            // Start the effect
            _pointSpriteEffect.Begin();
            _pointSpriteEffect.Techniques[0].Passes[0].Begin();

            // Draw
            var particles = emitter.GetParticlesArray();
            device.DrawUserPrimitives(PrimitiveType.PointList, particles, 0, emitter.ActiveParticles);

            // End the effect
            _pointSpriteEffect.Techniques[0].Passes[0].End();
            _pointSpriteEffect.End();
        }
    }
}
