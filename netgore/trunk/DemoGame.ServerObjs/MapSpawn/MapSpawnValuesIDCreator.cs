using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;

namespace DemoGame.Server
{
    [DbControllerQuery]
    public class MapSpawnValuesIDCreator : IDCreatorBase
    {
        public MapSpawnValuesIDCreator(DbConnectionPool connectionPool)
            : base(connectionPool, MapSpawnTable.TableName, "id", 1, 0)
        {
            QueryAsserts.ArePrimaryKeys(MapSpawnTable.DbKeyColumns, "id");
        }
    }
}