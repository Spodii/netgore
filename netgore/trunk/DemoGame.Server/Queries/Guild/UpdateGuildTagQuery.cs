using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateGuildTagQuery : DbQueryNonReader<UpdateGuildTagQuery.QueryArgs>
    {
        static readonly string _queryStr = FormatQueryString("UPDATE `{0}` SET `tag`=@value WHERE `id`=@id", GuildTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGuildTagQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public UpdateGuildTagQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(GuildTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id", "value");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["id"] = (int)item.ID;
            p["value"] = item.Value;
        }

        /// <summary>
        /// Arguments for the <see cref="UpdateGuildNameQuery"/> query.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The ID of the guild.
            /// </summary>
            public readonly GuildID ID;

            /// <summary>
            /// The new value.
            /// </summary>
            public readonly string Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="id">The id.</param>
            /// <param name="value">The value.</param>
            public QueryArgs(GuildID id, string value)
            {
                ID = id;
                Value = value;
            }
        }
    }
}