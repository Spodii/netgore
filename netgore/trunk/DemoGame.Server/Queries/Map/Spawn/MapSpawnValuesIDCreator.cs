using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class MapSpawnValuesIDCreator : IDCreatorBase<MapSpawnValuesID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnValuesIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public MapSpawnValuesIDCreator(DbConnectionPool connectionPool) : base(connectionPool, MapSpawnTable.TableName, "id", 1)
        {
            QueryAsserts.ArePrimaryKeys(MapSpawnTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="MapSpawnValuesID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="MapSpawnValuesID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="MapSpawnValuesID"/>.</returns>
        protected override MapSpawnValuesID FromInt(int value)
        {
            return new MapSpawnValuesID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="MapSpawnValuesID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(MapSpawnValuesID value)
        {
            return (int)value;
        }
    }
}