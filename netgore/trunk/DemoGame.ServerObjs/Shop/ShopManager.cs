using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    public static class ShopManager
    {
        static readonly DArray<Shop> _shops = new DArray<Shop>(32, false);
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets if this class has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        public static Shop GetShop(ShopID index)
        {
            if (!_shops.CanGet((int)index))
                return null;

            Shop ret = _shops[(int)index];
            Debug.Assert(ret.ID == index);
            return ret;
        }

        public static void Initialize(DbController dbController)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            if (dbController == null)
                throw new ArgumentNullException("dbController");

            // Load the shop IDs
            var shopIDs = dbController.GetQuery<SelectShopIDsQuery>().Execute();
            
            // Load the shops
            foreach (var id in shopIDs)
            {
                var shopItemTables = dbController.GetQuery<SelectShopItemsQuery>().Execute(id);
                var shopTable = dbController.GetQuery<SelectShopQuery>().Execute(id);
                var shop = new Shop(shopTable, shopItemTables);

                _shops.Insert((int)id, shop);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Loaded shop `{0}`", shop);
            }

            // Trim the DArray
            _shops.Trim();
        }
    }
}
