using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectMapSpawnsOnMapQuery : DbQueryReader<MapIndex>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `map_id`=@mapID", DBTables.MapSpawn);

        public SelectMapSpawnsOnMapQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectMapSpawnQueryValues> Execute(MapIndex id)
        {
            List<SelectMapSpawnQueryValues> ret = new List<SelectMapSpawnQueryValues>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var v = MapSpawnQueryHelper.ReadMapSpawnValues(r);
                    ret.Add(v);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@mapID");
        }

        protected override void SetParameters(DbParameterValues p, MapIndex mapID)
        {
            p["@mapID"] = (int)mapID;
        }
    }
}
