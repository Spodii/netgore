using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.World;

namespace DemoGame
{
    /// <summary>
    /// Base class for keeping track of a collection of equipped items.
    /// </summary>
    public abstract class EquippedBase<T> : IDisposable, IEnumerable<KeyValuePair<EquipmentSlot, T>> where T : ItemEntityBase
    {
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

        bool _disposed = false;

        /// <summary>
        /// Notifies listeners when an item has been equipped.
        /// </summary>
        public event TypedEventHandler<EquippedBase<T>, EquippedEventArgs<T>> Equipped;

        /// <summary>
        /// Notifies listeners when an item has been unequipped.
        /// </summary>
        public event TypedEventHandler<EquippedBase<T>, EquippedEventArgs<T>> Unequipped;

        /// <summary>
        /// Gets the item at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Slot to get the item of.</param>
        /// <returns>Item at the specified <paramref name="slot"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
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
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Gets all of the equipped items in this collection.
        /// </summary>
        public IEnumerable<T> Items
        {
            get
            {
                for (var i = 0; i < _equipped.Length; i++)
                {
                    if (_equipped[i] != null)
                        yield return _equipped[i];
                }
            }
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
        /// When overridden in the derived class, handles when this object is disposed.
        /// </summary>
        /// <param name="disposeManaged">True if dispose was called directly; false if this object was garbage collected.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
        }

        /// <summary>
        /// Gets the EquipmentSlot of the specified <paramref name="item"/> in this EquippedBase.
        /// </summary>
        /// <param name="item">Item to find the EquipmentSlot for.</param>
        /// <returns>EquipmentSlot that the <paramref name="item"/> is in this EquippedBase.</returns>
        /// <exception cref="ArgumentException">Specified item could not be found in this EquippedBase.</exception>
        public EquipmentSlot GetSlot(T item)
        {
            for (var i = 0; i < _equipped.Length; i++)
            {
                if (item == this[i])
                    return (EquipmentSlot)i;
            }

            throw new ArgumentException("Specified item could not be found in this EquippedBase.");
        }

        void InternalDispose(bool disposeManaged)
        {
            foreach (var item in _equipped.Where(x => x != null))
            {
                item.Disposed -= ItemDisposeHandler;
            }

            Dispose(disposeManaged);
        }

        /// <summary>
        /// Handles when an ItemEntity being handled by this <see cref="EquippedBase{T}"/> is disposed while still equipped.
        /// </summary>
        /// <param name="sender">Item that was disposed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void ItemDisposeHandler(Entity sender, EventArgs e)
        {
            var item = (T)sender;

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
            for (var i = 0; i < _equipped.Length; i++)
            {
                var item = this[i];
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
        /// <param name="checkIfCanEquip">If true, the specified <paramref name="item"/> will
        /// be checked if it can be euqipped with <see cref="CanEquip"/>. If false, this additional
        /// check will be completely bypassed.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added to the specified
        /// <paramref name="slot"/>, else false. When false, it is guarenteed the equipment will
        /// not have been modified.</returns>
        protected bool TrySetSlot(EquipmentSlot slot, T item, bool checkIfCanEquip = true)
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
            var index = slot.GetValue();

            // If the slot is equal to the value we are trying to set it, we never want
            // to do anything extra, so just abort
            var currentItem = this[index];
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
                item.Disposed -= ItemDisposeHandler;
                item.Disposed += ItemDisposeHandler;

                // Set the item into the slot
                _equipped[index] = item;

                OnEquipped(item, slot);

                if (Equipped != null)
                    Equipped.Raise(this, new EquippedEventArgs<T>(item, slot));
            }
            else
            {
                // Remove item (set to null)

                // Check if the item can be removed
                if (!CanRemove(slot))
                    return false;

                var oldItem = _equipped[index];

                // Remove the listener for the OnDispose event
                oldItem.Disposed -= ItemDisposeHandler;

                // Remove the item
                _equipped[index] = null;

                OnUnequipped(oldItem, slot);

                if (Unequipped != null)
                    Unequipped.Raise(this, new EquippedEventArgs<T>(oldItem, slot));
            }

            // Slot setting was successful (since we always aborted early with false if it wasn't)
            return true;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            _disposed = true;

            GC.SuppressFinalize(this);

            InternalDispose(true);
        }

        #endregion

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
            for (var i = 0; i < _equipped.Length; i++)
            {
                var item = this[i];
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