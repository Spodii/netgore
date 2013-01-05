using System;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Represents the rank a <see cref="IGuildMember"/> is in a guild.
    /// </summary>
    public struct GuildRank : IEquatable<GuildRank>
    {
        readonly byte _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildRank"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public GuildRank(byte value)
        {
            _value = value;
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
            return obj is GuildRank && this == (GuildRank)obj;
        }

        /// <summary>
        /// Indicates whether this instance and a specified value are equal.
        /// </summary>
        /// <param name="other">Another value to compare to.</param>
        /// <returns>
        /// true if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public bool Equals(GuildRank other)
        {
            return other._value == _value;
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
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Features.Guilds.GuildRank"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte(GuildRank value)
        {
            return value._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="NetGore.Features.Guilds.GuildRank"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GuildRank(byte value)
        {
            return new GuildRank(value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(GuildRank left, GuildRank right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(GuildRank left, GuildRank right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator --
        /// </summary>
        /// <param name="rank">Argument.</param>
        /// <returns>New GuildRank with one less.</returns>
        public static GuildRank operator --(GuildRank rank)
        {
            return new GuildRank((byte)(rank._value - 1));
        }

        /// <summary>
        /// Implements the operator ++.
        /// </summary>
        /// <param name="rank">Argument.</param>
        /// <returns>New GuildRank plus one.</returns>
        public static GuildRank operator ++(GuildRank rank)
        {
            return new GuildRank((byte)(rank._value + 1));
        }
    }
}