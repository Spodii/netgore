using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildMembersListQuery : DbQueryReader<GuildID>
    {
        static readonly string _queryStr =
            string.Format(
                "SELECT t2.name AS 'name', t1.rank AS 'rank' FROM `{0}` AS t1 INNER JOIN `{1}` AS t2 ON t1.character_id = t2.id WHERE t1.guild_id = @guildID ORDER BY t1.rank DESC, t2.name ASC",
                GuildMemberTable.TableName, CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGuildMembersListQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildMembersListQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTable.DbKeyColumns, "id");
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "name");
            QueryAsserts.ContainsColumns(GuildMemberTable.DbColumns, "rank", "character_id", "guild_id");
        }

        public IEnumerable<GuildMemberNameRank> Execute(GuildID guildID)
        {
            var ret = new List<GuildMemberNameRank>();

            using (var r = ExecuteReader(guildID))
            {
                while (r.Read())
                {
                    var name = r.GetString("name");
                    var rank = r.GetByte("rank");
                    var value = new GuildMemberNameRank(name, rank);
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