using System.Linq;
using Lidgren.Network;

namespace NetGore.Network
{
    /// <summary>
    /// Helper methods for sockets.
    /// </summary>
    static class SocketHelper
    {
        /// <summary>
        /// Gets a <see cref="NetOutgoingMessage"/>.
        /// </summary>
        /// <param name="netPeer">The <see cref="NetPeer"/> instance that the message is to be used on.</param>
        /// <param name="size">The minimum initial size of the <see cref="NetOutgoingMessage"/> in bytes.</param>
        /// <returns>
        /// The <see cref="NetOutgoingMessage"/> instance to use.
        /// </returns>
        public static NetOutgoingMessage GetNetOutgoingMessage(NetPeer netPeer, int size)
        {
            // Round up to the next power-of-2 size (if not already there) to make allocations a bit more consistency-sized
            // and grabbing from the internal pool will hopefully be faster, even if it does mean over-allocating a bit more.
            // Also, never use less than 16 bytes since there is not much point in cluttering things up with tiny buffers.
            if (size < 16)
                size = 16;
            else
                size = BitOps.NextPowerOf2(size);

            // Grab the NetOutgoingMessage from the internal buffer
            return netPeer.CreateMessage(size);
        }
    }
}