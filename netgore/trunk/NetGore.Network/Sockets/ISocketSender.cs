using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a socket that can send data.
    /// </summary>
    public interface ISocketSender
    {
        /// <summary>
        /// Gets if the socket is currently connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Asynchronously sends data to the socket.
        /// </summary>
        /// <param name="sourceStream">BitStream containing the data to send.</param>
        void Send(BitStream sourceStream);
    }
}