using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectUserPasswordQuery : DbQueryReader<string>
    {
        const string _queryString = "SELECT `password` FROM `users` WHERE `name`=@name";

        public SelectUserPasswordQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public string Execute(string userName)
        {
            using (IDataReader r = ExecuteReader(userName))
            {
                if (!r.Read())
                    return null;

                return r[0].ToString();
            }
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@name");
        }

        protected override void SetParameters(DbParameterValues p, string userName)
        {
            p["@name"] = userName;
        }
    }
}