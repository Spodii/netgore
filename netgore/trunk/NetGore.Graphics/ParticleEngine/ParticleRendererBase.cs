using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Simple helper base class for a <see cref="IParticleRenderer"/>.
    /// </summary>
    public abstract class ParticleRendererBase : IParticleRenderer
    {
        SpriteBlendMode _blendMode = SpriteBlendMode.Additive;
        bool _isDisposed = false;

        /// <summary>
        /// Gets if this <see cref="IParticleRenderer"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SpriteBlendMode"/> to use when rendering the particles.
        /// </summary>
        public SpriteBlendMode BlendMode
        {
            get { return _blendMode; }
            set { _blendMode = value; }
        }

        /// <summary>
        /// When overridden in the derived class, handles the actual disposing.
        /// </summary>
        protected abstract void InternalDispose();

        /// <summary>
        /// When overridden in the derived class, handles rendering the <paramref name="emitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        protected abstract void InternalRenderEmitter(ParticleEmitter emitter);

        /// <summary>
        /// Renders a <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        public void RenderEmitter(ParticleEmitter emitter)
        {
            if (emitter.ParticleTexture == null || emitter.ActiveParticles <= 0)
                return;

            InternalRenderEmitter(emitter);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            InternalDispose();
        }
    }
}
