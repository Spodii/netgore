using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectAllianceQuery : DbQueryReader<byte>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", DBTables.Alliance);

        public SelectAllianceQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectAllianceQueryValues Execute(byte id)
        {
            SelectAllianceQueryValues ret;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new ArgumentOutOfRangeException("id", string.Format("No alliance found for id `{0}`.", id));

                string name = r.GetString("name");

                ret = new SelectAllianceQueryValues(id, name);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, byte id)
        {
            p["@id"] = id;
        }
    }
}
