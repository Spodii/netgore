using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCTemplateDropsQuery : DbQueryReader<int>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _queryString = "SELECT * FROM `npc_templates_drops` WHERE `npc_guid`=@npcGuid";

        public SelectNPCTemplateDropsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCTemplateDropsQueryValues> Execute(int npcGuid)
        {
            var ret = new List<SelectNPCTemplateDropsQueryValues>();

            using (IDataReader r = ExecuteReader(npcGuid))
            {
                while (r.Read())
                {
                    ushort itemGuid = r.GetUInt16("item_guid");
                    byte min = r.GetByte("min");
                    byte max = r.GetByte("max");
                    ushort chance = r.GetUInt16("chance");

                    NPCDropChance dropChance = new NPCDropChance(min, max, chance);
                    var value = new SelectNPCTemplateDropsQueryValues(itemGuid, dropChance);
                    ret.Add(value);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@npcGuid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@npcGuid"] = item;
        }
    }
}
