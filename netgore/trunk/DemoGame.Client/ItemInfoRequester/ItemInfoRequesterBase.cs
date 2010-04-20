using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages requests for the <see cref="IItemTable"/> for items in various collections.
    /// </summary>
    abstract class ItemInfoRequesterBase<T>
    {
        readonly ItemTable _reusableItemInfo = new ItemTable();
        readonly ISocketSender _socket;
        bool _isItemInfoSet;

        IItemTable _item;
        bool _needToRequest;
        T _slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfoRequesterBase{T}"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        protected ItemInfoRequesterBase(ISocketSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;
        }

        /// <summary>
        /// Gets the current <see cref="IItemTable"/>. Can be null.
        /// </summary>
        public IItemTable CurrentItemInfo
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets the current slot being handled.
        /// </summary>
        public T CurrentSlot
        {
            get { return _slot; }
        }

        /// <summary>
        /// The socket used to send the requests.
        /// </summary>
        public ISocketSender Socket
        {
            get { return _socket; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="BitStream"/> containing the data
        /// needed to make a request for the given slot's <see cref="IItemTable"/>.
        /// </summary>
        /// <param name="slotToRequest">The slot to request the <see cref="IItemTable"/> for.</param>
        /// <returns>The <see cref="BitStream"/> containing the data needed to make a request for
        /// the given slot's <see cref="IItemTable"/>.</returns>
        protected abstract BitStream GetRequest(T slotToRequest);

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="slot"/> contains a null
        /// <see cref="IItemTable"/>. This allows for bypassing having to request the item info for a slot that
        /// is known to be empty.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>True if the <see cref="slot"/> is known to be null; otherwise false.</returns>
        protected virtual bool IsSlotNull(T slot)
        {
            return false;
        }

        /// <summary>
        /// Handles when the ItemInfo for the given slot has been received.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="itemInfo">The item info. Can be null, which will be equivilant to saying that the
        /// item has no information or does not exist.</param>
        public void ReceiveInfo(T slot, IItemTable itemInfo)
        {
            if (_slot.Equals(slot))
            {
                _item = itemInfo;
                _isItemInfoSet = true;
            }
        }

        /// <summary>
        /// Handles when the ItemInfo for the given slot has been received.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read the <see cref="IItemTable"/> from. An internal
        /// <see cref="IItemTable"/> instance will be used for containing the data. Must not be null.</param>
        public void ReceiveInfo(T slot, IValueReader reader)
        {
            _reusableItemInfo.ReadState(reader);

            if (_slot.Equals(slot))
            {
                _item = _reusableItemInfo;
                _isItemInfoSet = true;
            }
        }

        /// <summary>
        /// Gets the <see cref="IItemTable"/> for the item in the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the item to get the info for.</param>
        /// <param name="itemInfo">When this method returns true, contains the <see cref="IItemTable"/> for the
        /// item at the given <paramref name="slot"/>.</param>
        /// <returns>The <see cref="IItemTable"/> of the item for the given <paramref name="slot"/>, or null if
        /// the given <paramref name="slot"/> contained no item.</returns>
        public bool TryGetInfo(T slot, out IItemTable itemInfo)
        {
            // Check if the slot has changed
            if (!_slot.Equals(slot))
            {
                _slot = slot;
                _needToRequest = true;
                _isItemInfoSet = false;
            }

            // Send the request for the slot if it hasn't been made yet
            if (_needToRequest)
            {
                if (IsSlotNull(_slot))
                    ReceiveInfo(_slot, (IItemTable)null);
                else
                {
                    using (var pw = GetRequest(_slot))
                    {
                        Socket.Send(pw);
                    }

                    _needToRequest = false;
                    itemInfo = null;
                    return false;
                }
            }

            itemInfo = _item;
            return _isItemInfoSet;
        }

        /// <summary>
        /// Unsets the current <see cref="IItemTable"/>, forcing the data to be reacquired.
        /// </summary>
        public void Unset()
        {
            _slot = default(T);
            _item = null;
            _needToRequest = true;
            _isItemInfoSet = false;
        }
    }
}