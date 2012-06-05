using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteCharacterEquippedItemQuery : DbQueryNonReader<DeleteCharacterEquippedItemQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCharacterEquippedItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public DeleteCharacterEquippedItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(CharacterEquippedTable.DbKeyColumns, "character_id", "slot");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // DELETE FROM `{0}` WHERE `character_id`=@character_id AND `slot`=@slot

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Delete(CharacterEquippedTable.TableName).Where(
                    f.And(f.Equals(s.EscapeColumn("character_id"), s.Parameterize("character_id")),
                        f.Equals(s.EscapeColumn("slot"), s.Parameterize("slot"))));
            return q.ToString();
        }

        public void Execute(CharacterID characterID, EquipmentSlot slot)
        {
            Execute(new QueryArgs(characterID, slot));
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
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["slot"] = (int)item.Slot;
            p["character_id"] = (int)item.CharacterID;

            Debug.Assert(Convert.ToInt32(p["slot"]) == (int)item.Slot);
            Debug.Assert(Convert.ToInt32(p["character_id"]) == (int)item.CharacterID);
        }

        /// <summary>
        /// Arguments for the <see cref="DeleteCharacterEquippedItemQuery"/> query.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            readonly CharacterID _characterID;
            readonly EquipmentSlot _equipmentSlot;

            /// <summary>
            /// Gets the <see cref="CharacterID"/>.
            /// </summary>
            public CharacterID CharacterID
            {
                get { return _characterID; }
            }

            /// <summary>
            /// Gets the <see cref="EquipmentSlot"/>.
            /// </summary>
            public EquipmentSlot Slot
            {
                get { return _equipmentSlot; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="characterID">The <see cref="CharacterID"/>.</param>
            /// <param name="equipmentSlot">The equipment slot.</param>
            public QueryArgs(CharacterID characterID, EquipmentSlot equipmentSlot)
            {
                _characterID = characterID;
                _equipmentSlot = equipmentSlot;
            }
        }
    }
}