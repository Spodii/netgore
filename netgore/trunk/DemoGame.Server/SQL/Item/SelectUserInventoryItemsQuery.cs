using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectUserInventoryItemsQuery : SelectItemQueryBase<int>
    {
        const string _queryString =
            "SELECT items.* FROM `items`,`user_inventory` " + "WHERE user_inventory.user_guid = @userGuid " +
            "AND items.guid = user_inventory.item_guid";

        public SelectUserInventoryItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemValues> Execute(int userGuid)
        {
            var retValues = new List<ItemValues>();

            using (IDataReader r = ExecuteReader(userGuid))
            {
                while (r.Read())
                {
                    ItemValues values = GetItemValues(r);
                    retValues.Add(values);
                }
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@userGuid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@userGuid"] = item;
        }
    }
}