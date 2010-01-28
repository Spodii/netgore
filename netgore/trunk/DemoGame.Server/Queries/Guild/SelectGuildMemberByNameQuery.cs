using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildMemberByNameQuery : DbQueryReader<string>
    {
        static readonly string _queryStr =
            string.Format(
                "SELECT t1.* FROM `{0}` AS t1 INNER JOIN `{1}` AS t2 ON t1.character_id = t2.id WHERE t2.name = @name;",
                GuildMemberTable.TableName, CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGuildMemberByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildMemberByNameQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(GuildMemberTable.DbColumns, "character_id");
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "id", "name");
        }

        public IGuildMemberTable Execute(string name)
        {
            using (var r = ExecuteReader(name))
            {
                if (!r.Read())
                    return null;

                var ret = new GuildMemberTable();
                ret.TryReadValues(r);
                return ret;
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["@name"] = item;
        }
    }
}