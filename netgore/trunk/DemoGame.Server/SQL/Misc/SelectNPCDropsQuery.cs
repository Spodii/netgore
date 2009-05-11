using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCDropsQuery : DbQueryReader
    {
        const string _queryString = "SELECT * FROM `npc_drops";

        public SelectNPCDropsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCDropValues> Execute()
        {
            List<SelectNPCDropValues> ret = new List<SelectNPCDropValues>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    // Read the values
                    ushort guid = r.GetUInt16("guid");
                    int itemGuid = r.GetInt32("item_guid");
                    byte min = r.GetByte("min");
                    byte max = r.GetByte("max");
                    ushort chance = r.GetUInt16("chance");

                    // Create and enqueue the return object
                    var values = new SelectNPCDropValues(guid, itemGuid, min, max, chance);
                    ret.Add(values);
                }
            }

            return ret;
        }
    }
}
