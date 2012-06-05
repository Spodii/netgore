using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildMemberByNameQuery : DbQueryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGuildMemberByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildMemberByNameQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(GuildMemberTable.DbColumns, "character_id");
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "id", "name");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT t1.* FROM `{0}` AS t1 INNER JOIN `{1}` AS t2 ON t1.character_id = t2.id WHERE t2.name = @name

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(GuildMemberTable.TableName, "t1").AllColumns("t1").InnerJoinOnColumn(CharacterTable.TableName, "t2",
                    "id", "t1", "character_id").Where(f.Equals(s.EscapeColumn("t2.name"), s.Parameterize("name")));
            return q.ToString();
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
            return CreateParameters("name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["name"] = item;
        }
    }
}