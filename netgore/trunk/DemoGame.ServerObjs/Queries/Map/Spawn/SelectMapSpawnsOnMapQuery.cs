using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectMapSpawnsOnMapQuery : DbQueryReader<MapIndex>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `map_id`=@mapID", MapSpawnTable.TableName);

        public SelectMapSpawnsOnMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectMapSpawnQueryValues> Execute(MapIndex id)
        {
            var ret = new List<SelectMapSpawnQueryValues>();

            using (IDataReader r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    SelectMapSpawnQueryValues v = MapSpawnQueryHelper.ReadMapSpawnValues(r);
                    ret.Add(v);
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
            return CreateParameters("@mapID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="mapID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, MapIndex mapID)
        {
            p["@mapID"] = (int)mapID;
        }
    }
}