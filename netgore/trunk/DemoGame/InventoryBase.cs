using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    public abstract class InventoryBase
    {
        /// <summary>
        /// The maximum size (number of different item sets) of the Inventory. Any slot greater than or equal to
        /// the MaxInventorySize is considered invalid.
        /// </summary>
        public const int MaxInventorySize = 6 * 6;
    }

    /// <summary>
    /// Base class for an Inventory that contains ItemEntities.
    /// </summary>
    public abstract class InventoryBase<T> : InventoryBase, IEnumerable<KeyValuePair<InventorySlot, T>> where T : ItemEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly T[] _buffer = new T[MaxInventorySize];

        /// <summary>
        /// Gets or sets (protected) the item in the Inventory at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Index of the slot to get the Item from.</param>
        /// <returns>Item in the specified Inventory slot, or null if the slot is empty or invalid.</returns>
        public T this[InventorySlot slot]
        {
            get
            {
                // Check for a valid index
                if (slot >= MaxInventorySize || slot < 0)
                {
                    const string errmsg = "Tried to get invalid inventory slot `{0}`";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, slot);
                    Debug.Fail(string.Format(errmsg, slot));
                    return null;
                }

                return _buffer[(int)slot];
            }

            protected set
            {
                // Check for a valid index
                if (slot >= MaxInventorySize || slot < 0)
                {
                    const string errmsg = "Tried to set invalid inventory slot `{0}`";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, slot);
                    Debug.Fail(string.Format(errmsg, slot));
                    return;
                }

                // Check for a change
                T oldItem = this[slot];
                if (oldItem == value)
                    return;

                // Ensure that, when setting an item into a slot, that no item is already there
                if (oldItem != null && value != null)
                {
                    const string errmsg = "Set an item ({0}) on a slot ({1}) that already contained an item ({2}).";
                    Debug.Fail(string.Format(errmsg, value, slot, oldItem));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, value, slot, oldItem);

                    // Try to resolve the problem by removing and disposing of the old item
                    ClearSlot(slot, true);
                }

                // Attach (if item added) or remove (if item removed) hook to the Dispose event
                if (oldItem != null)
                    oldItem.OnDispose -= ItemDisposeHandler;
                else
                    value.OnDispose += ItemDisposeHandler;

                // Change the ItemEntity reference
                _buffer[(int)slot] = value;

                // Allow for additional processing
                HandleSlotChanged(slot, value, oldItem);
            }
        }

        /// <summary>
        /// Adds as much of the item, if not all, to the inventory as possible. The actual 
        /// item object is not given to the Inventory, but rather a deep copy of it. 
        /// The passed item's amount will be reduced either to 0 if the Inventory took all
        /// of the item, or reduced if only some of the item could be taken.
        /// </summary>
        /// <param name="item">Item that will be added to the Inventory.</param>
        /// <returns>The remainder of the item that failed to be added to the inventory, or null if all of the
        /// item was added.</returns>
        public T Add(T item)
        {
            // Try to stack the item in as many slots as possible until it runs out or we run out of slots
            InventorySlot stackSlot;
            while (TryFindStackableSlot(item, out stackSlot))
            {
                T invItem = this[stackSlot];
                if (invItem == null)
                {
                    const string errmsg = "This should never be a null item. If it is, TryFindStackableSlot() may be broken.";
                    Debug.Fail(errmsg);
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                }
                else
                {
                    // Stack as much of the item into the existing inventory item
                    byte stackAmount = (byte)Math.Min(ItemEntityBase.MaxStackSize - invItem.Amount, item.Amount);
                    invItem.Amount += stackAmount;
                    item.Amount -= stackAmount;

                    // If we stacked all of the item, we're done
                    if (item.Amount == 0)
                        return null;
                }
            }

            // Could not stack, or only some of the item was stacked, so add item to empty slots
            InventorySlot emptySlot;
            while (TryFindEmptySlot(out emptySlot))
            {
                // Deep-copy the item and set it in the inventory
                T copy = (T)item.DeepCopy();
                this[emptySlot] = copy;

                // Reduce the amount of the item by the amount we took
                byte amountTaken = Math.Min(ItemEntityBase.MaxStackSize, item.Amount);
                copy.Amount = amountTaken;
                item.Amount -= amountTaken;

                // If we took all of the item, we are done
                if (item.Amount == 0)
                {
                    item.Dispose();
                    return null;
                }
            }

            // Failed to add all of the item
            return item;
        }

        /// <summary>
        /// Checks if the specified <paramref name="item"/> can be added to the inventory completely
        /// and successfully, but does not actually add the item.
        /// </summary>
        /// <param name="item">Item to try to fit into the inventory.</param>
        /// <returns>True if the <paramref name="item"/> can be added to the inventory properly or if the
        /// <paramref name="item"/> is null, else false.</returns>
        public bool CanAdd(T item)
        {
            if (item == null)
                return true;

            // If there are any free slots, we know it can be added
            InventorySlot emptySlot;
            if (TryFindEmptySlot(out emptySlot))
                return true;

            // FUTURE: Add support for returning true if the item can be stacked

            return false;
        }

        /// <summary>
        /// Completely removes the item in a given slot, including optionally disposing it.
        /// </summary>
        /// <param name="slot">Slot of the item to remove.</param>
        /// <param name="dispose">If true, the item in the slot will also be disposed. If false, the item
        /// will be removed from the Inventory, but not disposed.</param>
        protected void ClearSlot(InventorySlot slot, bool dispose)
        {
            // Get the item at the slot
            T item = this[slot];

            // Check for a valid item
            if (item == null)
            {
                const string errmsg = "Slot `{0}` already contains no item.";
                Debug.Fail(string.Format("Slot `{0}` already contains no item.", slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // Remove the item reference
            this[slot] = null;

            // Dispose of the item
            if (dispose)
                item.Dispose();
        }

        /// <summary>
        /// Gets the slot for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item to find the slot for.</param>
        /// <returns>Slot for the specified <paramref name="item"/>.</returns>
        /// <exception cref="ArgumentException">The specified item is not in the Inventory.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> was null.</exception>
        public InventorySlot GetSlot(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            for (int i = 0; i < MaxInventorySize; i++)
            {
                if (this[new InventorySlot(i)] == item)
                    return new InventorySlot(i);
            }

            throw new ArgumentException("The specified item is not in the Inventory.", "item");
        }

        /// <summary>
        /// When overridden in the derived class, performs additional processing to handle an Inventory slot
        /// changing. This is only called when the object references changes, not when any part of the object
        /// (such as the Item's amount) changes. It is guarenteed that if <paramref name="newItem"/> is null,
        /// <paramref name="oldItem"/> will not be, and vise versa. Both will never be null or non-null.
        /// </summary>
        /// <param name="slot">Slot that the change took place in.</param>
        /// <param name="newItem">The item that was added to the <paramref name="slot"/>.</param>
        /// <param name="oldItem">The item that used to be in the <paramref name="slot"/>,
        /// or null if the slot used to be empty.</param>
        protected virtual void HandleSlotChanged(InventorySlot slot, T newItem, T oldItem)
        {
        }

        /// <summary>
        /// Handles when an item is disposed while still in the Inventory.
        /// </summary>
        /// <param name="entity">Entity that was disposed.</param>
        void ItemDisposeHandler(Entity entity)
        {
            T item = (T)entity;

            // Try to get the slot
            InventorySlot slot;
            try
            {
                slot = GetSlot(item);
            }
            catch (ArgumentException)
            {
                const string errmsg = "Inventory item `{0}` was disposed, but was not be found in the Inventory.";
                Debug.Fail(string.Format(errmsg, item));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, item);
                return;
            }

            // Remove the item from the Inventory
            RemoveAt(slot);
        }

        /// <summary>
        /// Removes all items from the InventoryBase.
        /// </summary>
        /// <param name="dispose">If true, then all of the items in the InventoryBase will be disposed of. If false,
        /// they will only be removed from the InventoryBase, but could still referenced by other objects.</param>
        public void RemoveAll(bool dispose)
        {
            for (InventorySlot i = new InventorySlot(0); i < _buffer.Length; i++)
            {
                if (this[i] != null)
                    ClearSlot(i, dispose);
            }
        }

        /// <summary>
        /// Removes the ItemEntity in the given <paramref name="slot"/> from the Inventory. The removed item is
        /// not disposed, so if the ItemEntity must be disposed (that is, it won't be used anywhere else), be
        /// sure to dispose of it!
        /// </summary>
        /// <param name="slot">Slot of the item to remove.</param>
        public void RemoveAt(InventorySlot slot)
        {
            ClearSlot(slot, false);
        }

        /// <summary>
        /// Gets the index of the first unused Inventory slot.
        /// </summary>
        /// <param name="emptySlot">If function returns true, contains the index of the first unused Inventory slot.</param>
        /// <returns>True if an empty slot was found, otherwise false.</returns>
        protected bool TryFindEmptySlot(out InventorySlot emptySlot)
        {
            // Iterate through each slot
            for (int i = 0; i < MaxInventorySize; i++)
            {
                // Return on the first null item
                if (this[new InventorySlot(i)] == null)
                {
                    emptySlot = new InventorySlot(i);
                    return true;
                }
            }

            // All slots are in use
            emptySlot = new InventorySlot(0);
            return false;
        }

        /// <summary>
        /// Gets the first slot that the given <paramref name="item"/> can be stacked on.
        /// </summary>
        /// <param name="item">Item that will try to stack on existing items.</param>
        /// <param name="stackableSlot">If function returns true, contains the index of the first slot that
        /// the <paramref name="item"/> can be stacked on. This slot is not guaranteed to be able to hold 
        /// all of the item, but it does guarantee to be able to hold at least one unit of the item.</param>
        /// <returns>True if a stackable slot was found, otherwise false.</returns>
        protected bool TryFindStackableSlot(ItemEntityBase item, out InventorySlot stackableSlot)
        {
            // Iterate through each slot
            for (int i = 0; i < MaxInventorySize; i++)
            {
                T invItem = this[new InventorySlot(i)];

                // Skip empty slots
                if (invItem == null)
                    continue;

                // Make sure the item isn't already at the stacking limit
                if (invItem.Amount >= ItemEntityBase.MaxStackSize)
                    continue;

                // Check if the item can stack with our item
                if (!invItem.CanStack(item))
                    continue;

                // Stackable slot found
                stackableSlot = new InventorySlot(i);
                return true;
            }

            // No stackable slot found
            stackableSlot = new InventorySlot(0);
            return false;
        }

        #region IEnumerable<KeyValuePair<InventorySlot,T>> Members

        public IEnumerator<KeyValuePair<InventorySlot, T>> GetEnumerator()
        {
            for (InventorySlot i = new InventorySlot(0); i < _buffer.Length; i++)
            {
                T item = this[i];
                if (item != null)
                    yield return new KeyValuePair<InventorySlot, T>(i, item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}