using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.World;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectMapQuery : DbQueryReader<MapID>
    {
        static readonly string _queryStr = FormatQueryString("SELECT * FROM `{0}` WHERE `id`=@id", MapTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMapQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectMapQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public IMapTable Execute(MapID id)
        {
            MapTable ret;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    return null;

                ret = new MapTable();
                ret.ReadValues(r);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, MapID item)
        {
            p["id"] = (int)item;
        }
    }

    [DbControllerQuery]
    public class SelectMapIDsQuery : DbQueryReader
    {
        static readonly string _queryStr = FormatQueryString("SELECT `id` FROM `{0}`", MapTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMapIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectMapIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public IEnumerable<MapID> Execute()
        {
            var ret = new List<MapID>();
            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var i = r.GetMapID(0);
                    ret.Add(i);
                }
            }

            return ret;
        }
    }
}