using System;
using System.Linq;

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
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadParticleModifierException"/> class.
        /// </summary>
        /// <param name="modifierType">Type name of the <see cref="EmitterModifier"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParticleEmitterLoadEmitterModifierException(string modifierType, Exception innerException)
            : base(GetErrorMessage(modifierType), innerException)
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