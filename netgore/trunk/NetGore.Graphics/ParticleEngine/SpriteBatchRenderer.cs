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
                _spriteBatch.Draw(emitter.ParticleTexture, p.Position, source, new Color(p.Color), p.Rotation, origin, scale, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();
        }

        protected override void InternalDispose()
        {
            if (!_spriteBatch.IsDisposed)
                _spriteBatch.Dispose();
        }
    }
}
