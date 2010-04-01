using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Contains a stat type and the corresponding value for the stat.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public struct Stat<TStatType>
    {
        readonly TStatType _statType;
        readonly StatValueType _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> struct.
        /// </summary>
        /// <param name="statType">Type of the stat.</param>
        /// <param name="value">The value.</param>
        public Stat(TStatType statType, StatValueType value)
        {
            _statType = statType;
            _value = value;
        }

        /// <summary>
        /// Gets the type of the stat.
        /// </summary>
        public TStatType StatType
        {
            get { return _statType; }
        }

        /// <summary>
        /// Gets the value of the stat.
        /// </summary>
        public StatValueType Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Stats.Stat{TStatType}"/> to <see cref="System.Collections.Generic.KeyValuePair{TStatType,StatValueType}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator KeyValuePair<TStatType, StatValueType>(Stat<TStatType> value)
        {
            return new KeyValuePair<TStatType, StatValueType>(value.StatType, value.Value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Stats.Stat{TStatType}"/> to <see cref="System.Collections.Generic.KeyValuePair{TStatType,Int32}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator KeyValuePair<TStatType, int>(Stat<TStatType> value)
        {
            return new KeyValuePair<TStatType, int>(value.StatType, value.Value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Collections.Generic.KeyValuePair{TStatType,StatValueType}"/> to <see cref="NetGore.Stats.Stat{TStatType}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Stat<TStatType>(KeyValuePair<TStatType, StatValueType> value)
        {
            return new Stat<TStatType>(value.Key, value.Value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Collections.Generic.KeyValuePair{TStatType,Int32}"/> to <see cref="NetGore.Stats.Stat{TStatType}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Stat<TStatType>(KeyValuePair<TStatType, int> value)
        {
            return new Stat<TStatType>(value.Key, value.Value);
        }
    }
}