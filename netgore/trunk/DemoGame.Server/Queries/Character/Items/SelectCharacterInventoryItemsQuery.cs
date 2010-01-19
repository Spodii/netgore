using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterInventoryItemsQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryString =
            string.Format(
                "SELECT {1}.slot AS \"character_inventory_slot\",{0}.* FROM `{0}`,`{1}` WHERE {1}.character_id = @characterID AND {0}.id = {1}.item_id",
                ItemTable.TableName, CharacterInventoryTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterInventoryItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterInventoryItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<KeyValuePair<InventorySlot, IItemTable>> Execute(CharacterID characterID)
        {
            var retValues = new List<KeyValuePair<InventorySlot, IItemTable>>();

            using (var r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    var slot = r.GetInventorySlot("character_inventory_slot");

                    var values = new ItemTable();
                    values.ReadValues(r);

                    var kvp = new KeyValuePair<InventorySlot, IItemTable>(slot, values);
                    retValues.Add(kvp);
                }
            }

            return retValues;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@characterID"] = (int)characterID;
        }
    }
}