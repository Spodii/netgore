using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SetAccountCurrentIPsNullQuery : DbQueryNonReader
    {
        static readonly string _queryStr = string.Format("UPDATE `{0}` SET `current_ip` = NULL", AccountTable.TableName);

        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SetAccountCurrentIPsNullQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
        }
    }
}
