using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertAccountIPQuery : DbQueryNonReader<InsertAccountIPQuery.QueryArgs>
    {
        static readonly string _queryStr = string.Format("INSERT INTO `{0}` SET `account_id`=@accountID, `ip`=@ip, `time`=NOW()",
                                                         AccountIpsTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertAccountIPQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertAccountIPQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public int Execute(AccountID accountID, uint ip)
        {
            return Execute(new QueryArgs(accountID, ip));
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
            return CreateParameters("accountID", "ip");
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
            p["ip"] = item.IP;
        }

        /// <summary>
        /// The arguments for the <see cref="InsertAccountIPQuery"/> query.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The account ID.
            /// </summary>
            public readonly AccountID AccountID;

            /// <summary>
            /// The IP.
            /// </summary>
            public readonly uint IP;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="accountID">The account ID.</param>
            /// <param name="ip">The ip.</param>
            public QueryArgs(AccountID accountID, uint ip)
            {
                AccountID = accountID;
                IP = ip;
            }
        }
    }
}