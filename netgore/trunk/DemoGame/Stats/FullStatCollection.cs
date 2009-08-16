using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// A specialized collection that contains every StatType. It is fast, but at the cost of not being able to contain
    /// only some StatTypes instead of all.
    /// </summary>
    public class FullStatCollection : IStatCollection
    {
        readonly StatCollectionType _collectionType;
        readonly IStat[] _stats;

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

        FullStatCollection(FullStatCollection source)
        {
            _stats = new IStat[source._stats.Length];
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i] = source._stats[i].DeepCopy();
            }
        }

        public bool AreValuesEqual(IStatCollection other)
        {
            for (int i = 0; i < _stats.Length; i++)
            {
                StatType statType = (StatType)i;
                if (this[statType] != other[statType])
                    return false;
            }

            return true;
        }

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

        public int this[StatType statType]
        {
            get { return _stats[statType.GetValue()].Value; }
            set { _stats[statType.GetValue()].Value = value; }
        }

        public StatCollectionType StatCollectionType
        {
            get { return _collectionType; }
        }

        public bool Contains(StatType statType)
        {
            return true;
        }

        public IStat GetStat(StatType statType)
        {
            return _stats[statType.GetValue()];
        }

        public bool TryGetStat(StatType statType, out IStat stat)
        {
            stat = GetStat(statType);
            return true;
        }

        public bool TryGetStatValue(StatType statType, out int value)
        {
            value = this[statType];
            return true;
        }

        #endregion
    }
}