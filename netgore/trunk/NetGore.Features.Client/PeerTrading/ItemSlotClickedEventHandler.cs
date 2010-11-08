using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.World;
using SFML.Window;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Delegate for handling the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.ItemSlotClicked"/> event.
    /// </summary>
    /// <param name="sender">The control the event too place on.</param>
    /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
    /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
    /// <param name="slot">The slot that was clicked.</param>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public delegate void ItemSlotClickedEventHandler<TChar, TItem, TItemInfo>(
        PeerTradeFormBase<TChar, TItem, TItemInfo> sender, MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        where TChar : Entity where TItem : Entity where TItemInfo : class;
}