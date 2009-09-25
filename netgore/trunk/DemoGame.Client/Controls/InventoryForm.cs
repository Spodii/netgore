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
    delegate void InventoryDropItemHandler(InventoryForm inventoryForm, InventorySlot slot);
    delegate void InventoryUseItemHandler(InventoryForm inventoryForm, InventorySlot slot);

    class InventoryForm : Form, IRestorableSettings
    {
        const int _columns = 6; // Number of items on each row
        const float _itemHeight = 32; // Height of each item slot
        const float _itemWidth = 32; // Width of each item slot
        const float _sepX = 2; // Amount of space on the X axis between each item
        const float _sepY = 2; // Amount of space on the Y axis between each item

        readonly ItemInfoRequesterBase<InventorySlot> _infoRequester;

        Inventory _inventory;

        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        /// <summary>
        /// Notifies listeners when an item was requested to be dropped.
        /// </summary>
        public event InventoryDropItemHandler OnRequestDropItem;

        /// <summary>
        /// Notifies listeners when an item was requested to be used.
        /// </summary>
        public event InventoryUseItemHandler OnRequestUseItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryForm"/> class.
        /// </summary>
        /// <param name="infoRequester">The item info tooltip.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public InventoryForm(ItemInfoRequesterBase<InventorySlot> infoRequester, Vector2 position, Control parent)
            : base(parent.GUIManager, "Inventory", position, new Vector2(200, 200), parent)
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");

            _infoRequester = infoRequester;

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

        void InventoryItemPB_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            InventoryItemPB itemPB = (InventoryItemPB)sender;

            if (e.Button == MouseButtons.Right)
            {
                if (GUIManager.KeysPressed.Contains(Keys.LeftShift) || GUIManager.KeysPressed.Contains(Keys.RightShift))
                {
                    // Drop
                    if (OnRequestDropItem != null)
                        OnRequestDropItem(this, itemPB.Slot);
                }
                else
                {
                    // Use
                    if (OnRequestUseItem != null)
                        OnRequestUseItem(this, itemPB.Slot);
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
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class InventoryItemPB : PictureBox
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;
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
                Tooltip = _tooltipHandler;
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
                // NOTE: Isn't there an overload or something for shaded text somewhere else?
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

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                InventoryItemPB src = (InventoryItemPB)sender;
                InventorySlot slot = src.Slot;
                ItemInfo itemInfo;

                if (!src._invForm._infoRequester.TryGetInfo(slot, out itemInfo))
                {
                    // The data has not been received yet - returning null will make the tooltip retry later
                    return null;
                }

                // Data was received, so format it and return it
                return ItemInfoHelper.GetStyledText(itemInfo);
            }
        }
    }
}