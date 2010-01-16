using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Extensions for the IStat interface.
    /// </summary>
    public static class IStatExtensions
    {
        /// <summary>
        /// Converts an IEnumerable of IStats to an IEnumerable of KeyValuePairs.
        /// </summary>
        /// <param name="stats">The IEnumerable of IStats.</param>
        /// <returns>An IEnumerable if KeyValuePairs created from the <paramref name="stats"/>.</returns>
        /// <typeparam name="T">The type of IStat.</typeparam>
        public static IEnumerable<KeyValuePair<StatType, int>> xxxToKeyValuePairs<T>(this IEnumerable<T> stats) where T : IStat
        {
            // Find the number of elements
            var count = stats.Count();

            // No elements? Return an empty IEnumerable
            if (count == 0)
                return Enumerable.Empty<KeyValuePair<StatType, int>>();

            // Allocate the return array
            var ret = new KeyValuePair<StatType, int>[count];

            // Enumerate through each element in the IEnumerable, adding each one to the next index in the return array
            var i = 0;
            foreach (var stat in stats)
            {
                ret[i] = new KeyValuePair<StatType, int>(stat.StatType, stat.Value);
                i++;
            }

            return ret;
        }
    }
}