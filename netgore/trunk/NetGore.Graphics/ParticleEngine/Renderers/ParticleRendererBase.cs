using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Simple helper base class for a <see cref="IParticleRenderer"/>.
    /// </summary>
    public abstract class ParticleRendererBase : IParticleRenderer
    {
        bool _isDisposed = false;

        /// <summary>
        /// When overridden in the derived class, handles the actual disposing.
        /// </summary>
        protected abstract void InternalDispose();

        /// <summary>
        /// When overridden in the derived class, handles rendering the <paramref name="emitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        protected abstract void InternalRenderEmitter(ParticleEmitter emitter);

        #region IParticleRenderer Members

        /// <summary>
        /// Gets if this <see cref="IParticleRenderer"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Renders a <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        public void RenderEmitter(ParticleEmitter emitter)
        {
            if (emitter.Sprite == null || emitter.ActiveParticles <= 0 || emitter.BlendMode == SpriteBlendMode.None)
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

        #endregion
    }
}