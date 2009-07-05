using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class UpdateCharacterQuery : UserQueryBase
    {
        public UpdateCharacterQuery(DbConnectionPool connectionPool)
            : base(connectionPool, string.Format("UPDATE `character` SET {0} WHERE `id`=@id", QueryFieldsStr))
        {
        }
    }
}