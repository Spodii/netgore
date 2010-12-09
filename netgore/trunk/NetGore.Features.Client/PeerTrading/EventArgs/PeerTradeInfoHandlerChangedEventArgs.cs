using System;
using System.Linq;
using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeInfoHandlerChanged"/> event.
    /// </summary>
    /// <typeparam name="TChar">The type of the char.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TItemInfo">The type of the item info.</typeparam>
    public class PeerTradeInfoHandlerChangedEventArgs<TChar, TItem, TItemInfo> : PeerTradeInfoHandlerChangedEventArgs
        where TChar : Entity where TItem : Entity where TItemInfo : class
    {
        readonly ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> _newHandler;
        readonly ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> _oldHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeInfoHandlerChangedEventArgs{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <param name="oldHandler">The old (last) peer trade information handler. Can be null.</param>
        /// <param name="newHandler">The new (current) peer trade information handler. Can be null.</param>
        public PeerTradeInfoHandlerChangedEventArgs(ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> oldHandler,
                                                    ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> newHandler)
        {
            _oldHandler = oldHandler;
            _newHandler = newHandler;
        }

        /// <summary>
        /// Gets the new (current) peer trade information handler. Can be null.
        /// </summary>
        public ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> NewHandler
        {
            get { return _newHandler; }
        }

        /// <summary>
        /// Gets the old (last) peer trade information handler. Can be null.
        /// </summary>
        public ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> OldHandler
        {
            get { return _oldHandler; }
        }
    }

    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeInfoHandlerChanged"/> event.
    /// </summary>
    public class PeerTradeInfoHandlerChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="PeerTradeInfoHandlerChangedEventArgs{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <typeparam name="TChar">The type of the char.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TItemInfo">The type of the item info.</typeparam>
        /// <param name="oldHandler">The old (last) peer trade information handler. Can be null.</param>
        /// <param name="newHandler">The new (current) peer trade information handler. Can be null.</param>
        /// <returns>An instance of the <see cref="PeerTradeInfoHandlerChangedEventArgs{TChar, TItem, TItemInfo}"/> class.</returns>
        public static PeerTradeInfoHandlerChangedEventArgs<TChar, TItem, TItemInfo> Create<TChar, TItem, TItemInfo>(
            ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> oldHandler,
            ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> newHandler) where TChar : Entity where TItem : Entity
            where TItemInfo : class
        {
            return new PeerTradeInfoHandlerChangedEventArgs<TChar, TItem, TItemInfo>(oldHandler, newHandler);
        }
    }
}