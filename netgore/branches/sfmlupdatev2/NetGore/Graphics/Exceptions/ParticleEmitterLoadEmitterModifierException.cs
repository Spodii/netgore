using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An <see cref="Exception"/> for when a <see cref="ParticleEmitter"/> fails to load because the
    /// <see cref="EmitterModifier"/>'s type cannot be found or instantiated.
    /// </summary>
    [Serializable]
    public sealed class ParticleEmitterLoadEmitterModifierException : ParticleEmitterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterModifierException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParticleEmitterLoadEmitterModifierException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterModifierException"/> class.
        /// </summary>
        public ParticleEmitterLoadEmitterModifierException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadParticleModifierException"/> class.
        /// </summary>
        /// <param name="modifierType">Type name of the <see cref="EmitterModifier"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParticleEmitterLoadEmitterModifierException(string modifierType, Exception innerException)
            : base(GetErrorMessage(modifierType), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterModifierException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        ParticleEmitterLoadEmitterModifierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        static string GetErrorMessage(string modifierType)
        {
            const string defaultValue = "<unknown>";

            if (string.IsNullOrEmpty(modifierType))
                modifierType = defaultValue;

            return
                string.Format(
                    "Failed to load the particle emitter because the emitter modifier `{0}` could not be instantiated." +
                    " Ensure that `{0}` is a valid class that can be instantiated.", modifierType);
        }
    }
}