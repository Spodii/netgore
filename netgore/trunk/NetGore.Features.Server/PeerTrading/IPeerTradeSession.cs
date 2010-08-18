using System.Linq;
using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    public interface IPeerTradeSession<TChar, TItem> where TChar : Entity where TItem : Entity
    {
        /// <summary>
        /// Gets the source character in the trade session. This is the character that started the trade.
        /// </summary>
        TChar CharSource { get; }

        /// <summary>
        /// Gets the amount of cash that the source character has put down in the trade. Will never be negative.
        /// </summary>
        int CharSourceCash { get; }

        /// <summary>
        /// Gets the target character in the trade session. This is the character that was requested to be traded with.
        /// </summary>
        TChar CharTarget { get; }

        /// <summary>
        /// Gets the amount of cash that the target character has put down in the trade. Will never be negative.
        /// </summary>
        int CharTargetCash { get; }

        /// <summary>
        /// Gets if <see cref="CharSource"/> has accepted the trade.
        /// </summary>
        bool HasCharSourceAccepted { get; }

        /// <summary>
        /// Gets if <see cref="CharTarget"/> has accepted the trade.
        /// </summary>
        bool HasCharTargetAccepted { get; }

        /// <summary>
        /// Gets if this trade session has been closed. If true, this trade session should not be used anymore and should
        /// be treated as if it were disposed.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// Marks a character on one side of the trade as accepting the trade. Both characters have to accept the trade for the
        /// trade to actually finalize.
        /// </summary>
        /// <param name="c">The character that is accepting the trade.</param>
        void AcceptTrade(TChar c);

        /// <summary>
        /// Adds cash to a character's side of the trade table. Either all of the <paramref name="cash"/> will be added, or none.
        /// </summary>
        /// <param name="c">The character adding cash to the trade table.</param>
        /// <param name="cash">The amount of cash to add. Cannot be less than or equal to zero.</param>
        /// <returns>True if the cash was successfully added; otherwise false.</returns>
        bool AddCash(TChar c, int cash);

        /// <summary>
        /// Cancels the trade.
        /// </summary>
        /// <param name="canceler">The character that is canceling the trade.</param>
        void Cancel(TChar canceler);

        /// <summary>
        /// Attempts to add an item to a character's side of the trade table.
        /// </summary>
        /// <param name="c">The character adding an item to the trade table.</param>
        /// <param name="item">The item that the character is adding to their side of the trade table.</param>
        /// <returns>The remainder of the <paramref name="item"/> that failed to be added to the trade table, or null if all of the
        /// item was added.</returns>
        TItem TryAddItem(TChar c, TItem item);

        /// <summary>
        /// Removes cash from a character's side of the trade table and gives it back to the character.
        /// </summary>
        /// <param name="c">The character removing cash from the trade table.</param>
        /// <param name="cash">The amount of cash to remove. Cannot be less than or equal to zero.</param>
        /// <returns>True if the specified amount of cash was removed from the trade table; false if not all of the cash could be removed,
        /// usually because the character does not have that much cash placed down in the trade.</returns>
        bool TryRemoveCash(TChar c, int cash);

        /// <summary>
        /// Removes an item from the trade table at the given slot and gives it back to the character.
        /// </summary>
        /// <param name="c">The character who is removing an item from their trade table.</param>
        /// <param name="slot">The slot on the trade table containing the item to remove.</param>
        /// <returns>True if the item was successfully removed; false if the item could not be removed for some reason or if the
        /// slot was empty.</returns>
        bool TryRemoveItem(TChar c, InventorySlot slot);
    }
}