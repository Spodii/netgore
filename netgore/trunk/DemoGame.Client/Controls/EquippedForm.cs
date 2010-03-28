using System;
using System.Linq;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    delegate void RequestUnequipHandler(EquippedForm equippedForm, EquipmentSlot slot);

    class EquippedForm : Form, IDragDropProvider
    {
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        readonly ItemInfoRequesterBase<EquipmentSlot> _infoRequester;
        UserEquipped _userEquipped;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedForm"/> class.
        /// </summary>
        /// <param name="infoRequester">The info requester.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public EquippedForm(ItemInfoRequesterBase<EquipmentSlot> infoRequester, Vector2 position, Control parent)
            : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");

            _infoRequester = infoRequester;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when a request has been made to unequip an item.
        /// </summary>
        public event RequestUnequipHandler RequestUnequip;

        /// <summary>
        /// Invokes the <see cref="RequestUnequip"/> event.
        /// </summary>
        /// <param name="slot">The <see cref="EquipmentSlot"/> to unequip.</param>
        public void InvokeRequestUnequip(EquipmentSlot slot)
        {
            if (RequestUnequip != null)
                RequestUnequip(this, slot);
        }

        public UserEquipped UserEquipped
        {
            get { return _userEquipped; }
            set { _userEquipped = value; }
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
            Vector2 loc = new Vector2(row, column);
            Vector2 pos = _itemSize * loc;

            new EquippedItemPB(this, pos, slot);
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
                    if (RequestUnequip != null)
                        RequestUnequip(this, slot);
                    break;
            }
        }

        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Equipment";
        }

        public class EquippedItemPB : PictureBox, IDragDropProvider
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly EquippedForm _equippedForm;
            readonly EquipmentSlot _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="EquippedItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The relative position.</param>
            /// <param name="slot">The <see cref="EquipmentSlot"/>.</param>
            public EquippedItemPB(EquippedForm parent, Vector2 pos, EquipmentSlot slot) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _equippedForm = parent;
                _slot = slot;
                Tooltip = _tooltipHandler;

                MouseUp += _equippedForm.EquippedItemPB_OnMouseUp;
            }

            /// <summary>
            /// Gets the <see cref="EquipmentSlot"/> for this slot.
            /// </summary>
            public EquipmentSlot Slot
            {
                get { return _slot; }
            }

            /// <summary>
            /// Gets the <see cref="ItemEntity"/> for this slot.
            /// </summary>
            ItemEntity Item
            {
                get
                {
                    UserEquipped equipped = _equippedForm.UserEquipped;
                    if (equipped == null)
                        return null;

                    return equipped[_slot];
                }
            }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            protected override void DrawControl(ISpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                var item = Item;
                if (item == null)
                    return;

                // Draw the item in the center of the slot
                Vector2 offset = (_itemSize - item.Grh.Size) / 2f;
                item.Draw(spriteBatch, ScreenPosition + offset);
            }

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                EquippedItemPB src = (EquippedItemPB)sender;
                EquipmentSlot slot = src.Slot;
                IItemTable itemInfo;

                if (!src._equippedForm._infoRequester.TryGetInfo(slot, out itemInfo))
                {
                    // The data has not been received yet - returning null will make the tooltip retry later
                    return null;
                }

                // Data was received, so format it and return it
                return ItemInfoHelper.GetStyledText(itemInfo);
            }

            /// <summary>
            /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
            /// supports having items dropped on it but not dragging, this will always return false. For items that can be
            /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
            /// there is some other reason that this item cannot currently be dragged.
            /// </summary>
            bool IDragDropProvider.CanDragContents
            {
                get { return Item != null; }
            }

            /// <summary>
            /// Draws the item that this <see cref="IDragDropProvider"/> contains for when this item
            /// is being dragged.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            /// <param name="position">The position to draw the sprite at.</param>
            /// <param name="color">The color to use when drawing the item.</param>
            void IDragDropProvider.DrawDraggedItem(ISpriteBatch spriteBatch, Vector2 position, Color color)
            {
                var item = Item;
                if (item == null)
                    return;

                item.Draw(spriteBatch, position, color);
            }

            /// <summary>
            /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
            /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
            /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
            /// otherwise false.</returns>
            bool IDragDropProvider.CanDrop(IDragDropProvider source)
            {
                return false;
            }

            /// <summary>
            /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
            /// <see cref="IDragDropProvider"/>.</param>
            void IDragDropProvider.Drop(IDragDropProvider source)
            {
            }

            /// <summary>
            /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
            /// dragged onto it but not yet dropped.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
            {
                DragDropProviderHelper.DrawDropHighlight(spriteBatch, GetScreenArea());
            }
        }

        /// <summary>
        /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
        /// supports having items dropped on it but not dragging, this will always return false. For items that can be
        /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
        /// there is some other reason that this item cannot currently be dragged.
        /// </summary>
        bool IDragDropProvider.CanDragContents
        {
            get { return false; }
        }

        /// <summary>
        /// Draws the item that this <see cref="IDragDropProvider"/> contains for when this item
        /// is being dragged.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="color">The color to use when drawing the item.</param>
        void IDragDropProvider.DrawDraggedItem(ISpriteBatch spriteBatch, Vector2 position, Color color)
        {
        }

        /// <summary>
        /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
        /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
        /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
        /// otherwise false.</returns>
        bool IDragDropProvider.CanDrop(IDragDropProvider source)
        {
            // Inventory item -> equipped: Equip item
            var asInvItem = source as InventoryForm.InventoryItemPB;
            if (asInvItem != null)
            {
                var item = asInvItem.Item;
                var inv = ((Control)source).Parent as InventoryForm;
                if (inv != null && inv.IsUserInventory && item != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
        /// <see cref="IDragDropProvider"/>.</param>
        void IDragDropProvider.Drop(IDragDropProvider source)
        {
            // Inventory item -> equipped: Equip item
            var asInvItem = source as InventoryForm.InventoryItemPB;
            if (asInvItem != null)
            {
                var item = asInvItem.Item;
                var inv = ((Control)source).Parent as InventoryForm;
                if (inv != null && inv.IsUserInventory && item != null)
                    inv.InvokeRequestUseItem(asInvItem.Slot);
            }
        }

        /// <summary>
        /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
        /// dragged onto it but not yet dropped.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
        {
        }
    }
}