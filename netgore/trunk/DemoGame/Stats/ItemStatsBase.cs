using System.Collections.Generic;

// FUTURE: Could optimize the OnStatChange by only hooking to the Stat.OnChange when OnStatChange has listeners

namespace DemoGame
{
    public abstract class ItemStatsBase : StatCollectionBase
    {
        /// <summary>
        /// Notifies the listener that any of the stats have raised their OnChange event
        /// </summary>
        public event StatChangeHandler OnStatChange;

        protected ItemStatsBase(IEnumerable<IStat> copyStatValuesFrom, StatCollectionType statCollectionType)
            : this(statCollectionType)
        {
            // NOTE: Can I get rid of this constructor?
            CopyStatValuesFrom(copyStatValuesFrom, true);
        }

        protected ItemStatsBase(IEnumerable<StatTypeValue> src, StatCollectionType statCollectionType) : this(statCollectionType)
        {
            foreach (var statInfo in src)
            {
                IStat stat = StatFactory.CreateStat(statInfo.StatType, statCollectionType, statInfo.Value);
                Add(stat);
            }
        }

        protected ItemStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }

        public override IStat GetStat(StatType statType)
        {
            return InternalGetStat(statType, true);
        }

        /// <summary>
        /// Checks if the stats in this ItemStatsBase contain the same values as another ItemStatsBase
        /// </summary>
        /// <param name="other">ItemStatsBase to compare against</param>
        /// <returns>True if each stat in this ItemStatsBase has the same value as the stats in
        /// <paramref name="other"/>, else false</returns>
        public bool HasEqualValues(ItemStatsBase other)
        {
            // Iterate through each stat
            foreach (IStat stat in this)
            {
                // Check if the values are equal, and return false on the first non-equal value found
                if (other[stat.StatType] != stat.Value)
                    return false;
            }

            // Everything matched, so return true
            return true;
        }

        /// <summary>
        /// Wrapper for creating a new Stat
        /// </summary>
        /// <typeparam name="T">Internal stat value type</typeparam>
        /// <param name="statType">Type of the Stat</param>
        /// <returns>The new Stat</returns>
        BaseStat<T> NewStat<T>(StatType statType) where T : IStatValueType, new()
        {
            var stat = new BaseStat<T>(statType);
            stat.OnChange += StatChanged;
            return stat;
        }

        /// <summary>
        /// Handler for listening to all of the stats and forwarding to the OnStatChange
        /// </summary>
        /// <param name="stat">Stat that changed</param>
        void StatChanged(IStat stat)
        {
            if (OnStatChange != null)
                OnStatChange(stat);
        }
    }
}