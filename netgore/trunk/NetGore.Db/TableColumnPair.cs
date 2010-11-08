using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Contains the name of a database column and the table it belongs to.
    /// </summary>
    public struct TableColumnPair
    {
        readonly string _column;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnPair"/> struct.
        /// </summary>
        /// <param name="table">The name of the table the <paramref name="column"/> belongs to.</param>
        /// <param name="column">The name of the database column.</param>
        public TableColumnPair(string table, string column)
        {
            _table = table;
            _column = column;
        }

        /// <summary>
        /// Gets the name of the table the <see cref="_column"/> belongs to.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the name of the database column.
        /// </summary>
        public string Column
        {
            get { return _column; }
        }
    }
}