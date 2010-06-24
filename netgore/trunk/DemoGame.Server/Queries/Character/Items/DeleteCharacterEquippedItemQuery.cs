using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteCharacterEquippedItemQuery : DbQueryNonReader<ICharacterEquippedTable>
    {
        static readonly string _queryStr =
            FormatQueryString("DELETE FROM `{0}` WHERE `character_id`=@character_id AND `slot`=@slot",
                          CharacterEquippedTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCharacterEquippedItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public DeleteCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(CharacterEquippedTable.DbKeyColumns, "character_id", "slot");
        }

        public void Execute(CharacterID characterID, EquipmentSlot slot)
        {
            Execute(new CharacterEquippedTable(characterID, default(ItemID), slot));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterEquippedTable.DbKeyColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterEquippedTable item)
        {
            item.TryCopyValues(p);

            Debug.Assert(Convert.ToInt32(p["slot"]) == (int)item.Slot);
            Debug.Assert(Convert.ToInt32(p["character_id"]) == (int)item.CharacterID);
        }
    }
}