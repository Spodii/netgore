using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Delegate for handling the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeInfoHandlerChanged"/> event.
    /// </summary>
    /// <param name="sender">The <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> that this event came from.</param>
    /// <param name="oldHandler">The old (last) peer trade information handler. Can be null.</param>
    /// <param name="newHandler">The new (current) peer trade information handler. Can be null.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public delegate void PeerTradeInfoHandlerChangedEventHandler<TChar, TItem, TItemInfo>(
        PeerTradeFormBase<TChar, TItem, TItemInfo> sender, ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> oldHandler,
        ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> newHandler) where TChar : Entity where TItem : Entity
        where TItemInfo : class;
}