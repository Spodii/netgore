using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;

namespace DemoGame.Server
{
    public abstract class CharacterInventory : InventoryBase<ItemEntity>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _character;

        readonly bool _isPersistent;

        protected CharacterInventory(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _isPersistent = character.IsPersistent;
        }

        public void DecreaseItemAmount(InventorySlot slot)
        {
            // TODO: It would be preferred that I hook to the Item to listen to the amount's changes instead

            // Get the item
            ItemEntity item = this[slot];
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

        bool _isLoading;

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
                    DbController.GetQuery<DeleteCharacterInventoryItemQuery>().Execute(oldItem.ID);

                // Stop listening for changes
                oldItem.OnChangeGraphicOrAmount -= ItemGraphicOrAmountChangeHandler;
            }
            else
            {
                // Item was added

                // Update the database
                if (!_isLoading)
                {
                    var args = new InsertCharacterInventoryItemQuery.QueryArgs(Character.ID, newItem.ID);
                    DbController.GetQuery<InsertCharacterInventoryItemQuery>().Execute(args); 
                }

                // Listen to the item for changes
                newItem.OnChangeGraphicOrAmount += ItemGraphicOrAmountChangeHandler;
            }

            // Prepare the slot for updating
            SendSlotUpdate(slot);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the Client that a slot in the Inventory has changed.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        protected virtual void SendSlotUpdate(InventorySlot slot)
        {
        }

        /// <summary>
        /// Gets the Character that this Inventory belongs to.
        /// </summary>
        public Character Character
        {
            get
            {
                return _character;
            }
        }

        /// <summary>
        /// Gets the DBController used by this CharacterInventory.
        /// </summary>
        public DBController DbController
        {
            get { return Character.DBController; }
        }

        /// <summary>
        /// Makes the Character drop the item from their Inventory.
        /// </summary>
        /// <param name="slot">Slot of the item to drop.</param>
        /// <returns>True if the item was successfully dropped, else false.</returns>
        public bool Drop(InventorySlot slot)
        {
            // Get the item to drop
            ItemEntity dropItem = this[slot];

            // Check for an invalid item
            if (dropItem == null)
            {
                const string errmsg = "Could not drop item since no item exists at slot `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return false;
            }

            // Remove the item from the inventory
            RemoveAt(slot);

            // Drop the item
            Character.DropItem(dropItem);

            return true;
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

            // TODO: Need to track the slots, too, I guess
            InventorySlot slot = new InventorySlot(0);
            var queryResults = DbController.GetQuery<SelectCharacterInventoryItemsQuery>().Execute(Character.ID);
            foreach (ItemValues values in queryResults) 
            {
                // Make sure no item is already in the slot... just in case
                if (this[slot] != null)
                {
                    Debug.Fail("An item is already in this slot.");
                    this[slot].Dispose();
                }

                // Set the item into the slot
                this[slot] = new ItemEntity(values);
                slot++;
            }

            _isLoading = false;
        }

        bool _disposed = false;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // If the Character is not persistent, we want to dispose of every item so it doesn't sit in the
            // database as garbage
            if (!_isPersistent)
            {
                RemoveAll(true);
            }
        }
    }
}