using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class PeerTradingInsertItemQuery : DbQueryNonReader<PeerTradingInsertItemQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradingInsertItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public PeerTradingInsertItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ActiveTradeItemTable.DbColumns, "item_id", "character_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT IGNORE INTO {0} {1}

            var q = qb.Insert(ActiveTradeItemTable.TableName).IgnoreExists().AddAutoParam(ActiveTradeItemTable.DbColumns);
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified item.
        /// </summary>
        /// <param name="characterID">The character ID.</param>
        /// <param name="itemID">The item ID.</param>
        public void Execute(ItemID itemID, CharacterID characterID)
        {
            Execute(new QueryArgs(itemID, characterID));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(ActiveTradeItemTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["item_id"] = (int)item.ItemID;
            p["character_id"] = (int)item.CharacterID;
        }

        /// <summary>
        /// The arguments for the <see cref="PeerTradingInsertItemQuery"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            /// <summary>
            /// The <see cref="CharacterID"/> for the row to insert.
            /// </summary>
            public CharacterID CharacterID;

            /// <summary>
            /// The <see cref="ItemID"/> of the row to insert.
            /// </summary>
            public ItemID ItemID;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="itemID">The item ID.</param>
            /// <param name="characterID">The character ID.</param>
            public QueryArgs(ItemID itemID, CharacterID characterID)
            {
                CharacterID = characterID;
                ItemID = itemID;
            }
        }
    }
}