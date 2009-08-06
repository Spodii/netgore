using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `map`.
/// </summary>
public interface IMapTable
{
/// <summary>
/// Gets the value for the database column `id`.
/// </summary>
System.UInt16 Id
{
get;
}
/// <summary>
/// Gets the value for the database column `name`.
/// </summary>
System.String Name
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `map`.
/// </summary>
public class MapTable : IMapTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"id", "name" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return _dbColumns;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "map";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _id;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Id
{
get
{
return _id;
}
set
{
this._id = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(255)`.
/// </summary>
public System.String Name
{
get
{
return _name;
}
set
{
this._name = value;
}
}

/// <summary>
/// MapTable constructor.
/// </summary>
public MapTable()
{
}
/// <summary>
/// MapTable constructor.
/// </summary>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
public MapTable(System.UInt16 @id, System.String @name)
{
Id = (System.UInt16)@id;
Name = (System.String)@name;
}
/// <summary>
/// MapTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public MapTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public MapTable(IMapTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
Id = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
Name = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
}

/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IMapTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@id"] = (System.UInt16)source.Id;
dic["@name"] = (System.String)source.Name;
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public void CopyValues(NetGore.Db.DbParameterValues paramValues)
{
CopyValues(this, paramValues);
}
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(IMapTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@id"] = (System.UInt16)source.Id;
paramValues["@name"] = (System.String)source.Name;
}

public void CopyValuesFrom(IMapTable source)
{
Id = (System.UInt16)source.Id;
Name = (System.String)source.Name;
}

}

}
