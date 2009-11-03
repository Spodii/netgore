using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}