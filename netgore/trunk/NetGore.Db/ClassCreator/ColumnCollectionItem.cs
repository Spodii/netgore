using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// An item for the <see cref="ColumnCollection"/>.
    /// </summary>
    public struct ColumnCollectionItem
    {
        /// <summary>
        /// The name of the column.
        /// </summary>
        public readonly string ColumnName;

        /// <summary>
        /// The key for this column.
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnCollectionItem"/> struct.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="key">The key.</param>
        public ColumnCollectionItem(string columnName, string key)
        {
            ColumnName = columnName;
            Key = key;
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
            return new ColumnCollectionItem(columnName, formatter.GetTypeString(typeof(T)) + "." + key);
        }
    }
}