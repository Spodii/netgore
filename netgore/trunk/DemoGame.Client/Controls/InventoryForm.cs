using System;
using System.Linq;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class InventoryForm : Form, IDragDropProvider
    {
        /// <summary>
        /// Delegate for handling events related to items in the inventory.
        /// </summary>
        /// <param name="inventoryForm">The <see cref="InventoryForm"/> the event came from.</param>
        /// <param name="slot">The inventory item slot the event is related to.</param>
        public delegate void InventoryItemHandler(InventoryForm inventoryForm, InventorySlot slot);

        /// <summary>
        /// Delegate for handling events related to items in the inventory.
        /// </summary>
        /// <param name="inventoryForm">The <see cref="InventoryForm"/> the event came from.</param>
        /// <param name="a">The first <see cref="InventorySlot"/> to swap.</param>
        /// <param name="b">The second <see cref="InventorySlot"/> to swap.</param>
        public delegate void InventorySwapItemHandler(InventoryForm inventoryForm, InventorySlot a, InventorySlot b);

        /// <summary>
        /// The number of items in each inventory row.
        /// </summary>
        const int _columns = 6;

        /// <summary>
        /// The size of each item box.
        /// </summary>
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        /// <summary>
        /// The amount of space between each item.
        /// </summary>
        static readonly Vector2 _padding = new Vector2(2, 2);

        readonly ItemInfoRequesterBase<InventorySlot> _infoRequester;
        readonly Func<Inventory, bool> _isUserInv;

        Inventory _inventory;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryForm"/> class.
        /// </summary>
        /// <param name="isUserInv">A <see cref="Func{T,U}"/> used to determine if an <see cref="Inventory"/> is the
        /// user's inventory.</param>
        /// <param name="infoRequester">The item info tooltip.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public InventoryForm(Func<Inventory, bool> isUserInv, ItemInfoRequesterBase<InventorySlot> infoRequester, Vector2 position,
                             Control parent) : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");
            if (infoRequester == null)
                throw new ArgumentNullException("isUserInv");

            _isUserInv = isUserInv;
            _infoRequester = infoRequester;

            Vector2 itemsSize = _columns * _itemSize;
            Vector2 paddingSize = (_columns + 1) * _padding;
            Size = itemsSize + paddingSize + Border.Size;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when an item was requested to be dropped.
        /// </summary>
        public event InventoryItemHandler RequestDropItem;

        /// <summary>
        /// Notifies listeners when an item was requested to swap slots.
        /// </summary>
        public event InventorySwapItemHandler RequestSwapSlots;

        /// <summary>
        /// Notifies listeners when an item was requested to be used.
        /// </summary>
        public event InventoryItemHandler RequestUseItem;

        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        /// <summary>
        /// Invokes the <see cref="RequestUseItem"/> event.
        /// </summary>
        /// <param name="slot">The <see cref="InventorySlot"/> to use.</param>
        public void InvokeRequestUseItem(InventorySlot slot )
        {
            if (RequestUseItem != null)
                RequestUseItem(this, slot);
        }

        /// <summary>
        /// Gets if this <see cref="InventoryForm"/> is for the inventory for the user.
        /// </summary>
        public bool IsUserInventory
        {
            get { return _isUserInv(Inventory); }
        }

        void CreateItemSlots()
        {
            Vector2 offset = _padding;
            Vector2 offsetMultiplier = _itemSize + _padding;

            for (int i = 0; i < GameData.MaxInventorySize; i++)
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
                    if (RequestDropItem != null)
                        RequestDropItem(this, itemPB.Slot);
                }
                else
                {
                    // Use
                    if (RequestUseItem != null)
                        RequestUseItem(this, itemPB.Slot);
                }
            }
        }

        void InvokeRequestSwapSlotsEvent(InventorySlot a, InventorySlot b)
        {
            if (RequestSwapSlots != null)
                RequestSwapSlots(this, a, b);
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

        #region IDragDropProvider Members

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
        /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
        /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
        /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
        /// otherwise false.</returns>
        bool IDragDropProvider.CanDrop(IDragDropProvider source)
        {
            // Equipped item -> Inventory: Remove equipment
            var asEquipped = source as EquippedForm.EquippedItemPB;
            if (asEquipped != null)
            {
                var inv = ((Control)source).Parent as EquippedForm;
                if (inv != null)
                    return true;
            }

            return false;
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
        /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
        /// dragged onto it but not yet dropped.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
        /// <see cref="IDragDropProvider"/>.</param>
        void IDragDropProvider.Drop(IDragDropProvider source)
        {
            // Equipped item -> Inventory: Remove equipment
            var asEquipped = source as EquippedForm.EquippedItemPB;
            if (asEquipped != null)
            {
                var inv = asEquipped.Parent as EquippedForm;
                if (inv != null)
                    inv.InvokeRequestUnequip(asEquipped.Slot);
            }
        }

        #endregion

        internal class InventoryItemPB : PictureBox, IDragDropProvider
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly InventoryForm _invForm;
            readonly InventorySlot _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="InventoryItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The relative position of the control.</param>
            /// <param name="slot">The <see cref="InventorySlot"/>.</param>
            public InventoryItemPB(InventoryForm parent, Vector2 pos, InventorySlot slot) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _invForm = parent;
                _slot = slot;
                Tooltip = _tooltipHandler;
            }

            /// <summary>
            /// Gets if this <see cref="InventoryItemPB"/> is from the <see cref="InventoryForm"/>
            /// for the user.
            /// </summary>
            public bool IsUserInventory
            {
                get { return _invForm.IsUserInventory; }
            }

            /// <summary>
            /// Gets the <see cref="ItemEntity"/> in this inventory slot.
            /// </summary>
            public ItemEntity Item
            {
                get
                {
                    Inventory inv = _invForm.Inventory;
                    if (inv == null)
                        return null;

                    if (inv[_slot] == null)
                        return null;

                    ItemEntity item = inv[_slot];
                    if (item == null)
                        return null;

                    return item;
                }
            }

            /// <summary>
            /// Gets the <see cref="InventorySlot"/> for this inventory item slot.
            /// </summary>
            public InventorySlot Slot
            {
                get { return _slot; }
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
                item.Draw(spriteBatch, ScreenPosition + offset.Round());

                // Draw the amount
                if (item.Amount > 1)
                    spriteBatch.DrawStringShaded(GUIManager.Font, item.Amount.ToString(), ScreenPosition, Color.White, Color.Black);
            }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                base.LoadSkin(skinManager);

                Sprite = GUIManager.SkinManager.GetSprite("item_slot");
            }

            /// <summary>
            /// Handles when a mouse button has been raised on the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseUp"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseUp"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseUp(MouseClickEventArgs e)
            {
                base.OnMouseUp(e);

                _invForm.InventoryItemPB_OnMouseUp(this, e);
            }

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                InventoryItemPB src = (InventoryItemPB)sender;
                InventorySlot slot = src.Slot;
                IItemTable itemInfo;

                if (!src._invForm._infoRequester.TryGetInfo(slot, out itemInfo))
                {
                    // The data has not been received yet - returning null will make the tooltip retry later
                    return null;
                }

                // Data was received, so format it and return it
                return ItemInfoHelper.GetStyledText(itemInfo);
            }

            #region IDragDropProvider Members

            /// <summary>
            /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
            /// supports having items dropped on it but not dragging, this will always return false. For items that can be
            /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
            /// there is some other reason that this item cannot currently be dragged.
            /// </summary>
            bool IDragDropProvider.CanDragContents
            {
                get
                {
                    // Only allow dragging from slots on the User's inventory that have an item
                    return Item != null && IsUserInventory;
                }
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
                // User inventory slot -> User inventory slot: Change slots
                var sourceInvItem = source as InventoryItemPB;
                if (sourceInvItem != null)
                {
                    if (sourceInvItem._invForm == _invForm && IsUserInventory)
                        return true;
                }

                return false;
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
            /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
            /// dragged onto it but not yet dropped.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
            {
                DragDropProviderHelper.DrawDropHighlight(spriteBatch, GetScreenArea());
            }

            /// <summary>
            /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
            /// <see cref="IDragDropProvider"/>.</param>
            void IDragDropProvider.Drop(IDragDropProvider source)
            {
                // User inventory slot -> User inventory slot: Change slots
                var sourceInvItem = source as InventoryItemPB;
                if (sourceInvItem != null && sourceInvItem._invForm == _invForm && IsUserInventory)
                {
                    var a = Slot;
                    var b = sourceInvItem.Slot;
                    _invForm.InvokeRequestSwapSlotsEvent(a, b);
                }
            }

            #endregion
        }
    }
}