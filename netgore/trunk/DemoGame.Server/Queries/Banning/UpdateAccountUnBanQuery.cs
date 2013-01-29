using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateAccountUnBanQuery : DbQueryNonReader<UpdateAccountUnBanQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountUnBanQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public UpdateAccountUnBanQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountBanTable.DbColumns, "account_id", "end_time", "issued_by","expired");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // UPDATE `{0}` SET `end_time` = NOW(), `issued_by` = @issuedBy, `expired` = 1 WHERE `account_id`=@accountID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Update(AccountBanTable.TableName).Add("end_time", f.Now()).AddParam("issued_by", "issuedBy").Add("expired", "1").Where(f.Equals(
                    s.EscapeColumn("account_id"), s.Parameterize("accountID")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified parameters.
        /// </summary>
        /// <param name="accountID">The ID of the account to unban.</param>
        /// <param name="issuedBy">The name of the person or system that issued the unban.</param>
        public int ExecuteWithResult(AccountID accountID, string issuedBy)
        {
            var args = new QueryArgs(accountID, issuedBy);
            return ExecuteWithResult(args);
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("accountID", "issuedBy");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["accountID"] = (int)item.AccountID;
            p["issuedBy"] = item.IssuedBy;
        }

        /// <summary>
        /// Arguments for the <see cref="UpdateAccountUnBanQuery"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            readonly AccountID _accountID;
            readonly string _issuedBy;

            /// <summary>
            /// Gets the ID of the account to ban.
            /// </summary>
            public AccountID AccountID
            {
                get { return _accountID; }
            }

            /// <summary>
            /// Gets the name of the person or system who issued the unban.
            /// </summary>
            public string IssuedBy
            {
                get { return _issuedBy; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="accountID">The account ID.</param>
            public QueryArgs(AccountID accountID, string issuedBy)
            {
                _accountID = accountID;
                _issuedBy = issuedBy;
            }
        }
    }
}