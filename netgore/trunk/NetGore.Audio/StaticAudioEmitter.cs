using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Audio
{
    /// <summary>
    /// A <see cref="IAudioEmitter"/> implementation for a stationary source.
    /// </summary>
    public class StaticAudioEmitter : IAudioEmitter
    {
        static readonly Stack<StaticAudioEmitter> _freeObjects = new Stack<StaticAudioEmitter>();
        static readonly object _freeObjectsSync = new object();

        Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticAudioEmitter"/> class.
        /// </summary>
        StaticAudioEmitter()
        {
        }

        /// <summary>
        /// Creates a <see cref="StaticAudioEmitter"/>.
        /// </summary>
        /// <param name="position">The world position of the <see cref="StaticAudioEmitter"/>.</param>
        /// <returns>A <see cref="StaticAudioEmitter"/>.</returns>
        public static StaticAudioEmitter Create(Vector2 position)
        {
            StaticAudioEmitter ret = null;

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
                        }
                    }
                }
            }

            // Create a new instance if we didn't get a free one
            if (ret == null)
                ret = new StaticAudioEmitter();

            ret.Initialize(position);

            return ret;
        }

        /// <summary>
        /// Initializes the <see cref="StaticAudioEmitter"/>.
        /// </summary>
        /// <param name="position">The position of the emitter.</param>
        void Initialize(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// Attempts to return an object if it is a <see cref="StaticAudioEmitter"/>. To best utilize the benefits
        /// of the <see cref="StaticAudioEmitter"/>, this should be called on any object when it is done being used
        /// if there is a chance it is a <see cref="StaticAudioEmitter"/>.
        /// </summary>
        /// <param name="obj">The objec to return.</param>
        public static void TryReturn(object obj)
        {
            StaticAudioEmitter casted = obj as StaticAudioEmitter;
            if (casted == null)
                return;

            lock (_freeObjectsSync)
            {
                _freeObjects.Push(casted);
            }
        }

        #region IAudioEmitter Members

        /// <summary>
        /// Gets the position of the audio emitter.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
        }

        #endregion
    }
}