using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a socket that abstracts the protocol implementation (ie TCP or UDP) into a single
    /// object that just sends and receives data.
    /// </summary>
    public interface IIPSocket : IDisposable
    {
        /// <summary>
        /// Gets the time that this IIPSocket was created.
        /// </summary>
        int TimeCreated { get; }

        /// <summary>
        /// Gets if this IIPSocket is currently connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the IP address string that this IIPSocket is connected to.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the IIPSocket itself.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the queue of received data.
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data.</returns>
        byte[][] GetRecvData();

        /// <summary>
        /// Sends data over a reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        void Send(BitStream data);

        /// <summary>
        /// Sets the port used to communicate with the remote connection over an unreliable stream.
        /// </summary>
        /// <param name="port">Port for the unreliable stream.</param>
        void SetRemoteUnreliablePort(int port);

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        void Send(BitStream data, bool reliable);
    }
}
