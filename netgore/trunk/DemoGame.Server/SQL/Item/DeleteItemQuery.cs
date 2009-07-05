using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class DeleteItemQuery : DbQueryNonReader<int>
    {
        const string _queryString = "DELETE FROM `item` WHERE `id`=@id LIMIT 1";

        public DeleteItemQuery(DbConnectionPool conn) : base(conn, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, int id)
        {
            p["@id"] = id;
        }
    }
}