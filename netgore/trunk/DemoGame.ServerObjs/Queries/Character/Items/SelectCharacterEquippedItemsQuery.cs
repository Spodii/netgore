using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterEquippedItemsQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryString =
            string.Format(
                "SELECT {0}.*,{1}.slot FROM `{0}`,`{1}` WHERE {1}.character_id = @characterID AND {0}.id = {1}.item_id",
                DBTables.Item, DBTables.CharacterEquipped);

        public SelectCharacterEquippedItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IDictionary<EquipmentSlot, ItemValues> Execute(CharacterID characterID)
        {
            var retValues = new Dictionary<EquipmentSlot, ItemValues>();

            using (IDataReader r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    EquipmentSlot slot = r.GetEquipmentSlot("slot");
                    ItemValues values = ItemQueryHelper.ReadItemValues(r);
                    retValues.Add(slot, values);
                }
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterID");
        }

        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@characterID"] = (int)characterID;
        }
    }
}