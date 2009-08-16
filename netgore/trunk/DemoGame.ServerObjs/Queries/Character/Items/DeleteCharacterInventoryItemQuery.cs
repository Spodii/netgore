using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class DeleteCharacterInventoryItemQuery : DbQueryNonReader<ICharacterInventoryTable>
    {
        static readonly string _queryString =
            string.Format("DELETE FROM `{0}` WHERE `character_id`=@character_id AND `slot`=@slot",
                          CharacterInventoryTable.TableName);

        public DeleteCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(CharacterInventoryTable.DbKeyColumns, "character_id", "slot");
        }

        public void Execute(CharacterID characterID, InventorySlot slot)
        {
            Execute(new CharacterInventoryTable(characterID, default(ItemID), slot));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterInventoryTable.DbKeyColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterInventoryTable item)
        {
            item.TryCopyValues(p);

            Debug.Assert(p["@slot"].Equals(item.Slot));
            Debug.Assert(p["@character_id"].Equals(item.CharacterID));
        }
    }
}