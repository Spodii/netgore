using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// An <see cref="IStatCollection{TStatType}"/> implementation that can contain as few or as many
    /// <typeparamref name="TStatType"/>s as needed. New <typeparamref name="TStatType"/>s can be added to the collection
    /// whenever needed.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public class DynamicStatCollection<TStatType> : IStatCollection<TStatType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        readonly StatCollectionType _statCollectionType;

        readonly Dictionary<TStatType, IStat<TStatType>> _stats =
            new Dictionary<TStatType, IStat<TStatType>>(EnumComparer<TStatType>.Instance);

        /// <summary>
        /// The delegate used to hook to the <see cref="IStat{TStatType}.Changed"/> event for
        /// <see cref="IStat{TStatType}"/>s in this collection.
        /// </summary>
        readonly IStatEventHandler<TStatType> _statChangedHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStatCollection{TStatType}"/> class.
        /// </summary>
        /// <param name="statCollectionType">The type of the collection.</param>
        protected DynamicStatCollection(StatCollectionType statCollectionType)
        {
            _statChangedHandler = Stat_Changed;
            _statCollectionType = statCollectionType;
        }

        /// <summary>
        /// Notifies listeners when a stat has been added to this collection.
        /// </summary>
        public event DynamicStatCollectionStatEventHandler<TStatType> StatAdded;

        /// <summary>
        /// Adds an <see cref="IStat{StatType}"/> to the collection.
        /// </summary>
        /// <param name="stat">IStat to add to the collection.</param>
        protected void Add(IStat<TStatType> stat)
        {
            _stats.Add(stat.StatType, stat);
            stat.Changed += _statChangedHandler;

            OnStatAdded(stat);

            if (StatAdded != null)
                StatAdded(this, stat);
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

        /// <summary>
        /// Adds <see cref="IStat{StatType}"/>s to the collection.
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
        /// Adds <see cref="IStat{StatType}"/>s to the collection.
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
        /// Gets an <see cref="IStat{StatType}"/> from this <see cref="DynamicStatCollection{StatType}"/>, or creates the
        /// <see cref="IStat{StatType}"/> for the <paramref name="statType"/> if the <see cref="IStat{StatType}"/>
        /// did not already exist in the collection.
        /// </summary>
        /// <param name="statType">Type of stat to get.</param>
        /// <returns>The <see cref="IStat{StatType}"/> in this <see cref="DynamicStatCollection{StatType}"/> for the
        /// <paramref name="statType"/>.</returns>
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
        /// When overridden in the derived class, handles when an <see cref="IStat{StatType}"/> is added to this
        /// <see cref="DynamicStatCollection{StatType}"/>. This will be invoked once and only once for every
        /// <see cref="IStat{StatType}"/> added to this <see cref="DynamicStatCollection{StatType}"/>.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{StatType}"/> that was added to this
        /// <see cref="DynamicStatCollection{StatType}"/>.</param>
        protected virtual void OnStatAdded(IStat<TStatType> stat)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when an <see cref="IStat{StatType}"/> in this
        /// <see cref="DynamicStatCollection{StatType}"/> has changed their value.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{StatType}"/> whos value has changed.</param>
        protected virtual void OnStatChanged(IStat<TStatType> stat)
        {
        }

        #region IStatCollection<TStatType> Members

        /// <summary>
        /// Gets or sets the <see cref="System.Int32"/> with the specified stat type.
        /// </summary>
        /// <value></value>
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
        /// Notifies listeners when any of the <see cref="IStat{TStatType}"/>s in this collection have raised
        /// their <see cref="IStat{StatType}.Changed"/> event.
        /// </summary>
        public event IStatCollectionStatEventHandler<TStatType> StatChanged;

        /// <summary>
        /// Gets the <see cref="StatCollectionType"/> that this collection is for.
        /// </summary>
        /// <value></value>
        public StatCollectionType StatCollectionType
        {
            get { return _statCollectionType; }
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
            return _stats.ContainsKey(statType);
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
        /// Gets the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <returns>
        /// The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </returns>
        public virtual IStat<TStatType> GetStat(TStatType statType)
        {
            return _stats[statType];
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
            return _stats.TryGetValue(statType, out stat);
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
            IStat<TStatType> stat;
            if (!TryGetStat(statType, out stat))
            {
                value = 0;
                return false;
            }

            value = stat.Value;
            return true;
        }

        #endregion
    }
}