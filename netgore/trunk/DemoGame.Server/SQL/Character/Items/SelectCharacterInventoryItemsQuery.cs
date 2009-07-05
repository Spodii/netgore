using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterInventoryItemsQuery : SelectItemQueryBase<uint>
    {
        static readonly string _queryString =
            string.Format("SELECT {0}.* FROM `{0}`,`{1}` WHERE {1}.character_id = @characterID AND {0}.id = {1}.item_id",
                          DBTables.Item, DBTables.CharacterInventory);

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