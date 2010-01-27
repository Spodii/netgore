using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceGuildMemberQuery : DbQueryNonReader<ReplaceGuildMemberQuery.QueryArgs>
    {
        static readonly string _queryStr = string.Format("INSERT INTO `{0}` (`character_id`,`guild_id`,`rank`,`joined`) VALUES (@charID, @guildID, @rank, NOW())" 
            + " ON DUPLICATE KEY UPDATE `guild_id`=@guildID, `rank`=@rank;",
            GuildMemberTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceGuildMemberQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public ReplaceGuildMemberQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(GuildMemberTable.DbColumns, "character_id", "guild_id", "rank", "joined");
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@charID", "@guildID", "@rank");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@charID"] = (int)item.CharacterID;
            p["@guildID"] = (int)item.GuildID;
            p["@rank"] = (byte)item.Rank;
        }

        /// <summary>
        /// Arguments for the <see cref="ReplaceGuildMemberQuery"/> query.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The character ID.
            /// </summary>
            public readonly CharacterID CharacterID;

            /// <summary>
            /// The guild ID.
            /// </summary>
            public readonly GuildID GuildID;

            /// <summary>
            /// The rank.
            /// </summary>
            public readonly GuildRank Rank;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="characterID">The character ID.</param>
            /// <param name="guildID">The guild ID.</param>
            /// <param name="guildRank">The guild rank.</param>
            public QueryArgs(CharacterID characterID, GuildID guildID, GuildRank guildRank)
            {
                CharacterID = characterID;
                GuildID = guildID;
                Rank = guildRank;
            }
        }
    }
}
