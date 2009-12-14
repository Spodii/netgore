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

        /// <summary>
        /// The size of each item box.
        /// </summary>
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        /// <summary>
        /// The amount of space between each item.
        /// </summary>
        static readonly Vector2 _padding = new Vector2(2, 2);

        readonly ItemInfoRequesterBase<InventorySlot> _infoRequester;

        Inventory _inventory;

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
            : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");

            _infoRequester = infoRequester;

            Vector2 itemsSize = _columns * _itemSize;
            Vector2 paddingSize = (_columns + 1) * _padding;
            Size = itemsSize + paddingSize + Border.Size;

            CreateItemSlots();
        }

        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        void CreateItemSlots()
        {
            Vector2 offset = _padding;
            Vector2 offsetMultiplier = _itemSize + _padding;

            for (int i = 0; i < Inventory.MaxInventorySize; i++)
            {
                int x = i % _columns;
                int y = i / _columns;
                Vector2 pos = offset + new Vector2(x, y) * offsetMultiplier;

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

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Inventory";
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

            public InventoryItemPB(InventoryForm parent, Vector2 pos, InventorySlot slot) : base(parent, pos, _itemSize)
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

            public InventorySlot Slot
            {
                get { return _slot; }
            }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
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
                Vector2 offset = (_itemSize - item.Grh.Size) / 2f;
                item.Draw(spriteBatch, ScreenPosition + offset);

                // Draw the amount
                if (item.Amount > 1)
                    DrawShadedText(spriteBatch, item.Amount.ToString(), ScreenPosition, Color.White, Color.Black);
            }

            void DrawShadedText(SpriteBatch sb, string txt, Vector2 pos, Color foreground, Color background)
            {
                // NOTE: Isn't there an extension method or something for shaded text somewhere else?
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