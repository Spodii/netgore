using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an unique audio sound.
    /// </summary>
    public interface IAudio
    {
        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        bool IsSingleInstance { get; }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the unique index of the <see cref="IAudio"/>.
        /// </summary>
        /// <returns>The unique index.</returns>
        int GetIndex();

        /// <summary>
        /// Plays the audio track. If <see cref="IsSingleInstance"/> is true, this will play the track if it is not
        /// already playing. Otherwise, this will spawn a new instance of the sound.
        /// </summary>
        void Play();

        /// <summary>
        /// Stops the audio track. If <see cref="IsSingleInstance"/> is true, this will stop the track. Otherwise,
        /// every instance of the track will be stopped.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates the audio.
        /// </summary>
        void Update();
    }
}