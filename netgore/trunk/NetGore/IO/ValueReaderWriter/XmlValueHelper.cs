using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Helper class for the <see cref="XmlValueWriter"/> and <see cref="XmlValueReader"/>.
    /// </summary>
    static class XmlValueHelper
    {
        /// <summary>
        /// The key name used for the Count value when writing a collection.
        /// </summary>
        public const string CountValueKey = "Count";

        /// <summary>
        /// The prefix used for items in a collection.
        /// </summary>
        public const string ItemKeyPrefix = "Item";

        /// <summary>
        /// Gets the key to use for an item in a collection.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>The key to use for an item in a collection.</returns>
        public static string GetItemKey(int index)
        {
            return ItemKeyPrefix + Parser.Invariant.ToString(index);
        }
    }
}