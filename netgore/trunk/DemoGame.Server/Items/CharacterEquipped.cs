using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DemoGame.Server.Queries;
using log4net;
using NetGore;

namespace DemoGame.Server
{
    public abstract class CharacterEquipped : EquippedBase<ItemEntity>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _character;

        protected CharacterEquipped(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;

            AddListeners();
        }

        public int GetStatBonus(StatType statType)
        {
            var contains = this.Where(item => item.Stats.Contains(statType));
            int sum = contains.Sum(item => item.Stats[statType]);
            return sum;
        }

        public DBController DBController
        {
            get { return Character.DBController; }
        }

        /// <summary>
        /// Loads the User's equipped items.
        /// </summary>
        public void Load()
        {
            if (!Character.IsPersistent)
                throw new Exception("Cannot call Load() when the Character's state is not persistent!");

            var items = DBController.GetQuery<SelectCharacterEquippedItemsQuery>().Execute(Character.ID);

            // Remove the listeners since we don't want to update the database when loading
            RemoveListeners();

            // Load all the items
            foreach (var item in items)
            {
                ItemEntity itemEntity = new ItemEntity(item.Value);
                TrySetSlot(item.Key, itemEntity);
                SendSlotUpdate(item.Key, itemEntity.GraphicIndex);
            }

            // Add the listeners back
            AddListeners();
        }

        void AddListeners()
        {
            OnEquip += CharacterEquipped_OnEquip;
            OnRemove += CharacterEquipped_OnRemove;
        }

        void RemoveListeners()
        {
            OnEquip -= CharacterEquipped_OnEquip;
            OnRemove -= CharacterEquipped_OnRemove;
        }

        protected virtual void SendSlotUpdate(EquipmentSlot slot, GrhIndex? graphicIndex)
        {
        }

        void CharacterEquipped_OnEquip(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            if (item == null)
            {
                Debug.Fail("Parameter `item` should never be null.");
                return;
            }

            if (Character.IsPersistent)
            {
                var values = new InsertCharacterEquippedItemQuery.QueryArgs(Character.ID, item.ID, slot);
                DBController.GetQuery<InsertCharacterEquippedItemQuery>().Execute(values);
            }

            SendSlotUpdate(slot, item.GraphicIndex);
        }

        void CharacterEquipped_OnRemove(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            if (Character.IsPersistent)
            {
                DBController.GetQuery<DeleteCharacterEquippedItemQuery>().Execute(item.ID);
            }

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
        }

        /// <summary>
        /// Gets the Character that this UserEquipped belongs to.
        /// </summary>
        public Character Character
        {
            get { return _character; }
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
    }
}
