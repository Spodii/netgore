using System.Linq;
using Lidgren.Network;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a connection made over IP that can be used to communicate with and control the connection with the
    /// other end.
    /// </summary>
    public interface IIPSocket : INetworkSender
    {
        /// <summary>
        /// Gets the IPv4 address and port that this IIPSocket is connected to as a string. This string is formatted
        /// as "xxx.xxx.xxx.xxx:yyyyy". Leading 0's from each segment are omitted.
        /// </summary>
        /// <example>27.0.1.160:12345</example>
        string Address { get; }

        /// <summary>
        /// Gets the average round-trip time of this connection in milliseconds.
        /// </summary>
        float AverageLatency { get; }

        /// <summary>
        /// Gets the IPv4 address as an unsigned 32-bit integer.
        /// </summary>
        uint IP { get; }

        /// <summary>
        /// Gets the port as a 16-bit unsigned integer.
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// Gets the status of the <see cref="IIPSocket"/>'s connection.
        /// </summary>
        NetConnectionStatus Status { get; }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the <see cref="IIPSocket"/> itself.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the time that this <see cref="IIPSocket"/> was created.
        /// </summary>
        TickCount TimeCreated { get; }

        /// <summary>
        /// Terminates this connection.
        /// </summary>
        /// <param name="reason">A string containing the reason why the connection was terminated. Can be null or empty, but
        /// recommended that a reason is provided.</param>
        void Disconnect(string reason);
    }
}