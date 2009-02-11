using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;

namespace DemoGame.Server
{
    public class UpdateUserQuery : UserQueryBase
    {
        public UpdateUserQuery(MySqlConnection conn) : base(conn)
        {
            // Initialize the query
            string query = string.Format("UPDATE {0} SET {1} WHERE `guid`=@guid", UsersTableName, QueryFieldsStr);
            Initialize(query);
        }
    }
}