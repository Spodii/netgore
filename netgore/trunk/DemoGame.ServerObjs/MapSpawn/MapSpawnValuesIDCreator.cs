using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.Queries;
using NetGore.Db;

namespace DemoGame.Server
{
    [DBControllerQuery]
    public class MapSpawnValuesIDCreator : IDCreatorBase
    {
        public MapSpawnValuesIDCreator(DbConnectionPool connectionPool) : base(connectionPool,
            DBTables.MapSpawn, "id", 1, 0)
        {
        }
    }
}
