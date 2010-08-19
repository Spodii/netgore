using System;
using System.Collections.Generic;
using System.Data.Common;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertWorldStatsCountUserConsumeItemQuery : DbQueryNonReader<KeyValuePair<int, int>>
    {
        static readonly string _queryStr = FormatQueryString("INSERT INTO `{0}` (`user_id`,`item_template_id`,`count`) VALUES (@userID,@itemTID,1)" +
                                                             " ON DUPLICATE KEY UPDATE `count`=`count`+1", WorldStatsCountUserConsumeItemTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertWorldStatsCountUserConsumeItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        public InsertWorldStatsCountUserConsumeItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(WorldStatsCountUserConsumeItemTable.DbColumns, "user_id", "item_template_id");
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="itemTemplateID">The item template ID.</param>
        /// <returns>Number of rows affected by the query.</returns>
        public int Execute(int userID, int itemTemplateID)
        {
            return Execute(new KeyValuePair<int, int>(userID, itemTemplateID));
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("userID", "itemTID");
        }

        #endregion

        #region Overrides of DbQueryNonReader<int>

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, KeyValuePair<int, int> item)
        {
            p["user_id"] = item.Key;
            p["item_template_id"] = item.Value;
        }

        #endregion
    }
}