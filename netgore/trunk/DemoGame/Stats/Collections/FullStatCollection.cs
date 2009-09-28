using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// A specialized collection that contains every StatType. This is the ideal IStatCollection to use when you want
    /// the collection to always contain every StatType.
    /// </summary>
    public class FullStatCollection : IStatCollection
    {
        readonly StatCollectionType _collectionType;
        readonly IStat[] _stats;

        /// <summary>
        /// FullStatCollection constructor.
        /// </summary>
        /// <param name="collectionType">The type of StatTypes that this collection contains.</param>
        public FullStatCollection(StatCollectionType collectionType)
        {
            _collectionType = collectionType;

            _stats = new IStat[StatTypeHelper.Instance.MaxValue + 1];

            foreach (StatType statType in StatTypeHelper.Values)
            {
                IStat istat = StatFactory.CreateStat(statType, collectionType);
                _stats[statType.GetValue()] = istat;
            }
        }

        /// <summary>
        /// FullStatCollection constructor.
        /// </summary>
        /// <param name="source">The FullStatCollection to copy the values from.</param>
        FullStatCollection(FullStatCollection source)
        {
            _collectionType = source._collectionType;

            _stats = new IStat[source._stats.Length];
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i] = source._stats[i].DeepCopy();
            }
        }

        /// <summary>
        /// Checks if the values of this FullStatCollection are equal to the values of another FullStatCollection.
        /// </summary>
        /// <param name="other">The FullStatCollection to compare against.</param>
        /// <returns>True if the values of this FullStatCollection are equal to the values
        /// of <paramref name="other"/>.</returns>
        public bool AreValuesEqual(FullStatCollection other)
        {
            for (int i = 0; i < _stats.Length; i++)
            {
                StatType statType = (StatType)i;
                if (this[statType] != other[statType])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a deep copy of this FullStatCollection/
        /// </summary>
        /// <returns>A deep copy of this FullStatCollection/</returns>
        public FullStatCollection DeepCopy()
        {
            return new FullStatCollection(this);
        }

        /// <summary>
        /// Sets the value of all stats in this collection.
        /// </summary>
        /// <param name="value">Value to set the stats to.</param>
        public void SetAll(int value)
        {
            foreach (IStat stat in _stats)
            {
                stat.Value = value;
            }
        }

        #region IStatCollection Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IStat> GetEnumerator()
        {
            for (int i = 0; i < _stats.Length; i++)
            {
                yield return _stats[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the value of the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get or set the value for.</param>
        /// <returns>The value of the stat with the given <paramref name="statType"/>.</returns>
        public int this[StatType statType]
        {
            get { return _stats[statType.GetValue()].Value; }
            set { _stats[statType.GetValue()].Value = value; }
        }

        /// <summary>
        /// Gets the StatCollectionType that this collection is for.
        /// </summary>
        public StatCollectionType StatCollectionType
        {
            get { return _collectionType; }
        }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to check if exists in the collection.</param>
        /// <returns>True if this collection contains the stat with the given <paramref name="statType"/>;
        /// otherwise false.</returns>
        public bool Contains(StatType statType)
        {
            return true;
        }

        /// <summary>
        /// Gets the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <returns>The IStat for the stat of the given <paramref name="statType"/>.</returns>
        public IStat GetStat(StatType statType)
        {
            return _stats[statType.GetValue()];
        }

        /// <summary>
        /// Tries to get the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="stat">The IStat for the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be null.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStat(StatType statType, out IStat stat)
        {
            stat = GetStat(statType);
            return true;
        }

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStatValue(StatType statType, out int value)
        {
            value = this[statType];
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
        public void CopyValuesFrom(IEnumerable<KeyValuePair<StatType, int>> values, bool checkContains)
        {
            foreach (var value in values)
            {
                if (checkContains && !Contains(value.Key))
                    continue;

                this[value.Key] = value.Value;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of the KeyValuePairs in this IStatCollection, where the key is the StatType and the
        /// value is the value of for the stat with the corresponding StatType.
        /// </summary>
        /// <returns>An IEnumerable of the KeyValuePairs in this IStatCollection, where the key is the StatType and the
        /// value is the value of for the stat with the corresponding StatType.</returns>
        public IEnumerable<KeyValuePair<StatType, int>> ToKeyValuePairs()
        {
            return this.xxxToKeyValuePairs();
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
        public void CopyValuesFrom(IEnumerable<IStat> values, bool checkContains)
        {
            CopyValuesFrom(values.xxxToKeyValuePairs(), checkContains);
        }

        #endregion
    }
}