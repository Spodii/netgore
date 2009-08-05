using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class UpdateMapSpawnQuery : DbQueryNonReader<MapSpawnValues>
    {
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", DBTables.MapSpawn,
                                                            FormatParametersIntoString(MapSpawnQueryHelper.AllDBFieldsExceptID));

        public UpdateMapSpawnQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
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