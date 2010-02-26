using System.Linq;

namespace NetGore.Features.Friends
{
    /// <summary>
    /// A struct containing a FriendList member's name and rank.
    /// </summary>
    public struct FriendName //: IEquatable<FriendName>, IComparable<FriendName>
    {
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendName"/> struct.
        /// </summary>
        /// <param name="name">The FriendList member's name.</param>
        /// <param name="rank">The FriendList member's rank.</param>
        public FriendName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the FriendList member's name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        //        /// <summary>
        //        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        //        /// </summary>
        //        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        //        /// <returns>
        //        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        //        /// </returns>
        //        public override bool Equals(object obj)
        //        {
        //            if (obj is FriendName)
        //                return Equals((FriendName)obj);

        //            return base.Equals(obj);
        //        }

        //        /// <summary>
        //        /// Returns a hash code for this instance.
        //        /// </summary>
        //        /// <returns>
        //        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        //        /// </returns>
        //        public override int GetHashCode()
        //        {
        //            unchecked
        //            {
        //                return ((_name != null ? _name.GetHashCode() : 0) * 397) ^ _rank.GetHashCode();
        //            }
        //        }

        //        #region IComparable<FriendName> Members

        //        /// <summary>
        //        /// Compares the current object with another object of the same type.
        //        /// </summary>
        //        /// <returns>
        //        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        //        /// The return value has the following meanings: Value Meaning Less than zero This object is less than
        //        /// the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>.
        //        /// Greater than zero This object is greater than <paramref name="other"/>. 
        //        /// </returns>
        //        /// <param name="other">An object to compare with this object.</param>
        //        public int CompareTo(FriendName other)
        //        {
        //            if (other.Rank == Rank)
        //                return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name);

        //            if (other.Rank < Rank)
        //                return -1;

        //            return 1;
        //        }

        //        #endregion

        //        #region IEquatable<FriendName> Members

        //        /// <summary>
        //        /// Equalses the specified other.
        //        /// </summary>
        //        /// <param name="other">The other.</param>
        //        /// <returns></returns>
        //        public bool Equals(FriendName other)
        //        {
        //            return Equals(other._name, _name) && other._rank.Equals(_rank);
        //        }

        //        #endregion

        //        /// <summary>
        //        /// Performs an implicit conversion from <see cref="FriendName"/> to <see cref="KeyValuePair{TKey,TValue}"/>.
        //        /// </summary>
        //        /// <param name="v">The value.</param>
        //        /// <returns>The result of the conversion.</returns>
        //        public static implicit operator KeyValuePair<string, FriendListRank>(FriendName v)
        //        {
        //            return new KeyValuePair<string, FriendListRank>(v.Name, v.Rank);
        //        }

        //        /// <summary>
        //        /// Performs an implicit conversion from <see cref="KeyValuePair{K,V}"/> to <see cref="FriendName"/>.
        //        /// </summary>
        //        /// <param name="v">The value.</param>
        //        /// <returns>The result of the conversion.</returns>
        //        public static implicit operator FriendName(KeyValuePair<string, FriendListRank> v)
        //        {
        //            return new FriendName(v.Key, v.Value);
        //        }
    }
}