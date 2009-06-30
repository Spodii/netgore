using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for an object that can process messages received from a socket.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Handles received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="rec">SocketReceiveData to process.</param>
        void Process(SocketReceiveData rec);

        /// <summary>
        /// Handles received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="socket">Socket the data came from.</param>
        /// <param name="data">Data to process.</param>
        void Process(IIPSocket socket, byte[] data);

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="recvData">List of SocketReceiveData to process.</param>
        void Process(IEnumerable<SocketReceiveData> recvData);
    }
}