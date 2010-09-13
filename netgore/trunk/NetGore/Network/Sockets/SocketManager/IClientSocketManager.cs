using Lidgren.Network;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for handling when the status changes in a <see cref="IClientSocketManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IClientSocketManager"/> the event came from.</param>
    /// <param name="newStatus">The new <see cref="NetConnectionStatus"/>.</param>
    /// <param name="reason">The reason for the status change.</param>
    public delegate void ClientSocketManagerStatusChangedEventHandler(IClientSocketManager sender, NetConnectionStatus newStatus, string reason);

    /// <summary>
    /// Interface for a manager of client sockets. Enforces the idea of outbound connections only, does not listen, and
    /// only allows one connect to be made at a time.
    /// Implementations of this interface are guaranteed to be thread-safe.
    /// </summary>
    public interface IClientSocketManager : ISocketManager
    {
        /// <summary>
        /// Gets the <see cref="IIPSocket"/> used to communicate with the other end of the connection. Will be null if no
        /// connection has been established.
        /// </summary>
        IIPSocket RemoteSocket { get; }

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
        /// Notifies listeners when the status of the connection has changed.
        /// </summary>
        event ClientSocketManagerStatusChangedEventHandler StatusChanged;

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