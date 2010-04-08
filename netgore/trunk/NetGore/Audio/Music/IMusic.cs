using System.Linq;
using SFML.Audio;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for a music track.
    /// </summary>
    public interface IMusic : IAudio
    {
        /// <summary>
        /// Gets the music track index.
        /// </summary>
        MusicID Index { get; }

        /// <summary>
        /// Gets the current state of the music track.
        /// </summary>
        SoundStatus State { get; }

        /// <summary>
        /// Pauses the music track if it is playing.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the music track if it was paused.
        /// </summary>
        void Resume();
    }
}