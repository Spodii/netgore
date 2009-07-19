using System.Diagnostics;
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

        readonly ItemStats _baseStats = new ItemStats(StatCollectionType.Base);
        readonly ItemStats _reqStats = new ItemStats(StatCollectionType.Requirement);
        string _description;
        SPValueType _hp;
        bool _isUpdated;
        SPValueType _mp;
        string _name;
        int _slot;
        ItemInfoSource _source;
        int _value;

        /// <summary>
        /// Gets the stats of the item.
        /// </summary>
        public ItemStats BaseStats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets the item's description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        public SPValueType HP
        {
            get { return _hp; }
        }

        /// <summary>
        /// Gets if the last item information request has been handled. If false, the values
        /// for this ItemInfo are likely invalid.
        /// </summary>
        public bool IsUpdated
        {
            get { return _isUpdated; }
        }

        public SPValueType MP
        {
            get { return _mp; }
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        public ItemStats ReqStats
        {
            get { return _reqStats; }
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
        public void GetInventoryItemInfo(InventorySlot slot)
        {
            StartRequest();

            // Set the source
            _source = ItemInfoSource.Inventory;
            _slot = (int)slot;

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
        /// <param name="hp">How much HP the item recovers.</param>
        /// <param name="mp">How much MP the item recovers.</param>
        public void SetItemInfo(string name, string desc, int value, SPValueType hp, SPValueType mp)
        {
            _name = name;
            _description = desc;
            _value = value;
            _hp = hp;
            _mp = mp;
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