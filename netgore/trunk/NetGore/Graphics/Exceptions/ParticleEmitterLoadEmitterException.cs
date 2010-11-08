using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An <see cref="Exception"/> for when a <see cref="ParticleEmitter"/> fails to load because the
    /// <see cref="ParticleEmitter"/>'s type cannot be found or instantiated.
    /// </summary>
    [Serializable]
    public sealed class ParticleEmitterLoadEmitterException : ParticleEmitterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParticleEmitterLoadEmitterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterException"/> class.
        /// </summary>
        public ParticleEmitterLoadEmitterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterException"/> class.
        /// </summary>
        /// <param name="emitterType">Type name of the <see cref="ParticleEmitter"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParticleEmitterLoadEmitterException(string emitterType, Exception innerException)
            : base(GetErrorMessage(emitterType), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        ParticleEmitterLoadEmitterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        static string GetErrorMessage(string emitterType)
        {
            const string defaultValue = "<unknown>";

            if (string.IsNullOrEmpty(emitterType))
                emitterType = defaultValue;

            return
                string.Format(
                    "Failed to load emitter of type `{0}` - the emitter's type could not be instantiated." +
                    " Ensure that `{0}` is a valid class that can be instantiated.", emitterType);
        }
    }
}