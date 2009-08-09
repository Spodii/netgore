using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAllianceHostileQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `alliance_id`=@id",
                                                            DBTables.AllianceHostile);

        public SelectAllianceHostileQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<AllianceHostileTable> Execute(AllianceID id)
        {
            var ret = new List<AllianceHostileTable>();

            using (IDataReader r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var item = new AllianceHostileTable(r);
                    ret.Add(item);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = (int)id;
        }
    }
}