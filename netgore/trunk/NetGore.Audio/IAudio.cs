using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an unique audio sound.
    /// </summary>
    public interface IAudio
    {
        /// <summary>
        /// Gets the fully qualified name of the asset used by this <see cref="IAudio"/>. This is the name used
        /// when loading from the <see cref="ContentManager"/>. It cannot be used to reference this
        /// <see cref="IAudio"/> in the underlying <see cref="AudioManagerBase"/>.
        /// </summary>
        string AssetName { get; }

        /// <summary>
        /// Gets the <see cref="AudioManagerBase"/> that contains this <see cref="IAudio"/>.
        /// </summary>
        AudioManagerBase AudioManager { get; }

        /// <summary>
        /// Gets if only one instance of this <see cref="IAudio"/> may be playing at a time.
        /// </summary>
        bool IsSingleInstance { get; }

        /// <summary>
        /// Gets the name of the <see cref="IAudio"/>. This is the name used to reference this particular
        /// audio track when not referencing it by index.
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

        /// <summary>
        /// Updates the volume of the audio to match the volume specified by the <see cref="AudioManager"/>.
        /// </summary>
        void UpdateVolume();
    }
}