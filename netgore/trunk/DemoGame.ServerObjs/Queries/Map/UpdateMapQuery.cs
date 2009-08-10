using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class UpdateMapQuery : DbQueryNonReader<IMapTable>
    {
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", MapTable.TableName,
                                                            FormatParametersIntoString(MapTable.DbNonKeyColumns));

        public UpdateMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(MapQueryHelper.AllDBFields);
        }

        protected override void SetParameters(DbParameterValues p, IMapTable map)
        {
            ((MapTable)map).CopyValues(p);
        }
    }
}