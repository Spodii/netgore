using System;
using System.Linq;
using Lidgren.Network;

namespace NetGore.Network
{
    /// <summary>
    /// <see cref="EventArgs"/> for when the status of a <see cref="IClientSocketManager"/> changes.
    /// </summary>
    public class ClientSocketManagerStatusChangedEventArgs : EventArgs
    {
        readonly NetConnectionStatus _newStatus;
        readonly string _reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSocketManagerStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newStatus">The new <see cref="NetConnectionStatus"/>.</param>
        /// <param name="reason">The reason for the status change.</param>
        public ClientSocketManagerStatusChangedEventArgs(NetConnectionStatus newStatus, string reason)
        {
            _newStatus = newStatus;
            _reason = reason;
        }

        /// <summary>
        /// Gets the new <see cref="NetConnectionStatus"/>.
        /// </summary>
        public NetConnectionStatus NewStatus
        {
            get { return _newStatus; }
        }

        /// <summary>
        /// Gets the reason for the status change.
        /// </summary>
        public string Reason
        {
            get { return _reason; }
        }
    }
}