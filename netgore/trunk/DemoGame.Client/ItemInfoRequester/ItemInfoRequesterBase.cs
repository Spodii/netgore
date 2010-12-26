using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages sending requests to the server for the <see cref="IItemTable"/> for items in various collections.
    /// </summary>
    /// <typeparam name="T">The slot type.</typeparam>
    public abstract class ItemInfoRequesterBase<T>
    {
        readonly ItemTable _reusableItemInfo = new ItemTable();
        readonly INetworkSender _socket;

        bool _isItemInfoSet;
        IItemTable _itemInfo;
        bool _needToRequest;
        T _slot;
        object _slotObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfoRequesterBase{T}"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <exception cref="ArgumentNullException"><paramref name="socket" /> is <c>null</c>.</exception>
        protected ItemInfoRequesterBase(INetworkSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;
        }

        /// <summary>
        /// Gets the current <see cref="IItemTable"/>. Can be null.
        /// </summary>
        public IItemTable CurrentItemInfoInfo
        {
            get { return _itemInfo; }
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
        public INetworkSender Socket
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
        /// When overridden in the derived class, gets the object in the <paramref name="slot"/>. It doesn't matter what kind of object
        /// is used, just as long as it is an object that can be used to determine if the slot's contents have changed.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>An object representing the item in the <paramref name="slot"/>. Can be null if the slot is empty.</returns>
        protected abstract object GetSlotObject(T slot);

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="slot"/> contains a null
        /// <see cref="IItemTable"/>. This allows for bypassing having to request the item info for a slot that
        /// is known to be empty.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>True if the <see cref="slot"/> is known to be null; otherwise false.</returns>
        protected abstract bool IsSlotNull(T slot);

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
                _itemInfo = itemInfo;
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
                _itemInfo = _reusableItemInfo;
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
            // If the item in the slot is null, don't even bother - just return a null IItemTable
            if (IsSlotNull(slot))
            {
                itemInfo = null;
                return true;
            }

            // Check if the slot to get the information has changed, or if the item in the slot is different
            var currSlotObj = GetSlotObject(slot);
            if (!_slot.Equals(slot) || (_slotObj != GetSlotObject(slot)))
            {
                _slot = slot;
                _needToRequest = true;
                _isItemInfoSet = false;
                _slotObj = currSlotObj;
            }

            // Send the request for the slot if it hasn't been made yet
            if (_needToRequest)
            {
                // Send a request to the server for the information on the item in the slot
                using (var pw = GetRequest(_slot))
                {
                    Socket.Send(pw, ClientMessageType.GUIItemInfoRequest);
                }

                _needToRequest = false;
                itemInfo = null;
                return false;
            }

            itemInfo = _itemInfo;
            return _isItemInfoSet;
        }

        /// <summary>
        /// Unsets the current <see cref="IItemTable"/>, forcing the data to be reacquired.
        /// </summary>
        public void Unset()
        {
            _slot = default(T);
            _itemInfo = null;
            _slotObj = null;
            _needToRequest = true;
            _isItemInfoSet = false;
        }
    }
}