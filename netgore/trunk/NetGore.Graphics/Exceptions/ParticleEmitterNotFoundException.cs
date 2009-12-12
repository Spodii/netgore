using System;
using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An <see cref="Exception"/> for when trying to load a <see cref="ParticleEmitter"/> that does not exist.
    /// </summary>
    public sealed class ParticleEmitterNotFoundException : ParticleEmitterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterNotFoundException"/> class.
        /// </summary>
        /// <param name="emitterName">Name of the emitter.</param>
        public ParticleEmitterNotFoundException(string emitterName)
            : base(string.Format("Particle emitter `{0}` was not found.", emitterName))
        {
        }
    }
}