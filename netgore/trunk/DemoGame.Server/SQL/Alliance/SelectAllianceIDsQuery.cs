using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectAllianceIDsQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT `id` FROM `{0}`", DBTables.Alliance);

        public SelectAllianceIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<byte> Execute()
        {
            var ret = new List<byte>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    byte allianceID = r.GetByte("id");
                    ret.Add(allianceID);
                }
            }

            return ret;
        }
    }
}
