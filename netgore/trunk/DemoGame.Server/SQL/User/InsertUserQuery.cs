using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertUserQuery : UserQueryBase
    {
        // TODO: This won't even work because the password is never set... but its not like new users even work yet

        public InsertUserQuery(DbConnectionPool connectionPool)
            : base(
                connectionPool,
                string.Format("INSERT INTO {0} SET `password`=@password,`guid`=@guid,{1}", UsersTableName, QueryFieldsStr))
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            // TODO: Initialize password parameter
            return base.InitializeParameters();
        }

        protected override void SetParameters(DbParameterValues p, User user)
        {
            // TODO: Set the password parameter
            base.SetParameters(p, user);
        }
    }
}