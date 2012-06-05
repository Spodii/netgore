using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAllianceIDsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(AllianceTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}`

            var q = qb.Select(AllianceTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<AllianceID> Execute()
        {
            var ret = new List<AllianceID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var allianceID = r.GetAllianceID("id");
                    ret.Add(allianceID);
                }
            }

            return ret;
        }
    }
}