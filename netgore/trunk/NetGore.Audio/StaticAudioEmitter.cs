using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Audio
{
    /// <summary>
    /// A <see cref="IAudioEmitter"/> implementation for a stationary source.
    /// </summary>
    class StaticAudioEmitter : IAudioEmitter
    {
        Vector2 _position;

        /// <summary>
        /// Initializes the <see cref="StaticAudioEmitter"/>.
        /// </summary>
        /// <param name="position">The position of the emitter.</param>
        public void Initialize(Vector2 position)
        {
            _position = position;
        }

        #region IAudioEmitter Members

        /// <summary>
        /// Gets the position of the audio emitter.
        /// </summary>
        /// <value></value>
        public Vector2 Position
        {
            get { return _position; }
        }

        #endregion
    }
}