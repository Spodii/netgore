using System;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a socket using TCP. Does not cover the connecting aspect of the socket,
    /// just the communication with the socket.
    /// </summary>
    public interface ITCPSocket : ISocketSender, IDisposable
    {
        /// <summary>
        /// Notifies the listeners when the TCPSocket has been disposed.
        /// </summary>
        event TCPSocketEvent OnDispose;

        /// <summary>
        /// Notifies the listeners when the socket has successfully sent data, and how much data was sent.
        /// Due to internal buffering, OnSend may not be raised for every send.
        /// </summary>
        event TCPSocketEvent<int> OnSend;

        /// <summary>
        /// Gets the remote endpoint address.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the ITCPSocket itself.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the queue of complete received data
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data</returns>
        byte[][] GetRecvData();
    }
}