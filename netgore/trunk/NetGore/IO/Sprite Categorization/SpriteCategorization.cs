using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable class that contains the categorization information for a sprite.
    /// </summary>
    [ImmutableObject(true)]
    [TypeConverter(typeof(SpriteCategorizationConverter))]
    public class SpriteCategorization : IEquatable<SpriteCategorization>
    {
        /// <summary>
        /// The delimiter used to separate category branches, and the title from the category.
        /// </summary>
        public const string Delimiter = ".";

        readonly SpriteCategory _category;
        readonly SpriteTitle _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteCategorization"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The title.</param>
        /// <exception cref="ArgumentNullException"><paramref name="category" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        public SpriteCategorization(SpriteCategory category, SpriteTitle title)
        {
            if (category == null)
                throw new ArgumentNullException("category");
            if (title == null)
                throw new ArgumentNullException("title");

            _category = category;
            _title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteCategorization"/> class.
        /// </summary>
        /// <param name="categoryAndTitle">The category and title concatenated into a single string.</param>
        public SpriteCategorization(string categoryAndTitle)
        {
            SplitCategoryAndTitle(categoryAndTitle, out _category, out _title);
        }

        /// <summary>
        /// Gets the category part of the categorization.
        /// </summary>
        [Description("The category of the sprite.")]
        [DisplayName("Category")]
        public SpriteCategory Category
        {
            get { return _category; }
        }

        /// <summary>
        /// Gets the title part of the categorization.
        /// </summary>
        [Description("The title of the sprite.")]
        [DisplayName("Title")]
        public SpriteTitle Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            return obj is SpriteCategorization && this == (SpriteCategorization)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_category.GetHashCode() * 397) ^ _title.GetHashCode();
            }
        }

        /// <summary>
        /// Splits apart the category and title from a single string.
        /// </summary>
        /// <param name="categoryAndTitle">The concatenated category and title.</param>
        /// <returns>The <see cref="SpriteCategorization"/> created from the split.</returns>
        public static SpriteCategorization SplitCategoryAndTitle(string categoryAndTitle)
        {
            SpriteCategory category;
            SpriteTitle title;
            SplitCategoryAndTitle(categoryAndTitle, out category, out title);

            return new SpriteCategorization(category, title);
        }

        /// <summary>
        /// Splits apart the category and title from a single string.
        /// </summary>
        /// <param name="categoryAndTitle">The concatenated category and title.</param>
        /// <param name="category">The resulting category</param>.
        /// <param name="title">The resulting title.</param>
        /// <exception cref="ArgumentNullException"><paramref name="categoryAndTitle"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="categoryAndTitle"/> is not property formatted.</exception>
        public static void SplitCategoryAndTitle(string categoryAndTitle, out SpriteCategory category, out SpriteTitle title)
        {
            if (string.IsNullOrEmpty(categoryAndTitle))
                throw new ArgumentNullException("categoryAndTitle");

            categoryAndTitle = categoryAndTitle.Replace("\\", Delimiter).Replace("/", Delimiter);

            var lastSep = categoryAndTitle.LastIndexOf(Delimiter, StringComparison.Ordinal);
            if (lastSep == -1)
                throw new ArgumentException("No delimiters found.", "categoryAndTitle");

            // Split at the last separator
            var categoryStr = categoryAndTitle.Substring(0, lastSep);
            var titleStr = categoryAndTitle.Substring(lastSep + 1);

            category = new SpriteCategory(categoryStr);
            title = new SpriteTitle(titleStr);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Category + Delimiter + Title;
        }

        #region IEquatable<SpriteCategorization> Members

        /// <summary>
        /// Checks if this <see cref="SpriteCategorization"/> is equal to another <see cref="SpriteCategorization"/>.
        /// </summary>
        /// <param name="other">The other <see cref="SpriteCategorization"/>.</param>
        /// <returns>True if the two are equal; otherwise false.</returns>
        public bool Equals(SpriteCategorization other)
        {
            if (other == null)
                return false;

            return Title.Equals(other.Title) && Category.Equals(other.Category);
        }

        #endregion

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SpriteCategorization a, SpriteCategorization b)
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
        public static bool operator !=(SpriteCategorization a, SpriteCategorization b)
        {
            return !(a == b);
        }
    }
}