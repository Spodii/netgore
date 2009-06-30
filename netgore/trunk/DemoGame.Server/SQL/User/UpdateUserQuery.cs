using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class UpdateUserQuery : UserQueryBase
    {
        public UpdateUserQuery(DbConnectionPool connectionPool)
            : base(connectionPool, string.Format("UPDATE {0} SET {1} WHERE `guid`=@guid", UsersTableName, QueryFieldsStr))
        {
        }
    }
}