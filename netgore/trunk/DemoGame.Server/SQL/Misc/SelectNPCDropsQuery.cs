using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCDropsQuery : DbQueryReader<object>
    {
        const string _queryString = "SELECT * FROM `npc_drops";

        public SelectNPCDropsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCDropValues> Execute()
        {
            List<SelectNPCDropValues> ret = new List<SelectNPCDropValues>();

            using (var r = ExecuteReader(null))
            {
                while (r.Read())
                {
                    // HACK: Remove r.GetOrdinal()s
                    ushort guid = (ushort)r.GetInt16(r.GetOrdinal("guid")); // HACK: Use r.GetUShort()
                    int itemGuid = r.GetInt32(r.GetOrdinal("item_guid"));
                    byte min = r.GetByte(r.GetOrdinal("min"));
                    byte max = r.GetByte(r.GetOrdinal("max"));
                    ushort chance = (ushort)r.GetInt16(r.GetOrdinal("chance")); // HACK: Use r.GetUShort()

                    var values = new SelectNPCDropValues(guid, itemGuid, min, max, chance);
                    ret.Add(values);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        protected override void SetParameters(DbParameterValues p, object item)
        {
            throw new MethodAccessException();
        }
    }
}
