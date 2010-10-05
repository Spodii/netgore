using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.World;

namespace DemoGame.Editor
{
    public partial class MapScreenControl
    {
        static readonly List<MapScreenControl> _instances = new List<MapScreenControl>();
        static readonly object _instancesSync = new object();

        /// <summary>
        /// Gets the <see cref="MapScreenControl"/> instances.
        /// </summary>
        public static IEnumerable<MapScreenControl> Instances
        {
            get
            {
                lock (_instancesSync)
                {
                    return _instances.ToImmutable();
                }
            }
        }

        /// <summary>
        /// Finds the <see cref="MapScreenControl"/> for a given map.
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <returns>The <see cref="MapScreenControl"/> for the given <paramref name="map"/>, or null if not found.</returns>
        public static MapScreenControl TryFindInstance(IMap map)
        {
            lock (_instancesSync)
            {
                return _instances.FirstOrDefault(x => x != null && !x.IsDisposed && x.Map == map);
            }
        }
    }
}