using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertCharacterInventoryItemQuery : DbQueryNonReader<ICharacterInventoryTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCharacterInventoryItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertCharacterInventoryItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO `{0}` {1}
            //      ON DUPLICATE KEY UPDATE <{1} - keys>

            var q =
                qb.Insert(CharacterInventoryTable.TableName).AddAutoParam(CharacterInventoryTable.DbColumns).ODKU().AddFromInsert(
                    CharacterInventoryTable.DbKeyColumns);
            return q.ToString();
        }

        public void Execute(CharacterID characterID, ItemID itemID, InventorySlot slot)
        {
            Execute(new CharacterInventoryTable(characterID, itemID, slot));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterInventoryTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterInventoryTable item)
        {
            item.CopyValues(p);
        }
    }
}