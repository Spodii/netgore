using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;
using Sprite = NetGore.Graphics.Sprite;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that displays the items the user has in their inventory.
    /// </summary>
    public class InventoryForm : Form, IDragDropProvider
    {
        /// <summary>
        /// The number of items in each inventory row.
        /// </summary>
        const int _columns = 6;

        /// <summary>
        /// The background to use for drawing the item amount.
        /// </summary>
        public static Color ItemAmountBackColor = Color.Black;

        /// <summary>
        /// The foreground color to use for drawing the item amount.
        /// </summary>
        public static Color ItemAmountForeColor = Color.White;

        /// <summary>
        /// The size of each item box.
        /// </summary>
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        /// <summary>
        /// The amount of space between each item.
        /// </summary>
        static readonly Vector2 _padding = new Vector2(2, 2);

        readonly DragDropHandler _dragDropHandler;
        readonly ItemInfoRequesterBase<InventorySlot> _infoRequester;
        readonly Func<Inventory, bool> _isUserInv;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryForm"/> class.
        /// </summary>
        /// <param name="dragDropHandler">The drag-drop handler.</param>
        /// <param name="isUserInv">A func used to determine if an <see cref="Inventory"/> is the
        /// user's inventory.</param>
        /// <param name="infoRequester">The item info tooltip.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="infoRequester"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="isUserInv"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dragDropHandler"/> is null.</exception>
        public InventoryForm(DragDropHandler dragDropHandler, Func<Inventory, bool> isUserInv,
                             ItemInfoRequesterBase<InventorySlot> infoRequester, Vector2 position, Control parent)
            : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");
            if (isUserInv == null)
                throw new ArgumentNullException("isUserInv");
            if (dragDropHandler == null)
                throw new ArgumentNullException("dragDropHandler");

            _dragDropHandler = dragDropHandler;
            _isUserInv = isUserInv;
            _infoRequester = infoRequester;

            var itemsSize = _columns * _itemSize;
            var paddingSize = (_columns + 1) * _padding;
            Size = itemsSize + paddingSize + Border.Size;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when an item was requested to be dropped.
        /// </summary>
        public event TypedEventHandler<InventoryForm, EventArgs<InventorySlot>> RequestDropItem;

        /// <summary>
        /// Notifies listeners when an item was requested to be used.
        /// </summary>
        public event TypedEventHandler<InventoryForm, EventArgs<InventorySlot>> RequestUseItem;

        public Inventory Inventory { get; set; }

        /// <summary>
        /// Gets if this <see cref="InventoryForm"/> is for the inventory for the user.
        /// </summary>
        public bool IsUserInventory
        {
            get { return _isUserInv(Inventory); }
        }

        void CreateItemSlots()
        {
            var offset = _padding;
            var offsetMultiplier = _itemSize + _padding;

            for (var i = 0; i < GameData.MaxInventorySize; i++)
            {
                var x = i % _columns;
                var y = i / _columns;
                var pos = offset + new Vector2(x, y) * offsetMultiplier;

                new InventoryItemPB(this, pos, new InventorySlot(i));
            }
        }

        /// <summary>
        /// Handles when the mouse button has been raised on a <see cref="InventoryItemPB"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void InventoryItemPB_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var itemPB = (InventoryItemPB)sender;

            if (e.Button == Mouse.Button.Right)
            {
                if (GUIManager.IsKeyDown(Keyboard.Key.LShift) || GUIManager.IsKeyDown(Keyboard.Key.RShift))
                {
                    // Drop
                    if (RequestDropItem != null)
                        RequestDropItem.Raise(this, EventArgsHelper.Create(itemPB.Slot));
                }
                else
                {
                    // Use
                    if (RequestUseItem != null)
                        RequestUseItem.Raise(this, EventArgsHelper.Create(itemPB.Slot));
                }
            }
        }

        /// <summary>
        /// Invokes the <see cref="RequestUseItem"/> event.
        /// </summary>
        /// <param name="slot">The <see cref="InventorySlot"/> to use.</param>
        public void InvokeRequestUseItem(InventorySlot slot)
        {
            if (RequestUseItem != null)
                RequestUseItem.Raise(this, EventArgsHelper.Create(slot));
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
            return _dragDropHandler.CanDrop(source, this);
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
            _dragDropHandler.Drop(source, this);
        }

        #endregion

        public class InventoryItemPB : PictureBox, IDragDropProvider, IQuickBarItemProvider
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly InventorySlot _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="InventoryItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The relative position of the control.</param>
            /// <param name="slot">The <see cref="InventorySlot"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
            public InventoryItemPB(InventoryForm parent, Vector2 pos, InventorySlot slot) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _slot = slot;
                Tooltip = _tooltipHandler;
            }

            /// <summary>
            /// Gets the <see cref="InventoryForm"/> that this <see cref="InventoryItemPB"/> is on.
            /// </summary>
            public InventoryForm InventoryForm
            {
                get { return (InventoryForm)Parent; }
            }

            /// <summary>
            /// Gets if this <see cref="InventoryItemPB"/> is from the <see cref="InventoryForm"/>
            /// for the user.
            /// </summary>
            public bool IsUserInventory
            {
                get { return InventoryForm.IsUserInventory; }
            }

            /// <summary>
            /// Gets the <see cref="ItemEntity"/> in this inventory slot.
            /// </summary>
            public ItemEntity Item
            {
                get
                {
                    var inv = InventoryForm.Inventory;
                    if (inv == null)
                        return null;

                    if (inv[_slot] == null)
                        return null;

                    var item = inv[_slot];
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
                var offset = (_itemSize - item.Grh.Size) / 2f;
                item.Draw(spriteBatch, ScreenPosition + offset.Round());

                // Draw the amount
                if (item.Amount > 1)
                    spriteBatch.DrawStringShaded(GUIManager.Font, item.Amount.ToString(), ScreenPosition, ItemAmountForeColor,
                        ItemAmountBackColor);
            }


            public ISprite SpriteOccupied { get; set; }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                base.LoadSkin(skinManager);

                Sprite = GUIManager.SkinManager.GetSprite("item_slot");
               // SpriteOccupied = 
            }

            /// <summary>
            /// Handles when a mouse button has been raised on the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseUp"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseUp"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseUp(e);

                InventoryForm.InventoryItemPB_OnMouseUp(this, e);
            }

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                var src = (InventoryItemPB)sender;
                var slot = src.Slot;
                IItemTable itemInfo;

                if (!src.InventoryForm._infoRequester.TryGetInfo(slot, out itemInfo))
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
                return InventoryForm._dragDropHandler.CanDrop(source, this);
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
                InventoryForm._dragDropHandler.Drop(source, this);
            }

            #endregion

            #region IQuickBarItemProvider Members

            /// <summary>
            /// Gets the <see cref="QuickBarItemType"/> and value to add to the quick bar.
            /// </summary>
            /// <param name="type">When this method returns true, contains the <see cref="QuickBarItemType"/>
            /// to add.</param>
            /// <param name="value">When this method returns true, contains the value for for the quick bar item.</param>
            /// <returns>
            /// True if the item can be added to the quick bar; otherwise false.
            /// </returns>
            bool IQuickBarItemProvider.TryAddToQuickBar(out QuickBarItemType type, out int value)
            {
                type = QuickBarItemType.Inventory;
                value = (int)Slot;

                var item = Item;
                if (item == null)
                    return false;

                return true;
            }

            #endregion
        }
    }
}