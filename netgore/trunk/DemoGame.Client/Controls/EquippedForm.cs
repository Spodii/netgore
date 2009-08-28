using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    delegate void RequestUnequipHandler(EquippedForm equippedForm, EquipmentSlot slot);

    class EquippedForm : Form, IRestorableSettings
    {
        const float _itemHeight = 32; // Height of each item slot
        const float _itemWidth = 32; // Width of each item slot

        readonly ItemInfoTooltip _itemInfoTooltip;
        UserEquipped _userEquipped;

        public event RequestUnequipHandler OnRequestUnequip;

        public ItemInfoTooltip ItemInfoTooltip
        {
            get { return _itemInfoTooltip; }
        }

        public UserEquipped UserEquipped
        {
            get { return _userEquipped; }
            set { _userEquipped = value; }
        }

        public EquippedForm(ItemInfoTooltip itemInfoTooltip, Vector2 position, Control parent)
            : base(parent.GUIManager, "Equipment", position, new Vector2(200, 200), parent)
        {
            if (itemInfoTooltip == null)
                throw new ArgumentNullException("itemInfoTooltip");

            _itemInfoTooltip = itemInfoTooltip;

            CreateItemSlots();
        }

        void CreateItemSlots()
        {
            CreateItemSlots(0, 0, EquipmentSlot.Head);
            CreateItemSlots(1, 0, EquipmentSlot.Body);

            CreateItemSlots(0, 1, EquipmentSlot.LeftHand);
            CreateItemSlots(1, 1, EquipmentSlot.RightHand);
        }

        void CreateItemSlots(int row, int column, EquipmentSlot slot)
        {
            Vector2 size = new Vector2(_itemWidth, _itemHeight);
            Vector2 loc = new Vector2(row, column);
            Vector2 pos = size * loc;

            new EquippedItemPB(this, pos, slot);
        }

        void EquippedItemPB_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (UserEquipped == null)
                return;

            EquipmentSlot slot = ((EquippedItemPB)sender).Slot;
            if (UserEquipped[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseEnter(sender, ItemInfoSource.Equipped, slot.GetValue());
        }

        void EquippedItemPB_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (UserEquipped == null)
                return;

            EquipmentSlot slot = ((EquippedItemPB)sender).Slot;
            ItemInfoTooltip.HandleMouseLeave(sender, ItemInfoSource.Equipped, slot.GetValue());
        }

        void EquippedItemPB_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (UserEquipped == null)
                return;

            EquipmentSlot slot = ((EquippedItemPB)sender).Slot;
            if (UserEquipped[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseMove(sender, ItemInfoSource.Equipped, slot.GetValue());
        }

        void EquippedItemPB_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            if (UserEquipped == null)
                return;

            EquipmentSlot slot = ((EquippedItemPB)sender).Slot;
            if (UserEquipped[slot] == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (OnRequestUnequip != null)
                        OnRequestUnequip(this, slot);
                    break;
            }
        }

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(items.AsFloat("X", Position.X), items.AsFloat("Y", Position.Y));
            IsVisible = items.AsBool("IsVisible", IsVisible);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            {
                new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible)
            };
        }

        #endregion

        class EquippedItemPB : PictureBox
        {
            readonly EquippedForm _equippedForm;

            readonly EquipmentSlot _slot;

            public EquipmentSlot Slot
            {
                get { return _slot; }
            }

            public EquippedItemPB(EquippedForm parent, Vector2 pos, EquipmentSlot slot)
                : base(pos, null, new Vector2(_itemWidth, _itemHeight), parent)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _equippedForm = parent;
                _slot = slot;

                OnMouseLeave += _equippedForm.EquippedItemPB_OnMouseLeave;
                OnMouseEnter += _equippedForm.EquippedItemPB_OnMouseEnter;
                OnMouseMove += _equippedForm.EquippedItemPB_OnMouseMove;
                OnMouseUp += _equippedForm.EquippedItemPB_OnMouseUp;
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                UserEquipped equipped = _equippedForm.UserEquipped;
                if (equipped == null)
                    return;

                if (equipped[_slot] == null)
                    return;

                ItemEntity item = equipped[_slot];
                if (item == null)
                    return;

                // Draw the item in the center of the slot
                Vector2 offset = new Vector2(_itemWidth, _itemHeight);
                offset -= item.Grh.Size;
                offset /= 2;

                item.Draw(spriteBatch, ScreenPosition + offset);
            }
        }
    }
}