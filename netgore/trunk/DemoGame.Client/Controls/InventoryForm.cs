using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    class InventoryForm : Form, IRestorableSettings
    {
        const int _columns = 6; // Number of items on each row
        const float _itemHeight = 32; // Height of each item slot
        const float _itemWidth = 32; // Width of each item slot
        const float _sepX = 2; // Amount of space on the X axis between each item
        const float _sepY = 2; // Amount of space on the Y axis between each item

        readonly ItemInfoTooltip _itemInfoTooltip;

        Inventory _inventory;

        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public ItemInfoTooltip ItemInfoTooltip
        {
            get { return _itemInfoTooltip; }
        }

        public InventoryForm(ItemInfoTooltip itemInfoTooltip, Vector2 position, Control parent)
            : base(parent.GUIManager, "Inventory", position, new Vector2(200, 200), parent)
        {
            if (itemInfoTooltip == null)
                throw new ArgumentNullException("itemInfoTooltip");

            _itemInfoTooltip = itemInfoTooltip;

            Vector2 itemsSize = _columns * new Vector2(_itemWidth, _itemHeight);
            Vector2 paddingSize = 2 * _columns * new Vector2(_sepX, _sepY);
            Size = itemsSize + paddingSize;

            CreateItemSlots();
        }

        void CreateItemSlots()
        {
            Vector2 offset = new Vector2(_sepX, _sepY);

            for (int i = 0; i < Inventory.MaxInventorySize; i++)
            {
                int x = i % _columns;
                int y = i / _columns;
                Vector2 pos = offset + new Vector2(x * (_itemWidth + _sepX), y * (_itemHeight + _sepY));

                new InventoryItemPB(this, pos, (byte)i);
            }
        }

        void InventoryItemPB_OnMouseEnter(object sender, MouseEventArgs e)
        {
            byte slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseEnter(sender, ItemInfoSource.Inventory, slot);
        }

        void InventoryItemPB_OnMouseLeave(object sender, MouseEventArgs e)
        {
            byte slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseLeave(sender, ItemInfoSource.Inventory, slot);
        }

        void InventoryItemPB_OnMouseMove(object sender, MouseEventArgs e)
        {
            byte slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseMove(sender, ItemInfoSource.Inventory, slot);
        }

        void InventoryItemPB_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            InventoryItemPB itemPB = (InventoryItemPB)sender;

            if (e.Button == MouseButtons.Right)
            {
                if (GUIManager.KeysPressed.Contains(Keys.LeftShift) || GUIManager.KeysPressed.Contains(Keys.RightShift))
                {
                    // Drop
                    Inventory.Drop(itemPB.Slot);
                }
                else
                {
                    // Use
                    Inventory.Use(itemPB.Slot);
                }
            }
        }

        #region IRestorableSettings Members

        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
            IsVisible = bool.Parse(items["IsVisible"]);
        }

        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
                   { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class InventoryItemPB : PictureBox
        {
            readonly InventoryForm _invForm;

            readonly byte _slot;

            public byte Slot
            {
                get { return _slot; }
            }

            public InventoryItemPB(InventoryForm parent, Vector2 pos, byte slot)
                : base(pos, null, new Vector2(_itemWidth, _itemHeight), parent)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _invForm = parent;
                _slot = slot;

                OnMouseLeave += _invForm.InventoryItemPB_OnMouseLeave;
                OnMouseEnter += _invForm.InventoryItemPB_OnMouseEnter;
                OnMouseMove += _invForm.InventoryItemPB_OnMouseMove;
                OnMouseUp += _invForm.InventoryItemPB_OnMouseUp;

                Skin.OnChange += Skin_OnChange;
                LoadSprite();
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                Inventory inv = _invForm.Inventory;
                if (inv == null)
                    return;

                if (inv[_slot] == null)
                    return;

                ItemEntity item = inv[_slot];
                if (item == null)
                    return;

                // Draw the item in the center of the slot
                Vector2 offset = new Vector2(_itemWidth, _itemHeight);
                offset -= item.Grh.Size;
                offset /= 2;

                item.Draw(spriteBatch, ScreenPosition + offset);

                // Draw the amount
                if (item.Amount > 1)
                    DrawShadedText(spriteBatch, item.Amount.ToString(), ScreenPosition, Color.White, Color.Black);
            }

            void DrawShadedText(SpriteBatch sb, string txt, Vector2 pos, Color foreground, Color background)
            {
                sb.DrawString(GUIManager.Font, txt, pos + new Vector2(1, 0), background);
                sb.DrawString(GUIManager.Font, txt, pos + new Vector2(0, 1), background);
                sb.DrawString(GUIManager.Font, txt, pos + new Vector2(-1, 0), background);
                sb.DrawString(GUIManager.Font, txt, pos + new Vector2(0, -1), background);
                sb.DrawString(GUIManager.Font, txt, pos, foreground);
            }

            void LoadSprite()
            {
                GrhData grhData = Skin.GetSkinGrhData(string.Empty, "item_slot");
                if (grhData == null)
                    return;

                Sprite = new Grh(grhData);
            }

            void Skin_OnChange(string newSkin, string oldSkin)
            {
                LoadSprite();
            }
        }
    }
}