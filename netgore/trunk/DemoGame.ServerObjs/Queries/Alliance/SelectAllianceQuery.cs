using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using NetGore.Db;
using DemoGame.Server.DbObjs;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAllianceQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", AllianceTable.TableName);

        public SelectAllianceQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public AllianceTable Execute(AllianceID id)
        {
            AllianceTable ret;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new ArgumentOutOfRangeException("id", string.Format("No alliance found for id `{0}`.", id));

                ret = new AllianceTable(r);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = (int)id;
        }
    }
}