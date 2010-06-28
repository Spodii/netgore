using System.Collections.Generic;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for an object that can process messages received from a socket.
    /// </summary>
    public interface ISocketReceiveDataProcessor
    {
        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="recvData">The <see cref="SocketReceiveData"/>s to process.</param>
        void Process(IEnumerable<SocketReceiveData> recvData);
    }
}