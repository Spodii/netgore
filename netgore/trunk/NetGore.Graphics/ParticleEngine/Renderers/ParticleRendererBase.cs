using System.Collections.Generic;
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
        /// When overridden in the derived class, handles rendering the <see cref="ParticleEmitter"/>s.
        /// </summary>
        /// <param name="camera">the <see cref="Camera2D"/> describing the world view.</param>
        /// <param name="additiveEmitters">The valid <see cref="ParticleEmitter"/>s where
        /// <see cref="SpriteBlendMode"/> is set to <see cref="SpriteBlendMode.Additive"/>.</param>
        /// <param name="alphaEmitters">The valid <see cref="ParticleEmitter"/>s where
        /// <see cref="SpriteBlendMode"/> is set to <see cref="SpriteBlendMode.AlphaBlend"/>.</param>
        protected abstract void InternalRenderEmitter(Camera2D camera, IEnumerable<ParticleEmitter> additiveEmitters,
                                                      IEnumerable<ParticleEmitter> alphaEmitters);

        /// <summary>
        /// When overridden in the derived class, gets if the <see cref="ParticleRendererBase"/> is in
        /// a valid state to draw.
        /// </summary>
        /// <returns>True if in a valid state to draw; otherwise false.</returns>
        protected virtual bool InValidRenderState()
        {
            return true;
        }

        #region IParticleRenderer Members

        /// <summary>
        /// Gets if this <see cref="IParticleRenderer"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Renders the <see cref="ParticleEmitter"/>s.
        /// </summary>
        /// <param name="camera">the <see cref="Camera2D"/> describing the world view.</param>
        /// <param name="emitters">The <see cref="ParticleEmitter"/>s to render.</param>
        public void Draw(Camera2D camera, IEnumerable<ParticleEmitter> emitters)
        {
            if (emitters == null)
                return;

            if (!InValidRenderState())
                return;

            var validEmitters = emitters.Where(x => x.Sprite != null && x.ActiveParticles > 0);
            var additiveEmitters = validEmitters.Where(x => x.BlendMode == SpriteBlendMode.Additive);
            var alphaEmitters = validEmitters.Where(x => x.BlendMode == SpriteBlendMode.AlphaBlend);

            InternalRenderEmitter(camera, additiveEmitters, alphaEmitters);
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