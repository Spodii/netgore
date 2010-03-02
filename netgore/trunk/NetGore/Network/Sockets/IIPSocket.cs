using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Handles events from the <see cref="IIPSocket"/>.
    /// </summary>
    /// <param name="socket">The <see cref="IIPSocket"/> the event came from.</param>
    public delegate void IIPSocketEventHandler(IIPSocket socket);

    /// <summary>
    /// Interface for a socket that abstracts the protocol implementation (ie TCP or UDP) into a single
    /// object that just sends and receives data.
    /// </summary>
    public interface IIPSocket : IDisposable
    {
        /// <summary>
        /// Gets the IPv4 address and port that this IIPSocket is connected to as a string. This string is formatted
        /// as "xxx.xxx.xxx.xxx:yyyyy". Trailing 0's from each segment are omitted.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the IPv4 address as an unsigned 32-bit integer.
        /// </summary>
        uint IP { get; }

        /// <summary>
        /// Gets if this <see cref="IIPSocket"/> is currently connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the maximum size of a message to the reliable channel.
        /// </summary>
        int MaxReliableMessageSize { get; }

        /// <summary>
        /// Gets the maximum size of a message to the unreliable channel.
        /// </summary>
        int MaxUnreliableMessageSize { get; }

        /// <summary>
        /// Gets the port as a 16-bit unsigned integer.
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the <see cref="IIPSocket"/> itself.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the time that this <see cref="IIPSocket"/> was created.
        /// </summary>
        int TimeCreated { get; }

        /// <summary>
        /// Gets the queue of received data.
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data.</returns>
        byte[][] GetRecvData();

        /// <summary>
        /// Notifies listeners when this <see cref="IIPSocket"/> has been disposed.
        /// </summary>
        event IIPSocketEventHandler Disposed;

        /// <summary>
        /// Sends data over a reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        void Send(BitStream data);

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        void Send(BitStream data, bool reliable);

        /// <summary>
        /// Sends data over the reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        void Send(byte[] data);

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        void Send(byte[] data, bool reliable);

        /// <summary>
        /// Sets the port used to communicate with the remote connection over an unreliable stream.
        /// </summary>
        /// <param name="port">Port for the unreliable stream.</param>
        void SetRemoteUnreliablePort(int port);
    }
}