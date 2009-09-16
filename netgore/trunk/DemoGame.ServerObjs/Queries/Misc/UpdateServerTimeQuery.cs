using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateServerTimeQuery : DbQueryNonReader
    {
        static readonly string _queryStr = string.Format("UPDATE `{0}` SET `server_time`=NOW()",
            ServerTimeTable.TableName);

        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public UpdateServerTimeQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
        }
    }
}
