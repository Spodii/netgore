using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectNPCTemplateQuery : DbQueryReader<int>
    {
        const string _queryString = "SELECT * FROM `character_template` WHERE `id`=@id";
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SelectNPCTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectNPCTemplateQueryValues Execute(int templateID)
        {
            SelectNPCTemplateQueryValues ret;

            using (IDataReader r = ExecuteReader(templateID))
            {
                if (!r.Read())
                {
                    const string errmsg = "No NPCTemplate found for ID `{0}`.";
                    throw new ArgumentException(string.Format(errmsg, templateID), "templateID");
                }

                // Check that the correct record was grabbed
                int dbID = r.GetInt32("id");
                if (dbID != templateID)
                {
                    const string errmsg = "Performed SELECT for NPC Template with id `{0}`, but got id `{1}`.";
                    string err = string.Format(errmsg, templateID, dbID);
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

                ret = new SelectNPCTemplateQueryValues(templateID, name, bodyIndex, ai, alliance, respawn, giveExp, giveCash, stats);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, int templateID)
        {
            p["@id"] = templateID;
        }
    }
}