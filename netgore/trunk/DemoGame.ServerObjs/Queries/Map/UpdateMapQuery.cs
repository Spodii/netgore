using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class UpdateMapQuery : DbQueryNonReader<MapBase>
    {
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", DBTables.Map,
                                                            FormatParametersIntoString(MapQueryHelper.AllDBFieldsExceptID));

        public UpdateMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
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