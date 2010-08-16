using System.Collections.Generic;
using System.Data.Common;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class PeerTradingRemoveCashQuery : DbQueryNonReader<CharacterID>
    {
        static readonly string _queryStr = FormatQueryString("DELETE FROM `{0}` WHERE `character_id`=@characterID",
                                                             ActiveTradeCashTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradingRemoveCashQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        public PeerTradingRemoveCashQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(ActiveTradeCashTable.DbKeyColumns, "character_id");
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("character_id");
        }

        #endregion

        #region Overrides of DbQueryNonReader<CharacterID>

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID item)
        {
            p["character_id"] = (int)item;
        }

        #endregion
    }
}