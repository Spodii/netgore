/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

using System;
namespace DemoGame.DbObjs
{
/// <summary>
/// Contains the metadata for a database column.
/// </summary>
public class ColumnMetadata
{
    /// <summary>
    /// The comment of the column from the database.
    /// </summary>
    readonly string _comment;

    /// <summary>
    /// A string containing the database column type.
    /// </summary>
    readonly string _databaseType;

    /// <summary>
    /// The default value of the database column. Can be null.
    /// </summary>
    readonly object _defaultValue;

    /// <summary>
    /// If this database column is a foreign key.
    /// </summary>
    readonly bool _isForeignKey;

    /// <summary>
    /// If this database column is a primary key.
    /// </summary>
    readonly bool _isPrimaryKey;

    /// <summary>
    /// The name of the column.
    /// </summary>
    readonly string _name;

    /// <summary>
    /// If this database column's value can be null.
    /// </summary>
    readonly bool _nullable;

    /// <summary>
    /// The system Type used with this database column.
    /// </summary>
    readonly Type _type;

    /// <summary>
    /// Gets the database comment of the column from the database.
    /// </summary>
    public string Comment
    {
        get { return _comment; }
    }

    /// <summary>
    /// Gets a string containing the database column type.
    /// </summary>
    public string DatabaseType
    {
        get { return _databaseType; }
    }

    /// <summary>
    /// Gets the default value of the database column. Can be null.
    /// </summary>
    public object DefaultValue
    {
        get { return _defaultValue; }
    }

    /// <summary>
    /// Gets if this database column is a foreign key.
    /// </summary>
    public bool IsForeignKey
    {
        get { return _isForeignKey; }
    }

    /// <summary>
    /// Gets if this database column is a primary key.
    /// </summary>
    public bool IsPrimaryKey
    {
        get { return _isPrimaryKey; }
    }

    /// <summary>
    /// Gets the name of the database column.
    /// </summary>
    public string Name
    {
        get { return _name; }
    }

    /// <summary>
    /// Gets if this database column's value can be null.
    /// </summary>
    public bool Nullable
    {
        get { return _nullable; }
    }

    /// <summary>
    /// Gets the system Type used with this database column.
    /// </summary>
    public Type Type
    {
        get { return _type; }
    }

    /// <summary>
    /// ColumnMetadata constructor.
    /// </summary>
    /// <param name="name">A string containing the database column type.</param>
    /// <param name="comment">The comment of the column from the database.</param>
    /// <param name="databaseType">A string containing the database column type.</param>
    /// <param name="defaultValue">The default value of the database column. Can be null.</param>
    /// <param name="type">The system Type used with this database column.</param>
    /// <param name="nullable">If this database column's value can be null.</param>
    /// <param name="isPrimaryKey">If this database column is a primary key.</param>
    /// <param name="isForeignKey">If this database column is a foreign key.</param>
    public ColumnMetadata(string name, string comment, string databaseType, object defaultValue, Type type, bool nullable,
                          bool isPrimaryKey, bool isForeignKey)
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
