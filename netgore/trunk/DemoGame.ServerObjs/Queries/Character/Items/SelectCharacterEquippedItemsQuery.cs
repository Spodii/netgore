using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
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
                ItemTable.TableName, CharacterEquippedTable.TableName);

        public SelectCharacterEquippedItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IDictionary<EquipmentSlot, IItemTable> Execute(CharacterID characterID)
        {
            var retValues = new Dictionary<EquipmentSlot, IItemTable>();

            using (IDataReader r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    EquipmentSlot slot = r.GetEquipmentSlot("slot");
                    var values = new ItemTable();
                    values.ReadValues(r);
                    retValues.Add(slot, values);
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
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@characterID"] = (int)characterID;
        }
    }
}