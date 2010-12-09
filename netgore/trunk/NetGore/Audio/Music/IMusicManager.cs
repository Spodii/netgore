using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an object that manages the game music.
    /// </summary>
    public interface IMusicManager : IDisposable
    {
        /// <summary>
        /// Gets or sets if music will loop. Default is true.
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/>s for all music tracks.
        /// </summary>
        IEnumerable<IMusicInfo> MusicInfos { get; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for the music track currently playing. Will be null if no music
        /// is playing.
        /// </summary>
        IMusicInfo Playing { get; }

        /// <summary>
        /// Gets or sets the global volume of all music. This value must be in a range of 0 to 100, where 0 is
        /// silence and 100 is the full volume. If a value is specified that does not fall into this range, it will be
        /// altered to fit this range. Default is 100.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="id">The id of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="id"/>, or null if the value
        /// was invalid.</returns>
        IMusicInfo GetMusicInfo(MusicID id);

        /// <summary>
        /// Gets the <see cref="IMusicInfo"/> for a music track.
        /// </summary>
        /// <param name="name">The name of the <see cref="IMusicInfo"/> to get.</param>
        /// <returns>The <see cref="IMusicInfo"/> for the given <paramref name="name"/>, or null if the value
        /// was invalid.</returns>
        IMusicInfo GetMusicInfo(string name);

        /// <summary>
        /// Pauses the currently playing music, if any music is playing.
        /// </summary>
        void Pause();

        /// <summary>
        /// Plays a music track by the given <see cref="MusicID"/>.
        /// </summary>
        /// <param name="id">The ID of the music to play.</param>
        /// <returns>
        /// True if the music played successfully; otherwise false.
        /// </returns>
        bool Play(MusicID id);

        /// <summary>
        /// Reloads the music information.
        /// </summary>
        void ReloadData();

        /// <summary>
        /// Reloads the music information.
        /// </summary>
        /// <param name="values">All of the <see cref="IMusicInfo"/>s to load.</param>
        void ReloadData(IEnumerable<IMusicInfo> values);

        /// <summary>
        /// Resumes the currently paused music, if there is any paused music.
        /// </summary>
        void Resume();

        /// <summary>
        /// Saves the <see cref="IMusicInfo"/>s in this <see cref="IMusicManager"/> to file.
        /// </summary>
        void Save();

        /// <summary>
        /// Stops the currently playing music.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates the <see cref="IMusicManager"/>.
        /// </summary>
        void Update();
    }
}