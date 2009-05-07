using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Db;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public class SelectNPCTemplateQuery : DbQueryReader<int>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _queryString = "SELECT * FROM `npc_templates` WHERE `guid`=@guid";

        public SelectNPCTemplateQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectNPCTemplateQueryValues Execute(int guid)
        {
            SelectNPCTemplateQueryValues ret;

            using (var r = ExecuteReader(guid))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("No NPCTemplate found for guid `{0}`", guid), "guid");

                // HACK: Remove r.GetOrdinal()s
                int dbGuid = r.GetInt32(r.GetOrdinal("guid"));
                if (dbGuid != guid)
                {
                    const string errmsg = "Performed SELECT for NPC Template with guid `{0}`, but got guid `{1}`!";
                    string err = string.Format(errmsg, guid, dbGuid);
                    log.Fatal(err);
                    Debug.Fail(err);
                    throw new DataException(string.Format(errmsg, guid, dbGuid));
                }

                string name = r.GetString(r.GetOrdinal("name"));
                string ai = r.GetString(r.GetOrdinal("ai"));
                string alliance = r.GetString(r.GetOrdinal("alliance"));
                ushort bodyIndex = (ushort)r.GetInt16(r.GetOrdinal("body")); // HACK: Use r.GetUShort()
                ushort respawn = (ushort)r.GetInt16(r.GetOrdinal("respawn")); // HACK: Use r.GetUShort()
                ushort giveExp = (ushort)r.GetInt16(r.GetOrdinal("give_exp")); // HACK: Use r.GetUShort()
                ushort giveCash = (ushort)r.GetInt16(r.GetOrdinal("give_cash")); // HACK: Use r.GetUShort()
                
                // Get the NPCDrop indices
                string dropsStrRaw = r.GetString(r.GetOrdinal("drops"));
                string[] dropsStr = dropsStrRaw.Split(',');
                List<int> drops = new List<int>(dropsStr.Length);
                foreach (var d in dropsStr)
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

                // Get the NPCStats
                // HACK: Do this properly
                NPCStats stats = new NPCStats();
                foreach (StatType statType in NPCStats.NonModStats)
                {
                    IStat stat;
                    if (!stats.TryGetStat(statType, out stat))
                        continue;

                    string columnName = statType.GetDatabaseField();
                    int ordinal;
                    try
                    {
                        ordinal = r.GetOrdinal(columnName);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }

                    stat.Read(r, ordinal);
                }

                ret = new SelectNPCTemplateQueryValues(guid, name, bodyIndex, ai, alliance, respawn, giveExp, giveCash, drops, stats);
            }

            return ret; 
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@guid");
        }

        protected override void SetParameters(DbParameterValues p, int guid)
        {
            p["@guid"] = guid;
        }
    }
}
