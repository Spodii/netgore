using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// BitStream seek origin.
    /// </summary>
    public enum BitStreamSeekOrigin : byte
    {
        /// <summary>
        /// Specifies the beginning of the stream.
        /// </summary>
        Beginning,

        /// <summary>
        /// Specifies the current position of the stream.
        /// </summary>
        Current
    }
}