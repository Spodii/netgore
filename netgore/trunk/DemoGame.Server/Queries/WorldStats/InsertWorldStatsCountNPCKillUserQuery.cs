using System;
using System.Collections.Generic;
using System.Data.Common;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertWorldStatsCountNPCKillUserQuery : DbQueryNonReader<KeyValuePair<int, int>>
    {
        static readonly string _queryStr = FormatQueryString("INSERT INTO `{0}` (`user_id`,`npc_template_id`,`count`) VALUES (@userID,@npcTID,1)" +
                                                             " ON DUPLICATE KEY UPDATE `count`=`count`+1", WorldStatsCountNpcKillUserTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertWorldStatsCountNPCKillUserQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        public InsertWorldStatsCountNPCKillUserQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(WorldStatsCountNpcKillUserTable.DbColumns, "user_id", "npc_template_id");
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="npcTID">The NPC template ID.</param>
        /// <returns>Number of rows affected by the query.</returns>
        public int Execute(int userID, int npcTID)
        {
            return Execute(new KeyValuePair<int, int>(userID, npcTID));
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("userID", "npcTID");
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
            p["userID"] = item.Key;
            p["npcTID"] = item.Value;
        }

        #endregion
    }
}