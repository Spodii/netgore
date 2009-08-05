using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class InsertMapSpawnQuery : DbQueryNonReader<MapSpawnValues>
    {
        static readonly string _queryString = string.Format("INSERT INTO `{0}` {1}", DBTables.MapSpawn,
                                                            FormatParametersIntoValuesString(MapSpawnQueryHelper.AllDBFields));

        public InsertMapSpawnQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(MapSpawnQueryHelper.AllDBFields);
        }

        protected override void SetParameters(DbParameterValues p, MapSpawnValues item)
        {
            MapSpawnQueryHelper.SetParameters(p, item);
        }
    }
}