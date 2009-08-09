using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class InsertMapQuery : DbQueryNonReader<IMapTable>
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

        protected override void SetParameters(DbParameterValues p, IMapTable map)
        {
            ((MapTable)map).CopyValues(p);
        }
    }
}