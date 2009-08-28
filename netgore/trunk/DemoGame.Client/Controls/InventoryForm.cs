using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryForm"/> class.
        /// </summary>
        /// <param name="itemInfoTooltip">The item info tooltip.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public InventoryForm(ItemInfoTooltip itemInfoTooltip, Vector2 position, Control parent)
            : base(parent.GUIManager, "Inventory", position, new Vector2(200, 200), parent)
        {
            if (itemInfoTooltip == null)
                throw new ArgumentNullException("itemInfoTooltip");

            _itemInfoTooltip = itemInfoTooltip;

            Vector2 itemsSize = _columns * new Vector2(_itemWidth, _itemHeight);
            Vector2 paddingSize = (_columns + 1) * new Vector2(_sepX, _sepY);
            Vector2 borderSize = (Border != null ? Border.Size : Vector2.Zero);
            Size = itemsSize + paddingSize + borderSize;

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

                new InventoryItemPB(this, pos, new InventorySlot(i));
            }
        }

        void InventoryItemPB_OnMouseEnter(object sender, MouseEventArgs e)
        {
            InventorySlot slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseEnter(sender, ItemInfoSource.Inventory, (int)slot);
        }

        void InventoryItemPB_OnMouseLeave(object sender, MouseEventArgs e)
        {
            InventorySlot slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseLeave(sender, ItemInfoSource.Inventory, (int)slot);
        }

        void InventoryItemPB_OnMouseMove(object sender, MouseEventArgs e)
        {
            InventorySlot slot = ((InventoryItemPB)sender).Slot;

            if (Inventory[slot] == null)
                return;

            ItemInfoTooltip.HandleMouseMove(sender, ItemInfoSource.Inventory, (int)slot);
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

        class InventoryItemPB : PictureBox
        {
            readonly InventoryForm _invForm;

            readonly InventorySlot _slot;

            public InventorySlot Slot
            {
                get { return _slot; }
            }

            public InventoryItemPB(InventoryForm parent, Vector2 pos, InventorySlot slot)
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