using System.Linq;
using SFML.Graphics;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an object that manages all of the audio.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Gets or sets the global volume of all audio. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float GlobalVolume { get; set; }

        /// <summary>
        /// Gets or sets the world position of the audio listener. This is almost always the user's character position.
        /// Only valid for when using 3D audio.
        /// </summary>
        Vector2 ListenerPosition { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicManager"/> instance.
        /// </summary>
        IMusicManager MusicManager { get; }

        /// <summary>
        /// Gets the <see cref="ISoundManager"/> instance.
        /// </summary>
        ISoundManager SoundManager { get; }

        /// <summary>
        /// Stops all forms of audio.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates all the audio.
        /// </summary>
        void Update();
    }
}