using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class InsertCharacterInventoryItemQuery : DbQueryNonReader<CharacterInventoryTable>
    {
        static readonly string _queryString = string.Format(
            "INSERT INTO `{0}` SET {1}", CharacterInventoryTable.TableName,
            FormatParametersIntoString(CharacterInventoryTable.DbColumns));

        public InsertCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterInventoryTable.DbColumns.Select(x => "@" + x));
        }

        protected override void SetParameters(DbParameterValues p, CharacterInventoryTable item)
        {
            item.CopyValues(p);
        }
    }
}