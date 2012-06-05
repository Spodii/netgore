using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildsQuery(DbConnectionPool connectionPool) : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}`

            var q = qb.Select(GuildTable.TableName).AllColumns();
            return q.ToString();
        }

        public IEnumerable<IGuildTable> Execute()
        {
            var ret = new List<IGuildTable>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var row = new GuildTable();
                    row.ReadValues(r);
                    ret.Add(row);
                }
            }

            return ret;
        }
    }
}