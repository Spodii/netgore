using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Features.Shops
{
    public abstract class ShopBase<TShopItem> : IShop<TShopItem>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly TShopItem[] _emptyShopItems = new TShopItem[0];
        static readonly ShopSettings _shopSettings = ShopSettings.Instance;

        readonly bool _canBuy;
        readonly ShopID _id;
        readonly string _name;
        readonly TShopItem[] _shopItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopBase{TShopItem}"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="canBuy">Whether or not the shop can buy items from shoppers.</param>
        /// <param name="shopItems">The shop items.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="shopItems"/> contains more items than the max
        /// number supported by <see cref="ShopSettings.MaxShopItems"/>.</exception>
        protected ShopBase(ShopID id, string name, bool canBuy, IEnumerable<TShopItem> shopItems)
        {
            _id = id;
            _name = string.IsNullOrEmpty(name) ? "Unnamed Shop" : name;
            _canBuy = canBuy;
            _shopItems = shopItems == null ? _emptyShopItems : shopItems.ToArray();

            if (_shopItems.Length > _shopSettings.MaxShopItems)
            {
                const string errmsg = "There are too many items in the shop `{0}` ({1} > {2})!";
                var err = string.Format(errmsg, this, _shopItems.Length, _shopSettings.MaxShopItems);
                log.Fatal(err);
                Debug.Fail(err);
                throw new ArgumentOutOfRangeException(err, "shopItems");
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Name + " [" + ID + "]";
        }

        /// <summary>
        /// When overridden in the derived class, writes the the shop item to the <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">The key name to give the value to write.</param>
        /// <param name="shopItem">The shop item to write.</param>
        protected abstract void WriteShopItem(IValueWriter writer, string name, TShopItem shopItem);

        #region IShop<TShopItem> Members

        /// <summary>
        /// Gets if this shop can buy items from shoppers instead of just sell items to them.
        /// </summary>
        public bool CanBuy
        {
            get { return _canBuy; }
        }

        /// <summary>
        /// Gets the ID of the shop.
        /// </summary>
        public ShopID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the name of the shop.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the items in this shop.
        /// </summary>
        public IEnumerable<TShopItem> ShopItems
        {
            get { return _shopItems; }
        }

        /// <summary>
        /// Gets the item at the specified <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the shop item.</param>
        /// <returns>The shop item at the specified <paramref name="slot"/>, or null if
        /// the slot was invalid or contains no item.</returns>
        public TShopItem GetShopItem(ShopItemIndex slot)
        {
            var i = slot.GetRawValue();

            if (i < 0 || i >= _shopItems.Length)
                return default(TShopItem);

            return _shopItems[i];
        }

        /// <summary>
        /// Writes the information describing the shop items to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteShopItems(IValueWriter writer)
        {
            writer.Write("Items", (byte)_shopItems.Length);
            for (var i = 0; i < _shopItems.Length; i++)
            {
                WriteShopItem(writer, "Item" + i, _shopItems[i]);
            }
        }

        #endregion
    }
}