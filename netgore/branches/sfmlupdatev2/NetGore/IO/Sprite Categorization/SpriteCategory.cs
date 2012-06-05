using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the category of a sprite.
    /// </summary>
    [ImmutableObject(true)]
    [TypeConverter(typeof(SpriteCategoryConverter))]
    public sealed class SpriteCategory : IEquatable<SpriteCategory>, IComparable<SpriteCategory>
    {
        readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteCategory"/> class.
        /// </summary>
        /// <param name="value">The sprite category. This value will be automatically sanitized.</param>
        /// <exception cref="ArgumentException"><see cref="IsValid"/> returns false for <paramref name="value"/>
        /// after being sanitized.</exception>
        public SpriteCategory(string value)
        {
            value = Sanitize(value);

            if (!IsValid(value))
            {
                const string errmsg = "The argument `{0}` is not valid.";
                throw new ArgumentException(string.Format(errmsg, value), "value");
            }

            _value = value;
        }

        /// <summary>
        /// Checks if this object is equal to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the two are equal; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return obj is SpriteCategory && this == (SpriteCategory)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table. </returns>
        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_value);
        }

        /// <summary>
        /// Checks if a string is valid for a <see cref="SpriteCategory"/>.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // Consecutive delimiters
            if (value.Contains(SpriteCategorization.Delimiter + SpriteCategorization.Delimiter))
                return false;

            // Delimiter at start or end
            if (value.EndsWith(SpriteCategorization.Delimiter))
                return false;

            if (value.StartsWith(SpriteCategorization.Delimiter))
                return false;

            return true;
        }

        /// <summary>
        /// Sanitizes a string to be used as a <see cref="SpriteCategory"/>.
        /// </summary>
        /// <param name="category">The string to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        public static string Sanitize(string category)
        {
            // Replace real path separators with the correct category delimiter
            category = category.Replace("/", SpriteCategorization.Delimiter).Replace("\\", SpriteCategorization.Delimiter);

            // Remove delimiter from start
            if (category.StartsWith(SpriteCategorization.Delimiter))
                category = category.Substring(1);

            // Remove delimiter from end
            if (category.EndsWith(SpriteCategorization.Delimiter))
                category = category.Substring(0, category.Length - 1);

            return category;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _value;
        }

        #region IComparable<SpriteCategory> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(SpriteCategory other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_value, other._value);
        }

        #endregion

        #region IEquatable<SpriteCategory> Members

        /// <summary>
        /// Checks if this <see cref="SpriteCategory"/> is equal to another <see cref="SpriteCategory"/>.
        /// </summary>
        /// <param name="other">The other <see cref="SpriteCategory"/>.</param>
        /// <returns>True if the two are equal; otherwise false.</returns>
        public bool Equals(SpriteCategory other)
        {
            return _value.Equals(other._value, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SpriteCategory a, SpriteCategory b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return ReferenceEquals(a, b);

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SpriteCategory a, SpriteCategory b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.IO.SpriteCategory"/>.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SpriteCategory(string category)
        {
            return new SpriteCategory(category);
        }
    }
}