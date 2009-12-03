using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    public abstract class CharacterEquipped : EquippedBase<ItemEntity>, IDisposable, IModStatContainer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Character _character;

        readonly bool _isPersistent;

        readonly EquippedPaperDoll _paperDoll;
        bool _disposed = false;

        protected CharacterEquipped(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _isPersistent = character.IsPersistent;
            _paperDoll = new EquippedPaperDoll(Character);

            AddListeners();
        }

        public void SynchronizePaperdollTo(User user)
        {
            _paperDoll.SynchronizeBodyLayersTo(user);
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

        void AddListeners()
        {
            // TODO: Add two virtual methods, AfterEquip() and AfterRemove(), to replace the need for these event hooks
            OnEquip += CharacterEquipped_OnEquip;
            OnRemove += CharacterEquipped_OnRemove;
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
            ItemEntityBase item = this[slot];
            if (item == null)
                return true;

            return Character.Inventory.CanAdd((ItemEntity)item);
        }

        void CharacterEquipped_OnEquip(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            Debug.Assert(item != null);

            if (_isPersistent)
            {
                CharacterEquippedTable values = new CharacterEquippedTable(Character.ID, item.ID, slot);
                DbController.GetQuery<InsertCharacterEquippedItemQuery>().Execute(values);
            }

            SendSlotUpdate(slot, item.GraphicIndex);

            _paperDoll.NotifyAdded(slot, item);
        }

        void CharacterEquipped_OnRemove(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            Debug.Assert(item != null);

            if (_isPersistent)
                DbController.GetQuery<DeleteCharacterEquippedItemQuery>().Execute(Character.ID, slot);

            // Do not try working with a disposed ItemEntity! Instead, just let it die off.
            if (item.IsDisposed)
                return;

            ItemEntity remainder = Character.Inventory.Add(item);

            SendSlotUpdate(slot, null);

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
                item.Dispose();
        }

        /// <summary>
        /// Loads the Character's equipped items. The Character that this CharacterEquipped belongs to
        /// must be persistent since there is nothing for a non-persistent Character to load.
        /// </summary>
        public void Load()
        {
            if (!_isPersistent)
            {
                const string errmsg = "Don't call Load() when the Character's state is not persistent!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            var items = DbController.GetQuery<SelectCharacterEquippedItemsQuery>().Execute(Character.ID);

            // Remove the listeners since we don't want to update the database when loading
            RemoveListeners();

            // Load all the items
            foreach (var item in items)
            {
                ItemEntity itemEntity = new ItemEntity(item.Value);
                if (TrySetSlot(item.Key, itemEntity))
                {
                    SendSlotUpdate(item.Key, itemEntity.GraphicIndex);
                    _paperDoll.NotifyAdded(item.Key, itemEntity);
                }
                else
                {
                    Debug.Fail("Uhm, the Character couldn't load their equipped item. What should we do...?");
                }
            }

            // Add the listeners back
            AddListeners();
        }

        void RemoveListeners()
        {
            OnEquip -= CharacterEquipped_OnEquip;
            OnRemove -= CharacterEquipped_OnRemove;
        }

        protected virtual void SendSlotUpdate(EquipmentSlot slot, GrhIndex? graphicIndex)
        {
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // If the Character is not persistent, we want to dispose of every ItemEntity so it doesn't sit in the
            // database as garbage
            if (!_isPersistent)
            {
                foreach (ItemEntity item in this.Select(x => x.Value))
                {
                    item.Dispose();
                }
            }
        }

        #endregion

        #region IModStatContainer Members

        /// <summary>
        /// Gets the modifier value for the given <paramref name="statType"/>, where a positive value adds to the
        /// mod stat value, a negative value subtracts from the mod stat value, and a value of 0 does not modify
        /// the mod stat value.
        /// </summary>
        /// <param name="statType">The StatType to get the modifier value for.</param>
        /// <returns>
        /// The modifier value for the given <paramref name="statType"/>.
        /// </returns>
        public int GetStatModBonus(StatType statType)
        {
            // TODO: [STATS] This totally sucks. Add some kind of cache.
            return this.SelectMany(x => x.Value.BaseStats).Where(x => x.StatType == statType).Select(x => x.Value).Sum();
        }

        #endregion

        class EquippedPaperDoll
        {
            static readonly int _maxSlotValue = EquipmentSlotHelper.Instance.MaxValue;

            readonly string[] _bodies;
            readonly Character _character;

            public EquippedPaperDoll(Character character)
            {
                _bodies = new string[_maxSlotValue];
                _character = character;
            }

            public void NotifyAdded(EquipmentSlot slot, IItemTable item)
            {
                var slotID = slot.GetValue();

                if (_bodies[slotID] != null)
                    NotifyRemoved(slot);

                if (string.IsNullOrEmpty(item.EquippedBody))
                    return;

                _bodies[slotID] = item.EquippedBody;
                SynchronizeBodyLayers();
            }

            public void NotifyRemoved(EquipmentSlot slot)
            {
                var slotID = slot.GetValue();

                if (_bodies[slotID] == null)
                    return;

                _bodies[slotID] = null;
                SynchronizeBodyLayers();
            }

            void SynchronizeBodyLayers()
            {
                var map = _character.Map;
                if (map == null)
                    return;

                using (var pw = ServerPacket.SetCharacterPaperDoll(_character.MapEntityIndex, _bodies.Where(x => x != null)))
                {
                    _character.Map.Send(pw);
                }
            }

            internal void SynchronizeBodyLayersTo(User user)
            {
                var bodiesToSend = _bodies.Where(x => x != null);
                if (bodiesToSend.Count() == 0)
                    return;

                using (var pw = ServerPacket.SetCharacterPaperDoll(_character.MapEntityIndex, bodiesToSend))
                {
                    user.Send(pw);
                }
            }
        }
    }
}