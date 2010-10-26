using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A query that checks if a User with the specified name exists.
    /// </summary>
    [DbControllerQuery]
    public class UserExistsQuery : DbQueryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserExistsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public UserExistsQuery(DbConnectionPool connectionPool) : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT COUNT(*) FROM `{0}` WHERE `name`=@name

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ViewUserCharacterTable.TableName).AddFunc(f.Count()).Where(f.Equals(s.EscapeColumn("name"),
                                                                                          s.Parameterize("name")));
            return q.ToString();
        }

        /// <summary>
        /// Checks if a user with the specified name exists.
        /// </summary>
        /// <param name="userName">User name to look for.</param>
        /// <returns>True if a user with the specified name exists, else false.</returns>
        public bool Execute(string userName)
        {
            using (var r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return false;

                var count = r.GetInt32(0);

                return count > 0;
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="userName"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="userName">The value used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string userName)
        {
            p["name"] = userName;
        }
    }
}