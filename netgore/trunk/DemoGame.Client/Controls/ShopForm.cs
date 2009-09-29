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
using NetGore.RPGComponents;

namespace DemoGame.Client.Controls
{
    delegate void ShopFormPurchaseHandler(ShopForm shopForm, ShopItemIndex slot);

    class ShopForm : Form, IRestorableSettings
    {
        const int _columns = 6; // Number of items on each row
        const float _itemHeight = 32; // Height of each item slot
        const float _itemWidth = 32; // Width of each item slot
        const float _sepX = 2; // Amount of space on the X axis between each item
        const float _sepY = 2; // Amount of space on the Y axis between each item

        ShopInfo _shopInfo;
        public event ShopFormPurchaseHandler OnPurchase;

        public ShopInfo ShopInfo
        {
            get { return _shopInfo; }
        }

        public ShopForm(Vector2 position, Control parent)
            : base(parent.GUIManager, "Shop", position, new Vector2(200, 200), parent)
        {
            IsVisible = false;

            Vector2 itemsSize = _columns * new Vector2(_itemWidth, _itemHeight);
            Vector2 paddingSize = (_columns + 1) * new Vector2(_sepX, _sepY);
            Vector2 borderSize = (Border != null ? Border.Size : Vector2.Zero);
            Size = itemsSize + paddingSize + borderSize;

            CreateItemSlots();
        }

        void CreateItemSlots()
        {
            Vector2 offset = new Vector2(_sepX, _sepY);

            for (int i = 0; i < GameData.MaxShopItems; i++)
            {
                int x = i % _columns;
                int y = i / _columns;
                Vector2 pos = offset + new Vector2(x * (_itemWidth + _sepX), y * (_itemHeight + _sepY));

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
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            readonly ShopItemIndex _index;
            readonly ShopForm _shopForm;
            Grh _grh;

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

            public ShopItemPB(ShopForm parent, Vector2 pos, ShopItemIndex index)
                : base(pos, null, new Vector2(_itemWidth, _itemHeight), parent)
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
                        // TODO: Get the correct time
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
                Vector2 offset = new Vector2(_itemWidth, _itemHeight);
                offset -= _grh.Size;
                offset /= 2;

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