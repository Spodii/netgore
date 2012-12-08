using System;
using System.Linq;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Represents the index of an item in a shop.
    /// </summary>
    public struct ShopItemIndex : IEquatable<ShopItemIndex>
    {
        /// <summary>
        /// Represents the largest possible value of ShopItemIndex. This field is constant.
        /// </summary>
        public const byte MaxValue = byte.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of ShopItemIndex. This field is constant.
        /// </summary>
        public const byte MinValue = byte.MinValue;

        readonly byte _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItemIndex"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        public ShopItemIndex(byte id)
        {
            _value = id;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ShopItemIndex"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator byte(ShopItemIndex v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="ShopItemIndex"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator ShopItemIndex(byte v)
        {
            return new ShopItemIndex(v);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ShopItemIndex other)
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
            return obj is ShopItemIndex && this == (ShopItemIndex)obj;
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
        public static bool operator ==(ShopItemIndex left, ShopItemIndex right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ShopItemIndex left, ShopItemIndex right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Gets the raw internal value of this ShopItemIndex.
        /// </summary>
        /// <returns>The raw internal value.</returns>
        public ushort GetRawValue()
        {
            return _value;
        }
    }
}