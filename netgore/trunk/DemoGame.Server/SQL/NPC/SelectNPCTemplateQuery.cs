using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCTemplateQuery : DbQueryReader<int>
    {
        const string _queryString = "SELECT * FROM `npc_templates` WHERE `guid`=@guid";
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SelectNPCTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectNPCTemplateQueryValues Execute(int guid)
        {
            SelectNPCTemplateQueryValues ret;

            using (IDataReader r = ExecuteReader(guid))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("No NPCTemplate found for guid `{0}`", guid), "guid");

                // Check that the correct record was grabbed
                int dbGuid = r.GetInt32("guid");
                if (dbGuid != guid)
                {
                    const string errmsg = "Performed SELECT for NPC Template with guid `{0}`, but got guid `{1}`!";
                    string err = string.Format(errmsg, guid, dbGuid);
                    log.Fatal(err);
                    Debug.Fail(err);
                    throw new DataException(err);
                }

                // Load the general NPC template values
                string name = r.GetString("name");
                string ai = r.GetString("ai");
                string alliance = r.GetString("alliance");
                ushort bodyIndex = r.GetUInt16("body");
                ushort respawn = r.GetUInt16("respawn");
                ushort giveExp = r.GetUInt16("give_exp");
                ushort giveCash = r.GetUInt16("give_cash");

                // Get the NPCDrop indices
                string npcDropsString = r.GetString("drops");
                var drops = ParseNPCDropsString(guid, npcDropsString);

                // Get the NPCStats
                NPCStats stats = new NPCStats();
                foreach (StatType statType in NPCStats.NonModStats)
                {
                    IStat stat;
                    if (!stats.TryGetStat(statType, out stat))
                        continue;

                    string columnName = statType.GetDatabaseField();
                    int ordinal;
                    if (!r.ContainsField(columnName, out ordinal))
                        continue;

                    stat.Read(r, ordinal);
                }

                ret = new SelectNPCTemplateQueryValues(guid, name, bodyIndex, ai, alliance, respawn, giveExp, giveCash, drops,
                                                       stats);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@guid");
        }

        static List<int> ParseNPCDropsString(int guid, string npcDropsString)
        {
            var dropsStr = npcDropsString.Split(',');
            var drops = new List<int>(dropsStr.Length);

            foreach (string d in dropsStr)
            {
                if (d.Length == 0)
                    continue;

                int value;
                if (!int.TryParse(d, out value))
                {
                    const string errmsg = "Failed to parse NPCDrop index `{0}` for NPCTemplate `{1}`.";
                    Debug.Fail(string.Format(errmsg, d, guid));
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, d, guid);
                }

                drops.Add(value);
            }

            return drops;
        }

        protected override void SetParameters(DbParameterValues p, int guid)
        {
            p["@guid"] = guid;
        }
    }
}