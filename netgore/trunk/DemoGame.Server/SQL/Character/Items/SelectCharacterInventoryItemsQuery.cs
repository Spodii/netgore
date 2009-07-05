using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectCharacterInventoryItemsQuery : SelectItemQueryBase<uint>
    {
        const string _queryString =
            "SELECT item.* FROM `item`,`character_inventory` " + "WHERE character_inventory.character_id = @characterID " +
            "AND item.id = character_inventory.item_id";

        public SelectCharacterInventoryItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemValues> Execute(uint characterID)
        {
            var retValues = new List<ItemValues>();

            using (IDataReader r = ExecuteReader(characterID))
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
            return CreateParameters("@characterID");
        }

        protected override void SetParameters(DbParameterValues p, uint characterID)
        {
            p["@characterID"] = characterID;
        }
    }
}