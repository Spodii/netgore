using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// An item for the <see cref="ColumnCollection"/>.
    /// </summary>
    public struct ColumnCollectionItem : IEquatable<ColumnCollectionItem>
    {
        readonly string _columnName;
        readonly string _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnCollectionItem"/> struct.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="key">The key.</param>
        public ColumnCollectionItem(string columnName, string key)
        {
            _columnName = columnName;
            _key = key;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
        }

        /// <summary>
        /// Gets the key for this column.
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Creates a <see cref="ColumnCollectionItem"/> from an enum.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <param name="formatter">The <see cref="CodeFormatter"/>.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The resulting <see cref="ColumnCollectionItem"/>.</returns>
        public static ColumnCollectionItem FromEnum<T>(CodeFormatter formatter, string columnName, T key)
        {
            var keyStr = formatter.GetTypeString(typeof(T)) + "." + key;
            return new ColumnCollectionItem(columnName, keyStr);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ColumnCollectionItem other)
        {
            return Equals(other._columnName, _columnName) && Equals(other._key, _key);
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
            return obj is ColumnCollectionItem && this == (ColumnCollectionItem)obj;
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
                return ((_columnName != null ? _columnName.GetHashCode() : 0) * 397) ^ (_key != null ? _key.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ColumnCollectionItem left, ColumnCollectionItem right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ColumnCollectionItem left, ColumnCollectionItem right)
        {
            return !left.Equals(right);
        }
    }
}