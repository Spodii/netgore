using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Contains the database schema, table, and column for a fully qualified column reference.
    /// </summary>
    public struct SchemaTableColumn
    {
        readonly string _schema;
        readonly string _column;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaTableColumn"/> struct.
        /// </summary>
        /// <param name="schema">The name of the schema that the <see cref="_table"/> belongs to.</param>
        /// <param name="table">The name of the table the <paramref name="column"/> belongs to.</param>
        /// <param name="column">The name of the database column.</param>
        public SchemaTableColumn(string schema, string table, string column)
        {
            _schema = schema;
            _table = table;
            _column = column;
        }

        /// <summary>
        /// Gets the name of the database column.
        /// </summary>
        public string Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the name of the table the <see cref="_column"/> belongs to.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the name of the schema that the <see cref="_table"/> belongs to.
        /// </summary>
        public string Schema
        {
            get { return _schema; }
        }
    }
}