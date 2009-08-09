using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class InsertCharacterEquippedItemQuery : DbQueryNonReader<CharacterEquippedTable>
    {
        static readonly string _queryString =
            string.Format("INSERT INTO `{0}` SET {1}",
                          CharacterEquippedTable.TableName, FormatParametersIntoString(CharacterEquippedTable.DbColumns));

        public InsertCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterEquippedTable.DbColumns.Select(x => "@" + x));
        }

        protected override void SetParameters(DbParameterValues p, CharacterEquippedTable item)
        {
            item.CopyValues(p);
        }
    }
}