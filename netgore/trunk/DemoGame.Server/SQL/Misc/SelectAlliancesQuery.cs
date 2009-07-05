using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectAlliancesQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}`", DBTables.Alliance);

        public SelectAlliancesQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<Dictionary<string, object>> Execute()
        {
            IEnumerable<Dictionary<string, object>> ret;

            using (IDataReader r = ExecuteReader())
            {
                ret = DataToDictionary(r);
            }

            return ret;
        }
    }
}