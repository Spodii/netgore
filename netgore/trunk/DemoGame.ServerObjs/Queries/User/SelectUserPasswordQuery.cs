using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectUserPasswordQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT `password` FROM `{0}` WHERE `name`=@name", CharacterTable.TableName);

        public SelectUserPasswordQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public string Execute(string userName)
        {
            using (IDataReader r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return null;

                return r[0].ToString();
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