using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SetAccountCurrentIPNullQuery : DbQueryNonReader<AccountID>
    {
        static readonly string _queryStr = string.Format("UPDATE `{0}` SET `current_ip` = NULL WHERE `id`=@id",
                                                         AccountTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SetAccountCurrentIPNullQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SetAccountCurrentIPNullQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(AccountTable.DbKeyColumns, "id");
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
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AccountID item)
        {
            p["@id"] = (int)item;
        }
    }
}