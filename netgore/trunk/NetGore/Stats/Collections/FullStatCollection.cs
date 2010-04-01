using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// A specialized collection that contains every <typeparamref name="TStatType"/>. This is the ideal
    /// <see cref="IStatCollection{StatType}"/> to use when you want the collection to always contain every
    /// <typeparamref name="TStatType"/>.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public class FullStatCollection<TStatType> : IStatCollection<TStatType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// The size to make the <see cref="_stats"/> array for each instance.
        /// </summary>
        static readonly int _statsArraySize;

        /// <summary>
        /// Cache of the func to convert a <typeparamref name="TStatType"/> to int.
        /// </summary>
        static readonly Func<TStatType, int> _statTypeToInt;

        readonly StatCollectionType _collectionType;
        readonly IStat<TStatType>[] _stats;

        /// <summary>
        /// Initializes the <see cref="FullStatCollection{TStatType}"/> class.
        /// </summary>
        static FullStatCollection()
        {
            // Cache the TStatType -> int conversion func to speed up calls slightly
            _statTypeToInt = EnumHelper<TStatType>.GetToIntFunc();
            Debug.Assert(_statTypeToInt != null);

            _statsArraySize = EnumHelper<TStatType>.MaxValue + 1;
            Debug.Assert(_statsArraySize > 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullStatCollection{TStatType}"/> class.
        /// </summary>
        /// <param name="collectionType">The type of <typeparamref name="TStatType"/>s that this collection contains.</param>
        public FullStatCollection(StatCollectionType collectionType)
        {
            IStatEventHandler<TStatType> statChangedHandler = Stat_Changed;

            _collectionType = collectionType;
            _stats = new IStat<TStatType>[_statsArraySize];

            foreach (var statType in EnumHelper<TStatType>.Values)
            {
                var stat = new Stat<TStatType>(statType, 0);
                stat.Changed += statChangedHandler;

                _stats[_statTypeToInt(statType)] = stat;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullStatCollection{StatType}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="FullStatCollection{StatType}"/> to copy the values from.</param>
        FullStatCollection(FullStatCollection<TStatType> source)
        {
            IStatEventHandler<TStatType> statChangedHandler = Stat_Changed;

            _collectionType = source._collectionType;
            _stats = new IStat<TStatType>[source._stats.Length];

            for (int i = 0; i < _stats.Length; i++)
            {
                var stat = source._stats[i].DeepCopy();
                stat.Changed += statChangedHandler;

                _stats[i] = stat;
            }
        }

        /// <summary>
        /// Checks if the values of this <see cref="FullStatCollection{StatType}"/> are equal to the values of
        /// another <see cref="FullStatCollection{StatType}"/>.
        /// </summary>
        /// <param name="other">The <see cref="FullStatCollection{StatType}"/> to compare against.</param>
        /// <returns>True if the values of this <see cref="FullStatCollection{StatType}"/> are equal to the values
        /// of <paramref name="other"/>.</returns>
        public bool AreValuesEqual(FullStatCollection<TStatType> other)
        {
            return _stats.All(x => x.Value == other[x.StatType]);
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="FullStatCollection{StatType}"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="FullStatCollection{StatType}"/>.</returns>
        public FullStatCollection<TStatType> DeepCopy()
        {
            return new FullStatCollection<TStatType>(this);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an <see cref="IStat{StatType}"/> in this
        /// <see cref="DynamicStatCollection{StatType}"/> has changed their value.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{StatType}"/> whos value has changed.</param>
        protected virtual void OnStatChanged(IStat<TStatType> stat)
        {
        }

        /// <summary>
        /// Sets the value of all stats in this <see cref="FullStatCollection{StatType}"/>.
        /// </summary>
        /// <param name="value">Value to set all of the stat values to.</param>
        public void SetAll(int value)
        {
            foreach (var stat in _stats)
            {
                stat.Value = value;
            }
        }

        /// <summary>
        /// Handles the <see cref="IStat{TStatType}.Changed"/> events for <see cref="IStat{StatType}"/>s in this
        /// <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{TStatType}"/> that raised the event.</param>
        void Stat_Changed(IStat<TStatType> stat)
        {
            OnStatChanged(stat);

            if (StatChanged != null)
                StatChanged(this, stat);
        }

        #region IStatCollection<TStatType> Members

        /// <summary>
        /// Notifies listeners when any of the <see cref="IStat{TStatType}"/>s in this collection have raised
        /// their <see cref="IStat{StatType}.Changed"/> event.
        /// </summary>
        public event IStatCollectionStatEventHandler<TStatType> StatChanged;

        /// <summary>
        /// Gets or sets the <see cref="System.Int32"/> with the specified stat type.
        /// </summary>
        public int this[TStatType statType]
        {
            get { return _stats[_statTypeToInt(statType)].Value; }
            set { _stats[_statTypeToInt(statType)].Value = value; }
        }

        /// <summary>
        /// Gets the <see cref="StatCollectionType"/> that this collection is for.
        /// </summary>
        public StatCollectionType StatCollectionType
        {
            get { return _collectionType; }
        }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to check if exists in the collection.</param>
        /// <returns>
        /// True if this collection contains the <see cref="IStat{TStatType}"/> with the given
        /// <paramref name="statType"/>; otherwise false.
        /// </returns>
        public bool Contains(TStatType statType)
        {
            return true;
        }

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given
        /// <typeparamref name="TStatType"/> into this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <param name="values">IEnumerable of stat types and stat values to copy into this
        /// <see cref="IStatCollection{TStatType}"/>.</param>
        /// <param name="checkContains">If true, each stat type in <paramref name="values"/> will first be checked
        /// if it is in this <see cref="IStatCollection{TStatType}"/> before trying to copy over the value.
        /// Any stat type in <paramref name="values"/> but not in this <see cref="IStatCollection{TStatType}"/> will be
        /// skipped. If false, no checking will be done. Any stat type in <paramref name="values"/> but not in this
        /// <see cref="IStatCollection{TStatType}"/> will behave the same as if the value of a stat type not in this
        /// <see cref="IStatCollection{TStatType}"/> was attempted to be assigned in any other way.</param>
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
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given stat type
        /// into this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <param name="values">IEnumerable of <typeparamref name="TStatType"/>s and stat values to copy into this
        /// <see cref="IStatCollection{TStatType}"/>.</param>
        /// <param name="checkContains">If true, each stat type in <paramref name="values"/> will first be checked
        /// if it is in this <see cref="IStatCollection{TStatType}"/> before trying to copy over the value. Any stat
        /// type in <paramref name="values"/> but not in this <see cref="IStatCollection{TStatType}"/> will be skipped.
        /// If false, no checking will be done. Any stat type in <paramref name="values"/> but not in this
        /// <see cref="IStatCollection{TStatType}"/> will behave the same as if the value of a stat type not in this
        /// <see cref="IStatCollection{TStatType}"/> was attempted to be assigned in any other way.</param>
        public void CopyValuesFrom(IEnumerable<IStat<TStatType>> values, bool checkContains)
        {
            CopyValuesFrom(values.ToKeyValuePairs(), checkContains);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IStat<TStatType>> GetEnumerator()
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <returns>
        /// The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </returns>
        public IStat<TStatType> GetStat(TStatType statType)
        {
            return _stats[_statTypeToInt(statType)];
        }

        /// <summary>
        /// Tries to get the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <param name="stat">The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// If this method returns false, this value will be null.</param>
        /// <returns>
        /// True if the <see cref="IStat{StatType}"/> with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.
        /// </returns>
        public bool TryGetStat(TStatType statType, out IStat<TStatType> stat)
        {
            stat = GetStat(statType);
            return true;
        }

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>
        /// True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.
        /// </returns>
        public bool TryGetStatValue(TStatType statType, out int value)
        {
            value = this[statType];
            return true;
        }

        #endregion
    }
}