using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectAllianceHostileQuery : DbQueryReader<byte>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `alliance_id`=@id", DBTables.AllianceHostile);

        public SelectAllianceHostileQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectAllianceHostileQueryValues Execute(byte id)
        {
            var hostileIDs = new List<byte>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    byte hostileID = r.GetByte("hostile_id");
                    hostileIDs.Add(hostileID);
                }
            }

            SelectAllianceHostileQueryValues ret = new SelectAllianceHostileQueryValues(id, hostileIDs);
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
