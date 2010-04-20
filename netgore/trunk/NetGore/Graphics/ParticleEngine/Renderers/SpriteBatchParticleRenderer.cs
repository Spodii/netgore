using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="IParticleRenderer"/> that draws with a <see cref="SpriteBatch"/>.
    /// </summary>
    public sealed class SpriteBatchParticleRenderer : ParticleRendererBase
    {
        BlendMode _startingBlendMode = BlendMode.Alpha;

        /// <summary>
        /// Gets or sets the <see cref="ISpriteBatch"/> used to draw.
        /// </summary>
        public ISpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BlendMode"/> the <see cref="SpriteBatch"/> is using
        /// when the <see cref="SpriteBatchParticleRenderer"/> is called to render.
        /// </summary>
        public BlendMode StartingBlendMode
        {
            get { return _startingBlendMode; }
            set
            {
                if (value == BlendMode.None)
                    throw new ArgumentException("value cannot be BlendMode.None.");

                _startingBlendMode = value;
            }
        }

        void BeginSpriteBatch(ICamera2D camera)
        {
            var blendMode = StartingBlendMode == BlendMode.Add ? BlendMode.Alpha : BlendMode.Add;

            SpriteBatch.Begin(blendMode, camera);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <see cref="ParticleRendererBase"/> is in
        /// a valid state to draw.
        /// </summary>
        /// <returns>
        /// True if in a valid state to draw; otherwise false.
        /// </returns>
        protected override bool InValidRenderState()
        {
            return SpriteBatch != null;
        }

        /// <summary>
        /// When overridden in the derived class, handles the actual disposing.
        /// </summary>
        protected override void InternalDispose()
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles rendering the <see cref="ParticleEmitter"/>s.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> describing the world view.</param>
        /// <param name="additiveEmitters">The valid <see cref="ParticleEmitter"/>s where
        /// <see cref="BlendMode"/> is set to <see cref="BlendMode.Add"/>.</param>
        /// <param name="alphaEmitters">The valid <see cref="ParticleEmitter"/>s where
        /// <see cref="BlendMode"/> is set to <see cref="BlendMode.Alpha"/>.</param>
        protected override void InternalRenderEmitter(ICamera2D camera, IEnumerable<ParticleEmitter> additiveEmitters,
                                                      IEnumerable<ParticleEmitter> alphaEmitters)
        {
            IEnumerable<ParticleEmitter> first;
            IEnumerable<ParticleEmitter> second;

            // Figure out which collection to render first
            if (StartingBlendMode == BlendMode.Alpha)
            {
                first = alphaEmitters;
                second = additiveEmitters;
            }
            else
            {
                first = additiveEmitters;
                second = alphaEmitters;
            }

            if (!first.IsEmpty())
                RenderEmitters(first);

            if (!second.IsEmpty())
            {
                SpriteBatch.End();
                BeginSpriteBatch(camera);
                RenderEmitters(second);
                SpriteBatch.End();

                // Start the SpriteBatch again back as normal
                SpriteBatch.Begin(StartingBlendMode, camera);
            }
        }

        void RenderEmitter(ParticleEmitter emitter)
        {
            var origin = emitter.Sprite.Size / 2f;

            var particles = emitter.GetParticlesArray();
            for (var i = 0; i < emitter.ActiveParticles; i++)
            {
                var p = particles[i];
                emitter.Sprite.Draw(SpriteBatch, p.Position, p.Color, SpriteEffects.None, p.Rotation, origin, p.Scale);
            }
        }

        void RenderEmitters(IEnumerable<ParticleEmitter> emitters)
        {
            foreach (var emitter in emitters)
            {
                RenderEmitter(emitter);
            }
        }
    }
}