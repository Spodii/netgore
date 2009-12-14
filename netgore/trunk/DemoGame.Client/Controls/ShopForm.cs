using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client.Controls
{
    delegate void ShopFormPurchaseHandler(ShopForm shopForm, ShopItemIndex slot);

    class ShopForm : Form, IRestorableSettings
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

        ShopInfo _shopInfo;
        public event ShopFormPurchaseHandler OnPurchase;

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

        public ShopInfo ShopInfo
        {
            get { return _shopInfo; }
        }

        void CreateItemSlots()
        {
            Vector2 offset = _padding;
            Vector2 offsetMultiplier = _itemSize + _padding;

            for (int i = 0; i < GameData.MaxShopItems; i++)
            {
                int x = i % _columns;
                int y = i / _columns;
                Vector2 pos = offset + new Vector2(x, y) * offsetMultiplier;

                new ShopItemPB(this, pos, new ShopItemIndex((byte)i));
            }
        }

        public void DisplayShop(ShopInfo shopInfo)
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
                if (OnPurchase != null)
                    OnPurchase(this, src.Index);
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
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[] { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y) };
        }

        #endregion

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
                OnMouseUp += _shopForm.ShopItemPB_OnMouseUp;

                Skin.OnChange += Skin_OnChange;
                LoadSprite();
            }

            public ShopItemIndex Index
            {
                get { return _index; }
            }

            public ItemInfo ItemInfo
            {
                get
                {
                    ShopInfo shopInfo = ShopInfo;
                    if (shopInfo == null)
                        return null;

                    return ShopInfo.GetItemInfo(Index);
                }
            }

            ShopInfo ShopInfo
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
                ItemInfo itemInfo = ItemInfo;

                if (itemInfo == null)
                {
                    _grh = null;
                    return;
                }

                if (_grh == null || _grh.GrhData.GrhIndex != itemInfo.GrhIndex)
                {
                    try
                    {
                        _grh = new Grh(itemInfo.GrhIndex, AnimType.Loop, 0);
                    }
                    catch (Exception ex)
                    {
                        _grh = null;
                        if (log.IsErrorEnabled)
                            log.ErrorFormat("Failed to load shop Grh with index `{0}`: `{1}`", itemInfo.GrhIndex, ex);
                    }
                }

                if (_grh == null)
                    return;

                // Draw the item in the center of the slot
                Vector2 offset = (_itemSize - _grh.Size) / 2f;
                _grh.Draw(spriteBatch, ScreenPosition + offset);
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
                ShopItemPB src = (ShopItemPB)sender;
                ItemInfo itemInfo = src.ItemInfo;
                return ItemInfoHelper.GetStyledText(itemInfo);
            }
        }
    }
}