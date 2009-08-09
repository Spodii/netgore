using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    public class InsertMapQuery : DbQueryNonReader<MapBase>
    {
        static readonly string _queryString = string.Format("INSERT INTO `{0}` {1}", MapTable.TableName,
                                                            FormatParametersIntoValuesString(MapTable.DbColumns));

        public InsertMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(MapTable.DbColumns);
        }

        protected override void SetParameters(DbParameterValues p, MapBase map)
        {
            MapQueryHelper.SetParameters(p, map);
        }
    }
}