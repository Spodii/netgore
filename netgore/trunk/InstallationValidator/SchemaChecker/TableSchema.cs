using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.IO;

namespace InstallationValidator.SchemaChecker
{
    [Serializable]
    public class TableSchema
    {
        readonly IEnumerable<ColumnSchema> _columns;
        readonly string _tableName;

        public IEnumerable<ColumnSchema> Columns
        {
            get { return _columns; }
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public TableSchema(string tableName, IEnumerable<ColumnSchema> columns)
        {
            _tableName = tableName;
            _columns = columns;
        }

        public TableSchema(IValueReader reader)
        {
            _tableName = reader.ReadString(_tableNameValueKey);
            _columns = reader.ReadManyNodes(_columnsNodeName, r => new ColumnSchema(r)).ToCompact();
        }

        const string _columnsNodeName = "Columns";
        const string _tableNameValueKey = "TableName";

        public void Write(IValueWriter writer)
        {
            writer.Write(_tableNameValueKey, _tableName);
            writer.WriteManyNodes(_columnsNodeName, _columns.ToArray(), (w, x) => x.Write(w));
        }
    }
}