using System;
using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Interface for a class that can render particle effects.
    /// </summary>
    public interface IParticleRenderer : IDisposable
    {
        /// <summary>
        /// Gets if this <see cref="IParticleRenderer"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Renders a <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to render.</param>
        void RenderEmitter(ParticleEmitter emitter);
    }
}