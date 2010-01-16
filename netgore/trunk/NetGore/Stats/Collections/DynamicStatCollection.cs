using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace NetGore.Stats
{
    /// <summary>
    /// An IStatCollection implementation that can contain as few or as many StatTypes as needed. New StatTypes
    /// can be added to the collection whenever needed.
    /// </summary>
    public class DynamicStatCollection<TStatType> : IStatCollection<TStatType>
         where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        readonly StatCollectionType _statCollectionType;
        readonly Dictionary<TStatType, IStat<TStatType>> _stats = 
            new Dictionary<TStatType, IStat<TStatType>>(EnumComparer<TStatType>.Instance);

        /// <summary>
        /// StatCollectionBase constructor.
        /// </summary>
        /// <param name="statCollectionType">The type of the collection.</param>
        protected DynamicStatCollection(StatCollectionType statCollectionType)
        {
            _statCollectionType = statCollectionType;
        }

        /// <summary>
        /// Adds an IStat to the collection.
        /// </summary>
        /// <param name="stat">IStat to add to the collection.</param>
        protected void Add(IStat<TStatType> stat)
        {
            _stats.Add(stat.StatType, stat);
        }

        /// <summary>
        /// Adds IStats to the collection.
        /// </summary>
        /// <param name="stats">IStats to add to the collection.</param>
        protected void Add(IEnumerable<IStat<TStatType>> stats)
        {
            foreach (var stat in stats)
            {
                Add(stat);
            }
        }

        /// <summary>
        /// Adds IStats to the collection.
        /// </summary>
        /// <param name="stats">IStats to add to the collection.</param>
        protected void Add(params IStat<TStatType>[] stats)
        {
            foreach (var stat in stats)
            {
                Add(stat);
            }
        }

        /// <summary>
        /// Gets an IStat from this StatCollectionBase, or creates the IStat for the <paramref name="statType"/>
        /// if the IStat did not already exist in the collection.
        /// </summary>
        /// <param name="statType">Type of stat to get.</param>
        /// <returns>The IStat in this StatCollectionBase for Stat type <param name="statType"</returns>
        protected IStat<TStatType> GetStatOrCreate(TStatType statType)
        {
            IStat<TStatType> stat;
            if (!_stats.TryGetValue(statType, out stat))
            {
                stat = StatFactory<TStatType>.CreateStat(statType, StatCollectionType);
                Add(stat);
            }

            return stat;
        }

        /// <summary>
        /// When overridden in the derived class, handles when an IStat is added to this StatCollectionBase. This will
        /// be invoked once and only once for every IStat added to this StatCollectionBase.
        /// </summary>
        /// <param name="stat">The IStat that was added to this StatCollectionBase.</param>
        protected virtual void HandleStatAdded(IStat<TStatType> stat)
        {
        }

        #region IStatCollection Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IStat<TStatType>> GetEnumerator()
        {
            return _stats.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the value of the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get or set the value for.</param>
        /// <returns>The value of the stat with the given <paramref name="statType"/>.</returns>
        public int this[TStatType statType]
        {
            get
            {
                IStat<TStatType> stat;
                if (!_stats.TryGetValue(statType, out stat))
                    return 0;

                return stat.Value;
            }
            set
            {
                IStat<TStatType> stat = GetStat(statType);
                stat.Value = value;
            }
        }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to check if exists in the collection.</param>
        /// <returns>True if this collection contains the stat with the given <paramref name="statType"/>;
        /// otherwise false.</returns>
        public bool Contains(TStatType statType)
        {
            return _stats.ContainsKey(statType);
        }

        /// <summary>
        /// Gets the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <returns>The IStat for the stat of the given <paramref name="statType"/>.</returns>
        public virtual IStat<TStatType> GetStat(TStatType statType)
        {
            return _stats[statType];
        }

        /// <summary>
        /// Tries to get the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="stat">The IStat for the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be null.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStat(TStatType statType, out IStat<TStatType> stat)
        {
            return _stats.TryGetValue(statType, out stat);
        }

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStatValue(TStatType statType, out int value)
        {
            IStat<TStatType> stat;
            if (!TryGetStat(statType, out stat))
            {
                value = 0;
                return false;
            }

            value = stat.Value;
            return true;
        }

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given StatType
        /// into this IStatCollection.
        /// </summary>
        /// <param name="values">IEnumerable of StatTypes and stat values to copy into this IStatCollection.</param>
        /// <param name="checkContains">If true, each StatType in <paramref name="values"/> will first be checked
        /// if it is in this IStatCollection before trying to copy over the value. Any StatType in
        /// <paramref name="values"/> but not in this IStatCollection will be skipped. If false, no checking will
        /// be done. Any StatType in <paramref name="values"/> but not in this IStatCollection will behave
        /// the same as if the value of a StatType not in this IStatCollection was attempted to be assigned
        /// in any other way.</param>
        public void CopyValuesFrom(IEnumerable<KeyValuePair<TStatType, int>> values, bool checkContains)
        {
            foreach (var value in values)
            {
                if (checkContains && !Contains(value.Key))
                    continue;

                this[value.Key] = value.Value;
            }
        }

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given StatType
        /// into this IStatCollection.
        /// </summary>
        /// <param name="values">IEnumerable of StatTypes and stat values to copy into this IStatCollection.</param>
        /// <param name="checkContains">If true, each StatType in <paramref name="values"/> will first be checked
        /// if it is in this IStatCollection before trying to copy over the value. Any StatType in
        /// <paramref name="values"/> but not in this IStatCollection will be skipped. If false, no checking will
        /// be done. Any StatType in <paramref name="values"/> but not in this IStatCollection will behave
        /// the same as if the value of a StatType not in this IStatCollection was attempted to be assigned
        /// in any other way.</param>
        public void CopyValuesFrom(IEnumerable<IStat<TStatType>> values, bool checkContains)
        {
            CopyValuesFrom(values.ToKeyValuePairs(), checkContains);
        }

        /// <summary>
        /// Gets the StatCollectionType that this collection is for.
        /// </summary>
        public StatCollectionType StatCollectionType
        {
            get { return _statCollectionType; }
        }

        #endregion
    }
}