using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An <see cref="Exception"/> for when a <see cref="ParticleEmitter"/> fails to load because the
    /// <see cref="ParticleEmitter"/>'s type cannot be found or instantiated.
    /// </summary>
    public sealed class ParticleEmitterLoadEmitterException : ParticleEmitterException
    {
        static string GetErrorMessage(string emitterType)
        {
            const string defaultValue = "<unknown>";

            if (string.IsNullOrEmpty(emitterType))
                emitterType = defaultValue;

            return string.Format("Failed to load emitter of type `{0}` - the emitter's type could not be instantiated." 
                + " Ensure that `{0}` is a valid class that can be instantiated.", emitterType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterLoadEmitterException"/> class.
        /// </summary>
        /// <param name="emitterType">Type name of the <see cref="ParticleEmitter"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParticleEmitterLoadEmitterException(string emitterType,
            Exception innerException)
            : base(GetErrorMessage(emitterType), innerException)
        {
        }
    }
}
