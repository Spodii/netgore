using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAccountQuery : DbQueryReader<SelectAccountQuery.QueryArgs>
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}` WHERE `name`=@name", AccountTable.TableName);

        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectAccountQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public bool TryExecute(string name, string password, UserAccount userAccount)
        {
            bool ret;

            using (var r = ExecuteReader(new QueryArgs(name, password)))
            {
                if (!r.Read())
                {
                    ret = false;
                }
                else
                {
                    userAccount.ReadValues(r);
                    ret = true;
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
            return CreateParameters("@name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <typeparam name="T">Type of the object containing the values to set.</typeparam>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@name"] = item.Name;
        }

        public struct QueryArgs
        {
            public readonly string Name;
            public readonly string Password;

            public QueryArgs(string name, string password)
            {
                Name = name;
                Password = password;
            }
        }
    }
}