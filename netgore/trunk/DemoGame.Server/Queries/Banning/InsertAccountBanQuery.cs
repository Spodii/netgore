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
    public class InsertAccountBanQuery : DbQueryNonReader<InsertAccountBanQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertAccountBanQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertAccountBanQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountBanTable.DbColumns, "account_id", "start_time", "end_time", "issued_by", "reason",
                "expired");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO `{0}` (`account_id`,`start_time`,`end_time`,`reason`,`issued_by`,`expired`)
            //      VALUES(@accountID,NOW(),DATE_ADD(NOW(), INTERVAL @secs SECOND),@reason,@issuedBy,0)

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Insert(AccountBanTable.TableName).AddParam("account_id", "accountID").Add("start_time", f.Now()).Add(
                    "end_time", f.DateAddInterval(f.Now(), f.Interval(QueryIntervalType.Second, s.Parameterize("secs")))).AddParam
                    ("reason", "reason").AddParam("issued_by", "issuedBy").Add("expired", "0");
            return q.ToString();
        }

        /// <summary>
        /// Executes the specified account ID.
        /// </summary>
        /// <param name="accountID">The ID of the account to ban.</param>
        /// <param name="length">The length of the ban.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the person or system that issued the ban.</param>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">Tried to perform an insert query for a key that already exists.</exception>
        public int ExecuteWithResult(AccountID accountID, TimeSpan length, string reason, string issuedBy)
        {
            var args = new QueryArgs(accountID, (int)length.TotalSeconds, reason, issuedBy);
            return ExecuteWithResult(args);
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("accountID", "secs", "reason", "issuedBy");
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
            p["secs"] = item.Secs;
            p["reason"] = item.Reason;
            p["issuedBy"] = item.IssuedBy;
        }

        /// <summary>
        /// Arguments for the <see cref="InsertAccountBanQuery"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            readonly AccountID _accountID;
            readonly int _secs;
            readonly string _reason;
            readonly string _issuedBy;

            /// <summary>
            /// Gets the ID of the account to ban.
            /// </summary>
            public AccountID AccountID
            {
                get { return _accountID; }
            }

            /// <summary>
            /// Gets the length of the ban in seconds.
            /// </summary>
            public int Secs
            {
                get { return _secs; }
            }

            /// <summary>
            /// Gets the reason for the ban.
            /// </summary>
            public string Reason
            {
                get { return _reason; }
            }

            /// <summary>
            /// Gets the name of the person or system who issued the ban.
            /// </summary>
            public string IssuedBy
            {
                get { return _issuedBy; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="accountID">The account ID.</param>
            /// <param name="secs">The secs.</param>
            /// <param name="reason">The reason.</param>
            /// <param name="issuedBy">The issued by.</param>
            public QueryArgs(AccountID accountID, int secs, string reason, string issuedBy)
            {
                _accountID = accountID;
                _secs = secs;
                _reason = reason;
                _issuedBy = issuedBy;
            }
        }
    }
}