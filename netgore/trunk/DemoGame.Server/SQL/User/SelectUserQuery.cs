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

                // HACK: Do not use r.GetOrdinal() externally
                var guid = (ushort)r.GetInt16(r.GetOrdinal("guid")); // HACK: Needs to be r.GetUInt16()
                var mapIndex = (ushort)r.GetInt16(r.GetOrdinal("map")); // HACK: Needs to be r.GetUInt16()
                var x = r.GetFloat(r.GetOrdinal("x"));
                var y = r.GetFloat(r.GetOrdinal("y"));
                var body = (ushort)r.GetInt16(r.GetOrdinal("body")); // HACK: Needs to be r.GetUInt16()

                var pos = new Vector2(x, y);

                foreach (var statType in UserStats.DatabaseStats)
                {
                    IStat stat;
                    if (!stats.TryGetStat(statType, out stat))
                        continue;

                    string columnName = statType.GetDatabaseField();
                    stat.Read(r, r.GetOrdinal(columnName)); // HACK: Need to remove r.GetOrdinal()
                }

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
