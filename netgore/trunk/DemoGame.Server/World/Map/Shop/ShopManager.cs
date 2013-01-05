using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Shops;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages the <see cref="Shop"/> instances.
    /// </summary>
    public class ShopManager : DbTableDataManager<ShopID, IShop<ShopItem>>
    {
        static readonly ShopManager _instance;

        SelectShopItemsQuery _selectShopItemsQuery;
        SelectShopQuery _selectShopQuery;

        /// <summary>
        /// Initializes the <see cref="ShopManager"/> class.
        /// </summary>
        static ShopManager()
        {
            var dbInstance = DbControllerBase.GetInstance();
            _instance = new ShopManager(dbInstance);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopManager"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        ShopManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// Gets an instance of the <see cref="ShopManager"/>.
        /// </summary>
        public static ShopManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, provides a chance to cache frequently used queries instead of
        /// having to grab the query from the <see cref="IDbController"/> every time. Caching is completely
        /// optional, but if you do cache any queries, it should be done here. Do not use this method for
        /// anything other than caching queries from the <paramref name="dbController"/>.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to grab the queries from.</param>
        protected override void CacheDbQueries(IDbController dbController)
        {
            _selectShopItemsQuery = dbController.GetQuery<SelectShopItemsQuery>();
            _selectShopQuery = dbController.GetQuery<SelectShopQuery>();

            base.CacheDbQueries(dbController);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected override IEnumerable<ShopID> GetIDs()
        {
            return DbController.GetQuery<SelectShopIDsQuery>().Execute();
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected override IShop<ShopItem> LoadItem(ShopID id)
        {
            var shopItemTables = _selectShopItemsQuery.Execute(id);
            var shopTable = _selectShopQuery.Execute(id);
            return new Shop(shopTable, shopItemTables);
        }
    }
}