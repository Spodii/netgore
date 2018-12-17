using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using NetGore;
using NetGore.Features.Shops;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that displays the contents of a shop.
    /// </summary>
    public class ShopForm : Form, IDragDropProvider
    {
        /// <summary>
        /// The number of items on each row.
        /// </summary>
        const int _columns = 6;

        /// <summary>
        /// The size of each item slot.
        /// </summary>
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        /// <summary>
        /// The amount of space between each item slot.
        /// </summary>
        static readonly Vector2 _padding = new Vector2(2, 2);

        static readonly ShopSettings _shopSettings = ShopSettings.Instance;

        readonly DragDropHandler _dragDropHandler;
        ShopInfo<IItemTemplateTable> _shopInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopForm"/> class.
        /// </summary>
        /// <param name="dragDropHandler">The drag-drop handler callback.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public ShopForm(DragDropHandler dragDropHandler, Vector2 position, Control parent)
            : base(parent, position, new Vector2(200, 200))
        {
            _dragDropHandler = dragDropHandler;

            IsVisible = false;

            var itemsSize = _columns * _itemSize;
            var paddingSize = (_columns + 1) * _padding;
            Size = itemsSize + paddingSize + Border.Size;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when a request has been made to purchase an item from the shop. 
        /// </summary>
        public event TypedEventHandler<ShopForm, EventArgs<ShopItemIndex>> RequestPurchase;

        /// <summary>
        /// Gets the information for the current shop, or null if no shop is set.
        /// </summary>
        public ShopInfo<IItemTemplateTable> ShopInfo
        {
            get { return _shopInfo; }
        }

        /// <summary>
        /// Creates the <see cref="ShopItemPB"/>s for the form.
        /// </summary>
        void CreateItemSlots()
        {
            var offset = _padding;
            var offsetMultiplier = _itemSize + _padding;

            for (var i = 0; i < _shopSettings.MaxShopItems; i++)
            {
                var x = i % _columns;
                var y = i / _columns;
                var pos = offset + new Vector2(x, y) * offsetMultiplier;

                new ShopItemPB(this, pos, new ShopItemIndex((byte)i));
            }
        }

        /// <summary>
        /// Shows the shop form.
        /// </summary>
        /// <param name="shopInfo">The info for the shop to display.</param>
        public void DisplayShop(ShopInfo<IItemTemplateTable> shopInfo)
        {
            _shopInfo = shopInfo;
            IsVisible = true;
        }

        /// <summary>
        /// Hides the shop form.
        /// </summary>
        public void HideShop()
        {
            IsVisible = false;
            _shopInfo = null;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Shop";
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseUp"/> event from the <see cref="ShopItemPB"/>s on this form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void ShopItemPB_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var src = (ShopItemPB)sender;
            if (src.ItemInfo != null)
            {
                if (RequestPurchase != null)
                    RequestPurchase.Raise(this, EventArgsHelper.Create(src.Slot));
            }
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

        /// <summary>
        /// A <see cref="PictureBox"/> that contains an item in a <see cref="ShopForm"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class ShopItemPB : PictureBox, IDragDropProvider
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly ShopItemIndex _slot;

            Grh _grh;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShopItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The pos.</param>
            /// <param name="index">The <see cref="ShopItemIndex"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
            public ShopItemPB(ShopForm parent, Vector2 pos, ShopItemIndex index) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _slot = index;
                Tooltip = _tooltipHandler;
                MouseUp += ShopForm.ShopItemPB_OnMouseUp;
            }

            /// <summary>
            /// Gets the <see cref="IItemTemplateTable"/> for the item in this slot, or null if there is no
            /// item in the slot.
            /// </summary>
            public IItemTemplateTable ItemInfo
            {
                get
                {
                    var shopInfo = ShopInfo;
                    if (shopInfo == null)
                        return null;

                    return ShopInfo.GetItemInfo(Slot);
                }
            }

            /// <summary>
            /// Gets the <see cref="ShopForm"/> that this <see cref="ShopItemPB"/> is on.
            /// </summary>
            public ShopForm ShopForm
            {
                get { return (ShopForm)Parent; }
            }

            /// <summary>
            /// Gets the shop information for the shop that this item belongs to.
            /// </summary>
            ShopInfo<IItemTemplateTable> ShopInfo
            {
                get { return ShopForm.ShopInfo; }
            }

            /// <summary>
            /// Gets the <see cref="ShopItemIndex"/> of this slot.
            /// </summary>
            public ShopItemIndex Slot
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
                var itemInfo = ItemInfo;

                if (itemInfo == null)
                {
                    _grh = null;
                    return;
                }

                if (_grh == null || _grh.GrhData.GrhIndex != itemInfo.Graphic)
                {
                    try
                    {
                        _grh = new Grh(itemInfo.Graphic, AnimType.Loop, 0);
                    }
                    catch (Exception ex)
                    {
                        _grh = null;
                        if (log.IsErrorEnabled)
                            log.ErrorFormat("Failed to load shop Grh with index `{0}`: `{1}`", itemInfo.Graphic, ex);
                    }
                }

                if (_grh == null)
                    return;

                // Draw the item in the center of the slot
                var offset = (_itemSize - _grh.Size) / 2f;
                _grh.Draw(spriteBatch, ScreenPosition + offset);
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
            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseUp(e);
                
            }

            /// <summary>
            /// Gets the text for the <see cref="Tooltip"/>.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The args.</param>
            /// <returns>The text for the <see cref="Tooltip"/>.</returns>
            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                var src = (ShopItemPB)sender;
                var itemInfo = src.ItemInfo;
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
                get { return true; }
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
                return ShopForm._dragDropHandler.CanDrop(source, this);
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
                if (_grh == null)
                    return;

                _grh.Draw(spriteBatch, position, color);
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
                ShopForm._dragDropHandler.Drop(source, this);
            }

            #endregion
        }
    }
}