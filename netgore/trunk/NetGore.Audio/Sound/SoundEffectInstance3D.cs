using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// An object containing a <see cref="SoundEffectInstance"/> and the <see cref="IAudioEmitter"/> emitting
    /// the sound.
    /// </summary>
    public class SoundEffectInstance3D
    {
        static readonly Stack<SoundEffectInstance3D> _freeObjects = new Stack<SoundEffectInstance3D>();
        static readonly object _freeObjectsSync = new object();
        IAudioEmitter _emitter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectInstance3D"/> class.
        /// </summary>
        SoundEffectInstance3D()
        {
        }

        /// <summary>
        /// Gets the <see cref="IAudioEmitter"/> emitting the <see cref="Sound"/>.
        /// </summary>
        public IAudioEmitter Emitter
        {
            get { return _emitter; }
        }

        /// <summary>
        /// Attempts to return an object if it is a <see cref="SoundEffectInstance3D"/>. To best utilize the benefits
        /// of the <see cref="SoundEffectInstance3D"/>, this should be called on any object when it is done being used
        /// if there is a chance it is a <see cref="SoundEffectInstance3D"/>.
        /// </summary>
        /// <param name="obj">The objec to return.</param>
        public static void TryReturn(object obj)
        {
            // TODO: ## Add back in sounds
        }
    }
}