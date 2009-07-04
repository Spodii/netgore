using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// A query that checks if a User with the specified name exists.
    /// </summary>
    public class UserExistsQuery : DbQueryReader<string>
    {
        const string _queryString = "SELECT COUNT(*) FROM `characters` WHERE `name`=@name";

        /// <summary>
        /// UserExistsQuery constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public UserExistsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// Checks if a user with the specified name exists.
        /// </summary>
        /// <param name="userName">User name to look for.</param>
        /// <returns>True if a user with the specified name exists, else false.</returns>
        public bool Execute(string userName)
        {
            using (IDataReader r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return false;

                int count = r.GetInt32(0);

                return count > 0;
            }
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
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="userName">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string userName)
        {
            p["@name"] = userName;
        }
    }
}
