using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `name`=@name", DBTables.Character);

        public SelectCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectCharacterQueryValues Execute(string userName, CharacterStatsBase stats)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");

            SelectCharacterQueryValues ret;

            using (IDataReader r = ExecuteReader(userName))
            {
                ret = ReadQueryValues(r, stats);
            }

            return ret;
        }

        public static SelectCharacterQueryValues ReadQueryValues(IDataReader r, CharacterStatsBase stats)
        {
            // HACK: I know this method is a horrible, horrible design, but SelectCharacterQuery wont be around much longer

            if (!r.Read())
                throw new ArgumentException("Could not find user.");

            // Read the general user values
            CharacterID id = r.GetCharacterID("id");
            string name = r.GetString("name");
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
            var ret = new SelectCharacterQueryValues(id, name, mapIndex, pos, body, stats);
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