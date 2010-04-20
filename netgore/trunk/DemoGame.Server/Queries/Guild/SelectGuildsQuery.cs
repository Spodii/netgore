using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildsQuery : DbQueryReader
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}`", GuildTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
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