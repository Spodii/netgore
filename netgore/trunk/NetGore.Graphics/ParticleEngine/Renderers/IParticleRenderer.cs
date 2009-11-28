using System;
using System.Collections.Generic;
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
        /// Renders the <see cref="ParticleEmitter"/>s.
        /// </summary>
        /// <param name="camera">the <see cref="Camera2D"/> describing the world view.</param>
        /// <param name="emitters">The <see cref="ParticleEmitter"/>s to render.</param>
        void Draw(Camera2D camera, IEnumerable<ParticleEmitter> emitters);
    }
}