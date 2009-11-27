using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Structure for forwarding received data from an <see cref="IIPSocket"/> through to the
    /// <see cref="SocketManager"/>.
    /// </summary>
    public struct SocketReceiveData
    {
        /// <summary>
        /// Queue of bytes, each entry containing an individual message received
        /// </summary>
        public readonly byte[][] Data;

        /// <summary>
        /// Socket the data came from.
        /// </summary>
        public readonly IIPSocket Socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketReceiveData"/> struct.
        /// </summary>
        /// <param name="socket">Socket the data came from.</param>
        /// <param name="data">Data received.</param>
        public SocketReceiveData(IIPSocket socket, byte[][] data)
        {
            Socket = socket;
            Data = data;
        }
    }
}