using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectRepeatableQuestsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectRepeatableQuestsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public SelectRepeatableQuestsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(QuestTable.DbColumns, "id", "repeatable");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}` WHERE `repeatable` = 1

            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Select(QuestTable.TableName).Add("id")
                .Where(f.Equals(s.EscapeColumn("repeatable"), "1"));
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