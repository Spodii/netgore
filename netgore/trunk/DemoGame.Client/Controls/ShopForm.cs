using System;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Features.Shops;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    delegate void ShopFormPurchaseHandler(ShopForm shopForm, ShopItemIndex slot);

    class ShopForm : Form, IDragDropProvider
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

        ShopInfo<IItemTemplateTable> _shopInfo;

        readonly DragDropHandler _dragDropHandler;

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

            Vector2 itemsSize = _columns * _itemSize;
            Vector2 paddingSize = (_columns + 1) * _padding;
            Size = itemsSize + paddingSize + Border.Size;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when a request has been made to purchase an item from the shop. 
        /// </summary>
        public event ShopFormPurchaseHandler RequestPurchase;

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
            Vector2 offset = _padding;
            Vector2 offsetMultiplier = _itemSize + _padding;

            for (int i = 0; i < _shopSettings.MaxShopItems; i++)
            {
                int x = i % _columns;
                int y = i / _columns;
                Vector2 pos = offset + new Vector2(x, y) * offsetMultiplier;

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

        void ShopItemPB_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            ShopItemPB src = (ShopItemPB)sender;
            if (src.ItemInfo != null)
            {
                if (RequestPurchase != null)
                    RequestPurchase(this, src.Slot);
            }
        }

        /// <summary>
        /// A <see cref="PictureBox"/> that contains an item in a <see cref="ShopForm"/>.
        /// </summary>
        public class ShopItemPB : PictureBox, IDragDropProvider
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;

            readonly ShopItemIndex _slot;
            readonly ShopForm _shopForm;

            Grh _grh;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShopItemPB"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The pos.</param>
            /// <param name="index">The <see cref="ShopItemIndex"/>.</param>
            public ShopItemPB(ShopForm parent, Vector2 pos, ShopItemIndex index) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _shopForm = parent;
                _slot = index;
                Tooltip = _tooltipHandler;
                MouseUp += _shopForm.ShopItemPB_OnMouseUp;
            }

            /// <summary>
            /// Gets the <see cref="ShopItemIndex"/> of this slot.
            /// </summary>
            public ShopItemIndex Slot
            {
                get { return _slot; }
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
            /// Gets the shop information for the shop that this item belongs to.
            /// </summary>
            ShopInfo<IItemTemplateTable> ShopInfo
            {
                get { return _shopForm.ShopInfo; }
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
                Vector2 offset = (_itemSize - _grh.Size) / 2f;
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
            /// Gets the text for the <see cref="Tooltip"/>.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The args.</param>
            /// <returns>The text for the <see cref="Tooltip"/>.</returns>
            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                ShopItemPB src = (ShopItemPB)sender;
                var itemInfo = src.ItemInfo;
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
                get { return true; }
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
            /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
            /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
            /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
            /// otherwise false.</returns>
            bool IDragDropProvider.CanDrop(IDragDropProvider source)
            {
                return _shopForm._dragDropHandler.CanDrop(source, this);
            }

            /// <summary>
            /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
            /// <see cref="IDragDropProvider"/>.</param>
            void IDragDropProvider.Drop(IDragDropProvider source)
            {
                _shopForm._dragDropHandler.Drop(source, this);
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
            return _dragDropHandler.CanDrop(source, this);
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