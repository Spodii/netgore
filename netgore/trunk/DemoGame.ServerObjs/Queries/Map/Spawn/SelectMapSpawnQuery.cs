using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectMapSpawnQuery : DbQueryReader<MapSpawnValuesID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", MapSpawnTable.TableName);

        public SelectMapSpawnQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(MapSpawnTable.DbKeyColumns, "id");
        }

        public IMapSpawnTable Execute(MapSpawnValuesID id)
        {
            MapSpawnTable ret;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new ArgumentException("id");

                ret = new MapSpawnTable();
                ret.ReadValues(r);
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
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, MapSpawnValuesID item)
        {
            p["@id"] = (int)item;
        }
    }
}