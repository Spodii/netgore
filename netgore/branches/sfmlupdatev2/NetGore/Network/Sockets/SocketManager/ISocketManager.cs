using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a general purpose socket manager. Intended to be used with the more specific interfaces
    /// <see cref="IClientSocketManager"/> and <see cref="IServerSocketManager"/>.
    /// </summary>
    public interface ISocketManager
    {
        /// <summary>
        /// Handles processing of the underlying connection(s) and promoting data to the upper layer to be handled
        /// by the application. Should be called once per frame.
        /// </summary>
        void Heartbeat();
    }
}