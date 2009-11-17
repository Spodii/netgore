using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents the category of for a sprite.
    /// </summary>
    public sealed class SpriteTitle
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
                throw new ArgumentException("value");

            _value = value;
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
        /// Checks if a string is valid for a <see cref="SpriteTitle"/>.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // No delimiters allowed
            if (value.Contains(SpriteCategorization.Delimiter))
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
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.IO.SpriteTitle"/>.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SpriteTitle(string assetName)
        {
            return new SpriteTitle(assetName);
        }
    }
}
