using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents the unique ID of a MapSpawnValues.
    /// </summary>
    public struct MapSpawnValuesID
    {
        readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnValuesID"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        public MapSpawnValuesID(int id)
        {
            _value = id;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DemoGame.Server.MapSpawnValuesID"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(MapSpawnValuesID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="DemoGame.Server.MapSpawnValuesID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapSpawnValuesID(int v)
        {
            return new MapSpawnValuesID(v);
        }
    }
}