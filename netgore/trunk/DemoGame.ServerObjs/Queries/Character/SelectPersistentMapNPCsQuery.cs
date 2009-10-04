using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectPersistentMapNPCsQuery : DbQueryReader<MapIndex>
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}` WHERE `map_id`=@mapID",
                                                         NpcCharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectPersistentMapNPCsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectPersistentMapNPCsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(NpcCharacterTable.DbColumns, "id", "map_id");
        }

        public IEnumerable<CharacterID> Execute(MapIndex map)
        {
            List<CharacterID> ret = new List<CharacterID>();

            using (var r = ExecuteReader(map))
            {
                while (r.Read())
                {
                    var id = r.GetCharacterID(0);
                    ret.Add(id);
                }
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
            return CreateParameters("@mapID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, MapIndex item)
        {
            p["@mapID"] = (int)item;
        }
    }
}