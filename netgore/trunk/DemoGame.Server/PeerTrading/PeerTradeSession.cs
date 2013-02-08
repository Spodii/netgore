using System;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.PeerTrading;
using NetGore.World;

namespace DemoGame.Server.PeerTrading
{
    /// <summary>
    /// Handles a single session of trading of items between two characters.
    /// </summary>
    public class PeerTradeSession : PeerTradeSessionBase<User, ItemEntity>
    {
        static readonly PeerTradingRemoveCashQuery _removeCashQuery;
        static readonly PeerTradingReplaceCashQuery _replaceCashQuery;
        static readonly PeerTradingSettings _settings = PeerTradingSettings.Instance;
        static readonly ServerPeerTradeInfoHandler _tradeInfoHandler = ServerPeerTradeInfoHandler.Instance;

        /// <summary>
        /// Initializes the <see cref="PeerTradeSession"/> class.
        /// </summary>
        static PeerTradeSession()
        {
            // Cash the queries we will need
            var dbController = DbControllerBase.GetInstance();
            _replaceCashQuery = dbController.GetQuery<PeerTradingReplaceCashQuery>();
            _removeCashQuery = dbController.GetQuery<PeerTradingRemoveCashQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeSession"/> class.
        /// </summary>
        /// <param name="charSource">The first character in the trade session.
        /// This is the character that started the trade.</param>
        /// <param name="charTarget">The second character in the trade session.
        /// This is the character that was requested to be traded with.</param>
        PeerTradeSession(User charSource, User charTarget) : base(charSource, charTarget)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if the state of the characters in this trading session are still valid.
        /// </summary>
        /// <returns>True if the states are still valid; false if the trade needs to be terminated.</returns>
        protected override bool AreCharacterStatesValid()
        {
            return AreCharacterStatesValidInternal(CharSource, CharTarget);
        }

        /// <summary>
        /// Performs the actual checking of if a <see cref="PeerTradeSession"/>'s characters are valid.
        /// This method is checked both before trades have started, and periodically during the trade.
        /// </summary>
        /// <param name="charSource">The first character in the trade session.
        /// This is the character that started the trade.</param>
        /// <param name="charTarget">The second character in the trade session.
        /// This is the character that was requested to be traded with.</param>
        /// <returns>True if the states are still valid; false if the trade needs to be terminated.</returns>
        static bool AreCharacterStatesValidInternal(Character charSource, Character charTarget)
        {
            if (charSource.Map != charTarget.Map || charSource.GetDistance(charTarget) > _settings.MaxDistance)
                return false;

            if (!charSource.IsAlive || !charTarget.IsAlive)
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
        protected override bool CanFitItemsInInventory(User character, IInventory<ItemEntity> items)
        {
            return character.Inventory.CanAdd(items);
        }

        /// <summary>
        /// Creates a <see cref="PeerTradeSession"/>.
        /// </summary>
        /// <param name="charSource">The first character in the trade session.
        /// This is the character that started the trade.</param>
        /// <param name="charTarget">The second character in the trade session.
        /// This is the character that was requested to be traded with.</param>
        /// <returns>The <see cref="PeerTradeSession"/> for the <paramref name="charSource"/> and <paramref name="charTarget"/>, or
        /// null if the trade cannot be started between the two characters.</returns>
        public static PeerTradeSession Create(User charSource, User charTarget)
        {
            // Perform creation-only tests (these tests only happen once, before the trade)
            if (charSource == null || charTarget == null)
                return null;

            if (charSource.PeerTradeSession != null || charTarget.PeerTradeSession != null)
                return null;

            if (charSource == charTarget)
                return null;

            // Perform the periodic tests (these tests happen all throughout the trade session)
            if (!AreCharacterStatesValidInternal(charSource, charTarget))
                return null;

            // Tests passed - create the session
            var ts = new PeerTradeSession(charSource, charTarget);

            // Set the trade session references onto the characters in the trade
            charSource.PeerTradeSession = ts;
            charTarget.PeerTradeSession = ts;

            return ts;
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IInventory{TItem}"/> for the given character
        /// that will be used to hold items that they are putting up for trading.
        /// </summary>
        /// <param name="owner">The character that the trade table is for.</param>
        /// <param name="slots">The number of slots the inventory should have.</param>
        /// <returns>The <see cref="IInventory{TItem}"/> instance to use.</returns>
        protected override IInventory<ItemEntity> CreateTradeTable(User owner, int slots)
        {
            return new TradeSessionInventory(owner, slots);
        }

        /// <summary>
        /// When overridden in the derived class, gives the <paramref name="character"/> the specified amount of <paramref name="cash"/>.
        /// It should not matter where the <paramref name="cash"/> is coming from, or why it is being given to the
        /// <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the <paramref name="cash"/>.</param>
        /// <param name="cash">The amount of cash to give to the <paramref name="character"/>.</param>
        protected override void GiveCashToCharacter(User character, int cash)
        {
            character.Cash += cash;
        }

        /// <summary>
        /// When overridden in the derived class, gives the <paramref name="character"/> an <paramref name="item"/>. It should not
        /// matter where the <paramref name="item"/> is coming from, or why it is being given to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the <paramref name="item"/>.</param>
        /// <param name="item">The item to give to the <paramref name="character"/>.</param>
        protected override void GiveItemToCharacter(User character, ItemEntity item)
        {
            var template = item.ItemTemplateID;
            if (template.HasValue)
                EventCounterManager.ItemTemplate.Increment(template.Value, ItemTemplateEventCounterType.PeerTraded, item.Amount);

            // Give the character the item
            var giveRemainder = character.TryGiveItem(item);

            if (giveRemainder == null)
                return;

            // If there is a remainder (could not give them back all of the item), try adding it back to the trade table
            var charTT = GetTradeTable(character);
            var ttRemainder = charTT.TryAdd(giveRemainder);

            if (ttRemainder == null)
                return;

            // If there is ANOTHER remainder (could not add the remainder to the trade table), just make them drop it
            character.DropItem(ttRemainder);
        }

        /// <summary>
        /// When overridden in the derived class, handles when there are too many items in the trade table to give them all to
        /// a character. The derived class should provide some sort of notification to the character (if a player-controlled character)
        /// that the items cannot fit. No action actually needs to take place to resolve this problem.
        /// </summary>
        /// <param name="character">The character who cannot accept all of the items being given to them.</param>
        protected override void OnCannotFitItems(User character)
        {
            character.Send(GameMessage.PeerTradingNotEnoughSpaceInInventory, ServerMessageType.GUI);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the amount of cash a character has put down in the
        /// trade table has changed.
        /// </summary>
        /// <param name="c">The character who's cash value has changed.</param>
        /// <param name="oldValue">The previous amount of cash they had placed down in the trade.</param>
        /// <param name="newValue">The current amount of cash they have placed down in the trade.</param>
        protected override void OnCashChanged(User c, int oldValue, int newValue)
        {
            // Update the value in the database
            _replaceCashQuery.Execute(c.ID, newValue);

            // Update the client
            _tradeInfoHandler.WriteCashChanged(this, c, newValue);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the status for a character accepting the trade has changed.
        /// </summary>
        /// <param name="c">The character who's accept status has changed.</param>
        /// <param name="accepted"><paramref name="c"/>'s trade acceptance status.</param>
        protected override void OnCharAcceptedStatusChanged(User c, bool accepted)
        {
            _tradeInfoHandler.WriteAcceptStatusChanged(this, c, accepted);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a trade has been canceled.
        /// </summary>
        /// <param name="canceler">The character that canceled the trade.</param>
        protected override void OnTradeCanceled(User canceler)
        {
            _tradeInfoHandler.WriteTradeCanceled(this, canceler);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the trade has finished. This can be due to both canceling and
        /// accepting the trade. However, this method will not be invoked by the base class when it is not cleanly cleaned up.
        /// That is, it will not be called if the object reference is lost and is garbage collected. All clean-up of trades
        /// should be done in here.
        /// </summary>
        protected override void OnTradeClosed()
        {
            // Remove the cash columns from the database (since, unlike items, it is not removed automatically)
            _removeCashQuery.Execute(CharSource.ID);
            _removeCashQuery.Execute(CharTarget.ID);

            // Send that the trade has closed
            _tradeInfoHandler.WriteTradeClosed(this);

            // Remove the trade session from the characters
            CharSource.PeerTradeSession = null;
            CharTarget.PeerTradeSession = null;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a trade has been successfully completed.
        /// </summary>
        protected override void OnTradeCompleted()
        {
            _tradeInfoHandler.WriteTradeCompleted(this);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a trade session has been opened. This is only called
        /// once and is called after the trade session is fully set up.
        /// </summary>
        protected override void OnTradeOpened()
        {
            _tradeInfoHandler.WriteTradeOpened(this);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a trade table slot has been changed.
        /// </summary>
        /// <param name="slotOwner">The owner of the slot that has changed. That is, who's side of the trade table has changed.</param>
        /// <param name="slot">The slot that has changed.</param>
        /// <param name="item">The item that currently occupies the <paramref name="slot"/>.</param>
        protected override void OnTradeTableSlotChanged(User slotOwner, InventorySlot slot, ItemEntity item)
        {
            _tradeInfoHandler.WriteTradeTableSlotChanged(this, slotOwner, slot, item);
        }

        /// <summary>
        /// The temporary inventory used as part of a <see cref="PeerTradeSession"/> to hold the items added by a <see cref="Character"/>
        /// from one side of the trade.
        /// </summary>
        class TradeSessionInventory : InventoryBase<ItemEntity>
        {
            static readonly PeerTradingInsertItemQuery _insertItemQuery;
            static readonly PeerTradingRemoveItemQuery _removeItemQuery;

            readonly Character _owner;

            /// <summary>
            /// Initializes the <see cref="TradeSessionInventory"/> class.
            /// </summary>
            static TradeSessionInventory()
            {
                // Cache the queries this class will be using
                var dbController = DbControllerBase.GetInstance();
                _insertItemQuery = dbController.GetQuery<PeerTradingInsertItemQuery>();
                _removeItemQuery = dbController.GetQuery<PeerTradingRemoveItemQuery>();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TradeSessionInventory"/> class.
            /// </summary>
            /// <param name="owner">The owner of this object.</param>
            /// <param name="slots">The number of slots in the inventory.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
            public TradeSessionInventory(Character owner, int slots) : base(slots)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// When overridden in the derived class, performs additional processing to handle an Inventory slot
            /// changing. This is only called when the object references changes, not when any part of the object
            /// (such as the Item's amount) changes. It is guarenteed that if <paramref name="newItem"/> is null,
            /// <paramref name="oldItem"/> will not be, and vise versa. Both will never be null or non-null.
            /// </summary>
            /// <param name="slot">Slot that the change took place in.</param>
            /// <param name="newItem">The item that was added to the <paramref name="slot"/>, or null if the slot changed to empty.</param>
            /// <param name="oldItem">The item that used to be in the <paramref name="slot"/>,
            /// or null if the slot used to be empty.</param>
            protected override void HandleSlotChanged(InventorySlot slot, ItemEntity newItem, ItemEntity oldItem)
            {
                base.HandleSlotChanged(slot, newItem, oldItem);

                // Do not need to update the database if the item has not changed
                if (newItem == oldItem)
                    return;

                if (oldItem != null)
                {
                    // Remove the old item from the database (after making sure it isn't in the inventory still)
                    if (!TryGetSlot(oldItem).HasValue)
                        _removeItemQuery.Execute(oldItem.ID);
                }

                if (newItem != null)
                {
                    if (newItem.ID == 0)
                    {
                        Debug.WriteLine(newItem.Name + ": ItemID cannot be zero!");
                        return;
                    }

                    // Add the new item to the database
                    Debug.Assert(TryGetSlot(newItem).HasValue, "How is the new item not in the database?!");
                    _insertItemQuery.Execute(newItem.ID, _owner.ID);
                }
            }
        }
    }
}