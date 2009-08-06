using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `map_spawn`.
/// </summary>
public interface IMapSpawnTable
{
/// <summary>
/// Gets the value for the database column `amount`.
/// </summary>
System.Byte Amount
{
get;
}
/// <summary>
/// Gets the value for the database column `character_id`.
/// </summary>
System.UInt16 CharacterId
{
get;
}
/// <summary>
/// Gets the value for the database column `height`.
/// </summary>
System.UInt16 Height
{
get;
}
/// <summary>
/// Gets the value for the database column `id`.
/// </summary>
System.Int32 Id
{
get;
}
/// <summary>
/// Gets the value for the database column `map_id`.
/// </summary>
System.UInt16 MapId
{
get;
}
/// <summary>
/// Gets the value for the database column `width`.
/// </summary>
System.UInt16 Width
{
get;
}
/// <summary>
/// Gets the value for the database column `x`.
/// </summary>
System.UInt16 X
{
get;
}
/// <summary>
/// Gets the value for the database column `y`.
/// </summary>
System.UInt16 Y
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `map_spawn`.
/// </summary>
public class MapSpawnTable : IMapSpawnTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"amount", "character_id", "height", "id", "map_id", "width", "x", "y" };
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
public const System.String TableName = "map_spawn";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 8;
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.UInt16 _characterId;
/// <summary>
/// The field that maps onto the database column `height`.
/// </summary>
System.UInt16 _height;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _id;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapId;
/// <summary>
/// The field that maps onto the database column `width`.
/// </summary>
System.UInt16 _width;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.UInt16 _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.UInt16 _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Amount
{
get
{
return _amount;
}
set
{
this._amount = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 CharacterId
{
get
{
return _characterId;
}
set
{
this._characterId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `height`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Height
{
get
{
return _height;
}
set
{
this._height = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.
/// </summary>
public System.Int32 Id
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
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 MapId
{
get
{
return _mapId;
}
set
{
this._mapId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `width`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Width
{
get
{
return _width;
}
set
{
this._width = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 X
{
get
{
return _x;
}
set
{
this._x = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `y`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Y
{
get
{
return _y;
}
set
{
this._y = value;
}
}

/// <summary>
/// MapSpawnTable constructor.
/// </summary>
public MapSpawnTable()
{
}
/// <summary>
/// MapSpawnTable constructor.
/// </summary>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="characterId">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="mapId">The initial value for the corresponding property.</param>
/// <param name="width">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public MapSpawnTable(System.Byte @amount, System.UInt16 @characterId, System.UInt16 @height, System.Int32 @id, System.UInt16 @mapId, System.UInt16 @width, System.UInt16 @x, System.UInt16 @y)
{
this.Amount = @amount;
this.CharacterId = @characterId;
this.Height = @height;
this.Id = @id;
this.MapId = @mapId;
this.Width = @width;
this.X = @x;
this.Y = @y;
}
/// <summary>
/// MapSpawnTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public MapSpawnTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public MapSpawnTable(IMapSpawnTable source)
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
this.Amount = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("amount"));
this.CharacterId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("character_id"));
this.Height = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("height"));
this.Id = (System.Int32)dataReader.GetInt32(dataReader.GetOrdinal("id"));
this.MapId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("map_id"));
this.Width = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("width"));
this.X = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("x"));
this.Y = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("y"));
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
public static void CopyValues(IMapSpawnTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@amount"] = (System.Byte)source.Amount;
dic["@character_id"] = (System.UInt16)source.CharacterId;
dic["@height"] = (System.UInt16)source.Height;
dic["@id"] = (System.Int32)source.Id;
dic["@map_id"] = (System.UInt16)source.MapId;
dic["@width"] = (System.UInt16)source.Width;
dic["@x"] = (System.UInt16)source.X;
dic["@y"] = (System.UInt16)source.Y;
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
public static void CopyValues(IMapSpawnTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@amount"] = (System.Byte)source.Amount;
paramValues["@character_id"] = (System.UInt16)source.CharacterId;
paramValues["@height"] = (System.UInt16)source.Height;
paramValues["@id"] = (System.Int32)source.Id;
paramValues["@map_id"] = (System.UInt16)source.MapId;
paramValues["@width"] = (System.UInt16)source.Width;
paramValues["@x"] = (System.UInt16)source.X;
paramValues["@y"] = (System.UInt16)source.Y;
}

public void CopyValuesFrom(IMapSpawnTable source)
{
this.Amount = (System.Byte)source.Amount;
this.CharacterId = (System.UInt16)source.CharacterId;
this.Height = (System.UInt16)source.Height;
this.Id = (System.Int32)source.Id;
this.MapId = (System.UInt16)source.MapId;
this.Width = (System.UInt16)source.Width;
this.X = (System.UInt16)source.X;
this.Y = (System.UInt16)source.Y;
}

}

}
