using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.Db;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public class SelectUserQuery : DbQueryReader<string>
    {
        const string _queryString = "SELECT * FROM `users` WHERE `name`=@name";

        public SelectUserQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectUserQueryValues Execute(string userName, CharacterStatsBase stats)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");

            SelectUserQueryValues ret;

            using (var r = ExecuteReader(userName))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find a user with the name `{0}`", userName));

                // Read the general user values
                var guid = r.GetUInt16("guid");
                var mapIndex = r.GetUInt16("map"); 
                var x = r.GetFloat("x");
                var y = r.GetFloat("y");
                var body = r.GetUInt16("body");

                var pos = new Vector2(x, y);

                // Read the user's stats
                foreach (var statType in UserStats.DatabaseStats)
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
