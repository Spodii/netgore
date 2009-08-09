using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class DeleteCharacterEquippedItemQuery : DbQueryNonReader<ItemID>
    {
        static readonly string _queryString = string.Format("DELETE FROM `{0}` WHERE `item_id`=@itemID LIMIT 1",
                                                           CharacterEquippedTable.TableName);

        public DeleteCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID");
        }

        protected override void SetParameters(DbParameterValues p, ItemID itemID)
        {
            p["@itemID"] = (int)itemID;
        }
    }
}