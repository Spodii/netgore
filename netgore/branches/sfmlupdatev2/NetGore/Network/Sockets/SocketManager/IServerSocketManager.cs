using System.Collections.Generic;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a manager of server sockets. Enforces the idea that connections are made to it instead of it making
    /// the connections, listens for connections, and multiple connections can be made to it.
    /// Implementations of this interface are guaranteed to be thread-safe.
    /// </summary>
    public interface IServerSocketManager : ISocketManager
    {
        /// <summary>
        /// Gets the live connections to the server.
        /// </summary>
        IEnumerable<IIPSocket> Connections { get; }

        /// <summary>
        /// Gets the number of live connections to the server.
        /// </summary>
        int ConnectionsCount { get; }

        /// <summary>
        /// Disconnects all active connections and rejects incoming connections.
        /// </summary>
        void Shutdown();
    }
}