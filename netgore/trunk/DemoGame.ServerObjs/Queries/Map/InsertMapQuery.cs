using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class InsertMapQuery : DbQueryNonReader<MapBase>
    {
        static readonly string _queryString = string.Format("INSERT INTO `{0}` {1}", DBTables.Map,
                                                            FormatParametersIntoValuesString(MapQueryHelper.AllDBFields));

        public InsertMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(MapQueryHelper.AllDBFields);
        }

        protected override void SetParameters(DbParameterValues p, MapBase map)
        {
            MapQueryHelper.SetParameters(p, map);
        }
    }
}