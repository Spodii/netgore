using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for an inventory for a <see cref="Character"/>.
    /// </summary>
    public abstract class CharacterInventory : InventoryBase<ItemEntity>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _character;
        readonly bool _isPersistent;

        bool _disposed = false;
        bool _isLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterInventory"/> class.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the inventory belongs to.</param>
        protected CharacterInventory(Character character) : base(GameData.MaxInventorySize)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _isPersistent = character.IsPersistent;
        }

        /// <summary>
        /// Gets the Character that this Inventory belongs to.
        /// </summary>
        public Character Character
        {
            get { return _character; }
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this CharacterInventory.
        /// </summary>
        public IDbController DbController
        {
            get { return Character.DbController; }
        }

        /// <summary>
        /// Decrements the amount of the item at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the inventory item who's amount is to be decremented.</param>
        public void DecreaseItemAmount(InventorySlot slot)
        {
            // Get the ItemEntity
            var item = this[slot];
            if (item == null)
            {
                const string errmsg = "Tried to decrease amount of inventory slot `{0}`, but it contains no item.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return;
            }

            // Check for a valid amount
            if (item.Amount == 0)
            {
                // If amount is already 0, we will show a warning but still remove the item
                const string errmsg = "Item in slot `{0}` already contains an amount of 0.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);

                // Remove the item
                ClearSlot(slot, true);
            }
            else
            {
                // Decrease the amount
                item.Amount--;

                // Remove the item if it ran out
                if (item.Amount <= 0)
                    ClearSlot(slot, true);
            }
        }

        /// <summary>
        /// Makes the Character drop an item from their Inventory.
        /// </summary>
        /// <param name="slot">Slot of the item to drop.</param>
        /// <returns>True if the item was successfully dropped, else false.</returns>
        public bool Drop(InventorySlot slot)
        {
            // Get the item to drop
            var dropItem = this[slot];

            // Check for an invalid item
            if (dropItem == null)
            {
                const string errmsg = "Could not drop item since no item exists at slot `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return false;
            }

            // Remove the item from the inventory
            RemoveAt(slot, false);

            // Drop the item
            Character.DropItem(dropItem);

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, performs additional processing to handle an inventory slot
        /// changing. This is only called when the object references changes, not when any part of the object
        /// (such as the Item's amount) changes. It is guarenteed that if <paramref name="newItem"/> is null,
        /// <paramref name="oldItem"/> will not be, and vise versa. Both will never be null or non-null.
        /// </summary>
        /// <param name="slot">Slot that the change took place in.</param>
        /// <param name="newItem">The ItemEntity that was added to the <paramref name="slot"/>.</param>
        /// <param name="oldItem">The ItemEntity that used to be in the <paramref name="slot"/>,
        /// or null if the slot used to be empty.</param>
        protected override void HandleSlotChanged(InventorySlot slot, ItemEntity newItem, ItemEntity oldItem)
        {
            // If we are loading the Inventory, we do not want to do database updates since that would be redundant
            // and likely cause problems
            if (!_isPersistent)
                return;

            if (newItem == null)
            {
                // Item was removed

                // Update the database
                if (!_isLoading)
                    DbController.GetQuery<DeleteCharacterInventoryItemQuery>().Execute(Character.ID, slot);

                // Stop listening for changes
                oldItem.GraphicOrAmountChanged -= ItemGraphicOrAmountChangeHandler;
            }
            else
            {
                // Item was added

                // Update the database
                if (!_isLoading)
                    DbController.GetQuery<ReplaceCharacterInventoryItemQuery>().Execute(Character.ID, newItem.ID, slot);

                // Listen to the item for changes
                newItem.GraphicOrAmountChanged += ItemGraphicOrAmountChangeHandler;
            }

            // Prepare the slot for updating
            SendSlotUpdate(slot);
        }

        /// <summary>
        /// Handles when an item in the UserInventory has changed the amount or graphic, and notifies the
        /// InventoryUpdater to handle the change.
        /// </summary>
        /// <param name="item">Item that has changed.</param>
        void ItemGraphicOrAmountChangeHandler(ItemEntity item)
        {
            Debug.Assert(_isPersistent, "This should NEVER be called when IsPersistent == false!");

            InventorySlot slot;

            // Try to get the slot
            try
            {
                slot = GetSlot(item);
            }
            catch (ArgumentException ex)
            {
                log.Warn(string.Format("Failed to get the inventory slot of item `{0}`", item), ex);
                return;
            }

            // Set the slot as changed
            SendSlotUpdate(slot);
        }

        /// <summary>
        /// Loads the Character's inventory items from the database.
        /// </summary>
        public void Load()
        {
            _isLoading = true;

            var queryResults = DbController.GetQuery<SelectCharacterInventoryItemsQuery>().Execute(Character.ID);
            foreach (var values in queryResults)
            {
                // Make sure no item is already in the slot... just in case
                if (this[values.Key] != null)
                {
                    const string errmsg =
                        "Character `{0}` already had an item in slot `{1}` ({2})." +
                        " It is going to have to be disposed to make room for the newest loaded item `{3}`." +
                        " If this ever happens, its likely a problem.";

                    var item = this[values.Key];

                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, Character, values.Key, item, values.Value);
                    Debug.Fail(string.Format(errmsg, Character, values.Key, item, values.Value));

                    item.Dispose();
                }

                // Set the item into the slot
                this[values.Key] = new ItemEntity(values.Value);
            }

            _isLoading = false;
        }

        /// <summary>
        /// When overridden in the derived class, notifies the Client that a slot in the Inventory has changed.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        protected virtual void SendSlotUpdate(InventorySlot slot)
        {
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // If the Character is not persistent, we want to dispose of every item so it doesn't sit in the
            // database as garbage
            if (!_isPersistent)
                RemoveAll(true);
        }

        #endregion
    }
}