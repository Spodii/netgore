using System.Collections.Generic;
using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    public class ItemStatsBase : StatCollection<StatType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatsBase"/> class.
        /// </summary>
        /// <param name="src">The stat types and corresponding values of the stats to add to the collection.</param>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        public ItemStatsBase(IEnumerable<Stat<StatType>> src, StatCollectionType statCollectionType)
            : base(statCollectionType, src)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatsBase"/> class.
        /// </summary>
        /// <param name="statCollectionType">The type of the collection.</param>
        public ItemStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}