using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;

namespace DemoGame
{
    /// <summary>
    /// An IStatCollection implementation that can contain as few or as many StatTypes as needed.
    /// </summary>
    public class StatCollectionBase : IStatCollection
    {
        readonly StatCollectionType _statCollectionType;
        readonly Dictionary<StatType, IStat> _stats = new Dictionary<StatType, IStat>(EnumComparer<StatType>.Instance);

        /// <summary>
        /// StatCollectionBase constructor.
        /// </summary>
        /// <param name="statCollectionType">The type of the collectoin.</param>
        protected StatCollectionBase(StatCollectionType statCollectionType)
        {
            _statCollectionType = statCollectionType;
        }

        /// <summary>
        /// Adds an IStat to the collection.
        /// </summary>
        /// <param name="stat">IStat to add to the collection.</param>
        protected void Add(IStat stat)
        {
            _stats.Add(stat.StatType, stat);
        }

        /// <summary>
        /// Adds IStats to the collection.
        /// </summary>
        /// <param name="stats">IStats to add to the collection.</param>
        protected void Add(IEnumerable<IStat> stats)
        {
            foreach (IStat stat in stats)
            {
                Add(stat);
            }
        }

        /// <summary>
        /// Adds IStats to the collection.
        /// </summary>
        /// <param name="stats">IStats to add to the collection.</param>
        protected void Add(params IStat[] stats)
        {
            foreach (IStat stat in stats)
            {
                Add(stat);
            }
        }

        /// <summary>
        /// Takes the stat values from the given source and copies them into this collection.
        /// </summary>
        /// <param name="sourceStats">The source to copy all the stat values from.</param>
        /// <param name="errorOnFailure">If true, an ArgumentException will be thrown if the <paramref name="sourceStats"/>
        /// contains one or more stats that are not in this collection. If false, any key in the
        /// <paramref name="sourceStats"/> that is not in this collection will just be skipped.</param>
        public void CopyStatValuesFrom(IEnumerable<KeyValuePair<StatType, int>> sourceStats, bool errorOnFailure)
        {
            // Iterate through each stat in the source
            foreach (var sourceStat in sourceStats)
            {
                // Check that this collection handles the given stat
                IStat stat;
                if (!TryGetStat(sourceStat.Key, out stat))
                {
                    if (errorOnFailure)
                    {
                        const string errmsg =
                            "Tried to copy over the value of StatType `{0}`, but the destination " +
                            "StatCollection did not contain the stat.";
                        throw new ArgumentException(string.Format(errmsg, stat.StatType));
                    }
                    continue;
                }

                // This collection contains the stat, too, so copy the value over
                stat.Value = sourceStat.Value;
            }
        }

        /// <summary>
        /// Copies all of the IStat.Values from each stat in the source enumerable where
        /// the IStat.CanWrite is true. Any stat that is in the source enumerable that is not
        /// in the destination, or any stat where CanWrite is false will not have their value copied over.
        /// </summary>
        /// <param name="sourceStats">Source collection of IStats</param>
        /// <param name="errorOnFailure">If true, an ArgumentException will be thrown
        /// if one or more of the StatTypes in the sourceStats do not exist in this StatCollection. If
        /// false, this will not be treated like an error and the value will just not be copied over.</param>
        public void CopyStatValuesFrom(IEnumerable<IStat> sourceStats, bool errorOnFailure)
        {
            var keyValuePairs = sourceStats.Select(x => new KeyValuePair<StatType, int>(x.StatType, x.Value));
            CopyStatValuesFrom(keyValuePairs, errorOnFailure);
        }

        /// <summary>
        /// Gets an IStat from this StatCollectionBase, or creates the IStat for the <paramref name="statType"/>
        /// if the IStat did not already exist in the collection.
        /// </summary>
        /// <param name="statType">Type of stat to get.</param>
        /// <returns>The IStat in this StatCollectionBase for Stat type <param name="statType"</returns>
        protected IStat GetStatOrCreate(StatType statType)
        {
            IStat stat;
            if (!_stats.TryGetValue(statType, out stat))
            {
                stat = StatFactory.CreateStat(statType, StatCollectionType);
                Add(stat);
            }

            return stat;
        }

        /// <summary>
        /// When overridden in the derived class, handles when an IStat is added to this StatCollectionBase. This will
        /// be invoked once and only once for every IStat added to this StatCollectionBase.
        /// </summary>
        /// <param name="stat">The IStat that was added to this StatCollectionBase.</param>
        protected virtual void HandleStatAdded(IStat stat)
        {
        }

        public IEnumerable<KeyValuePair<StatType, int>> ToKeyValuePairs()
        {
            return this.Select(x => new KeyValuePair<StatType, int>(x.StatType, x.Value)).ToArray();
        }

        #region IStatCollection Members

        public IEnumerator<IStat> GetEnumerator()
        {
            return _stats.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[StatType statType]
        {
            get
            {
                IStat stat;
                if (!_stats.TryGetValue(statType, out stat))
                    return 0;

                return stat.Value;
            }
            set
            {
                IStat stat = GetStat(statType);
                stat.Value = value;
            }
        }

        public bool Contains(StatType statType)
        {
            return _stats.ContainsKey(statType);
        }

        public virtual IStat GetStat(StatType statType)
        {
            return _stats[statType];
        }

        public bool TryGetStat(StatType statType, out IStat stat)
        {
            return _stats.TryGetValue(statType, out stat);
        }

        public bool TryGetStatValue(StatType statType, out int value)
        {
            IStat stat;
            if (!TryGetStat(statType, out stat))
            {
                value = 0;
                return false;
            }

            value = stat.Value;
            return true;
        }

        public StatCollectionType StatCollectionType
        {
            get { return _statCollectionType; }
        }

        #endregion
    }
}