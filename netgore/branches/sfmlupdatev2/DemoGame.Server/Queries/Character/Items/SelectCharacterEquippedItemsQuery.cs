using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterEquippedItemsQuery : DbQueryReader<CharacterID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterEquippedItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterEquippedItemsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT i.*,c.slot FROM `item` i
                    INNER JOIN `character_equipped` c
                        ON i.id = c.item_id
                    WHERE c.character_id = @characterID;
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ItemTable.TableName, "i").AllColumns("i").Add("c.slot").InnerJoinOnColumn(
                    CharacterEquippedTable.TableName, "c", "item_id", "i", "id").Where(f.Equals("c.character_id",
                        s.Parameterize("characterID")));
            return q.ToString();
        }

        public IEnumerable<KeyValuePair<EquipmentSlot, IItemTable>> Execute(CharacterID characterID)
        {
            var retValues = new Dictionary<EquipmentSlot, IItemTable>();

            using (var r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    var slot = r.GetEquipmentSlot("slot");
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
            return CreateParameters("characterID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["characterID"] = (int)characterID;
        }
    }
}