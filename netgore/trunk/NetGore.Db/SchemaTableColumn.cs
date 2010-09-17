namespace NetGore.Db
{
    /// <summary>
    /// Contains the database schema, table, and column for a fully qualified column reference.
    /// </summary>
    public struct SchemaTableColumn
    {
        /// <summary>
        /// The name of the schema that the <see cref="Table"/> belongs to.
        /// </summary>
        public readonly string Schema;

        /// <summary>
        /// The name of the database column.
        /// </summary>
        public readonly string Column;

        /// <summary>
        /// The name of the table the <see cref="Column"/> belongs to.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaTableColumn"/> struct.
        /// </summary>
        /// <param name="schema">The name of the schema that the <see cref="Table"/> belongs to.</param>
        /// <param name="table">The name of the table the <paramref name="column"/> belongs to.</param>
        /// <param name="column">The name of the database column.</param>
        public SchemaTableColumn(string schema, string table, string column)
        {
            Schema = schema;
            Table = table;
            Column = column;
        }
    }
}