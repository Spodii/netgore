using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectUserQuery : DbQueryReader<string>
    {
        const string _queryString = "SELECT * FROM `users` WHERE `name`=@name";

        public SelectUserQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectUserQueryValues Execute(string userName, CharacterStatsBase stats)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");

            SelectUserQueryValues ret;

            using (IDataReader r = ExecuteReader(userName))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find a user with the name `{0}`", userName));

                // Read the general user values
                ushort guid = r.GetUInt16("guid");
                MapIndex mapIndex = r.GetMapIndex("map");
                float x = r.GetFloat("x");
                float y = r.GetFloat("y");
                ushort body = r.GetUInt16("body");

                Vector2 pos = new Vector2(x, y);

                // Read the user's stats
                foreach (StatType statType in UserStats.DatabaseStats)
                {
                    IStat stat;
                    if (!stats.TryGetStat(statType, out stat))
                        continue;

                    string columnName = statType.GetDatabaseField();
                    stat.Read(r, columnName);
                }

                // Create the return object
                ret = new SelectUserQueryValues(guid, mapIndex, pos, body, stats);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@name");
        }

        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["@name"] = item;
        }
    }
}