using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SetAccountCurrentIPNullQuery : DbQueryNonReader<AccountID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetAccountCurrentIPNullQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SetAccountCurrentIPNullQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(AccountTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // UPDATE `{0}` SET `current_ip` = NULL WHERE `id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Update(AccountTable.TableName).Add("current_ip", "NULL").Where(f.Equals(s.EscapeColumn("id"),
                    s.Parameterize("id")));
            return q.ToString();
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
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AccountID item)
        {
            p["id"] = (int)item;
        }
    }
}