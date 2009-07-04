using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectCharacterEquippedItemsQuery : SelectItemQueryBase<uint>
    {
        const string _queryString =
            "SELECT items.*,character_equipped.slot FROM `items`,`character_equipped` " + 
            "WHERE character_equipped.character_guid = @characterID " +
            "AND items.guid = character_equipped.item_guid";

        public SelectCharacterEquippedItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IDictionary<EquipmentSlot, ItemValues> Execute(uint userGuid)
        {
            var retValues = new Dictionary<EquipmentSlot, ItemValues>();

            using (IDataReader r = ExecuteReader(userGuid))
            {
                while (r.Read())
                {
                    EquipmentSlot slot = r.GetEquipmentSlot("slot");
                    ItemValues values = GetItemValues(r);
                    retValues.Add(slot, values);
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