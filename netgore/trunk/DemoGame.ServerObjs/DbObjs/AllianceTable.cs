using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `alliance`.
/// </summary>
public interface IAllianceTable
{
/// <summary>
/// Gets the value for the database column `id`.
/// </summary>
DemoGame.Server.AllianceID Id
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
/// Provides a strongly-typed structure for the database table `alliance`.
/// </summary>
public class AllianceTable : IAllianceTable
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
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"id" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"name" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "alliance";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Byte _id;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID Id
{
get
{
return (DemoGame.Server.AllianceID)_id;
}
set
{
this._id = (System.Byte)value;
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
return (System.String)_name;
}
set
{
this._name = (System.String)value;
}
}

/// <summary>
/// AllianceTable constructor.
/// </summary>
public AllianceTable()
{
}
/// <summary>
/// AllianceTable constructor.
/// </summary>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
public AllianceTable(DemoGame.Server.AllianceID @id, System.String @name)
{
Id = (DemoGame.Server.AllianceID)@id;
Name = (System.String)@name;
}
/// <summary>
/// AllianceTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public AllianceTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public AllianceTable(IAllianceTable source)
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
Id = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(dataReader.GetOrdinal("id"));
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
public static void CopyValues(IAllianceTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@id"] = (DemoGame.Server.AllianceID)source.Id;
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
public static void CopyValues(IAllianceTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@id"] = (DemoGame.Server.AllianceID)source.Id;
paramValues["@name"] = (System.String)source.Name;
}

public void CopyValuesFrom(IAllianceTable source)
{
Id = (DemoGame.Server.AllianceID)source.Id;
Name = (System.String)source.Name;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "id":
return Id;
case "name":
return Name;
default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "id":
Id = (DemoGame.Server.AllianceID)value;
break;
case "name":
Name = (System.String)value;
break;
default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "id":
return new ColumnMetadata("id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);
case "name":
return new ColumnMetadata("name", "", "varchar(255)", null, typeof(System.String), false, false, false);
default:
throw new ArgumentException("Field not found.","fieldName");
}
}

}

}
