using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertWorldStatsQuestAcceptQuery : DbQueryNonReader<IWorldStatsQuestAcceptTable>
    {
        static readonly string _queryStr = FormatQueryString("REPLACE INTO `{0}` {1}", WorldStatsQuestAcceptTable.TableName,
                                                         FormatParametersIntoValuesString(WorldStatsQuestAcceptTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertWorldStatsQuestAcceptQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertWorldStatsQuestAcceptQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(WorldStatsQuestAcceptTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IWorldStatsQuestAcceptTable item)
        {
            item.CopyValues(p);
        }
    }
}