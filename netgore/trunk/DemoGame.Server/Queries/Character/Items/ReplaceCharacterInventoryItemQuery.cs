using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceCharacterInventoryItemQuery : DbQueryNonReader<ICharacterInventoryTable>
    {
        static readonly string _queryString = string.Format("REPLACE INTO `{0}` SET {1}", CharacterInventoryTable.TableName,
                                                            FormatParametersIntoString(CharacterInventoryTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceCharacterInventoryItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ReplaceCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
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
            return CreateParameters(CharacterInventoryTable.DbColumns.Select(x => "@" + x));
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