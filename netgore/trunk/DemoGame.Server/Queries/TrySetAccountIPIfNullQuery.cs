using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class TrySetAccountIPIfNullQuery : DbQueryReader<TrySetAccountIPIfNullQuery.QueryArgs>
    {
        static readonly string _queryStr =
            string.Format(
                "UPDATE `{0}` SET `current_ip`=@ip,`time_last_login`=NOW()" + " WHERE `id`=@id AND `current_ip` IS NULL",
                AccountTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public TrySetAccountIPIfNullQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "id", "time_last_login", "current_ip");
        }

        /// <summary>
        /// Executes a query to set the IP for the account with the given ID.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="ip">The IP.</param>
        /// <returns>True if the IP was successfully set, and the previous IP for the account was null;
        /// otherwise false.</returns>
        public bool Execute(AccountID accountID, uint ip)
        {
            bool ret;

            using (IDataReader r = ExecuteReader(new QueryArgs(accountID, ip)))
            {
                switch (r.RecordsAffected)
                {
                    case 0:
                        ret = false;
                        break;

                    case 1:
                        ret = true;
                        break;

                    default:
                        const string errmsg = "How the hell did we update more than one account!? Account ID: `{0}`.";
                        string err = string.Format(errmsg, accountID);
                        throw new Exception(err);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@ip", "@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@ip"] = item.IP;
            p["@id"] = item.AccountID;
        }

        /// <summary>
        /// The arguments for the <see cref="TrySetAccountIPIfNullQuery"/> query.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The account ID.
            /// </summary>
            public readonly AccountID AccountID;

            /// <summary>
            /// The ip.
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