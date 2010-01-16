using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Db.Schema
{
    /// <summary>
    /// Describes the schema for a database table.
    /// </summary>
    [Serializable]
    public class TableSchema
    {
        const string _columnsNodeName = "Columns";
        const string _tableNameValueKey = "TableName";

        readonly IEnumerable<ColumnSchema> _columns;
        readonly string _tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSchema"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The column schemas.</param>
        public TableSchema(string tableName, IEnumerable<ColumnSchema> columns)
        {
            _tableName = tableName;
            _columns = columns;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSchema"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public TableSchema(IValueReader reader)
        {
            _tableName = reader.ReadString(_tableNameValueKey);
            _columns = reader.ReadManyNodes(_columnsNodeName, r => new ColumnSchema(r)).ToCompact();
        }

        /// <summary>
        /// Gets the schema for the columns in the table.
        /// </summary>
        public IEnumerable<ColumnSchema> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// Writes the <see cref="TableSchema"/> the specified writer.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write(_tableNameValueKey, _tableName);
            writer.WriteManyNodes(_columnsNodeName, _columns.ToArray(), (w, x) => x.Write(w));
        }
    }
}