using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceMapQuery : DbQueryNonReader<IMapTable>
    {
        static readonly string _queryStr = FormatQueryString("REPLACE INTO `{0}` {1}", MapTable.TableName,
                                                            FormatParametersIntoValuesString(MapTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceMapQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ReplaceMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(MapTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="map">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IMapTable map)
        {
            (map).CopyValues(p);
        }
    }
}