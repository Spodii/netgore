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

    class ShopForm : Form
    {
        const int _columns = 6; // Number of items on each row

        /// <summary>
        /// The size of each item slot.
        /// </summary>
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        /// <summary>
        /// The amount of space between each item.
        /// </summary>
        static readonly Vector2 _padding = new Vector2(2, 2);

        static readonly ShopSettings _shopSettings = ShopSettings.Instance;

        ShopInfo<IItemTemplateTable> _shopInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopForm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public ShopForm(Vector2 position, Control parent) : base(parent, position, new Vector2(200, 200))
        {
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

        public ShopInfo<IItemTemplateTable> ShopInfo
        {
            get { return _shopInfo; }
        }

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

        public void DisplayShop(ShopInfo<IItemTemplateTable> shopInfo)
        {
            _shopInfo = shopInfo;
            IsVisible = true;
        }

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
                    RequestPurchase(this, src.Index);
            }
        }

        class ShopItemPB : PictureBox
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;
            readonly ShopItemIndex _index;
            readonly ShopForm _shopForm;
            Grh _grh;

            public ShopItemPB(ShopForm parent, Vector2 pos, ShopItemIndex index) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _shopForm = parent;
                _index = index;
                Tooltip = _tooltipHandler;
                MouseUp += _shopForm.ShopItemPB_OnMouseUp;
            }

            public ShopItemIndex Index
            {
                get { return _index; }
            }

            public IItemTemplateTable ItemInfo
            {
                get
                {
                    var shopInfo = ShopInfo;
                    if (shopInfo == null)
                        return null;

                    return ShopInfo.GetItemInfo(Index);
                }
            }

            ShopInfo<IItemTemplateTable> ShopInfo
            {
                get { return _shopForm.ShopInfo; }
            }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
            protected override void DrawControl(SpriteBatch spriteBatch)
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

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                ShopItemPB src = (ShopItemPB)sender;
                var itemInfo = src.ItemInfo;
                return ItemInfoHelper.GetStyledText(itemInfo);
            }
        }
    }
}