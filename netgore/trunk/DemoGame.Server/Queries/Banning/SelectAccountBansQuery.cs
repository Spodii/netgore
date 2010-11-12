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
    public class SelectAccountBansQuery : DbQueryReader<AccountID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountBansQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountBansQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountBanTable.DbColumns, "account_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `account_id`=@accountID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(AccountBanTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("account_id"),
                    s.Parameterize("accountID")));
            return q.ToString();
        }

        /// <summary>
        /// Gets the ban information for an account.
        /// </summary>
        /// <param name="accountID">The account to get the ban information for.</param>
        /// <returns>The ban information for the given <paramref name="accountID"/>.</returns>
        public IEnumerable<IAccountBanTable> Execute(AccountID accountID)
        {
            var ret = new List<IAccountBanTable>();

            using (var r = ExecuteReader(accountID))
            {
                while (r.Read())
                {
                    var row = new AccountBanTable();
                    row.ReadValues(r);
                    ret.Add(row);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("accountID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AccountID item)
        {
            p["accountID"] = (int)item;
        }
    }
}