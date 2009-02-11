using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;

namespace DemoGame.Server
{
    public class InsertUserQuery : UserQueryBase
    {
        public InsertUserQuery(MySqlConnection conn) : base(conn)
        {
            // Initialize the query
            string query = string.Format("INSERT INTO {0} SET `password`=@password,`guid`=@guid,{1}", UsersTableName,
                                         QueryFieldsStr);
            Initialize(query);
        }
    }
}