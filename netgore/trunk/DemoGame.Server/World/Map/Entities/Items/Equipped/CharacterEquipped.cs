using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for a <see cref="EquippedBase{T}"/> for any kind of <see cref="Character"/>.
    /// </summary>
    public abstract class CharacterEquipped : EquippedBase<ItemEntity>, IModStatContainer<StatType>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _character;
        readonly EquippedPaperDoll _paperDoll;

        /// <summary>
        /// When true, the <see cref="OnEquipped"/> and <see cref="OnUnequipped"/> methods will be ignored.
        /// </summary>
        bool _ignoreEquippedBaseEvents = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEquipped"/> class.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the instance is for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="character" /> is <c>null</c>.</exception>
        protected CharacterEquipped(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _paperDoll = new EquippedPaperDoll(Character);
        }

        /// <summary>
        /// Gets the Character that this UserEquipped belongs to.
        /// </summary>
        public Character Character
        {
            get { return _character; }
        }

        public IDbController DbController
        {
            get { return Character.DbController; }
        }

        /// <summary>
        /// Gets if the state of this <see cref="CharacterEquipped"/> is persistent.
        /// </summary>
        public bool IsPersistent
        {
            get { return Character.IsPersistent; }
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="item"/> can be 
        /// equipped at all by the owner of this EquippedBase.
        /// </summary>
        /// <param name="item">Item to check if able be equip.</param>
        /// <returns>True if the <paramref name="item"/> can be equipped, else false.</returns>
        public override bool CanEquip(ItemEntity item)
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the item in the given <paramref name="slot"/> 
        /// can be removed properly.
        /// </summary>
        /// <param name="slot">Slot of the item to be removed.</param>
        /// <returns>True if the item can be properly removed, else false.</returns>
        protected override bool CanRemove(EquipmentSlot slot)
        {
            var item = this[slot];
            if (item == null)
                return true;

            return Character.Inventory.CanAdd(item);
        }

        /// <summary>
        /// When overridden in the derived class, handles when this object is disposed.
        /// </summary>
        /// <param name="disposeManaged">True if dispose was called directly; false if this object was garbage collected.</param>
        protected override void Dispose(bool disposeManaged)
        {
            base.Dispose(disposeManaged);

            // If not persistent, destroy every item in the collection
            if (!IsPersistent)
                RemoveAll(true);
        }

        /// <summary>
        /// Equips an <paramref name="item"/>, automatically choosing the EquipmentSlot to use.
        /// </summary>
        /// <param name="item">Item to be equipped.</param>
        /// <returns>True if the item was successfully equipped, else false.</returns>
        public bool Equip(ItemEntity item)
        {
            // Do not equip invalid items
            if (item == null)
                return false;

            // Check that the item can be equipped at all
            if (!CanEquip(item))
                return false;

            // Get the possible slots
            var slots = GetPossibleSlots(item).ToImmutable();

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
                    foreach (var slot in emptySlots)
                    {
                        if (TrySetSlot(slot, item, false))
                            return true;
                    }

                    // Couldn't set on an empty slot, or there was no empty slots, so try all the non-empty slots
                    foreach (var slot in slots.Except(emptySlots))
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
        protected virtual IEnumerable<EquipmentSlot> GetPossibleSlots(ItemEntity item)
        {
            return item.Type.GetPossibleSlots();
        }

        /// <summary>
        /// Loads the Character's equipped items. The Character that this CharacterEquipped belongs to
        /// must be persistent since there is nothing for a non-persistent Character to load.
        /// </summary>
        public void Load()
        {
            if (!IsPersistent)
            {
                const string errmsg = "Don't call Load() when the Character's state is not persistent!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            var items = DbController.GetQuery<SelectCharacterEquippedItemsQuery>().Execute(Character.ID);

            // Remove the listeners since we don't want to update the database when loading
            _ignoreEquippedBaseEvents = true;

            // Load all the items
            foreach (var item in items)
            {
                var itemEntity = new ItemEntity(item.Value);
                if (TrySetSlot(item.Key, itemEntity))
                {
                    SendSlotUpdate(item.Key, itemEntity.GraphicIndex);
                    _paperDoll.NotifyAdded(item.Key, itemEntity);
                }
                else
                    Debug.Fail("Uhm, the Character couldn't load their equipped item. What should we do...?");
            }

            _ignoreEquippedBaseEvents = false;
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has been equipped.
        /// </summary>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        protected override void OnEquipped(ItemEntity item, EquipmentSlot slot)
        {
            if (_ignoreEquippedBaseEvents)
                return;

            Debug.Assert(item != null);

            item.IsPersistent = IsPersistent;

            if (IsPersistent)
            {
                var values = new CharacterEquippedTable(Character.ID, item.ID, slot);
                DbController.GetQuery<InsertCharacterEquippedItemQuery>().Execute(values);
            }

            SendSlotUpdate(slot, item.GraphicIndex);

            _paperDoll.NotifyAdded(slot, item);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has been removed.
        /// </summary>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        protected override void OnUnequipped(ItemEntity item, EquipmentSlot slot)
        {
            if (_ignoreEquippedBaseEvents)
                return;

            Debug.Assert(item != null);

            if (IsPersistent)
                DbController.GetQuery<DeleteCharacterEquippedItemQuery>().Execute(Character.ID, slot);

            SendSlotUpdate(slot, null);

            // Do not try working with a disposed ItemEntity! Instead, just let it die off.
            if (item.IsDisposed)
                return;

            var remainder = Character.Inventory.TryAdd(item);

            if (remainder != null)
            {
                const string errmsg =
                    "Character `{0}` removed equipped item `{1}` from slot `{2}`, " +
                    "but not all could be added back to their Inventory.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, Character, item, slot);

                // Make the Character drop the remainder
                Character.DropItem(remainder);
            }

            _paperDoll.NotifyRemoved(slot);

            // Make sure we dispose of items where the amount hit 0
            if (item.Amount == 0)
                item.Destroy();
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner of this object instance
        /// that an equipment slot has changed.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="graphicIndex">The new graphic index of the slot.</param>
        protected virtual void SendSlotUpdate(EquipmentSlot slot, GrhIndex? graphicIndex)
        {
        }

        /// <summary>
        /// Sends the paper-doll information for this <see cref="Character"/> to a specific client.
        /// </summary>
        /// <param name="client">The client to send this <see cref="Character"/>'s paper-doll information to.</param>
        public void SynchronizePaperdollTo(INetworkSender client)
        {
            _paperDoll.SynchronizeBodyLayersTo(client);
        }

        #region IModStatContainer<StatType> Members

        /// <summary>
        /// Gets the modifier value for the given <paramref name="statType"/>, where a positive value adds to the
        /// mod stat value, a negative value subtracts from the mod stat value, and a value of 0 does not modify
        /// the mod stat value.
        /// </summary>
        /// <param name="statType">The <see cref="StatType"/> to get the modifier value for.</param>
        /// <returns>
        /// The modifier value for the given <paramref name="statType"/>.
        /// </returns>
        public int GetStatModBonus(StatType statType)
        {
            var sum = 0;

            foreach (var item in Items)
            {
                sum += item.BaseStats[statType];
            }

            return sum;
        }

        #endregion

        /// <summary>
        /// Handles the synchronization of a <see cref="Character"/>'s paper-doll layers.
        /// </summary>
        class EquippedPaperDoll
        {
            static readonly int _maxSlotValue = EnumHelper<EquipmentSlot>.MaxValue + 1;

            readonly string[] _bodies;
            readonly Character _character;

            /// <summary>
            /// Initializes a new instance of the <see cref="EquippedPaperDoll"/> class.
            /// </summary>
            /// <param name="character">The <see cref="Character"/>.</param>
            public EquippedPaperDoll(Character character)
            {
                _bodies = new string[_maxSlotValue];
                _character = character;
            }

            /// <summary>
            /// Notifies this object when an item has been added into an <see cref="EquipmentSlot"/>.
            /// </summary>
            /// <param name="slot">The <see cref="EquipmentSlot"/> the item was added to.</param>
            /// <param name="item">The item.</param>
            public void NotifyAdded(EquipmentSlot slot, IItemTable item)
            {
                // Get the ID of the slot
                var slotID = slot.GetValue();

                // Check if there is a paper-doll item in the slot already. If so, remove it.
                if (_bodies[slotID] != null)
                    NotifyRemoved(slot);

                // Check that the new item has a paper-doll value
                if (string.IsNullOrEmpty(item.EquippedBody))
                    return;

                // The new item has a paper-doll value, so add it and synchronize
                _bodies[slotID] = item.EquippedBody;
                SynchronizeBodyLayers();
            }

            /// <summary>
            /// Notifies this object when an item has been removed from an <see cref="EquipmentSlot"/>.
            /// </summary>
            /// <param name="slot">The <see cref="EquipmentSlot"/> the item was removed from.</param>
            public void NotifyRemoved(EquipmentSlot slot)
            {
                // Get the ID of the slot
                var slotID = slot.GetValue();

                // Check if there was a paper-doll item in the slot
                if (_bodies[slotID] == null)
                    return;

                // There was a paper-doll item in the slot, so remove it and synchronize
                _bodies[slotID] = null;
                SynchronizeBodyLayers();
            }

            /// <summary>
            /// Handles synchronizing the paper-doll information to other clients.
            /// </summary>
            void SynchronizeBodyLayers()
            {
                // Get the map
                var map = _character.Map;
                if (map == null)
                    return;

                // Send the list of set paper-doll values
                using (var pw = ServerPacket.SetCharacterPaperDoll(_character.MapEntityIndex, _bodies.Where(x => x != null)))
                {
                    _character.Map.Send(pw, ServerMessageType.MapDynamicEntityProperty);
                }
            }

            /// <summary>
            /// Handles synchronizing the paper-doll information to a single client.
            /// </summary>
            /// <param name="client">The client to send the information to</param>
            internal void SynchronizeBodyLayersTo(INetworkSender client)
            {
                // Get the values to send
                var bodiesToSend = _bodies.Where(x => x != null);
                if (bodiesToSend.IsEmpty())
                    return;

                // Send the paper-doll information
                using (var pw = ServerPacket.SetCharacterPaperDoll(_character.MapEntityIndex, bodiesToSend))
                {
                    client.Send(pw, ServerMessageType.MapDynamicEntityProperty);
                }
            }
        }
    }
}