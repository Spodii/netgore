using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildEventsQuery : DbQueryReader<GuildID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGuildEventsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildEventsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(GuildEventTable.DbColumns, "guild_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `guild_id` = @guildID ORDER BY `id` DESC LIMIT 50

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(GuildEventTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("guild_id"),
                    s.Parameterize("guildID"))).OrderBy(s.EscapeColumn("id"), OrderByType.Descending).Limit(50);
            return q.ToString();
        }

        public IEnumerable<IGuildEventTable> Execute(GuildID guildID)
        {
            var ret = new List<IGuildEventTable>();

            using (var r = ExecuteReader(guildID))
            {
                while (r.Read())
                {
                    var value = new GuildEventTable();
                    value.ReadValues(r);
                    ret.Add(value);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("guildID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, GuildID item)
        {
            p["guildID"] = (int)item;
        }
    }
}