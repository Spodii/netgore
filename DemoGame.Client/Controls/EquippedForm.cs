using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that dislpays the user's equipped items.
    /// </summary>
    public class EquippedForm : Form, IDragDropProvider
    {
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        readonly DragDropHandler _dragDropHandler;
        readonly ItemInfoRequesterBase<EquipmentSlot> _infoRequester;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedForm"/> class.
        /// </summary>
        /// <param name="dragDropHandler">The drag-drop handler.</param>
        /// <param name="infoRequester">The info requester.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="infoRequester" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dragDropHandler" /> is <c>null</c>.</exception>
        public EquippedForm(DragDropHandler dragDropHandler, ItemInfoRequesterBase<EquipmentSlot> infoRequester, Vector2 position,
                            Control parent) : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");
            if (dragDropHandler == null)
                throw new ArgumentNullException("dragDropHandler");

            _dragDropHandler = dragDropHandler;
            _infoRequester = infoRequester;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when a request has been made to unequip an item.
        /// </summary>
        public event TypedEventHandler<EquippedForm, EventArgs<EquipmentSlot>> RequestUnequip;

        /// <summary>
        /// Gets or sets the <see cref="UserEquipped"/> containing the equipped items to display on this form.
        /// </summary>
        public UserEquipped UserEquipped { get; set; }

        void CreateItemSlots()
        {
            CreateItemSlots(0, 0, EquipmentSlot.Head);
            CreateItemSlots(1, 0, EquipmentSlot.Body);

            CreateItemSlots(0, 1, EquipmentSlot.LeftHand);
            CreateItemSlots(1, 1, EquipmentSlot.RightHand);
        }

        void CreateItemSlots(int row, int column, EquipmentSlot slot)
        {
            var loc = new Vector2(row, column);
            var pos = _itemSize * loc;

            new EquippedItemPB(this, pos, slot);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseUp"/> event from the <see cref="EquippedItemPB"/>s
        /// on this form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void EquippedItemPB_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (UserEquipped == null)
                return;

            var slot = ((EquippedItemPB)sender).Slot;
            if (UserEquipped[slot] == null)
                return;

            switch (e.Button)
            {
                case Mouse.Button.Right:
                    if (RequestUnequip != null)
                        RequestUnequip.Raise(this, EventArgsHelper.Create(slot));
                    break;
            }
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Equipment";
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

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class EquippedItemPB : PictureBox, IDragDropProvider
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly EquipmentSlot _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="EquippedItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The relative position.</param>
            /// <param name="slot">The <see cref="EquipmentSlot"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
            public EquippedItemPB(EquippedForm parent, Vector2 pos, EquipmentSlot slot) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _slot = slot;
                Tooltip = _tooltipHandler;
            }

            /// <summary>
            /// Gets the <see cref="EquippedForm"/> that this <see cref="EquippedItemPB"/> is on.
            /// </summary>
            public EquippedForm EquippedForm
            {
                get { return (EquippedForm)Parent; }
            }

            /// <summary>
            /// Gets the <see cref="ItemEntity"/> for this slot.
            /// </summary>
            ItemEntity Item
            {
                get
                {
                    var equipped = EquippedForm.UserEquipped;
                    if (equipped == null)
                        return null;

                    return equipped[_slot];
                }
            }

            /// <summary>
            /// Gets the <see cref="EquipmentSlot"/> for this slot.
            /// </summary>
            public EquipmentSlot Slot
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
                item.Draw(spriteBatch, ScreenPosition + offset);
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

                EquippedForm.EquippedItemPB_OnMouseUp(this, e);
            }

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                var src = (EquippedItemPB)sender;
                var slot = src.Slot;
                IItemTable itemInfo;

                if (!src.EquippedForm._infoRequester.TryGetInfo(slot, out itemInfo))
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
                get { return Item != null; }
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
            }

            #endregion
        }
    }
}