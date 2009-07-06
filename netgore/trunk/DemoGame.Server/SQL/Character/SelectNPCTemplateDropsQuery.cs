using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectNPCTemplateDropsQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly string _queryString = string.Format(
            "SELECT * FROM `{0}` WHERE `character_id`=@characterID", DBTables.CharacterTemplateDrops);

        public SelectNPCTemplateDropsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectNPCTemplateDropsQueryValues> Execute(CharacterTemplateID characterID)
        {
            var ret = new List<SelectNPCTemplateDropsQueryValues>();

            using (IDataReader r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    ItemID itemID = r.GetItemID("item_id");
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

        protected override void SetParameters(DbParameterValues p, CharacterTemplateID characterID)
        {
            p["@characterID"] = characterID;
        }
    }
}
