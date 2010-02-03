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
    public class ChangedStatsTracker<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Empty enumerable of <see cref="IStat{T}"/>s.
        /// </summary>
        static readonly IEnumerable<IStat<T>> _emptyStats = Enumerable.Empty<IStat<T>>();

        static readonly Func<T, int> _enumToInt;

        readonly int[] _lastValues;
        readonly IStatCollection<T> _statCollection;

        /// <summary>
        /// Initializes the <see cref="ChangedStatsTracker{T}"/> class.
        /// </summary>
        static ChangedStatsTracker()
        {
            _enumToInt = EnumHelper<T>.GetToIntFunc();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedStatsTracker{T}"/> class.
        /// </summary>
        /// <param name="statCollection">The stat collection to track for changes.</param>
        public ChangedStatsTracker(IStatCollection<T> statCollection)
        {
            _statCollection = statCollection;
            int size = _statCollection.Select(x => ToArrayIndex(x.StatType)).Max() + 1;
            _lastValues = new int[size];

            // Populate the last values array with the current values
            foreach (var istat in statCollection)
            {
                this[istat] = istat.Value;
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
        protected int this[IStat<T> key]
        {
            get { return this[key.StatType]; }
            set { this[key.StatType] = value; }
        }

        /// <summary>
        /// Gets the <see cref="IStat{T}"/>s that have changed since the last call to this method.
        /// </summary>
        /// <returns>The <see cref="IStat{T}"/>s that have changed since the last call to this method.</returns>
        public IEnumerable<IStat<T>> GetChangedStats()
        {
            List<IStat<T>> ret = null;

            foreach (var stat in _statCollection)
            {
                var index = ToArrayIndex(stat.StatType);
                var currentValue = stat.Value;

                if (_lastValues[index] != currentValue)
                {
                    if (ret == null)
                        ret = new List<IStat<T>>();

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