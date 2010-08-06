using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    public interface IPeerTradeSession<TChar, TItem>
        where TChar : Entity
        where TItem : Entity
    {

        /// <summary>
        /// Gets the first character in the trade session. This is the character that started the trade.
        /// </summary>
        TChar CharSource { get; }

        /// <summary>
        /// Gets the second character in the trade session. This is the character that was requested to be traded with.
        /// </summary>
        TChar CharTarget { get; }

        /// <summary>
        /// Gets if <see cref="CharSource"/> has accepted the trade.
        /// </summary>
        bool HasCharSourceAccepted { get; }

        /// <summary>
        /// Gets if <see cref="CharTarget"/> has accepted the trade.
        /// </summary>
        bool HasCharTargetAccepted { get; }

        /// <summary>
        /// Marks a character on one side of the trade as accepting the trade. Both characters have to accept the trade for the
        /// trade to actually finalize.
        /// </summary>
        /// <param name="c">The character that is accepting the trade.</param>
        void AcceptTrade(TChar c);

        /// <summary>
        /// Attempts to add an item to a character's side of the trade table.
        /// </summary>
        /// <param name="c">The character adding an item to the trade table.</param>
        /// <param name="item">The item that the character is adding to their side of the trade table.</param>
        /// <returns>The remainder of the <paramref name="item"/> that failed to be added to the trade table, or null if all of the
        /// item was added.</returns>
        TItem TryAddItem(TChar c, TItem item);

        /// <summary>
        /// Removes an item from the trade table at the given slot and gives it back to the character.
        /// </summary>
        /// <param name="c">The character who is removing an item from their trade table.</param>
        /// <param name="slot">The slot on the trade table containing the item to remove.</param>
        /// <returns>True if the item was successfully removed; false if the item could not be removed for some reason or if the
        /// slot was empty.</returns>
        bool TryRemoveItem(TChar c, InventorySlot slot);

        /// <summary>
        /// Gets if this trade session has been closed. If true, this trade session should not be used anymore and should
        /// be treated as if it were disposed.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// Cancels the trade.
        /// </summary>
        /// <param name="canceler">The character that is canceling the trade.</param>
        void Cancel(TChar canceler);
    }
}