using System.Collections.Generic;
using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    public class ItemStatsBase : DynamicStatCollection<StatType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatsBase"/> class.
        /// </summary>
        /// <param name="src">The stat types and corresponding values of the stats to add to the collection.</param>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        public ItemStatsBase(IEnumerable<KeyValuePair<StatType, int>> src, StatCollectionType statCollectionType)
            : this(statCollectionType)
        {
            foreach (var statInfo in src)
            {
                var stat = StatFactory<StatType>.CreateStat(statInfo.Key, statCollectionType, statInfo.Value);
                Add(stat);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStatsBase"/> class.
        /// </summary>
        /// <param name="statCollectionType">The type of the collection.</param>
        public ItemStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }

        /// <summary>
        /// Notifies listeners when any of the stats have raised their <see cref="IStat{StatType}.Changed"/> event.
        /// </summary>
        public event IStatEventHandler<StatType> StatChanged;

        /// <summary>
        /// Gets the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <returns>
        /// The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </returns>
        public override IStat<StatType> GetStat(StatType statType)
        {
            return GetStatOrCreate(statType);
        }

        /// <summary>
        /// Handler for listening to all of the stats and forwarding to the OnStatChange
        /// </summary>
        /// <param name="stat">Stat that changed</param>
        void HandleStatChanged(IStat<StatType> stat)
        {
            if (StatChanged != null)
                StatChanged(stat);
        }

        /// <summary>
        /// Checks if the stats in this ItemStatsBase contain the same values as another ItemStatsBase
        /// </summary>
        /// <param name="other">ItemStatsBase to compare against</param>
        /// <returns>True if each stat in this ItemStatsBase has the same value as the stats in
        /// <paramref name="other"/>, else false</returns>
        public bool HasEqualValues(ItemStatsBase other)
        {
            return this.All(x => x.Value == other[x.StatType]);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an <see cref="IStat{StatType}"/> is added to this
        /// <see cref="DynamicStatCollection{StatType}"/>. This will be invoked once and only once for every
        /// <see cref="IStat{StatType}"/> added to this <see cref="DynamicStatCollection{StatType}"/>.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{StatType}"/> that was added to this
        /// <see cref="DynamicStatCollection{StatType}"/>.</param>
        protected override void OnStatAdded(IStat<StatType> stat)
        {
            // Attach a listener to every stat to listen for changes
            stat.Changed += HandleStatChanged;
        }
    }
}