using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A struct containing a guild member's name and rank.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes")]
    public struct GuildMemberNameRank : IEquatable<GuildMemberNameRank>, IComparable<GuildMemberNameRank>
    {
        readonly string _name;
        readonly GuildRank _rank;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberNameRank"/> struct.
        /// </summary>
        /// <param name="name">The guild member's name.</param>
        /// <param name="rank">The guild member's rank.</param>
        public GuildMemberNameRank(string name, GuildRank rank)
        {
            _name = name;
            _rank = rank;
        }

        /// <summary>
        /// Gets the guild member's name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the guild member's rank.
        /// </summary>
        public GuildRank Rank
        {
            get { return _rank; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is GuildMemberNameRank && this == (GuildMemberNameRank)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_name != null ? _name.GetHashCode() : 0) * 397) ^ _rank.GetHashCode();
            }
        }

        #region IComparable<GuildMemberNameRank> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero This object is less than
        /// the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>.
        /// Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(GuildMemberNameRank other)
        {
            if (other.Rank == Rank)
                return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name);

            if (other.Rank < Rank)
                return -1;

            return 1;
        }

        #endregion

        #region IEquatable<GuildMemberNameRank> Members

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(GuildMemberNameRank other)
        {
            return Equals(other._name, _name) && other._rank.Equals(_rank);
        }

        #endregion

        /// <summary>
        /// Performs an implicit conversion from <see cref="GuildMemberNameRank"/> to <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator KeyValuePair<string, GuildRank>(GuildMemberNameRank v)
        {
            return new KeyValuePair<string, GuildRank>(v.Name, v.Rank);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="KeyValuePair{K,V}"/> to <see cref="GuildMemberNameRank"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator GuildMemberNameRank(KeyValuePair<string, GuildRank> v)
        {
            return new GuildMemberNameRank(v.Key, v.Value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(GuildMemberNameRank left, GuildMemberNameRank right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(GuildMemberNameRank left, GuildMemberNameRank right)
        {
            return left.Equals(right);
        }
    }
}