using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Contains a stat type and the corresponding value for the stat.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public struct Stat<TStatType> : IEquatable<Stat<TStatType>>
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
            return new KeyValuePair<TStatType, int>(value.StatType, value.Value.GetRawValue());
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
            return new Stat<TStatType>(value.Key, (StatValueType)value.Value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Stat<TStatType> other)
        {
            return Equals(other._statType, _statType) && other._value == _value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Stat<TStatType> && this == (Stat<TStatType>)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_statType.GetHashCode() * 397) ^ _value.GetHashCode();
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Stat<TStatType> left, Stat<TStatType> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Stat<TStatType> left, Stat<TStatType> right)
        {
            return !left.Equals(right);
        }
    }
}