using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CountGuildFoundersQuery : DbQueryReader<GuildID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountGuildFoundersQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public CountGuildFoundersQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT COUNT(*) FROM `{0}` WHERE `guild_id` = @guildID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(GuildMemberTable.TableName).AddFunc(f.Count()).Where(f.Equals(s.EscapeColumn("guild_id"),
                    s.Parameterize("guild_id")));
            return q.ToString();
        }

        public int Execute(GuildID guildID)
        {
            using (var r = ExecuteReader(guildID))
            {
                if (!r.Read())
                    return 0;

                return r.GetInt32(0);
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("guild_id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, GuildID item)
        {
            p["guild_id"] = (int)item;
        }
    }
}