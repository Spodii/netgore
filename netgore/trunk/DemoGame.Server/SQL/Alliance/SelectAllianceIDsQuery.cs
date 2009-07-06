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

        public IEnumerable<AllianceID> Execute()
        {
            var ret = new List<AllianceID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    AllianceID allianceID = r.GetAllianceID("id");
                    ret.Add(allianceID);
                }
            }

            return ret;
        }
    }
}
