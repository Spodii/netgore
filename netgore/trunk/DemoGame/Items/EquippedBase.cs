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
    /// <summary>
    /// Base class for keeping track of a collection of equipped items.
    /// </summary>
    public abstract class EquippedBase<T> : IEnumerable<KeyValuePair<EquipmentSlot, T>> where T : ItemEntityBase
    {
        /// <summary>
        /// Handles an event from the <see cref="EquippedBase{T}"/>.
        /// </summary>
        /// <param name="equippedBase">The <see cref="EquippedBase{T}"/>.</param>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        public delegate void EventHandler(EquippedBase<T> equippedBase, T item, EquipmentSlot slot);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Greatest index of all the EquipmentSlots.
        /// </summary>
        static readonly int _highestSlotIndex = EnumHelper<EquipmentSlot>.MaxValue;

        /// <summary>
        /// Array indexed by the numeric value of the EquipmentSlot, containing the item
        /// corresponding to that slot.
        /// </summary>
        readonly T[] _equipped = new T[_highestSlotIndex + 1];

        /// <summary>
        /// Notifies listeners when an item has been equipped.
        /// </summary>
        public event EventHandler Equipped;

        /// <summary>
        /// Notifies listeners when an item has been unequipped.
        /// </summary>
        public event EventHandler Unequipped;

        /// <summary>
        /// Gets the item at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Slot to get the item of.</param>
        /// <returns>Item at the specified <paramref name="slot"/>.</returns>
        public T this[EquipmentSlot slot]
        {
            get { return this[slot.GetValue()]; }
        }

        /// <summary>
        /// Gets the item at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Index of the slot. This must be a valid value, since no bounds-checking
        /// is performed (which is why this is only private).</param>
        /// <returns>Item at the specified <paramref name="slot"/>.</returns>
        T this[int slot]
        {
            get { return _equipped[slot]; }
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="item"/> can be
        /// equipped at all by the owner of this EquippedBase. This does not guarentee that the item
        /// will be equipped successfully when calling Equip().
        /// </summary>
        /// <param name="item">Item to check if able to be equipped.</param>
        /// <returns>True if the <paramref name="item"/> can be equipped, else false.</returns>
        public abstract bool CanEquip(T item);

        /// <summary>
        /// When overridden in the derived class, checks if the item in the given <paramref name="slot"/> 
        /// can be removed properly.
        /// </summary>
        /// <param name="slot">Slot of the item to be removed.</param>
        /// <returns>True if the item can be properly removed, else false.</returns>
        protected abstract bool CanRemove(EquipmentSlot slot);

        /// <summary>
        /// Equips an <paramref name="item"/>, automatically choosing the EquipmentSlot to use.
        /// </summary>
        /// <param name="item">Item to be equipped.</param>
        /// <returns>True if the item was successfully equipped, else false.</returns>
        public bool Equip(T item)
        {
            // Do not equip invalid items
            if (item == null)
                return false;

            // Check that the item can be equipped at all
            if (!CanEquip(item))
                return false;

            // Get the possible slots
            var slots = GetPossibleSlots(item);

            // Check for valid slots
            if (slots == null)
                return false;

            switch (slots.Count())
            {
                case 0:
                    // If there are no slots, abort
                    return false;

                case 1:
                    // If there is just one slot, try only that slot
                    return TrySetSlot(slots.First(), item, false);

                default:
                    // There are multiple slots, so first try on empty slots
                    var emptySlots = slots.Where(index => this[index] == null);
                    foreach (EquipmentSlot slot in emptySlots)
                    {
                        if (TrySetSlot(slot, item, false))
                            return true;
                    }

                    // Couldn't set on an empty slot, or there was no empty slots, so try all the non-empty slots
                    foreach (EquipmentSlot slot in slots.Except(emptySlots))
                    {
                        if (TrySetSlot(slot, item, false))
                            return true;
                    }

                    // Couldn't set in any slots
                    return false;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of EquipmentSlots possible for a given item.
        /// </summary>
        /// <param name="item">Item to get the possible EquipmentSlots for.</param>
        /// <returns>An IEnumerable of EquipmentSlots possible for the <paramref name="item"/>.</returns>
        protected virtual IEnumerable<EquipmentSlot> GetPossibleSlots(T item)
        {
            return item.Type.GetPossibleSlots();
        }

        /// <summary>
        /// Gets the EquipmentSlot of the specified <paramref name="item"/> in this EquippedBase.
        /// </summary>
        /// <param name="item">Item to find the EquipmentSlot for.</param>
        /// <returns>EquipmentSlot that the <paramref name="item"/> is in this EquippedBase.</returns>
        /// <exception cref="ArgumentException">Specified item could not be found in this EquippedBase.</exception>
        public EquipmentSlot GetSlot(T item)
        {
            for (int i = 0; i < _equipped.Length; i++)
            {
                if (item == this[i])
                    return (EquipmentSlot)i;
            }

            throw new ArgumentException("Specified item could not be found in this EquippedBase.");
        }

        /// <summary>
        /// Handles when an ItemEntity being handled by this EquippedBase is disposed while still equipped.
        /// </summary>
        /// <param name="entity">Item that was disposed.</param>
        protected virtual void ItemDisposeHandler(Entity entity)
        {
            T item = (T)entity;

            // Try to get the slot of the item
            EquipmentSlot slot;
            try
            {
                slot = GetSlot(item);
            }
            catch (ArgumentException)
            {
                const string errmsg = "Equipment item `{0}` was disposed, but was not be found in the EquippedBase.";
                Debug.Fail(string.Format(errmsg, item));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, item);
                return;
            }

            // Remove the item from the EquippedBase
            RemoveAt(slot);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has been equipped.
        /// </summary>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        protected virtual void OnEquipped(T item, EquipmentSlot slot)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has been removed.
        /// </summary>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        protected virtual void OnUnequipped(T item, EquipmentSlot slot)
        {
        }

        /// <summary>
        /// Removes all items from the EquippedBase.
        /// </summary>
        /// <param name="dispose">If true, then all of the items in the EquippedBase will be disposed of. If false,
        /// they will only be removed from the EquippedBase, but could still referenced by other objects.</param>
        public void RemoveAll(bool dispose)
        {
            for (int i = 0; i < _equipped.Length; i++)
            {
                T item = this[i];
                if (item == null)
                    continue;

                RemoveAt((EquipmentSlot)i);

                if (dispose)
                    item.Dispose();
            }
        }

        /// <summary>
        /// Removes an item from the specified <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Slot to remove the item from.</param>
        /// <returns>True if the item was successfully removed from the given <paramref name="slot"/> or if
        /// there was no item in the slot, else false if the item was unable to be removed.</returns>
        public bool RemoveAt(EquipmentSlot slot)
        {
            return TrySetSlot(slot, null, false);
        }

        /// <summary>
        /// Tries to set a given <paramref name="slot"/> to a new <paramref name="item"/>.
        /// </summary>
        /// <param name="slot">Slot to set the item in.</param>
        /// <param name="item">Item to set the slot to.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added to the specified
        /// <paramref name="slot"/>, else false. When false, it is guarenteed the equipment will
        /// not have been modified.</returns>
        protected bool TrySetSlot(EquipmentSlot slot, T item)
        {
            return TrySetSlot(slot, item, true);
        }

        /// <summary>
        /// Tries to set a given <paramref name="slot"/> to a new <paramref name="item"/>.
        /// </summary>
        /// <param name="slot">Slot to set the item in.</param>
        /// <param name="item">Item to set the slot to.</param>
        /// <param name="checkIfCanEquip">If true, the specified <paramref name="item"/> will
        /// be checked if it can be euqipped with <see cref="CanEquip"/>. If false, this additional
        /// check will be completely bypassed.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added to the specified
        /// <paramref name="slot"/>, else false. When false, it is guarenteed the equipment will
        /// not have been modified.</returns>
        bool TrySetSlot(EquipmentSlot slot, T item, bool checkIfCanEquip)
        {
            // Check for a valid EquipmentSlot
            if (!EnumHelper<EquipmentSlot>.IsDefined(slot))
            {
                const string errmsg = "Invalid EquipmentSlot `{0}` specified.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, slot);
                Debug.Fail(string.Format(errmsg, slot));
                return false;
            }

            // Get the array index for the slot
            byte index = slot.GetValue();

            // If the slot is equal to the value we are trying to set it, we never want
            // to do anything extra, so just abort
            T currentItem = this[index];
            if (currentItem == item)
                return false;

            if (item != null)
            {
                // Add item

                // Ensure the item can be equipped
                if (checkIfCanEquip)
                {
                    if (!CanEquip(item))
                        return false;
                }

                // If the slot is already in use, remove the item, aborting if removal failed
                if (currentItem != null)
                {
                    if (!RemoveAt(slot))
                        return false;
                }

                // Attach the listener for the OnDispose event
                item.Disposed += ItemDisposeHandler;

                // Set the item into the slot
                _equipped[index] = item;

                OnEquipped(item, slot);

                if (Equipped != null)
                    Equipped(this, item, slot);
            }
            else
            {
                // Remove item (set to null)

                // Check if the item can be removed
                if (!CanRemove(slot))
                    return false;

                T oldItem = _equipped[index];

                // Remove the listener for the OnDispose event
                oldItem.Disposed -= ItemDisposeHandler;

                // Remove the item
                _equipped[index] = null;

                OnUnequipped(oldItem, slot);

                if (Unequipped != null)
                    Unequipped(this, oldItem, slot);
            }

            // Slot setting was successful (since we always aborted early with false if it wasn't)
            return true;
        }

        #region IEnumerable<KeyValuePair<EquipmentSlot,T>> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        ///
        public IEnumerator<KeyValuePair<EquipmentSlot, T>> GetEnumerator()
        {
            for (int i = 0; i < _equipped.Length; i++)
            {
                T item = this[i];
                if (item != null)
                    yield return new KeyValuePair<EquipmentSlot, T>((EquipmentSlot)i, item);
            }
        }

        ///<summary>
        /// Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///</returns>
        ///
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}