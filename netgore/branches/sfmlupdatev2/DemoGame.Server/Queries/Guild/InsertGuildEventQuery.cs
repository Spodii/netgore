using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertGuildEventQuery : DbQueryNonReader<InsertGuildEventQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertGuildEventQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public InsertGuildEventQuery(DbConnectionPool connectionPool)
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
            // INSERT INTO `{0}` {1}

            var f = qb.Functions;
            var q = qb.Insert(GuildEventTable.TableName).AddAutoParam(GuildEventTable.DbColumns).Remove("id").Add("created",
                f.Now());
            return q.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("guild_id", "character_id", "target_character_id", "event_id", "arg0", "arg1", "arg2");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["guild_id"] = (int)item.GuildID;
            p["character_id"] = (int)item.CharacterID;
            p["target_character_id"] = (int?)item.TargetID;
            p["event_id"] = item.EventID;
            p["arg0"] = item.Arg0;
            p["arg1"] = item.Arg1;
            p["arg2"] = item.Arg2;
        }

        /// <summary>
        /// Arguments for the <see cref="InsertGuildEventQuery"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            /// <summary>
            /// Arg0.
            /// </summary>
            public readonly string Arg0;

            /// <summary>
            /// Arg1
            /// </summary>
            public readonly string Arg1;

            /// <summary>
            /// Arg2.
            /// </summary>
            public readonly string Arg2;

            /// <summary>
            /// The character ID.
            /// </summary>
            public readonly CharacterID CharacterID;

            /// <summary>
            /// The event ID.
            /// </summary>
            public readonly byte EventID;

            /// <summary>
            /// The guild ID.
            /// </summary>
            public readonly GuildID GuildID;

            /// <summary>
            /// The target character ID.
            /// </summary>
            public readonly CharacterID? TargetID;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="guildID">The guild ID.</param>
            /// <param name="charID">The char ID.</param>
            /// <param name="targetID">The target char ID.</param>
            /// <param name="eventID">The event ID.</param>
            /// <param name="arg0">The arg0.</param>
            /// <param name="arg1">The arg1.</param>
            /// <param name="arg2">The arg2.</param>
            public QueryArgs(GuildID guildID, CharacterID charID, CharacterID? targetID, byte eventID, string arg0, string arg1,
                             string arg2)
            {
                GuildID = guildID;
                CharacterID = charID;
                TargetID = targetID;
                EventID = eventID;
                Arg0 = arg0;
                Arg1 = arg1;
                Arg2 = arg2;
            }
        }
    }
}