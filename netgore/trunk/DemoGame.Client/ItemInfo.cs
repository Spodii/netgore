using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using log4net;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains the extended information for an item. Intended for requesting and displaying the
    /// information of the user's inventory, another character's inventory, etc. Can only serve a single
    /// item at a time (obviously).
    /// </summary>
    public class ItemInfo
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ItemStats _stats = new ItemStats();
        string _description;
        bool _isUpdated;
        string _name;
        int _slot;
        ItemInfoSource _source;
        int _value;

        /// <summary>
        /// Gets the item's description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets if the last item information request has been handled. If false, the values
        /// for this ItemInfo are likely invalid.
        /// </summary>
        public bool IsUpdated
        {
            get { return _isUpdated; }
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the slot from the Source that the ItemInfo came from.
        /// </summary>
        public int Slot
        {
            get { return _slot; }
        }

        /// <summary>
        /// Gets or sets the socket used to request the ItemInfo.
        /// </summary>
        public ISocketSender Socket { get; set; }

        /// <summary>
        /// Gets the source that the ItemInfo came from.
        /// </summary>
        public ItemInfoSource Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Gets the stats of the item.
        /// </summary>
        public ItemStats Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="socket">Socket used to request the ItemInfo.</param>
        public ItemInfo(ISocketSender socket)
        {
            Socket = socket;
        }

        /// <summary>
        /// Sends a request through the Socket to get the item information for a given user equipment slot.
        /// </summary>
        /// <param name="slot">Equipment slot to get the item information for.</param>
        public void GetEquipmentItemInfo(EquipmentSlot slot)
        {
            StartRequest();

            // Set the source
            _source = ItemInfoSource.Equipped;
            _slot = slot.GetIndex();

            // Check for a valid slot
            if (!slot.IsDefined())
            {
                const string errmsg = "Invalid equipment slot `{0}`.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, slot);
                return;
            }

            // Send the request
            using (PacketWriter pw = ClientPacket.GetEquipmentItemInfo(slot))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Sends a request through the Socket to get the item information for a given user inventory slot.
        /// </summary>
        /// <param name="slot">Inventory slot to get the item information for.</param>
        public void GetInventoryItemInfo(byte slot)
        {
            StartRequest();

            // Set the source
            _source = ItemInfoSource.Equipped;
            _slot = slot;

            // Check for a valid slot
            if (slot >= Inventory.MaxInventorySize)
            {
                const string errmsg = "Invalid inventory slot `{0}`.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, slot);
                return;
            }

            // Send the request
            using (PacketWriter pw = ClientPacket.GetInventoryItemInfo(slot))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Sets the ItemInfo as being fully updated.
        /// </summary>
        public void SetAsUpdated()
        {
            _isUpdated = true;
        }

        /// <summary>
        /// Sets the information of the item.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="desc">Item's description.</param>
        /// <param name="value">Value of the item.</param>
        public void SetItemInfo(string name, string desc, int value)
        {
            _name = name;
            _description = desc;
            _value = value;
        }

        void StartRequest()
        {
            _isUpdated = false;

            // Check for a valid socket
            if (Socket == null || !Socket.IsConnected)
            {
                const string errmsg = "Socket is not either null or not connected. Request failed.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return;
            }
        }
    }
}