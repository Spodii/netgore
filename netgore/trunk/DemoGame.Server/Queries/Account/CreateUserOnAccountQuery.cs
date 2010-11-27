using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CreateUserOnAccountQuery : DbQueryReader<CreateUserOnAccountQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserOnAccountQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CreateUserOnAccountQuery(DbConnectionPool connectionPool)
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
            // SELECT CreateUserOnAccount(@accountName, @userName)

            var s = qb.Settings;
            var q = qb.SelectFunction("create_user_on_account").Add(s.Parameterize("accountName"), s.Parameterize("userName"));
            return q.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("accountName", "userName");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["accountName"] = item.AccountName;
            p["userName"] = item.UserName;
        }

        public bool TryExecute(string accountName, string userName, out string errorMsg)
        {
            // Ensure the character name is valid
            if (!GameData.UserName.IsValid(userName))
            {
                errorMsg = "Invalid character name.";
                return false;
            }

            // Execute the query
            var queryArgs = new QueryArgs(accountName, userName);
            using (var r = ExecuteReader(queryArgs))
            {
                if (!r.Read())
                    errorMsg = "Unknown error executing stored function - query contained no results.";
                else
                    errorMsg = r.GetString(0) ?? string.Empty;
            }

            // Make sure the error message is trimmed
            errorMsg = errorMsg.Trim();

            // If there is an empty error message, it was successful
            return errorMsg.Length == 0;
        }

        /// <summary>
        /// The arguments for the <see cref="CreateUserOnAccountQuery"/> query.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            /// <summary>
            /// The account ID.
            /// </summary>
            public readonly string AccountName;

            /// <summary>
            /// The character name.
            /// </summary>
            public readonly string UserName;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="accountName">The account name.</param>
            /// <param name="userName">Name of the user character.</param>
            public QueryArgs(string accountName, string userName)
            {
                AccountName = accountName;
                UserName = userName;
            }
        }
    }
}