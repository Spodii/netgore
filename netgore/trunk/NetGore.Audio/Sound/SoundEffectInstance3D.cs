using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

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

        SoundEffectInstance _sound;

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
        /// Gets the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public SoundEffectInstance Sound
        {
            get { return _sound; }
        }

        /// <summary>
        /// Creates a <see cref="SoundEffectInstance3D"/>.
        /// </summary>
        /// <returns>A <see cref="SoundEffectInstance3D"/>.</returns>
        public static SoundEffectInstance3D Create(SoundEffectInstance sound, IAudioEmitter emitter)
        {
            SoundEffectInstance3D ret = null;

            // Grab a free object if there is any
            if (_freeObjects.Count > 0)
            {
                lock (_freeObjectsSync)
                {
                    if (_freeObjects.Count > 0)
                    {
                        try
                        {
                            ret = _freeObjects.Pop();
                        }
                        catch (InvalidOperationException)
                        {
                            ret = null;
                        }
                    }
                }
            }

            // Create a new instance if we didn't get a free one
            if (ret == null)
                ret = new SoundEffectInstance3D();

            ret.Initialize(sound, emitter);

            return ret;
        }

        /// <summary>
        /// Initializes the <see cref="SoundEffectInstance3D"/>.
        /// </summary>
        void Initialize(SoundEffectInstance sound, IAudioEmitter emitter)
        {
            _sound = sound;
            _emitter = emitter;
        }

        /// <summary>
        /// Attempts to return an object if it is a <see cref="SoundEffectInstance3D"/>. To best utilize the benefits
        /// of the <see cref="SoundEffectInstance3D"/>, this should be called on any object when it is done being used
        /// if there is a chance it is a <see cref="SoundEffectInstance3D"/>.
        /// </summary>
        /// <param name="obj">The objec to return.</param>
        public static void TryReturn(object obj)
        {
            SoundEffectInstance3D casted = obj as SoundEffectInstance3D;
            if (casted == null)
                return;

            lock (_freeObjectsSync)
            {
                _freeObjects.Push(casted);
            }
        }
    }
}