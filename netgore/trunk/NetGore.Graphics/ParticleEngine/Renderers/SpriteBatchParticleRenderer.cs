using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="IParticleRenderer"/> that draws with a <see cref="SpriteBatch"/>.
    /// </summary>
    public sealed class SpriteBatchParticleRenderer : ParticleRendererBase
    {
        const SpriteSortMode _spriteBatchSortMode = SpriteSortMode.Deferred;
        const SaveStateMode _spriteBatchStateMode = SaveStateMode.None;
        SpriteBlendMode _startingBlendMode = SpriteBlendMode.AlphaBlend;

        /// <summary>
        /// Gets or sets the <see cref="ISpriteBatch"/> used to draw.
        /// </summary>
        public ISpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SpriteBlendMode"/> the <see cref="SpriteBatch"/> is using
        /// when the <see cref="SpriteBatchRenderer"/> is called to render.
        /// </summary>
        public SpriteBlendMode StartingBlendMode
        {
            get { return _startingBlendMode; }
            set
            {
                if (value == SpriteBlendMode.None)
                    throw new ArgumentException("value cannot be SpriteBlendMode.None.");

                _startingBlendMode = value;
            }
        }

        void BeginSpriteBatch(ICamera2D camera)
        {
            var blendMode = StartingBlendMode == SpriteBlendMode.Additive ? SpriteBlendMode.AlphaBlend : SpriteBlendMode.Additive;
            var matrix = camera.Matrix;

            SpriteBatch.Begin(blendMode, _spriteBatchSortMode, _spriteBatchStateMode, matrix);
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
        /// <see cref="SpriteBlendMode"/> is set to <see cref="SpriteBlendMode.Additive"/>.</param>
        /// <param name="alphaEmitters">The valid <see cref="ParticleEmitter"/>s where
        /// <see cref="SpriteBlendMode"/> is set to <see cref="SpriteBlendMode.AlphaBlend"/>.</param>
        protected override void InternalRenderEmitter(ICamera2D camera, IEnumerable<ParticleEmitter> additiveEmitters,
                                                      IEnumerable<ParticleEmitter> alphaEmitters)
        {
            IEnumerable<ParticleEmitter> first;
            IEnumerable<ParticleEmitter> second;

            // Figure out which collection to render first
            if (StartingBlendMode == SpriteBlendMode.AlphaBlend)
            {
                first = alphaEmitters;
                second = additiveEmitters;
            }
            else
            {
                first = additiveEmitters;
                second = alphaEmitters;
            }

            if (first.Count() > 0)
                RenderEmitters(first);

            if (second.Count() > 0)
            {
                SpriteBatch.End();
                BeginSpriteBatch(camera);
                RenderEmitters(second);
                SpriteBatch.End();

                // Start the SpriteBatch again back as normal
                SpriteBatch.Begin(StartingBlendMode, _spriteBatchSortMode, _spriteBatchStateMode, camera.Matrix);
            }
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

        void RenderEmitter(ParticleEmitter emitter)
        {
            Vector2 origin = emitter.Sprite.Size / 2f;

            var particles = emitter.GetParticlesArray();
            for (int i = 0; i < emitter.ActiveParticles; i++)
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