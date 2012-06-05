using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CountRecentlyCreatedAccounts : DbQueryReader<uint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountRecentlyCreatedAccounts"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public CountRecentlyCreatedAccounts(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "creator_ip", "time_created");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT COUNT(*) FROM `account` WHERE `creator_ip`=@ip AND `time_created` > DATE_SUB(NOW(), INTERVAL 30 MINUTE)

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(AccountTable.TableName).AddFunc(f.Count()).Where(
                    f.And(f.Equals(s.EscapeColumn("creator_ip"), s.Parameterize("ip")),
                        f.GreaterThan(s.EscapeColumn("time_created"),
                            f.DateSubtractInterval(f.Now(), QueryIntervalType.Minute, 30))));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="ip">The IP to count the recently created accounts for.</param>
        /// <returns>The number of accounts recently created by this IP.</returns>
        public int Execute(uint ip)
        {
            using (var r = ExecuteReader(ip))
            {
                if (!r.Read())
                    return 0;

                return r.GetInt32(0);
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("ip");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, uint item)
        {
            p["ip"] = item;
        }
    }
}