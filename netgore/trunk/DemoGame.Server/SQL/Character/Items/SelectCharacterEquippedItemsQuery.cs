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
            "SELECT item.*,character_equipped.slot FROM `item`,`character_equipped` " + 
            "WHERE character_equipped.character_id = @characterID " +
            "AND item.id = character_equipped.item_id";

        public SelectCharacterEquippedItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IDictionary<EquipmentSlot, ItemValues> Execute(uint characterID)
        {
            var retValues = new Dictionary<EquipmentSlot, ItemValues>();

            using (IDataReader r = ExecuteReader(characterID))
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