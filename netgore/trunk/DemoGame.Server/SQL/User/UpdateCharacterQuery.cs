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
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", DBTables.Character, QueryFieldsStr);

        public UpdateCharacterQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }
    }
}