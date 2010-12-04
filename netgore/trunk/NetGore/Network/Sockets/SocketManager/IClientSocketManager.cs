using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a manager of client sockets. Enforces the idea of outbound connections only, does not listen, and
    /// only allows one connect to be made at a time.
    /// Implementations of this interface are guaranteed to be thread-safe.
    /// </summary>
    public interface IClientSocketManager : ISocketManager
    {
        /// <summary>
        /// Notifies listeners when the status of the connection has changed.
        /// </summary>
        event TypedEventHandler<IClientSocketManager, ClientSocketManagerStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Gets if it was us, the client, who terminated the connection to the server. This will only be true when
        /// the client is in the process of disconnecting or has disconnected, and will always be false when establishing
        /// a connection or connected.
        /// </summary>
        bool ClientDisconnected { get; }

        /// <summary>
        /// Gets if we are currently connected to the server.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the <see cref="IIPSocket"/> used to communicate with the other end of the connection. Will be null if no
        /// connection has been established.
        /// </summary>
        IIPSocket RemoteSocket { get; }

        /// <summary>
        /// Attempts to connects to the server.
        /// </summary>
        /// <param name="host">The server address.</param>
        /// <param name="port">The server port.</param>
        /// <param name="approvalMessage">The initial message to send to the server when making the connection. This can
        /// be utilized by the server to provide additional information for determining whether or not to accept the connection.
        /// Or, can be null to send nothing.</param>
        /// <returns>
        /// True if the connection attempt was successfully started. Does not mean that the connection was established, but
        /// just that it can be attempted. Will return false if a connection is already established or being established.
        /// </returns>
        bool Connect(string host, int port, BitStream approvalMessage = null);

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        void Disconnect();
    }
}