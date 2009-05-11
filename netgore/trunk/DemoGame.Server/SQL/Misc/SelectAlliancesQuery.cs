using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectAlliancesQuery : DbQueryReader
    {
        const string _queryString = "SELECT * FROM `alliances`";

        public SelectAlliancesQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<Dictionary<string, object>> Execute()
        {
            IEnumerable<Dictionary<string, object>> ret;

            using (var r = ExecuteReader())
            {
                ret = DataToDictionary(r);
            }

            return ret;
        }
    }
}
