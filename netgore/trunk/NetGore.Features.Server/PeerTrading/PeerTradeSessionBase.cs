using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Base class for handling a single session of trading of items between two characters.
    /// </summary>
    /// <typeparam name="TChar">The type of character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    public abstract class PeerTradeSessionBase<TChar, TItem> : IPeerTradeSession<TChar, TItem>
        where TChar : Entity where TItem : Entity
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly PeerTradingSettings _settings = PeerTradingSettings.Instance;

        readonly TChar _charSource;
        readonly TChar _charTarget;

        /// <summary>
        /// The trade table for <see cref="CharSource"/>.
        /// </summary>
        readonly IInventory<TItem> _ttSource;

        /// <summary>
        /// The trade table for <see cref="CharTarget"/>.
        /// </summary>
        readonly IInventory<TItem> _ttTarget;

        int _charSourceCash = 0;
        int _charTargetCash = 0;
        bool _hasCharSourceAccepted = false;
        bool _hasCharTargetAccepted = false;
        bool _isClosed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeSessionBase{TChar,TItem}"/> class.
        /// </summary>
        /// <param name="charSource">The first character in the trade session.
        /// This is the character that started the trade.</param>
        /// <param name="charTarget">The second character in the trade session.
        /// This is the character that was requested to be traded with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="charSource"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="charTarget"/> is null.</exception>
        protected PeerTradeSessionBase(TChar charSource, TChar charTarget)
        {
            if (charSource == null)
                throw new ArgumentNullException("charSource");
            if (charTarget == null)
                throw new ArgumentNullException("charTarget");

            _charSource = charSource;
            _charTarget = charTarget;

            _ttSource = CreateTradeTable(CharSource, _settings.MaxTradeSlots);
            _ttTarget = CreateTradeTable(charTarget, _settings.MaxTradeSlots);

            OnTradeOpened();

            // Ensure the character states are valid
            CloseIfCharacterStatesInvalid();
        }

        /// <summary>
        /// When overridden in the derived class, gets if the state of the characters in this trading session are still valid.
        /// This is checked periodically by the base class. If it ever returns false, the trade is forced to be aborted by
        /// the system.
        /// </summary>
        /// <returns>True if the states are still valid; false if the trade needs to be terminated.</returns>
        protected virtual bool AreCharacterStatesValid()
        {
            if (CharSource.IsDisposed || CharTarget.IsDisposed)
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, checks if a character can fit a set of items (presented as an <see cref="IInventory{TItem}"/>)
        /// into their inventory, or wherever else they wish to store the items they will be acquiring from the trade. No items should
        /// actually be added to the inventory.
        /// </summary>
        /// <param name="character">The character that will be given the items.</param>
        /// <param name="items">The items to be added to the <paramref name="character"/>'s inventory.</param>
        /// <returns>True if the <paramref name="character"/> can fit all of the <paramref name="items"/> into their inventory;
        /// otherwise false.</returns>
        protected abstract bool CanFitItemsInInventory(TChar character, IInventory<TItem> items);

        /// <summary>
        /// Clears the trade acceptance status for both characters in the trade, setting them back to not accepting the trade.
        /// This should be called whenever the table's contents change.
        /// </summary>
        protected void ClearAcceptStatus()
        {
            HasCharSourceAccepted = false;
            HasCharTargetAccepted = false;
        }

        /// <summary>
        /// Closes the trading if the character states are invalid.
        /// </summary>
        /// <returns>True if the trade was closed; otherwise false.</returns>
        bool CloseIfCharacterStatesInvalid()
        {
            if (!AreCharacterStatesValid())
            {
                // State is not valid
                const string errmsg =
                    "Peer trade `{0}` with `{1}` and `{2}` closed because AreCharacterStatesValid() returned false.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, CharSource, CharTarget);

                return true;
            }

            // State was valid
            return false;
        }

        /// <summary>
        /// Closes the trade session.
        /// </summary>
        /// <param name="canceler">If the trade was canceled, contains the character who canceled it. Use null for when the trade
        /// was completed successfully (not canceled).</param>
        void CloseTrade(TChar canceler)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return;
            }

            // Raise the complete/cancel event
            if (canceler == null)
                OnTradeCompleted();
            else
                OnTradeCanceled(canceler);

            // Give the remaining items in the table back to their owners. This will usually only need to be done if
            // a trade was not successfully completed.
            GiveItemsToCharacter(CharSource, _ttSource);
            GiveItemsToCharacter(CharTarget, _ttTarget);

            // Handle closing
            _isClosed = true;

            // Raise close event
            OnTradeClosed();
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IInventory{TItem}"/> for the given character
        /// that will be used to hold items that they are putting up for trading.
        /// </summary>
        /// <param name="owner">The character that the trade table is for.</param>
        /// <param name="slots">The number of slots the inventory should have.</param>
        /// <returns>The <see cref="IInventory{TItem}"/> instance to use.</returns>
        protected abstract IInventory<TItem> CreateTradeTable(TChar owner, int slots);

        /// <summary>
        /// Gets the trade table for a character.
        /// </summary>
        /// <param name="c">The character to get the trade table for.</param>
        /// <returns>The trade table for <paramref name="c"/>, or null if <paramref name="c"/>
        /// is not part of this trade session.</returns>
        protected IInventory<TItem> GetTradeTable(TChar c)
        {
            if (c == CharSource)
                return _ttSource;

            if (c == CharTarget)
                return _ttTarget;

            return null;
        }

        /// <summary>
        /// When overridden in the derived class, gives the <paramref name="character"/> the specified amount of <paramref name="cash"/>.
        /// It should not matter where the <paramref name="cash"/> is coming from, or why it is being given to the
        /// <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the <paramref name="cash"/>.</param>
        /// <param name="cash">The amount of cash to give to the <paramref name="character"/>.</param>
        protected abstract void GiveCashToCharacter(TChar character, int cash);

        /// <summary>
        /// When overridden in the derived class, gives the <paramref name="character"/> an <paramref name="item"/>. It should not
        /// matter where the <paramref name="item"/> is coming from, or why it is being given to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the <paramref name="item"/>.</param>
        /// <param name="item">The item to give to the <paramref name="character"/>.</param>
        protected abstract void GiveItemToCharacter(TChar character, TItem item);

        /// <summary>
        /// Gives all of the items from a trade table to a character.
        /// </summary>
        /// <param name="character">The character to give the items to.</param>
        /// <param name="tradeTable">The <see cref="IInventory{TItem}"/> containing the items to give the <paramref name="character"/>.</param>
        void GiveItemsToCharacter(TChar character, IInventory<TItem> tradeTable)
        {
            // One-by-one, take out the items from the trade table and give it to the appropriate character. We give the item
            // first before removing it from the trade table to ensure that, as long as the inventory implementations persist
            // to the database, recovery from a crash during the trade is possible.
            var validItemsAndSlot = tradeTable.Where(x => x.Value != null && !x.Value.IsDisposed).ToImmutable();
            foreach (var kvp in validItemsAndSlot)
            {
                Debug.Assert(kvp.Value == tradeTable[kvp.Key]);

                // Give the item
                GiveItemToCharacter(character, kvp.Value);

                // Remove the item from the trade table (if it wasn't already removed for us)
                if (tradeTable[kvp.Key] != null)
                    tradeTable.RemoveAt(kvp.Key, false);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when there are too many items in the trade table to give them all to
        /// a character. The derived class should provide some sort of notification to the character (if a player-controlled character)
        /// that the items cannot fit. No action actually needs to take place to resolve this problem.
        /// </summary>
        /// <param name="character">The character who cannot accept all of the items being given to them.</param>
        protected virtual void OnCannotFitItems(TChar character)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the amount of cash a character has put down in the
        /// trade table has changed.
        /// </summary>
        /// <param name="c">The character who's cash value has changed.</param>
        /// <param name="oldValue">The previous amount of cash they had placed down in the trade.</param>
        /// <param name="newValue">The current amount of cash they have placed down in the trade.</param>
        protected virtual void OnCashChanged(TChar c, int oldValue, int newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the status for a character accepting the trade has changed.
        /// </summary>
        /// <param name="c">The character who's accept status has changed.</param>
        /// <param name="accepted"><paramref name="c"/>'s trade acceptance status.</param>
        protected virtual void OnCharAcceptedStatusChanged(TChar c, bool accepted)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a trade has been canceled.
        /// </summary>
        /// <param name="canceler">The character that canceled the trade.</param>
        protected virtual void OnTradeCanceled(TChar canceler)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the trade has finished. This can be due to both canceling and
        /// accepting the trade. However, this method will not be invoked by the base class when it is not cleanly cleaned up.
        /// That is, it will not be called if the object reference is lost and is garbage collected. All clean-up of trades
        /// should be done in here.
        /// </summary>
        protected virtual void OnTradeClosed()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a trade has been successfully completed.
        /// </summary>
        protected virtual void OnTradeCompleted()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a trade session has been opened. This is only called
        /// once and is called after the trade session is fully set up.
        /// </summary>
        protected virtual void OnTradeOpened()
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a trade table slot has been changed.
        /// </summary>
        /// <param name="slotOwner">The owner of the slot that has changed. That is, who's side of the trade table has changed.</param>
        /// <param name="slot">The slot that has changed.</param>
        /// <param name="item">The item that currently occupies the <paramref name="slot"/>.</param>
        protected virtual void OnTradeTableSlotChanged(TChar slotOwner, InventorySlot slot, TItem item)
        {
        }

        /// <summary>
        /// Checks if the trade is ready to be finalized and, if so, finalizes it. Both characters have to have already
        /// accepted the trade and both characters must be able to fit the needed items in their inventories.
        /// </summary>
        /// <returns>True if the trade was finalized; otherwise false.</returns>
        void TryFinalize()
        {
            // Check that the characters have accepted the trade
            if (!HasCharSourceAccepted || !HasCharTargetAccepted)
                return;

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return;

            // Check if they can fit the items in their inventories. Check both characters before returning so we can notify them
            // both, if needed, that the items will not fit. If the items do not fit, also set their accepted status to false.
            var fitTestPassed = true;

            if (!CanFitItemsInInventory(CharSource, _ttTarget))
            {
                fitTestPassed = false;
                OnCannotFitItems(CharSource);
                HasCharSourceAccepted = false;
            }

            if (!CanFitItemsInInventory(CharTarget, _ttSource))
            {
                fitTestPassed = false;
                OnCannotFitItems(CharTarget);
                HasCharTargetAccepted = false;
            }

            if (!fitTestPassed)
                return;

            // Give the items to the characters
            GiveItemsToCharacter(CharSource, _ttTarget);
            GiveItemsToCharacter(CharTarget, _ttSource);

            // Give the cash to the characters
            if (CharSourceCash > 0)
                GiveCashToCharacter(CharTarget, CharSourceCash);

            if (CharTargetCash > 0)
                GiveCashToCharacter(CharSource, CharTargetCash);

            // Close down the trade
            CloseTrade(null);
        }

        #region IPeerTradeSession<TChar,TItem> Members

        /// <summary>
        /// Gets the source character in the trade session. This is the character that started the trade.
        /// </summary>
        public TChar CharSource
        {
            get { return _charSource; }
        }

        /// <summary>
        /// Gets the amount of cash that the source character has put down in the trade. Will never be negative.
        /// </summary>
        public int CharSourceCash
        {
            get
            {
                Debug.Assert(_charSourceCash >= 0, "The total amount of cash added to a trade should never be negative...");
                return _charSourceCash;
            }
        }

        /// <summary>
        /// Gets the target character in the trade session. This is the character that was requested to be traded with.
        /// </summary>
        public TChar CharTarget
        {
            get { return _charTarget; }
        }

        /// <summary>
        /// Gets the amount of cash that the target character has put down in the trade. Will never be negative.
        /// </summary>
        public int CharTargetCash
        {
            get
            {
                Debug.Assert(_charTargetCash >= 0, "The total amount of cash added to a trade should never be negative...");
                return _charTargetCash;
            }
        }

        /// <summary>
        /// Gets if <see cref="CharSource"/> has accepted the trade.
        /// </summary>
        public bool HasCharSourceAccepted
        {
            get { return _hasCharSourceAccepted; }
            private set
            {
                if (_hasCharSourceAccepted == value)
                    return;

                _hasCharSourceAccepted = value;

                OnCharAcceptedStatusChanged(CharSource, _hasCharSourceAccepted);
            }
        }

        /// <summary>
        /// Gets if <see cref="CharTarget"/> has accepted the trade.
        /// </summary>
        public bool HasCharTargetAccepted
        {
            get { return _hasCharTargetAccepted; }
            private set
            {
                if (_hasCharTargetAccepted == value)
                    return;

                _hasCharTargetAccepted = value;

                OnCharAcceptedStatusChanged(CharTarget, _hasCharTargetAccepted);
            }
        }

        /// <summary>
        /// Gets if this trade session has been closed. If true, this trade session should not be used anymore and should
        /// be treated as if it were disposed.
        /// </summary>
        public bool IsClosed
        {
            get { return _isClosed; }
        }

        /// <summary>
        /// Marks a character on one side of the trade as accepting the trade. Both characters have to accept the trade for the
        /// trade to actually finalize.
        /// </summary>
        /// <param name="c">The character that is accepting the trade.</param>
        public void AcceptTrade(TChar c)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Set the corresponding character as accepted
            if (c == CharSource)
                HasCharSourceAccepted = true;
            else if (c == CharTarget)
                HasCharTargetAccepted = true;
            else
            {
                const string errmsg = "`{0}` tried to accept the trade, but they are not part of this session!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c);
                Debug.Fail(string.Format(errmsg, c));
                return;
            }

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return;

            // Try to finalize the trade
            TryFinalize();
        }

        /// <summary>
        /// Adds cash to a character's side of the trade table. Either all of the <paramref name="cash"/> will be added, or none.
        /// </summary>
        /// <param name="c">The character adding cash to the trade table.</param>
        /// <param name="cash">The amount of cash to add. Cannot be less than or equal to zero.</param>
        /// <returns>True if the cash was successfully added; otherwise false.</returns>
        public bool AddCash(TChar c, int cash)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return false;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return false;

            if (cash <= 0)
                return false;

            // Add the cash
            int oldValue;
            int newValue;
            if (c == CharSource)
            {
                oldValue = _charSourceCash;
                _charSourceCash += cash;
                newValue = _charSourceCash;
            }
            else if (c == CharTarget)
            {
                oldValue = _charTargetCash;
                _charTargetCash += cash;
                newValue = _charTargetCash;
            }
            else
            {
                // Not a valid character
                const string errmsg = "`{0}` tried to add `{1}` cash to a trade session, but they are not part of this session!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c, cash);
                Debug.Fail(string.Format(errmsg, c, cash));
                return false;
            }

            // Raise the event
            OnCashChanged(c, oldValue, newValue);

            // Clear the accept status since the table changed
            ClearAcceptStatus();

            return true;
        }

        /// <summary>
        /// Cancels the trade.
        /// </summary>
        /// <param name="canceler">The character that is canceling the trade.</param>
        public void Cancel(TChar canceler)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return;
            }

            CloseTrade(canceler);
        }

        /// <summary>
        /// Attempts to add an item to a character's side of the trade table.
        /// </summary>
        /// <param name="c">The character adding an item to the trade table.</param>
        /// <param name="item">The item that the character is adding to their side of the trade table.</param>
        /// <returns>The remainder of the <paramref name="item"/> that failed to be added to the trade table, or null if all of the
        /// item was added.</returns>
        public TItem TryAddItem(TChar c, TItem item)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return item;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return item;
            }

            if (item == null)
            {
                const string errmsg = "Parameter `item` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return item;
            }

            var tradeTable = GetTradeTable(c);

            // Check for a valid trade table
            if (tradeTable == null)
            {
                const string errmsg = "`{0}` tried to add item to trade session for, but they are not part of this session!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c);
                Debug.Fail(string.Format(errmsg, c));
                return item;
            }

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return item;

            // Clear the accept status since the table will be changing
            ClearAcceptStatus();

            // Try to add the item
            IEnumerable<InventorySlot> changedSlots;
            var remaining = tradeTable.TryAdd(item, out changedSlots);

            // Update the slots that were changed
            foreach (var changedSlot in changedSlots)
            {
                OnTradeTableSlotChanged(c, changedSlot, tradeTable[changedSlot]);
            }

            return remaining;
        }

        /// <summary>
        /// Removes cash from a character's side of the trade table and gives it back to the character.
        /// </summary>
        /// <param name="c">The character removing cash from the trade table.</param>
        /// <param name="cash">The amount of cash to remove. Cannot be less than or equal to zero.</param>
        /// <returns>True if the specified amount of cash was removed from the trade table; false if not all of the cash could be removed,
        /// usually because the character does not have that much cash placed down in the trade.</returns>
        public bool TryRemoveCash(TChar c, int cash)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return false;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return false;

            if (cash <= 0)
                return false;

            // Remove the cash
            int oldValue;
            int newValue;
            if (c == CharSource)
            {
                if (CharSourceCash < cash)
                    return false;

                oldValue = _charSourceCash;
                _charSourceCash -= cash;
                newValue = _charSourceCash;
            }
            else if (c == CharTarget)
            {
                if (CharTargetCash < cash)
                    return false;

                oldValue = _charTargetCash;
                _charTargetCash -= cash;
                newValue = _charTargetCash;
            }
            else
            {
                // Not a valid character
                const string errmsg = "`{0}` tried to add `{1}` cash to a trade session, but they are not part of this session!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c, cash);
                Debug.Fail(string.Format(errmsg, c, cash));
                return false;
            }

            // Give the cash back to the character
            var cashDiff = oldValue - newValue;
            GiveCashToCharacter(c, cashDiff);

            // Raise the event
            OnCashChanged(c, oldValue, newValue);

            // Clear the accept status since the table changed
            ClearAcceptStatus();

            return true;
        }

        /// <summary>
        /// Removes an item from the trade table at the given slot and gives it back to the character.
        /// </summary>
        /// <param name="c">The character who is removing an item from their trade table.</param>
        /// <param name="slot">The slot on the trade table containing the item to remove.</param>
        /// <returns>True if the item was successfully removed; false if the item could not be removed for some reason or if the
        /// slot was empty.</returns>
        public bool TryRemoveItem(TChar c, InventorySlot slot)
        {
            if (IsClosed)
            {
                Debug.Fail("Trade session is already closed...");
                return false;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            var tradeTable = GetTradeTable(c);

            // Check for a valid trade table
            if (tradeTable == null)
            {
                const string errmsg = "`{0}` tried to remove an item from a trade session, but they are not part of this session!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c);
                Debug.Fail(string.Format(errmsg, c));
                return false;
            }

            // Check for a valid slot
            if (slot < 0 || slot >= tradeTable.TotalSlots)
            {
                const string errmsg = "Parameter `item` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            // Make sure the character states are valid
            if (CloseIfCharacterStatesInvalid())
                return false;

            // Get the item to remove
            var item = tradeTable[slot];
            if (item == null)
                return false;

            // Clear the accept status since the table will be changing
            ClearAcceptStatus();

            // Remove the item from the table
            tradeTable.RemoveAt(slot, false);

            // Give the item to the user
            GiveItemToCharacter(c, item);

            // Update the slot
            OnTradeTableSlotChanged(c, slot, null);

            return true;
        }

        #endregion
    }
}