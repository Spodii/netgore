using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public class ColumnCollection
    {
        static readonly char[] _vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

        public string CollectionPropertyName
        {
            get
            {
                if (Name.EndsWith("s"))
                    return Name + "es";
                else if (Name.Length > 1 && Name.EndsWith("y"))
                {
                    if (!_vowels.Contains(Name[Name.Length - 2]))
                        return Name.Substring(0, Name.Length - 1) + "ies";
                    else
                        return Name + "s";
                }
                else
                    return Name + "s";
            }
        }

        public IEnumerable<ColumnCollectionItem> Columns { get; private set; }
        public Type KeyType { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<string> Tables { get; private set; }
        public Type ValueType { get; private set; }

        public ColumnCollection(string name, Type keyType, Type valueType, IEnumerable<string> tables,
                                IEnumerable<ColumnCollectionItem> columns)
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