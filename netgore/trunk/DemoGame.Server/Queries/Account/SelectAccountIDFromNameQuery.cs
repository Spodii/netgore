using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAccountIDFromNameQuery : DbQueryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountIDFromNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectAccountIDFromNameQuery(DbConnectionPool connectionPool)
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
            // SELECT `id` FROM `{0}` WHERE `name`=@name

            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Select(AccountTable.TableName).Add("id").Where(f.Equals(s.EscapeColumn("name"), s.Parameterize("name")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="accountName">The account name to get the ID for.</param>
        /// <returns>The ID of the account with the given <paramref name="accountName"/>, or null if no such
        /// account exists.</returns>
        public AccountID? Execute(string accountName)
        {
            AccountID? ret;

            using (var r = ExecuteReader(accountName))
            {
                if (!r.Read())
                    ret = null;
                else
                    ret = r.GetAccountID(0);
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