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
            string.Format("UPDATE `{0}` SET `current_ip`=@ip WHERE `id`=@id AND `current_ip` IS NULL", AccountTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public TrySetAccountIPIfNullQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        /// <summary>
        /// Executes a query to set the IP for the account with the given ID.
        /// </summary>
        /// <param name="id">The account ID.</param>
        /// <param name="ip">The IP.</param>
        /// <returns>True if the IP was successfully set, and the previous IP for the account was null;
        /// otherwise false.</returns>
        public bool Execute(int id, uint ip)
        {
            bool ret;

            using (IDataReader r = ExecuteReader(new QueryArgs(id, ip)))
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
                        string err = string.Format(errmsg, id);
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
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@ip"] = item.IP;
            p["@id"] = item.AccountID;
        }

        public struct QueryArgs
        {
            public readonly int AccountID;
            public readonly uint IP;

            public QueryArgs(int accountID, uint ip)
            {
                AccountID = accountID;
                IP = ip;
            }
        }
    }
}