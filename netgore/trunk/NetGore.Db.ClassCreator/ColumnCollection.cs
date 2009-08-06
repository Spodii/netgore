using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    public class ColumnCollection
    {
        public string Name { get; private set; }
        public Type KeyType { get; private set; }
        public Type ValueType { get; private set; }
        public IEnumerable<string> Tables { get; private set; }
        public IEnumerable<ColumnCollectionItem> Columns { get; private set; }

        public ColumnCollection(string name, Type keyType, Type valueType, IEnumerable<string> tables, IEnumerable<ColumnCollectionItem> columns)
        {
            Name = name;
            KeyType = keyType;
            ValueType = valueType;
            Tables = tables;
            Columns = columns;
        }
    }

    public struct ColumnCollectionItem
    {
        public readonly string ColumnName;
        public readonly string Key;

        public ColumnCollectionItem(string columnName, string key)
        {
            ColumnName = columnName;
            Key = key;
        }

        public static ColumnCollectionItem FromEnum<T>(string columnName, T key)
        {
            return new ColumnCollectionItem(columnName, typeof(T).FullName + "." + key);
        }
    }
}
