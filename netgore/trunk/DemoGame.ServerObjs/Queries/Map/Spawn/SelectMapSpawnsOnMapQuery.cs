using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectMapSpawnsOnMapQuery : DbQueryReader<MapIndex>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `map_id`=@mapID", DBTables.MapSpawn);

        public SelectMapSpawnsOnMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectMapSpawnQueryValues> Execute(MapIndex id)
        {
            var ret = new List<SelectMapSpawnQueryValues>();

            using (IDataReader r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    SelectMapSpawnQueryValues v = MapSpawnQueryHelper.ReadMapSpawnValues(r);
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