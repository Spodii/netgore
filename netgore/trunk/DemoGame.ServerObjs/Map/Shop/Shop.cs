using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a shop where Characters may buy and (if <see cref="CanBuy"/> is set) sell items.
    /// </summary>
    public class Shop : IShopTable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly bool _canBuy;
        readonly ShopID _id;
        readonly string _name;
        readonly ShopItem[] _shopItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shop"/> class.
        /// </summary>
        /// <param name="shopTable">The shop table.</param>
        /// <param name="shopItemTables">The shop item tables.</param>
        public Shop(IShopTable shopTable, IEnumerable<IShopItemTable> shopItemTables)
            : this(shopTable, shopItemTables.Select(x => new ShopItem(x)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shop"/> class.
        /// </summary>
        /// <param name="shopTable">The shop table.</param>
        /// <param name="shopItems">The items in the shop.</param>
        public Shop(IShopTable shopTable, IEnumerable<ShopItem> shopItems)
        {
            _id = shopTable.ID;
            _name = shopTable.Name;
            _canBuy = Convert.ToBoolean(shopTable.CanBuy);
            _shopItems = shopItems.OrderBy(x => x.ItemTemplate, ItemTemplateComparer.SortByTypeAndValue).ToArray();

            if (_shopItems.Length > ShopItemIndex.MaxValue)
            {
                const string errmsg = "There are too many items in the shop `{0}` ({1} > {2})!";
                var err = string.Format(errmsg, this, _shopItems.Length, ShopItemIndex.MaxValue);
                log.Fatal(err);
                Debug.Fail(err);
                throw new Exception(err);
            }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="ShopItem"/>s in this <see cref="Shop"/>.
        /// </summary>
        public IEnumerable<ShopItem> ShopItems
        {
            get { return _shopItems; }
        }

        /// <summary>
        /// Gets the <see cref="ShopItem"/> at the specified <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the shop item.</param>
        /// <returns>The <see cref="ShopItem"/> at the specified <paramref name="slot"/>, or null if
        /// the slot was invalid or contains no item.</returns>
        public ShopItem GetShopItem(ShopItemIndex slot)
        {
            if (slot < 0 || slot >= _shopItems.Length)
                return null;

            return _shopItems[slot];
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

        public void WriteShopItems(BitStream w)
        {
            w.Write((byte)_shopItems.Length);
            for (var i = 0; i < _shopItems.Length; i++)
            {
                w.Write(_shopItems[i].ItemTemplate);
            }
        }

        #region IShopTable Members

        /// <summary>
        /// Gets if this shop can buy items instead of just sell them.
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
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>A deep copy of this table.</returns>
        IShopTable IShopTable.DeepCopy()
        {
            return new ShopTable(this);
        }

        #endregion
    }
}