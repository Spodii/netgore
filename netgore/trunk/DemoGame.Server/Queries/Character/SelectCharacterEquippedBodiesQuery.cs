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
    public class SelectCharacterEquippedBodiesQuery : DbQueryReader<CharacterID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterEquippedBodiesQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterEquippedBodiesQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ItemTable.DbColumns, "id", "equipped_body");
            QueryAsserts.ContainsColumns(CharacterEquippedTable.DbColumns, "item_id", "character_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT i.equipped_body FROM `item` i
            //  INNER JOIN `character_equipped` e
            //      ON i.id = e.item_id
            //	WHERE e.character_id = 1;
            //      AND i.equipped_body IS NOT NULL

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ItemTable.TableName, "i").Add("i.equipped_body").InnerJoinOnColumn(CharacterEquippedTable.TableName, "e",
                    "item_id", "i", "id").Where(f.And(f.Equals("e.character_id", s.Parameterize("charID")),
                        f.IsNotNull("i.equipped_body")));

            return q.ToString();
        }

        public IEnumerable<string> Execute(CharacterID charID)
        {
            var ret = new List<string>();

            using (var r = ExecuteReader(charID))
            {
                while (r.Read())
                {
                    var v = r.GetString(0);
                    ret.Add(v);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("charID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID item)
        {
            p["charID"] = (int)item;
        }
    }
}