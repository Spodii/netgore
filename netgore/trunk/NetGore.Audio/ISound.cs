using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for a sound track.
    /// </summary>
    public interface ISound : IAudio
    {
        /// <summary>
        /// Gets the sound track index.
        /// </summary>
        SoundID Index { get; }

        /// <summary>
        /// Gets or sets the object that is listening to the sounds. If null, last known position of the
        /// listener will be used for the 3D sounds.
        /// </summary>
        IAudioEmitter Listener { get; set; }

        /// <summary>
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="position">The world position of the source of the sound.</param>
        void Play(Vector2 position);

        /// <summary>
        /// Plays a new instance of the sound.
        /// </summary>
        /// <param name="emitter">The object that is emitting the sound.</param>
        void Play(IAudioEmitter emitter);
    }
}