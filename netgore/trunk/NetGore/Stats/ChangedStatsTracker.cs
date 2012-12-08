using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Keeps track of which stats have changed in an <see cref="IStatCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of stat.</typeparam>
    public class ChangedStatsTracker<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Empty enumerable of <see cref="Stat{TStatType}"/>s.
        /// </summary>
        static readonly IEnumerable<Stat<T>> _emptyStats = Enumerable.Empty<Stat<T>>();

        /// <summary>
        /// The <see cref="Func{T,U}"/> used to cast from <typeparamref name="T"/> to <see cref="Int32"/>.
        /// </summary>
        static readonly Func<T, int> _enumToInt;

        /// <summary>
        /// The size we will need to make the <see cref="_lastValues"/> array.
        /// </summary>
        static readonly int _valuesArraySize;

        /// <summary>
        /// Stores the last stat values so we can compare them to the current values.
        /// </summary>
        readonly StatValueType[] _lastValues;

        /// <summary>
        /// The <see cref="IStatCollection{T}"/> we are comparing against.
        /// </summary>
        readonly IStatCollection<T> _statCollection;

        /// <summary>
        /// Initializes the <see cref="ChangedStatsTracker{T}"/> class.
        /// </summary>
        static ChangedStatsTracker()
        {
            _enumToInt = EnumHelper<T>.GetToIntFunc();
            _valuesArraySize = EnumHelper<T>.MaxValue + 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedStatsTracker{T}"/> class.
        /// </summary>
        /// <param name="statCollection">The stat collection to track for changes.</param>
        public ChangedStatsTracker(IStatCollection<T> statCollection)
        {
            _statCollection = statCollection;
            _lastValues = new StatValueType[_valuesArraySize];

            // Populate the last values array with the current values
            foreach (var stat in statCollection)
            {
                this[stat.StatType] = stat.Value.GetRawValue();
            }
        }

        /// <summary>
        /// Gets or sets the last value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        protected int this[T key]
        {
            get { return _lastValues[ToArrayIndex(key)].GetRawValue(); }
            set { _lastValues[ToArrayIndex(key)] = (StatValueType)value; }
        }

        /// <summary>
        /// Gets or sets the last value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        protected int this[Stat<T> key]
        {
            get { return this[key.StatType]; }
            set { this[key.StatType] = value; }
        }

        /// <summary>
        /// Gets the <see cref="Stat{T}"/>s that have changed since the last call to this method.
        /// </summary>
        /// <returns>The <see cref="Stat{T}"/>s that have changed since the last call to this method.</returns>
        public IEnumerable<Stat<T>> GetChangedStats()
        {
            List<Stat<T>> ret = null;

            foreach (var stat in _statCollection)
            {
                var index = ToArrayIndex(stat.StatType);
                var currentValue = stat.Value;

                if (_lastValues[index] != currentValue)
                {
                    if (ret == null)
                        ret = new List<Stat<T>>();

                    _lastValues[index] = currentValue;
                    ret.Add(stat);
                }
            }

            if (ret == null)
                return _emptyStats;
            else
                return ret;
        }

        /// <summary>
        /// Gets the array index for a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The key to get the array index of.</param>
        /// <returns>The array index for the <paramref name="value"/>.</returns>
        static int ToArrayIndex(T value)
        {
            return _enumToInt(value);
        }
    }
}