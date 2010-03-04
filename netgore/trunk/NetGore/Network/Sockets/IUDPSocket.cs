using System;
using System.Linq;
using System.Net;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a single, basic, thread-safe socket that uses UDP.
    /// </summary>
    public interface IUDPSocket : IDisposable
    {
        /// <summary>
        /// Notifies listeners when the <see cref="ITCPSocket"/> has been disposed.
        /// </summary>
        event UDPSocketEventHandler Disposed;

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets the maximum size of the data that can be sent in a single send.
        /// </summary>
        int MaxSendSize { get; }

        /// <summary>
        /// Binds the <see cref="IUDPSocket"/> to a random available port.
        /// </summary>
        /// <returns>Port that the <see cref="IUDPSocket"/> binded to.</returns>
        int Bind();

        /// <summary>
        /// Binds the <see cref="IUDPSocket"/> to the <paramref name="port"/>.
        /// </summary>
        /// <param name="port">Port to bind to.</param>
        /// <returns>Port that the <see cref="IUDPSocket"/> binded to.</returns>
        int Bind(int port);

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        void Connect(EndPoint host);

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="data">The initial data to send.</param>
        void Connect(EndPoint host, byte[] data);

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="ip">The IP address of the host to connect to.</param>
        /// <param name="port">The port of the host to connect to.</param>
        void Connect(string ip, int port);

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="ip">The IP address of the host to connect to.</param>
        /// <param name="port">The port of the host to connect to.</param>
        /// <param name="data">The initial data to send.</param>
        void Connect(string ip, int port, byte[] data);

        /// <summary>
        /// Gets the queued data received by this <see cref="IUDPSocket"/>.
        /// </summary>
        /// <returns>The queued data received by this <see cref="IUDPSocket"/>, or null if empty.</returns>
        AddressedPacket[] GetRecvData();

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="length">Length of the data to send in bytes.</param>
        /// <param name="endPoint">The <see cref="EndPoint"/> to send the data to.</param>
        void Send(byte[] data, int length, EndPoint endPoint);

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="endPoint">The <see cref="EndPoint"/> to send the data to.</param>
        void Send(byte[] data, EndPoint endPoint);
    }
}