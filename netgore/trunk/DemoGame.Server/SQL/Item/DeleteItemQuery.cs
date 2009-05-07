using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using NetGore.Db;

namespace DemoGame.Server
{
    public class DeleteItemQuery : DbQueryNonReader<int>
    {
        const string _queryString = "DELETE FROM `items` WHERE `guid`=@guid LIMIT 1";

        public DeleteItemQuery(DbConnectionPool conn) : base(conn, _queryString)
        {
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@guid"] = item;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@guid");
        }
    }
}