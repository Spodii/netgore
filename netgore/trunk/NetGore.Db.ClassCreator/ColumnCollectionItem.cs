using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public struct ColumnCollectionItem
    {
        /// <summary>
        /// The column name.
        /// </summary>
        public readonly string ColumnName;

        /// <summary>
        /// The key.
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

        public static ColumnCollectionItem FromEnum<T>(CodeFormatter formatter, string columnName, T key)
        {
            return new ColumnCollectionItem(columnName, formatter.GetTypeString(typeof(T)) + "." + key);
        }
    }
}