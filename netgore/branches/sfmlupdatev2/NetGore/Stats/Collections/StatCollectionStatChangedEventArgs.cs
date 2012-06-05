using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// <see cref="EventArgs"/> for when the stat value in a <see cref="IStatCollection{T}"/> changes.
    /// </summary>
    /// <typeparam name="TStatType"></typeparam>
    public class StatCollectionStatChangedEventArgs<TStatType> : EventArgs
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        readonly StatValueType _newValue;
        readonly StatValueType _oldValue;
        readonly TStatType _statType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCollectionStatChangedEventArgs&lt;TStatType&gt;"/> class.
        /// </summary>
        /// <param name="statType">The stat that changed.</param>
        /// <param name="oldValue">The old value of the stat.</param>
        /// <param name="newValue">The new value of the stat.</param>
        public StatCollectionStatChangedEventArgs(TStatType statType, StatValueType oldValue, StatValueType newValue)
        {
            _statType = statType;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// Gets the new value of the stat.
        /// </summary>
        public StatValueType NewValue
        {
            get { return _newValue; }
        }

        /// <summary>
        /// Gets the old value of the stat.
        /// </summary>
        public StatValueType OldValue
        {
            get { return _oldValue; }
        }

        /// <summary>
        /// Gets the stat that changed.
        /// </summary>
        public TStatType StatType
        {
            get { return _statType; }
        }
    }
}