using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateIDsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(CharacterTemplateTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}`

            var q = qb.Select(CharacterTemplateTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<CharacterTemplateID> Execute()
        {
            var ret = new List<CharacterTemplateID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var id = r.GetCharacterTemplateID("id");
                    ret.Add(id);
                }
            }

            return ret;
        }
    }
}