using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectUserPasswordQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT `password` FROM `{0}` WHERE `name`=@name", DBTables.Character);

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