using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

// TODO: !! This query won't work! I need to use the CharacterID, ItemID, AND Slot!

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class DeleteCharacterInventoryItemQuery : DbQueryNonReader<ItemID>
    {
        static readonly string _queryString = string.Format("DELETE FROM `{0}` WHERE `item_id`=@itemID LIMIT 1",
                                                            CharacterInventoryTable.TableName);

        public DeleteCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="itemID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemID itemID)
        {
            p["@itemID"] = (int)itemID;
        }
    }
}