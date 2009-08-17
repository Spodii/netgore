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

            _stats = new IStat[StatFactory.AllStats.Count()];

            foreach (StatType statType in StatFactory.AllStats)
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

        #endregion
    }
}