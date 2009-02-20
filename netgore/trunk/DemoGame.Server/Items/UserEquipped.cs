using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and handles the collection of a single User's equipped items.
    /// </summary>
    public class UserEquipped : EquippedBase<ItemEntity>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly User _user;

        DeleteUserEquippedQuery DeleteUserEquipped
        {
            get { return User.Map.World.Parent.DBController.DeleteUserEquipped; }
        }

        InsertUserEquippedQuery InsertUserEquipped
        {
            get { return User.Map.World.Parent.DBController.InsertUserEquipped; }
        }

        SelectItemQuery SelectItem
        {
            get { return User.Map.World.Parent.DBController.SelectItem; }
        }

        /// <summary>
        /// Gets the User that this UserEquipped belongs to.
        /// </summary>
        public User User
        {
            get { return _user; }
        }

        /// <summary>
        /// UserEquipped constructor.
        /// </summary>
        /// <param name="user">User that this UserEquipped belongs to.</param>
        public UserEquipped(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            AddListeners();
            _user = user;
        }

        void AddListeners()
        {
            OnEquip += UserEquipped_OnEquip;
            OnRemove += UserEquipped_OnRemove;
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

            return User.Inventory.CanAdd((ItemEntity)item);
        }

        public int GetStatBonus(StatType statType)
        {
            var contains = this.Where(item => item.Stats.Contains(statType));
            int sum = contains.Sum(item => item.Stats[statType]);
            return sum;
        }

        /// <summary>
        /// Loads the User's equipped items.
        /// </summary>
        public void Load()
        {
            var items = SelectItem.ExecuteUserEquippedItems(User.Guid);

            // Remove the listeners since we don't want to update the database when loading
            RemoveListeners();

            // Load all the items
            foreach (var item in items)
            {
                ItemEntity itemEntity = new ItemEntity(item.Value);
                TrySetSlot(item.Key, itemEntity);
                SendClientUpdate(item.Key, itemEntity.GraphicIndex);
            }

            // Add the listeners back
            AddListeners();
        }

        void RemoveListeners()
        {
            OnEquip -= UserEquipped_OnEquip;
            OnRemove -= UserEquipped_OnRemove;
        }

        void SendClientUpdate(EquipmentSlot slot, ushort? graphicIndex)
        {
            using (PacketWriter msg = ServerPacket.UpdateEquipmentSlot(slot, graphicIndex))
            {
                User.Send(msg);
            }
        }

        void UserEquipped_OnEquip(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            if (item == null)
            {
                Debug.Fail("Parameter `item` should never be null.");
                return;
            }

            InsertUserEquippedValues values = new InsertUserEquippedValues(User.Guid, item.Guid, slot);
            InsertUserEquipped.Execute(values);

            SendClientUpdate(slot, item.GraphicIndex);
        }

        void UserEquipped_OnRemove(EquippedBase<ItemEntity> equippedBase, ItemEntity item, EquipmentSlot slot)
        {
            DeleteUserEquipped.Execute(item.Guid);
            ItemEntity remainder = User.Inventory.Add(item);

            SendClientUpdate(slot, null);

            if (remainder != null)
            {
                const string errmsg =
                    "User `{0}` removed equipped item `{1}` from slot `{2}`, " +
                    "but not all could be added back to their Inventory.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, User, item, slot);

                // Make the User drop the remainder
                User.DropItem(remainder);
            }
        }
    }
}