using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an object that manages game sounds.
    /// </summary>
    public interface ISoundManager
    {
        /// <summary>
        /// Gets the <see cref="ISoundInfo"/>s for all sounds.
        /// </summary>
        IEnumerable<ISoundInfo> SoundInfos { get; }

        /// <summary>
        /// Gets or sets the global volume of all sounds. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="id">The id of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        ISoundInfo GetSoundInfo(SoundID id);

        /// <summary>
        /// Gets the <see cref="ISoundInfo"/> for a sound.
        /// </summary>
        /// <param name="name">The name of the <see cref="ISoundInfo"/> to get.</param>
        /// <returns>The <see cref="ISoundInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        ISoundInfo GetSoundInfo(string name);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The world position that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id, Vector2 source);

        /// <summary>
        /// Plays a sound by the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sound to play.</param>
        /// <param name="source">The <see cref="IAudioEmitter"/> that the sound is coming from.</param>
        /// <returns>
        /// True if the sound played successfully; otherwise false.
        /// </returns>
        bool Play(SoundID id, IAudioEmitter source);

        /// <summary>
        /// Reloads the sound information.
        /// </summary>
        void ReloadData();

        /// <summary>
        /// Reloads the sound information.
        /// </summary>
        /// <param name="values">All of the <see cref="ISoundInfo"/>s to load.</param>
        void ReloadData(IEnumerable<ISoundInfo> values);

        /// <summary>
        /// Saves the <see cref="ISoundInfo"/>s in this <see cref="ISoundManager"/> to file.
        /// </summary>
        void Save();

        /// <summary>
        /// Stops all sounds.
        /// </summary>
        void Stop();

        /// <summary>
        /// Stops all instances of a sound with the given <see cref="SoundID"/>.
        /// </summary>
        /// <param name="id">The ID of the sounds to stop.</param>
        void Stop(SoundID id);

        /// <summary>
        /// Stops all 2D sounds. Any 3D sounds playing will continue to play.
        /// </summary>
        void Stop2D();

        /// <summary>
        /// Stops all 3D sounds. Any 2D sounds playing will continue to play.
        /// </summary>
        void Stop3D();

        /// <summary>
        /// Updates the <see cref="ISoundManager"/>.
        /// </summary>
        void Update();
    }
}