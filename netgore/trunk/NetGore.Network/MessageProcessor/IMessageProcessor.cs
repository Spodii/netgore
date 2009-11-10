using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for an object that can process messages received from a socket.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="recvData">List of SocketReceiveData to process.</param>
        void Process(IEnumerable<SocketReceiveData> recvData);
    }
}