using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectMapSpawnsOnMapQuery : DbQueryReader<MapID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `map_id`=@mapID", MapSpawnTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMapSpawnsOnMapQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectMapSpawnsOnMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<IMapSpawnTable> Execute(MapID id)
        {
            var ret = new List<IMapSpawnTable>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var values = new MapSpawnTable();
                    values.ReadValues(r);
                    ret.Add(values);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("mapID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="mapID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, MapID mapID)
        {
            p["mapID"] = (int)mapID;
        }
    }
}