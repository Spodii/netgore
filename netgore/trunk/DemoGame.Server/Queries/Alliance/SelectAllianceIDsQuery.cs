using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAllianceIDsQuery : DbQueryReader
    {
        static readonly string _queryStr = FormatQueryString("SELECT `id` FROM `{0}`", AllianceTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(AllianceTable.DbKeyColumns, "id");
        }

        public IEnumerable<AllianceID> Execute()
        {
            var ret = new List<AllianceID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var allianceID = r.GetAllianceID("id");
                    ret.Add(allianceID);
                }
            }

            return ret;
        }
    }
}