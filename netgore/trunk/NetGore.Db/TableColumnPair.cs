using System.Linq;
using NetGore;

namespace NetGore.Db
{
    /// <summary>
    /// Contains the name of a database column and the table it belongs to.
    /// </summary>
    public struct TableColumnPair
    {
        /// <summary>
        /// The name of the database column.
        /// </summary>
        public readonly string Column;

        /// <summary>
        /// The name of the table the <see cref="Column"/> belongs to.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnPair"/> struct.
        /// </summary>
        /// <param name="table">The name of the table the <paramref name="column"/> belongs to.</param>
        /// <param name="column">The name of the database column.</param>
        public TableColumnPair(string table, string column)
        {
            Table = table;
            Column = column;
        }
    }
}