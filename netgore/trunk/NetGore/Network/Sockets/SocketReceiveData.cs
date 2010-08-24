using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Structure for forwarding received data from an <see cref="IIPSocket"/> through to the
    /// <see cref="SocketManager"/>.
    /// </summary>
    public struct SocketReceiveData
    {
        readonly byte[][] _data;
        readonly IIPSocket _socket;

        /// <summary>
        /// Gets the received socket data. Each top-level array element is a full message, and each second-level
        /// array element is the individual bytes that make up the message.
        /// </summary>
        public byte[][] Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Gets the <see cref="IIPSocket"/> that this data came from.
        /// </summary>
        public IIPSocket Socket
        {
            get { return _socket; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketReceiveData"/> struct.
        /// </summary>
        /// <param name="socket">Socket the data came from.</param>
        /// <param name="data">Data received.</param>
        public SocketReceiveData(IIPSocket socket, byte[][] data)
        {
            _socket = socket;
            _data = data;
        }
    }
}