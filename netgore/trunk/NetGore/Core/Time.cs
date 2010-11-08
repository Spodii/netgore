using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Represents how long the application has been running in milliseconds.
    /// </summary>
    public struct TickCount : IEquatable<TickCount>
    {
        /// <summary>
        /// Stores the time that the application started. Or, more precisely, the time that this class was first called.
        /// </summary>
        static readonly int _startupTime = Environment.TickCount;

        readonly uint _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TickCount"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public TickCount(uint value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the largest possible value for a <see cref="TickCount"/>.
        /// </summary>
        public static TickCount MaxValue
        {
            get { return new TickCount(uint.MaxValue); }
        }

        /// <summary>
        /// Gets the smallest possible value for a <see cref="TickCount"/>.
        /// </summary>
        public static TickCount MinValue
        {
            get { return new TickCount(uint.MinValue); }
        }

        /// <summary>
        /// Gets the amount of time that has elapsed in milliseconds since this application has started. This value will initially
        /// start at 0 when the application starts up. After approximately 49.71 days of application up-time, the tick count
        /// will roll back over back to 0. This is intended to be used as a replacement to <see cref="Environment.TickCount"/> since it
        /// guarantees rolling over only after approximately 49.71 days of application up-time, whereas <see cref="Environment.TickCount"/>
        /// roll-over depends on how long the system has been running and can roll over at any time relative to when the application started.
        /// </summary>
        public static TickCount Now
        {
            get
            {
                // Instead of using TickCount.Now directly, we find the difference in the tick count compared to when the application
                // started, which allows us to start at 0.
                return (uint)(Environment.TickCount - _startupTime);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.TickCount"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator uint(TickCount time)
        {
            return time._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.TickCount"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator long(TickCount time)
        {
            return time._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.TickCount"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(TickCount time)
        {
            return time._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt32"/> to <see cref="NetGore.TickCount"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TickCount(uint value)
        {
            return new TickCount(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.TickCount"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(TickCount time)
        {
            return (int)time._value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="NetGore.TickCount"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator TickCount(int value)
        {
            return new TickCount((uint)value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int64"/> to <see cref="NetGore.TickCount"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator TickCount(long value)
        {
            return new TickCount((uint)value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TickCount other)
        {
            return other._value == _value;
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
            return obj is TickCount && this == (TickCount)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TickCount left, TickCount right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TickCount left, TickCount right)
        {
            return !left.Equals(right);
        }
    }
}