using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CountAccountCharactersByIDQuery : DbQueryReader<AccountID>
    {
        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Select(CharacterTable.TableName).AddFunc(f.Count()).Where(f.Equals(s.EscapeColumn("account_id"), "@accountID"));
            return q.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountAccountCharactersByIDQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CountAccountCharactersByIDQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "account_id");
        }

        public int Execute(AccountID accountID)
        {
            using (var r = ExecuteReader(accountID))
            {
                if (!r.Read())
                    throw new Exception("Failed to read");

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
            return CreateParameters("accountID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="accountID"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="accountID">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AccountID accountID)
        {
            p["accountID"] = (int)accountID;
        }
    }
}