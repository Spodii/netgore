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
        /// Gets the maximum size of the data that can be sent in a single send.
        /// </summary>
        int MaxSendSize { get; }

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