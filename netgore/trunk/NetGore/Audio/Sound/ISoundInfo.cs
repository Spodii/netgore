using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface containing information on sounds.
    /// </summary>
    public interface ISoundInfo
    {
        /// <summary>
        /// Gets the unique ID of the sound.
        /// </summary>
        SoundID ID { get; }

        /// <summary>
        /// Gets the unique name of the sound.
        /// </summary>
        string Name { get; }
    }
}