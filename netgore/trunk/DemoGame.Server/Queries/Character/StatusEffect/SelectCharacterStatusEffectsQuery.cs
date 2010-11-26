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
    public class SelectCharacterStatusEffectsQuery : DbQueryReader<CharacterID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterStatusEffectsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterStatusEffectsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterStatusEffectTable.DbColumns, "character_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT * FROM `{0}` 
                    WHERE `character_id`=@character_id
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(CharacterStatusEffectTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("character_id"),
                    s.Parameterize("character_id")));
            return q.ToString();
        }

        public IEnumerable<ICharacterStatusEffectTable> Execute(CharacterID id)
        {
            var ret = new List<ICharacterStatusEffectTable>(4);

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var item = new CharacterStatusEffectTable();
                    item.ReadValues(r);
                    ret.Add(item);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("character_id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID id)
        {
            p["character_id"] = (int)id;
        }
    }
}