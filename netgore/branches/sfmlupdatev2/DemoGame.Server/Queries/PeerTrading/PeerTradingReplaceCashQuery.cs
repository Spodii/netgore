using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class PeerTradingReplaceCashQuery : DbQueryNonReader<IActiveTradeCashTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradingReplaceCashQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        public PeerTradingReplaceCashQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.AreColumns(ActiveTradeCashTable.DbColumns, "character_id", "cash");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO {0} {1}
            //      ON DUPLICATE KEY UPDATE <{1} - keys>

            var q =
                qb.Insert(ActiveTradeCashTable.TableName).AddAutoParam(ActiveTradeCashTable.DbColumns).ODKU().AddFromInsert(
                    ActiveTradeCashTable.DbKeyColumns);
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified values.
        /// </summary>
        /// <param name="characterID">The character ID.</param>
        /// <param name="cash">The cash.</param>
        public void Execute(CharacterID characterID, int cash)
        {
            Execute(new QueryArgs(characterID, cash));
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(ActiveTradeCashTable.DbColumns);
        }

        #endregion

        #region Overrides of DbQueryNonReader<QueryArgs>

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IActiveTradeCashTable item)
        {
            item.CopyValues(p);
        }

        #endregion

        /// <summary>
        /// Arguments for the <see cref="PeerTradingReplaceCashQuery"/> class.
        /// </summary>
        struct QueryArgs : IActiveTradeCashTable
        {
            readonly CharacterID _characterID;
            readonly int _cash;

            /// <summary>
            /// Gets the character ID.
            /// </summary>
            public CharacterID CharacterID
            {
                get { return _characterID; }
            }

            /// <summary>
            /// Creates a deep copy of this table. All the values will be the same
            /// but they will be contained in a different object instance.
            /// </summary>
            /// <returns>
            /// A deep copy of this table.
            /// </returns>
            IActiveTradeCashTable IActiveTradeCashTable.DeepCopy()
            {
                return new QueryArgs(CharacterID, Cash);
            }

            /// <summary>
            /// Gets the cash.
            /// </summary>
            public int Cash
            {
                get { return _cash; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="characterID">The character ID.</param>
            /// <param name="cash">The cash.</param>
            public QueryArgs(CharacterID characterID, int cash)
            {
                _characterID = characterID;
                _cash = cash;
            }
        }
    }
}