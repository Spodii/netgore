using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a socket using TCP. Does not cover the connecting aspect of the socket,
    /// just the communication with the socket.
    /// </summary>
    public interface ITCPSocket : ISocketSender, IDisposable
    {
        /// <summary>
        /// Notifies listeners when the socket has successfully sent data, and how much data was sent.
        /// Due to internal buffering, this event will likely not be raised for every single individual send call made.
        /// </summary>
        event TCPSocketEventHandler<int> DataSent;

        /// <summary>
        /// Notifies listeners when the <see cref="ITCPSocket"/> has been disposed.
        /// </summary>
        event TCPSocketEventHandler Disposed;

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
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets the maximum size of the data that can be sent in a single send.
        /// </summary>
        int MaxSendSize { get; }

        /// <summary>
        /// Gets the port as a 16-bit unsigned integer.
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the <see cref="ITCPSocket"/> itself.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the <see cref="Environment.TickCount"/> of when this <see cref="ITCPSocket"/> was created.
        /// </summary>
        int TimeCreated { get; }

        /// <summary>
        /// Gets the queue of complete received data.
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data.</returns>
        byte[][] GetRecvData();
    }
}