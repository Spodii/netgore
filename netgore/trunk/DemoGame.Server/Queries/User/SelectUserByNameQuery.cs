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
    public class SelectUserByNameQuery : DbQueryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectUserByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectUserByNameQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ViewUserCharacterTable.DbColumns, "name");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `name`=@name

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ViewUserCharacterTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("name"),
                    s.Parameterize("name")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query for the specified user.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>The <see cref="IViewUserCharacterTable"/> for the user with the name <paramref name="userName"/>, or
        /// null if they do not exist.</returns>
        public IViewUserCharacterTable Execute(string userName)
        {
            ViewUserCharacterTable ret;

            using (var r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return null;

                ret = new ViewUserCharacterTable();
                ret.ReadValues(r);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["name"] = item;
        }
    }
}