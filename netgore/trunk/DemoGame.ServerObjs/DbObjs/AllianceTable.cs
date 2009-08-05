using System;
using System.Linq;
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
System.Byte Id
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
return _dbColumns;
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
public System.Byte Id
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
public AllianceTable(System.Byte @id, System.String @name)
{
this.Id = @id;
this.Name = @name;
}
/// <summary>
/// AllianceTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public AllianceTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
this.Id = (System.Byte)dataReader.GetValue(dataReader.GetOrdinal("id"));
this.Name = (System.String)dataReader.GetValue(dataReader.GetOrdinal("name"));
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
dic["@id"] = (System.Byte)source.Id;
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
paramValues["@id"] = (System.Byte)source.Id;
paramValues["@name"] = (System.String)source.Name;
}

}

}
