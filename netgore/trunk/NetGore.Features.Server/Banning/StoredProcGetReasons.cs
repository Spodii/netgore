using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace NetGore.Features.Banning
{
    [DbControllerQuery]
    sealed class StoredProcGetReasons : DbQueryReader<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcGetReasons"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        public StoredProcGetReasons(DbConnectionPool connectionPool)
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
            // CALL ft_banning_get_reasons(@accountID)

            var q = qb.CallProcedure("ft_banning_get_reasons").AddParam("accountID");
            return q.ToString();
        }

        /// <summary>
        /// Checks if an account is currently banned, and if so, gets the reason(s) why and how long the ban will last.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <param name="reasons">When this method returns true, contains the reasons as to why the account is
        /// banned, where each reason is delimited by a new line.</param>
        /// <param name="minsLeft">When this method returns true, contains the amount of time remaining in minutes for
        /// all bans to expire.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        public bool Execute(int accountID, out string reasons, out int minsLeft)
        {
            using (var r = ExecuteReader(accountID))
            {
                // Check for any results
                var isValid = r.Read();
                if (isValid)
                {
                    // Ensure we actually got valid results by checking if any of the two columns are now
                    isValid = !r.IsDBNull(0) && !r.IsDBNull(1);
                }

                if (!isValid)
                {
                    reasons = null;
                    minsLeft = 0;
                    return false;
                }

                reasons = r.GetString("reasons");

                // Reading the minutes is a little more complicated since there is no "type" attached to it
                var minsLeftRaw = r.GetString("mins_left");
                minsLeft = (int)Math.Round(double.Parse(minsLeftRaw));
            }

            return true;
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