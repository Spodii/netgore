using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface containing information on music.
    /// </summary>
    public interface IMusicInfo
    {
        /// <summary>
        /// Gets the unique ID of the music.
        /// </summary>
        MusicID ID { get; }

        /// <summary>
        /// Gets the unique name of the music.
        /// </summary>
        string Name { get; }
    }
}