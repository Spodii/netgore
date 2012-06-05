using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Extension methods for the <see cref="IMusicManager"/>. 
    /// </summary>
    public static class IMusicManagerExtensions
    {
        /// <summary>
        /// Plays a music track.
        /// </summary>
        /// <param name="musicManager">The music manager.</param>
        /// <param name="name">The name of the music to play.</param>
        /// <returns>True if the music track was successfully played; otherwise false.</returns>
        public static bool Play(this IMusicManager musicManager, string name)
        {
            var info = musicManager.GetMusicInfo(name);
            if (info == null)
                return false;

            return musicManager.Play(info.ID);
        }

        /// <summary>
        /// Plays a music track.
        /// </summary>
        /// <param name="musicManager">The music manager.</param>
        /// <param name="info">The <see cref="IMusicInfo"/> to play.</param>
        /// <returns>
        /// True if the music played successfully; otherwise false.
        /// </returns>
        public static bool Play(this IMusicManager musicManager, IMusicInfo info)
        {
            return musicManager.Play(info.ID);
        }
    }
}