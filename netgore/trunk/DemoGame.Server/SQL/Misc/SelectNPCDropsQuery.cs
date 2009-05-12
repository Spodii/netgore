using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCDropsQuery : DbQueryReader
    {
        const string _queryString = "SELECT * FROM `npc_drops`";

        public SelectNPCDropsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCDropQueryValues> Execute()
        {
            var ret = new List<SelectNPCDropQueryValues>();

            using (IDataReader r = ExecuteReader())
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
                    SelectNPCDropQueryValues values = new SelectNPCDropQueryValues(guid, itemGuid, min, max, chance);
                    ret.Add(values);
                }
            }

            return ret;
        }
    }
}