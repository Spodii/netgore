using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace NetGore.Features.Banning
{
    [DbControllerQuery]
    sealed class StoredProcIsBanned : DbQueryReader<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcIsBanned"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        public StoredProcIsBanned(DbConnectionPool connectionPool)
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
            // CALL ft_banning_isbanned(@accountID)

            var q = qb.CallProcedure("ft_banning_isbanned").AddParam("accountID");
            return q.ToString();
        }

        /// <summary>
        /// Checks if an account is currently banned.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        public bool Execute(int accountID)
        {
            using (var r = ExecuteReader(accountID))
            {
                if (!r.Read())
                    return false;

                var ret = r.GetInt32(0);

                if (ret != 0)
                    return true;
                else
                    return false;
            }
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
        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["accountID"] = item;
        }
    }
}