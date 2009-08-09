using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class DeleteMapSpawnQuery : DbQueryNonReader<MapSpawnValuesID>
    {
        static readonly string _queryString = string.Format("DELETE FROM `{0}` WHERE `id`=@id LIMIT 1", MapSpawnTable.TableName);

        public DeleteMapSpawnQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, MapSpawnValuesID item)
        {
            p["@id"] = (int)item;
        }
    }
}