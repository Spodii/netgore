using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Features.Shops;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a shop where Characters may buy and sell items.
    /// </summary>
    public class Shop : ShopBase<ShopItem>, IShopTable
    {
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
            : base(shopTable.ID, shopTable.Name, shopTable.CanBuy, SortShopItems(shopItems))
        {
        }

        static IEnumerable<ShopItem> SortShopItems(IEnumerable<ShopItem> shopItems)
        {
            return shopItems.Where(x => x != null).OrderBy(x => x.ItemTemplate, ItemTemplateComparer.SortByTypeAndValue);
        }

        /// <summary>
        /// When overridden in the derived class, writes the the shop item to the <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">The key name to give the value to write.</param>
        /// <param name="shopItem">The shop item to write.</param>
        protected override void WriteShopItem(IValueWriter writer, string name, ShopItem shopItem)
        {
            if (writer.SupportsNodes)
                writer.WriteStartNode(name);

            new ItemTemplateTable(shopItem.ItemTemplate).WriteState(writer);

            if (writer.SupportsNodes)
                writer.WriteEndNode(name);
        }

        #region IShopTable Members

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