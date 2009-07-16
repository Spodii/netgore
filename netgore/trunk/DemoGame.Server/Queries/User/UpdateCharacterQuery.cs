using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class UpdateCharacterQuery : UserQueryBase
    {
        static readonly string _queryString;

        static UpdateCharacterQuery()
        {
            var dbFields = CharacterQueryHelper.AllDBFields;
            var dbFieldsExceptID = dbFields.Where(x => x != "id");

            Debug.Assert(dbFieldsExceptID.Count() == dbFields.Count() - 1);

            var setString = FormatParametersIntoString(dbFieldsExceptID);
            _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", DBTables.Character, setString);
        }

        public UpdateCharacterQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }
    }
}