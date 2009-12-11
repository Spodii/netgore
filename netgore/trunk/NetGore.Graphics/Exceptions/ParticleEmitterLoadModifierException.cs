using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Graphics
{
    /// <summary>
    /// An <see cref="Exception"/> for when a <see cref="ParticleEmitter"/> fails to load because the
    /// <see cref="ParticleModifier"/>'s type cannot be found or instantiated.
    /// </summary>
    public sealed class ParticleEmitterLoadModifierException : ParticleEmitterException
    {
        static string GetErrorMessage(string modifierType)
        {
            const string defaultValue = "<unknown>";

            if (string.IsNullOrEmpty(modifierType))
                modifierType = defaultValue;

            return string.Format("Failed to load the particle emitter because the particle modifier `{0}` could not be instantiated."
                                 + " Ensure that `{0}` is a valid class that can be instantiated.", modifierType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadModifierException"/> class.
        /// </summary>
        /// <param name="modifierType">Type name of the <see cref="ParticleModifier"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParticleEmitterLoadModifierException(string modifierType, Exception innerException)
            : base(GetErrorMessage(modifierType), innerException)
        {
        }
    }
}