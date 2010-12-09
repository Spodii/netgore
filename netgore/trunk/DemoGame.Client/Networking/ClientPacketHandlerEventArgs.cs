using System;
using System.Linq;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// <see cref="EventArgs"/> for events on the <see cref="ClientPacketHandler"/>.
    /// </summary>
    public class ClientPacketHandlerEventArgs : EventArgs
    {
        readonly IIPSocket _socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketHandlerEventArgs"/> class.
        /// </summary>
        /// <param name="socket">The <see cref="IIPSocket"/> related to the event.</param>
        public ClientPacketHandlerEventArgs(IIPSocket socket)
        {
            _socket = socket;
        }

        /// <summary>
        /// Gets the <see cref="IIPSocket"/> related to the event.
        /// </summary>
        public IIPSocket Socket
        {
            get { return _socket; }
        }
    }

    /// <summary>
    /// <see cref="EventArgs"/> for events on the <see cref="ClientPacketHandler"/>.
    /// </summary>
    public class ClientPacketHandlerEventArgs<T> : ClientPacketHandlerEventArgs
    {
        readonly T _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketHandlerEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="socket">The <see cref="IIPSocket"/> related to the event.</param>
        /// <param name="args">The event arguments.</param>
        public ClientPacketHandlerEventArgs(IIPSocket socket, T args) : base(socket)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the event arguments.
        /// </summary>
        public T Args
        {
            get { return _args; }
        }
    }
}