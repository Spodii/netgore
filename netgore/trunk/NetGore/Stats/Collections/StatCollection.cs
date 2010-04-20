using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// A collection of stats.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public class StatCollection<TStatType> : IStatCollection<TStatType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Cache of the func to convert an int to a <typeparamref name="TStatType"/>.
        /// </summary>
        static readonly Func<int, TStatType> _intToStatType;

        /// <summary>
        /// Cache of the func to convert a <typeparamref name="TStatType"/> to int.
        /// </summary>
        static readonly Func<TStatType, int> _statTypeToInt;

        /// <summary>
        /// The size to make the <see cref="_stats"/> array for each instance.
        /// </summary>
        static readonly int _statsArraySize;

        readonly StatCollectionType _collectionType;
        readonly StatValueType[] _stats;

        /// <summary>
        /// Initializes the <see cref="StatCollection{TStatType}"/> class.
        /// </summary>
        static StatCollection()
        {
            // Cache the TStatType <-> int conversion func to speed up calls slightly
            _statTypeToInt = EnumHelper<TStatType>.GetToIntFunc();
            Debug.Assert(_statTypeToInt != null);

            _intToStatType = EnumHelper<TStatType>.GetFromIntFunc();
            Debug.Assert(_intToStatType != null);

            // Cache the size we need to make the _stats array for each instance
            _statsArraySize = EnumHelper<TStatType>.MaxValue + 1;
            Debug.Assert(_statsArraySize > 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCollection{TStatType}"/> class.
        /// </summary>
        /// <param name="collectionType">The type of stat collection.</param>
        public StatCollection(StatCollectionType collectionType)
        {
            _collectionType = collectionType;
            _stats = new StatValueType[_statsArraySize];
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
                // If the source is the same type as this, we can do an optimized copy
                var asThis = stats as StatCollection<TStatType>;
                if (asThis != null)
                {
                    // Specialized array copy
                    asThis._stats.CopyTo(_stats, 0);
                }
                else
                {
                    // Per-stat copy
                    foreach (var stat in stats)
                    {
                        this[stat.StatType] = stat.Value;
                    }
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
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another <see cref="IStatCollection{TStatType}"/> for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TStatType}"/> to compare to.</param>
        /// <returns>True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.</returns>
        public bool HasAllGreaterOrEqualValues(StatCollection<TStatType> other)
        {
            // Compare
            for (var i = 0; i < _stats.Length; i++)
            {
                var a = _stats[i];
                var b = other._stats[i];
                if (a < b)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another <see cref="IStatCollection{TStatType}"/> for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TStatType}"/> to compare to.</param>
        /// <returns>True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.</returns>
        public bool HasAllGreaterValues(StatCollection<TStatType> other)
        {
            for (var i = 0; i < _stats.Length; i++)
            {
                var a = _stats[i];
                var b = other._stats[i];
                if (a <= b)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another <see cref="IStatCollection{TStatType}"/> for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TStatType}"/> to compare to.</param>
        /// <returns>True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.</returns>
        public bool HasAnyGreaterOrEqualValues(StatCollection<TStatType> other)
        {
            for (var i = 0; i < _stats.Length; i++)
            {
                var a = _stats[i];
                var b = other._stats[i];
                if (a >= b)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another <see cref="IStatCollection{TStatType}"/> for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TStatType}"/> to compare to.</param>
        /// <returns>True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.</returns>
        public bool HasAnyGreaterValues(StatCollection<TStatType> other)
        {
            for (var i = 0; i < _stats.Length; i++)
            {
                var a = _stats[i];
                var b = other._stats[i];
                if (a >= b)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the values of this <see cref="StatCollection{StatType}"/> are equal to the values of
        /// another <see cref="StatCollection{StatType}"/>.
        /// </summary>
        /// <param name="other">The <see cref="StatCollection{StatType}"/> to compare against.</param>
        /// <returns>True if the values of this <see cref="StatCollection{StatType}"/> are equal to the values
        /// of <paramref name="other"/>.</returns>
        public bool HasSameValues(StatCollection<TStatType> other)
        {
            // Compare each stat value directly from the arrays, avoiding converting to/from the TStatType
            for (var i = 0; i < _stats.Length; i++)
            {
                if (_stats[i] != other._stats[i])
                    return false;
            }

            return true;
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
        public event StatCollectionStatChangedEventHandler<TStatType> StatChanged;

        /// <summary>
        /// Gets or sets the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to get or set the value of.</param>
        /// <returns>The value of the stat of the given <paramref name="statType"/>.</returns>
        public StatValueType this[TStatType statType]
        {
            get { return _stats[_statTypeToInt(statType)]; }
            set
            {
                // Get the index from the TStatType
                var i = _statTypeToInt(statType);

                // Get the stat value
                var oldValue = _stats[i];

                // Ensure the value has changed
                if (oldValue == value)
                    return;

                // Set the new value
                _stats[i] = value;

                // Raise the event
                OnStatChanged(statType, oldValue, value);

                if (StatChanged != null)
                    StatChanged(this, statType, oldValue, value);
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
            for (var i = 0; i < _stats.Length; i++)
            {
                yield return new Stat<TStatType>(_intToStatType(i), _stats[i]);
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
            // Check if we can use a specialized overload
            var asThis = other as StatCollection<TStatType>;
            if (asThis != null)
                return HasAllGreaterOrEqualValues(asThis);

            // Compare
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
            // Check if we can use a specialized overload
            var asThis = other as StatCollection<TStatType>;
            if (asThis != null)
                return HasAllGreaterValues(asThis);

            // Compare
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
            // Check if we can use a specialized overload
            var asThis = other as StatCollection<TStatType>;
            if (asThis != null)
                return HasAnyGreaterOrEqualValues(asThis);

            // Compare
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
            // Check if we can use a specialized overload
            var asThis = other as StatCollection<TStatType>;
            if (asThis != null)
                return HasAnyGreaterOrEqualValues(asThis);

            // Compare
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
            // If possible, use our specialized overload for better performance
            var asThis = other as StatCollection<TStatType>;
            if (asThis != null)
                return HasSameValues(asThis);

            // Compare each stat one by one
            for (var i = 0; i < _stats.Length; i++)
            {
                if (_stats[i] != other[_intToStatType(i)])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the value of all stats in this collection to the specified value.
        /// </summary>
        /// <param name="value">The value to set all stats to.</param>
        public void SetAll(StatValueType value)
        {
            for (var i = 0; i < _stats.Length; i++)
            {
                _stats[i] = value;
            }
        }

        #endregion
    }
}