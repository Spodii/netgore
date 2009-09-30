using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CountAccountCharactersByNameQuery : DbQueryReader<string>
    {
        static readonly string _queryStr =
            string.Format("SELECT COUNT(*) FROM `{0}`,`{1}` WHERE {1}.name=@name AND {0}.account_id=account.id",
                          CharacterTable.TableName, AccountTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="CountAccountCharactersByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CountAccountCharactersByNameQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "account_id");
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "id", "name");
        }

        public int Execute(string accountName)
        {
            using (var r = ExecuteReader(accountName))
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
            return CreateParameters("@name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="accountName"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="accountName">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string accountName)
        {
            p["@name"] = accountName;
        }
    }
}