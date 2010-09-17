using System.Collections.Generic;
using System.Data.Common;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectUserByNameQuery : DbQueryReader<string>
    {
        static readonly string _queryStr = FormatQueryString("SELECT * FROM `{0}` WHERE `name`=@name", UserCharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectUserByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectUserByNameQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(UserCharacterTable.DbColumns, "name");
        }

        /// <summary>
        /// Executes the query for the specified user.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>The <see cref="IUserCharacterTable"/> for the user with the name <paramref name="userName"/>, or
        /// null if they do not exist.</returns>
        public IUserCharacterTable Execute(string userName)
        {
            UserCharacterTable ret;

            using (var r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return null;

                ret = new UserCharacterTable();
                ret.ReadValues(r);
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
            return CreateParameters("name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["name"] = item;
        }
    }
}