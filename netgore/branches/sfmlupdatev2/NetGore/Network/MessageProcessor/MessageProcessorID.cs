using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// A value type that represents the unique ID of a message sent from the server to the client, or from the client to the
    /// server.
    /// </summary>
    public struct MessageProcessorID : IEquatable<MessageProcessorID>
    {
        readonly ushort _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorID"/> struct.
        /// </summary>
        /// <param name="v">The value.</param>
        public MessageProcessorID(ushort v)
        {
            _value = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorID"/> struct.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="v"/> is less than <see cref="ushort.MinValue"/>
        /// or greater than <see cref="ushort.MaxValue"/>.</exception>
        public MessageProcessorID(uint v)
        {
            if (v > ushort.MaxValue || v < ushort.MinValue)
                throw new ArgumentOutOfRangeException("v", "Value be between ushort.MinValue and ushort.MaxValue.");

            _value = (ushort)v;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Network.MessageProcessorID"/> to <see cref="System.UInt16"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ushort(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Network.MessageProcessorID"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator uint(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt16"/> to <see cref="NetGore.Network.MessageProcessorID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MessageProcessorID(ushort v)
        {
            return new MessageProcessorID(v);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MessageProcessorID other)
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
            return obj is MessageProcessorID && this == (MessageProcessorID)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
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
        public static bool operator ==(MessageProcessorID left, MessageProcessorID right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MessageProcessorID left, MessageProcessorID right)
        {
            return !left.Equals(right);
        }
    }
}