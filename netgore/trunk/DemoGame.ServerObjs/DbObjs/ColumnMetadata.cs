using System;
namespace DemoGame.Server.DbObjs
{
public class ColumnMetadata
{
    readonly string _name;
    readonly string _comment;
    readonly string _databaseType;
    readonly object _defaultValue;
    readonly Type _type;
    readonly bool _nullable;
    readonly bool _isPrimaryKey;
    readonly bool _isForeignKey;

    public string Name { get { return _name; } }

    public string Comment { get { return _comment; } }

    public string DatabaseType { get { return _databaseType; } }

    public object DefaultValue { get { return _defaultValue; } }

    public Type Type { get { return _type; } }

    public bool Nullable { get { return _nullable; } }

    public bool IsPrimaryKey { get { return _isPrimaryKey; } }

    public bool IsForeignKey { get { return _isForeignKey; } }

    public ColumnMetadata(string name, string comment, string databaseType, object defaultValue, Type type, bool nullable, bool isPrimaryKey, bool isForeignKey)
    {
        _name = name;
        _comment = comment;
        _databaseType = databaseType;
        _defaultValue = defaultValue;
        _type = type;
        _nullable = nullable;
        _isPrimaryKey = isPrimaryKey;
        _isForeignKey = isForeignKey;
    }
}
}
