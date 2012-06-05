using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Features.Shops;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes an item that a <see cref="Shop"/> has available for sale.
    /// </summary>
    public class ShopItem : IShopItemTable, IShopItem<IItemTemplateTable>
    {
        static readonly ItemTemplateManager _itemTemplateManager = ItemTemplateManager.Instance;
        readonly IItemTemplateTable _itemTemplate;
        readonly ShopID _shopID;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="shopItemTable">The shop item table.</param>
        public ShopItem(IShopItemTable shopItemTable)
        {
            _shopID = shopItemTable.ShopID;
            _itemTemplate = _itemTemplateManager[shopItemTable.ItemTemplateID];
        }

        #region IShopItem<IItemTemplateTable> Members

        /// <summary>
        /// Gets the template for this shop item.
        /// </summary>
        public IItemTemplateTable ItemTemplate
        {
            get { return _itemTemplate; }
        }

        #endregion

        #region IShopItemTable Members

        /// <summary>
        /// Gets the value of the database column `item_template_id`.
        /// </summary>
        ItemTemplateID IShopItemTable.ItemTemplateID
        {
            get { return ItemTemplate.ID; }
        }

        /// <summary>
        /// Gets the <see cref="ShopID"/> for the shop that this <see cref="ShopItem"/> belongs to.
        /// </summary>
        public ShopID ShopID
        {
            get { return _shopID; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IShopItemTable IShopItemTable.DeepCopy()
        {
            return new ShopItemTable(this);
        }

        #endregion
    }
}