using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestIDsQuery : DbQueryReader
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}`",
            QuestTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectQuestIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(QuestTable.DbKeyColumns, "id");
        }

        public IEnumerable<QuestID> Execute()
        {
            List<QuestID> ret = new List<QuestID>();

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
