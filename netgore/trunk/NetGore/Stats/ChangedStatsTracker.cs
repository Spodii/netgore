using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// Keeps track of which stats have changed.
    /// </summary>
    /// <typeparam name="T">The type of stat.</typeparam>
    public class ChangedStatsTracker<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Empty enumerable of <see cref="Stat{T}"/>s.
        /// </summary>
        static readonly IEnumerable<Stat<T>> _emptyStats = Enumerable.Empty<Stat<T>>();

        static readonly Func<T, int> _enumToInt;

        static readonly int _valuesArraySize;

        readonly StatValueType[] _lastValues;
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
                this[stat.StatType] = stat.Value;
            }
        }

        /// <summary>
        /// Gets or sets the last value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        protected int this[T key]
        {
            get { return _lastValues[ToArrayIndex(key)]; }
            set { _lastValues[ToArrayIndex(key)] = value; }
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