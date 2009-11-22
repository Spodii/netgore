using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    public sealed class SpriteBatchRenderer : ParticleRendererBase
    {
        SpriteBatch _spriteBatch;

        public SpriteBatchRenderer(GraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <summary>
        /// When overridden in the derived class, handles rendering the <paramref name="emitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        protected override void InternalRenderEmitter(ParticleEmitter emitter)
        {
            Rectangle source = new Rectangle(0, 0, emitter.ParticleTexture.Width, emitter.ParticleTexture.Height);
            Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

            _spriteBatch.Begin(BlendMode, SpriteSortMode.Deferred, SaveStateMode.None);

            var particles = emitter.GetParticlesArray();
            for (int i = 0; i < emitter.ActiveParticles; i++)
            {
                var p = particles[i];
                float scale = p.Scale / emitter.ParticleTexture.Width;
                _spriteBatch.Draw(emitter.ParticleTexture, p.Position, source, p.Color, p.Rotation, origin, scale, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// When overridden in the derived class, handles the actual disposing.
        /// </summary>
        protected override void InternalDispose()
        {
            if (!_spriteBatch.IsDisposed)
                _spriteBatch.Dispose();
        }
    }
}
