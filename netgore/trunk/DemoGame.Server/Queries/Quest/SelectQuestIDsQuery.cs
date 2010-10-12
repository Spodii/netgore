using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestIDsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectQuestIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(QuestTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}`

            var q = qb.Select(QuestTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<QuestID> Execute()
        {
            var ret = new List<QuestID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    ret.Add(r.GetQuestID(0));
                }
            }

            return ret;
        }
    }
}