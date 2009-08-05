using System.Linq;

namespace DemoGame.Server
{
    public struct MapSpawnValuesID
    {
        readonly int _value;

        public MapSpawnValuesID(int id)
        {
            _value = id;
        }

        public static implicit operator int(MapSpawnValuesID v)
        {
            return v._value;
        }

        public static implicit operator MapSpawnValuesID(int v)
        {
            return new MapSpawnValuesID(v);
        }
    }
}