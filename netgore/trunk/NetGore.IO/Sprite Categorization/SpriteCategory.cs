using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the category of for a sprite.
    /// </summary>
    public sealed class SpriteCategory
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
                throw new ArgumentException("value");

            _value = value;
        }

        /// <summary>
        /// Sanitizes a string to be used as a <see cref="SpriteCategory"/>.
        /// </summary>
        /// <param name="category">The string to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        public static string Sanitize(string category)
        {
            // Remove delimiter from start
            if (category.StartsWith(SpriteCategorization.Delimiter))
                category = category.Substring(1);

            // Remove delimiter from end
            if (category.EndsWith(SpriteCategorization.Delimiter))
                category = category.Substring(0, category.Length - 1);

            return category;
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.IO.SpriteCategory"/>.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SpriteCategory(string assetName)
        {
            return new SpriteCategory(assetName);
        }
    }
}
