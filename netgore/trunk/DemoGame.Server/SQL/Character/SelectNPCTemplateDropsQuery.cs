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
        const string _queryString = "SELECT * FROM `character_template_drops` WHERE `character_id`=@characterID";

        public SelectNPCTemplateDropsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCTemplateDropsQueryValues> Execute(int characterID)
        {
            var ret = new List<SelectNPCTemplateDropsQueryValues>();

            using (IDataReader r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    ushort itemID = r.GetUInt16("item_id");
                    byte min = r.GetByte("min");
                    byte max = r.GetByte("max");
                    ushort chance = r.GetUInt16("chance");

                    NPCDropChance dropChance = new NPCDropChance(min, max, chance);
                    var value = new SelectNPCTemplateDropsQueryValues(itemID, dropChance);
                    ret.Add(value);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterID");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@characterID"] = item;
        }
    }
}
