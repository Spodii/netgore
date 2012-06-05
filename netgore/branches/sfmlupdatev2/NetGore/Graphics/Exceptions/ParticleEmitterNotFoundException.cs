using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An <see cref="Exception"/> for when trying to load a <see cref="ParticleEmitter"/> that does not exist.
    /// </summary>
    [Serializable]
    public sealed class ParticleEmitterNotFoundException : ParticleEmitterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ParticleEmitterNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterNotFoundException"/> class.
        /// </summary>
        public ParticleEmitterNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterNotFoundException"/> class.
        /// </summary>
        /// <param name="emitterName">Name of the emitter.</param>
        public ParticleEmitterNotFoundException(string emitterName)
            : base(string.Format("Particle emitter `{0}` was not found.", emitterName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        ParticleEmitterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}