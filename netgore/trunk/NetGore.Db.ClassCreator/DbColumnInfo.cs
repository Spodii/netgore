using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public class DbColumnInfo
    {
        public string Comment { get; set; }
        public string DatabaseType { get; set; }
        public object DefaultValue { get; set; }
        public DbColumnKeyType KeyType { get; set; }
        public string Name { get; set; }
        public bool Nullable { get; set; }
        public Type Type { get; set; }

        public DbColumnInfo(string name, string databaseType, Type type, bool nullable, object defaultValue, string comment,
                            DbColumnKeyType keyType)
        {
            Name = name;
            DatabaseType = databaseType;
            Type = type;
            Nullable = nullable;
            DefaultValue = defaultValue;
            Comment = comment;
            KeyType = keyType;
        }
    }
}