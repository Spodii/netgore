using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectUserPasswordQuery : DbQueryReader<string>
    {
        const string _queryString = "SELECT `password` FROM `users` WHERE `name`=@name";

        public SelectUserPasswordQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public string Execute(string userName)
        {
            using (var r = ExecuteReader(userName))
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
