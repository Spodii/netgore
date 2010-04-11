using System.Linq;
using SFML.Graphics;

namespace NetGore.Audio
{
    /// <summary>
    /// Interface for an object that can be the source of audio.
    /// </summary>
    public interface IAudioEmitter
    {
        /// <summary>
        /// Gets the position of the audio emitter.
        /// </summary>
        Vector2 Position { get; }
    }
}