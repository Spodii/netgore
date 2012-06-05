using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Interface that describes an inventory, which is a fixed-sized collection that contains game items.
    /// </summary>
    /// <typeparam name="T">The type of game item contained in the inventory.</typeparam>
    public interface IInventory<T> : IEnumerable<KeyValuePair<InventorySlot, T>>
    {
        /// <summary>
        /// Gets the item in the Inventory at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Index of the slot to get the Item from.</param>
        /// <returns>Item in the specified Inventory slot, or null if the slot is empty or invalid.</returns>
        T this[InventorySlot slot] { get; }

        /// <summary>
        /// Gets the number of inventory slots that are currently unoccupied (contains no item).
        /// </summary>
        int FreeSlots { get; }

        /// <summary>
        /// Gets the items in this collection.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets the number of inventory slots that are currently occupied (contains an item).
        /// </summary>
        int OccupiedSlots { get; }

        /// <summary>
        /// Gets the total number of slots in this inventory (both free and occupied slots).
        /// </summary>
        int TotalSlots { get; }

        /// <summary>
        /// Tries to add an item to the inventory, returning the amount of the item that was successfully added. Any remainder
        /// of the <paramref name="item"/> that fails to be added will be destroyed instead of being returned like with
        /// TryAdd.
        /// </summary>
        /// <param name="item">Item that will be added to the Inventory.</param>
        /// <returns>The amount of the <paramref name="item"/> that was successfully added to the inventory. If this value is
        /// 0, none of the <paramref name="item"/> was added. If it is equal to the <paramref name="item"/>'s amount, then
        /// all of the item was successfully added.</returns>
        int Add(T item);

        /// <summary>
        /// Tries to add an item to the inventory, returning the amount of the item that was successfully added. Any remainder
        /// of the <paramref name="item"/> that fails to be added will be destroyed instead of being returned like with
        /// TryAdd.
        /// </summary>
        /// <param name="item">Item that will be added to the Inventory.</param>
        /// <param name="changedSlots">Contains the <see cref="InventorySlot"/>s that the <paramref name="item"/> was added to.</param>
        /// <returns>The amount of the <paramref name="item"/> that was successfully added to the inventory. If this value is
        /// 0, none of the <paramref name="item"/> was added. If it is equal to the <paramref name="item"/>'s amount, then
        /// all of the item was successfully added.</returns>
        int Add(T item, out IEnumerable<InventorySlot> changedSlots);

        /// <summary>
        /// Checks if the specified <paramref name="item"/> can be added to the inventory completely
        /// and successfully, but does not actually add the item.
        /// </summary>
        /// <param name="item">Item to try to fit into the inventory.</param>
        /// <returns>True if the <paramref name="item"/> can be added to the inventory properly; false if the <paramref name="item"/>
        /// is invalid or cannot fully fit into the inventory.</returns>
        bool CanAdd(T item);

        /// <summary>
        /// Checks if the specified <paramref name="items"/> can be added to the inventory completely
        /// and successfully, but does not actually add the items.
        /// </summary>
        /// <param name="items">Items to try to fit into the inventory.</param>
        /// <returns>True if the <paramref name="items"/> can be added to the inventory properly; false if the <paramref name="items"/>
        /// is invalid or cannot fully fit into the inventory.</returns>
        bool CanAdd(IEnumerable<T> items);

        /// <summary>
        /// Checks if the specified <paramref name="items"/> can be added to the inventory completely
        /// and successfully, but does not actually add the items.
        /// </summary>
        /// <param name="items">Items to try to fit into the inventory.</param>
        /// <returns>True if the <paramref name="items"/> can be added to the inventory properly; false if the <paramref name="items"/>
        /// is invalid or cannot fully fit into the inventory.</returns>
        bool CanAdd(IInventory<T> items);

        /// <summary>
        /// Gets the slot for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item to find the slot for.</param>
        /// <returns>Slot for the specified <paramref name="item"/>.</returns>
        /// <exception cref="ArgumentException">The specified item is not in the Inventory.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> was null.</exception>
        InventorySlot GetSlot(T item);

        /// <summary>
        /// Removes all items from the inventory.
        /// </summary>
        /// <param name="destroy">If true, then all of the items in the inventory will be destroyed. If false,
        /// they will only be removed from the inventory, but could still referenced by other objects.</param>
        void RemoveAll(bool destroy);

        /// <summary>
        /// Removes the item in the given <paramref name="slot"/> from the inventory. The removed item is
        /// not disposed, so if the item must be disposed (that is, it won't be used anywhere else), be
        /// sure to dispose of it!
        /// </summary>
        /// <param name="slot">Slot of the item to remove.</param>
        /// <param name="destroy">If true, the item at the given <paramref name="slot"/> will be destroyed. If false,
        /// the item will not be disposed and will still be referenceable.</param>
        void RemoveAt(InventorySlot slot, bool destroy);

        /// <summary>
        /// Swaps the items in two inventory slots.
        /// </summary>
        /// <param name="a">The first <see cref="InventorySlot"/> to swap.</param>
        /// <param name="b">The second <see cref="InventorySlot"/> to swap.</param>
        /// <returns>True if the swapping was successful; false if either of the <see cref="InventorySlot"/>s contained
        /// an invalid value or if the slots were the same slot.</returns>
        bool SwapSlots(InventorySlot a, InventorySlot b);

        /// <summary>
        /// Tries to add an item to the inventory, returning the remainder of the item that was not added.
        /// </summary>
        /// <param name="item">Item that will be added to the Inventory.</param>
        /// <returns>The remainder of the item that was not added to the inventory. If this returns null, all of the item
        /// was added to the inventory. Otherwise, this will return an object instance with the amount
        /// equal to the portion that failed to be added.</returns>
        T TryAdd(T item);

        /// <summary>
        /// Tries to add an item to the inventory, returning the remainder of the item that was not added.
        /// </summary>
        /// <param name="item">Item that will be added to the Inventory.</param>
        /// <param name="changedSlots">Contains the <see cref="InventorySlot"/>s that the <paramref name="item"/> was added to.</param>
        /// <returns>The remainder of the item that was not added to the inventory. If this returns null, all of the item
        /// was added to the inventory. Otherwise, this will return an object instance with the amount
        /// equal to the portion that failed to be added.</returns>
        T TryAdd(T item, out IEnumerable<InventorySlot> changedSlots);

        /// <summary>
        /// Gets the slot for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item to find the slot for.</param>
        /// <returns>Slot for the specified <paramref name="item"/>, or null if the <paramref name="item"/> is invalid or not in
        /// the inventory.</returns>
        InventorySlot? TryGetSlot(T item);
    }
}