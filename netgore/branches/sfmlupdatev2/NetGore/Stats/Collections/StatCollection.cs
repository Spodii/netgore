using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Stats
{
    /// <summary>
    /// A collection of stats.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public class StatCollection<TStatType> : IStatCollection<TStatType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        readonly StatCollectionType _collectionType;
        readonly IEnumTable<TStatType, StatValueType> _stats;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCollection{TStatType}"/> class.
        /// </summary>
        /// <param name="collectionType">The type of stat collection.</param>
        public StatCollection(StatCollectionType collectionType)
        {
            _collectionType = collectionType;
            _stats = EnumTable.Create<TStatType, StatValueType>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCollection{TStatType}"/> class.
        /// </summary>
        /// <param name="collectionType">The type of stat collection.</param>
        /// <param name="stats">The initial values to assign to the stats.</param>
        public StatCollection(StatCollectionType collectionType, IEnumerable<Stat<TStatType>> stats) : this(collectionType)
        {
            // Copy over the stat values for each stat
            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    this[stat.StatType] = stat.Value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCollection{StatType}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="StatCollection{StatType}"/> to copy the values from.</param>
        public StatCollection(IStatCollection<TStatType> source) : this(source.StatCollectionType, source)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a stat's value has changed.
        /// </summary>
        /// <param name="statType">The type of the stat that changed.</param>
        /// <param name="oldValue">The old value of the stat.</param>
        /// <param name="newValue">The new value of the stat.</param>
        protected virtual void OnStatChanged(TStatType statType, StatValueType oldValue, StatValueType newValue)
        {
        }

        #region IStatCollection<TStatType> Members

        /// <summary>
        /// Notifies listeners when the value of any of the stats in this collection have changed.
        /// </summary>
        public event TypedEventHandler<IStatCollection<TStatType>, StatCollectionStatChangedEventArgs<TStatType>> StatChanged;

        /// <summary>
        /// Gets or sets the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to get or set the value of.</param>
        /// <returns>The value of the stat of the given <paramref name="statType"/>.</returns>
        public StatValueType this[TStatType statType]
        {
            get { return _stats[statType]; }
            set
            {
                // Get the stat value
                var oldValue = _stats[statType];

                // Ensure the value has changed
                if (oldValue == value)
                    return;

                // Set the new value
                _stats[statType] = value;

                // Raise the event
                OnStatChanged(statType, oldValue, value);

                if (StatChanged != null)
                    StatChanged.Raise(this, new StatCollectionStatChangedEventArgs<TStatType>(statType, oldValue, value));
            }
        }

        /// <summary>
        /// Gets the <see cref="StatCollectionType"/> that this collection is for.
        /// </summary>
        public StatCollectionType StatCollectionType
        {
            get { return _collectionType; }
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <returns>
        /// A deep copy of this <see cref="IStatCollection{TStatType}"/> with all of the
        /// same stat values.
        /// </returns>
        public IStatCollection<TStatType> DeepCopy()
        {
            return new StatCollection<TStatType>(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Stat<TStatType>> GetEnumerator()
        {
            foreach (var stat in _stats)
            {
                yield return stat;
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
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another collection of stats for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>
        /// True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.
        /// </returns>
        public bool HasAllGreaterOrEqualValues(IEnumerable<Stat<TStatType>> other)
        {
            return other.All(x => this[x.StatType] >= x.Value);
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another collection of stats for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>
        /// True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.
        /// </returns>
        public bool HasAllGreaterValues(IEnumerable<Stat<TStatType>> other)
        {
            return other.All(x => this[x.StatType] > x.Value);
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another collection of stats for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>
        /// True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.
        /// </returns>
        public bool HasAnyGreaterOrEqualValues(IEnumerable<Stat<TStatType>> other)
        {
            return other.Any(x => this[x.StatType] >= x.Value);
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another collection of stats for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>
        /// True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.
        /// </returns>
        public bool HasAnyGreaterValues(IEnumerable<Stat<TStatType>> other)
        {
            return other.Any(x => this[x.StatType] > x.Value);
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/> contains the same stat values as another
        /// <see cref="IStatCollection{TStatType}"/> for the respective stat type.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TSstatType}"/> to compare against.</param>
        /// <returns>True if this <see cref="IStatCollection{TStatType}"/> contains the same values as the
        /// <paramref name="other"/> for the respective stat type; otherwise false.</returns>
        public bool HasSameValues(IStatCollection<TStatType> other)
        {
            return other.All(x => this[x.StatType] == x.Value);
        }

        /// <summary>
        /// Sets the value of all stats in this collection to the specified value.
        /// </summary>
        /// <param name="value">The value to set all stats to.</param>
        public void SetAll(StatValueType value)
        {
            _stats.SetAll(value);
        }

        #endregion
    }
}