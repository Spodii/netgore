using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the title of a sprite.
    /// </summary>
    [ImmutableObject(true)]
    [TypeConverter(typeof(SpriteTitleConverter))]
    public sealed class SpriteTitle : IEquatable<SpriteTitle>, IComparable<SpriteTitle>
    {
        readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTitle"/> class.
        /// </summary>
        /// <param name="value">The sprite title. This value will be automatically sanitized.</param>
        /// <exception cref="ArgumentException"><see cref="IsValid"/> returns false for <paramref name="value"/>
        /// after being sanitized.</exception>
        public SpriteTitle(string value)
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
            return obj is SpriteTitle && this == (SpriteTitle)obj;
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
        /// Checks if a string is valid for a <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // No delimiters allowed
            if (value.Contains(SpriteCategorization.Delimiter) || value.Contains('/') || value.Contains('\\'))
                return false;

            return true;
        }

        /// <summary>
        /// Sanitizes a string to be used as a <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="title">The string to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        public static string Sanitize(string title)
        {
            // Remove delimiter from start
            if (title.StartsWith(SpriteCategorization.Delimiter))
                title = title.Substring(1);

            return title;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _value;
        }

        #region IComparable<SpriteTitle> Members

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
        public int CompareTo(SpriteTitle other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_value, other._value);
        }

        #endregion

        #region IEquatable<SpriteTitle> Members

        /// <summary>
        /// Checks if this <see cref="SpriteTitle"/> is equal to another <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="other">The other <see cref="SpriteTitle"/>.</param>
        /// <returns>True if the two are equal; otherwise false.</returns>
        public bool Equals(SpriteTitle other)
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
        public static bool operator ==(SpriteTitle a, SpriteTitle b)
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
        public static bool operator !=(SpriteTitle a, SpriteTitle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.IO.SpriteTitle"/>.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SpriteTitle(string title)
        {
            return new SpriteTitle(title);
        }
    }
}