using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `map_spawn`.
/// </summary>
public class MapSpawnTable : IMapSpawnTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"amount", "character_template_id", "height", "id", "map_id", "width", "x", "y" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
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
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"amount", "character_template_id", "height", "map_id", "width", "x", "y" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
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
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.UInt16 _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `height`.
/// </summary>
System.Nullable<System.UInt16> _height;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapID;
/// <summary>
/// The field that maps onto the database column `width`.
/// </summary>
System.Nullable<System.UInt16> _width;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Nullable<System.UInt16> _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Nullable<System.UInt16> _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Amount
{
get
{
return (System.Byte)_amount;
}
set
{
this._amount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.CharacterTemplateID CharacterTemplateID
{
get
{
return (DemoGame.Server.CharacterTemplateID)_characterTemplateID;
}
set
{
this._characterTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `height`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<System.UInt16> Height
{
get
{
return (System.Nullable<System.UInt16>)_height;
}
set
{
this._height = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.MapSpawnValuesID ID
{
get
{
return (DemoGame.Server.MapSpawnValuesID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public NetGore.MapIndex MapID
{
get
{
return (NetGore.MapIndex)_mapID;
}
set
{
this._mapID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `width`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<System.UInt16> Width
{
get
{
return (System.Nullable<System.UInt16>)_width;
}
set
{
this._width = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<System.UInt16> X
{
get
{
return (System.Nullable<System.UInt16>)_x;
}
set
{
this._x = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `y`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<System.UInt16> Y
{
get
{
return (System.Nullable<System.UInt16>)_y;
}
set
{
this._y = (System.Nullable<System.UInt16>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IMapSpawnTable DeepCopy()
{
return new MapSpawnTable(this);
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
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="width">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public MapSpawnTable(System.Byte @amount, DemoGame.Server.CharacterTemplateID @characterTemplateID, System.Nullable<System.UInt16> @height, DemoGame.Server.MapSpawnValuesID @iD, NetGore.MapIndex @mapID, System.Nullable<System.UInt16> @width, System.Nullable<System.UInt16> @x, System.Nullable<System.UInt16> @y)
{
Amount = (System.Byte)@amount;
CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)@characterTemplateID;
Height = (System.Nullable<System.UInt16>)@height;
ID = (DemoGame.Server.MapSpawnValuesID)@iD;
MapID = (NetGore.MapIndex)@mapID;
Width = (System.Nullable<System.UInt16>)@width;
X = (System.Nullable<System.UInt16>)@x;
Y = (System.Nullable<System.UInt16>)@y;
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
System.Int32 i;

i = dataReader.GetOrdinal("amount");
Amount = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("character_template_id");
CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)(DemoGame.Server.CharacterTemplateID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("height");
Height = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("id");
ID = (DemoGame.Server.MapSpawnValuesID)(DemoGame.Server.MapSpawnValuesID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("map_id");
MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("width");
Width = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("x");
X = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("y");
Y = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
}

/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IMapSpawnTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@amount"] = (System.Byte)source.Amount;
dic["@character_template_id"] = (DemoGame.Server.CharacterTemplateID)source.CharacterTemplateID;
dic["@height"] = (System.Nullable<System.UInt16>)source.Height;
dic["@id"] = (DemoGame.Server.MapSpawnValuesID)source.ID;
dic["@map_id"] = (NetGore.MapIndex)source.MapID;
dic["@width"] = (System.Nullable<System.UInt16>)source.Width;
dic["@x"] = (System.Nullable<System.UInt16>)source.X;
dic["@y"] = (System.Nullable<System.UInt16>)source.Y;
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
paramValues["@character_template_id"] = (DemoGame.Server.CharacterTemplateID)source.CharacterTemplateID;
paramValues["@height"] = (System.Nullable<System.UInt16>)source.Height;
paramValues["@id"] = (DemoGame.Server.MapSpawnValuesID)source.ID;
paramValues["@map_id"] = (NetGore.MapIndex)source.MapID;
paramValues["@width"] = (System.Nullable<System.UInt16>)source.Width;
paramValues["@x"] = (System.Nullable<System.UInt16>)source.X;
paramValues["@y"] = (System.Nullable<System.UInt16>)source.Y;
}

public void CopyValuesFrom(IMapSpawnTable source)
{
Amount = (System.Byte)source.Amount;
CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)source.CharacterTemplateID;
Height = (System.Nullable<System.UInt16>)source.Height;
ID = (DemoGame.Server.MapSpawnValuesID)source.ID;
MapID = (NetGore.MapIndex)source.MapID;
Width = (System.Nullable<System.UInt16>)source.Width;
X = (System.Nullable<System.UInt16>)source.X;
Y = (System.Nullable<System.UInt16>)source.Y;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "amount":
return Amount;

case "character_template_id":
return CharacterTemplateID;

case "height":
return Height;

case "id":
return ID;

case "map_id":
return MapID;

case "width":
return Width;

case "x":
return X;

case "y":
return Y;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "amount":
Amount = (System.Byte)value;
break;

case "character_template_id":
CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)value;
break;

case "height":
Height = (System.Nullable<System.UInt16>)value;
break;

case "id":
ID = (DemoGame.Server.MapSpawnValuesID)value;
break;

case "map_id":
MapID = (NetGore.MapIndex)value;
break;

case "width":
Width = (System.Nullable<System.UInt16>)value;
break;

case "x":
X = (System.Nullable<System.UInt16>)value;
break;

case "y":
Y = (System.Nullable<System.UInt16>)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "amount":
return new ColumnMetadata("amount", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "height":
return new ColumnMetadata("height", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "map_id":
return new ColumnMetadata("map_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "width":
return new ColumnMetadata("width", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "x":
return new ColumnMetadata("x", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "y":
return new ColumnMetadata("y", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void TryReadValues(System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "amount":
Amount = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "character_template_id":
CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)(DemoGame.Server.CharacterTemplateID)dataReader.GetUInt16(i);
break;


case "height":
Height = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "id":
ID = (DemoGame.Server.MapSpawnValuesID)(DemoGame.Server.MapSpawnValuesID)dataReader.GetInt32(i);
break;


case "map_id":
MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);
break;


case "width":
Width = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "x":
X = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "y":
Y = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


}

}
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The key must already exist in the DbParameterValues
/// for the value to be copied over. If any of the keys in the DbParameterValues do not
/// match one of the column names, or if there is no field for a key, then it will be
/// ignored. Because of this, it is important to be careful when using this method
/// since columns or keys can be skipped without any indication.
/// </summary>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public void TryCopyValues(NetGore.Db.DbParameterValues paramValues)
{
TryCopyValues(this, paramValues);
}
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The key must already exist in the DbParameterValues
/// for the value to be copied over. If any of the keys in the DbParameterValues do not
/// match one of the column names, or if there is no field for a key, then it will be
/// ignored. Because of this, it is important to be careful when using this method
/// since columns or keys can be skipped without any indication.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(IMapSpawnTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@amount":
paramValues[i] = source.Amount;
break;


case "@character_template_id":
paramValues[i] = source.CharacterTemplateID;
break;


case "@height":
paramValues[i] = source.Height;
break;


case "@id":
paramValues[i] = source.ID;
break;


case "@map_id":
paramValues[i] = source.MapID;
break;


case "@width":
paramValues[i] = source.Width;
break;


case "@x":
paramValues[i] = source.X;
break;


case "@y":
paramValues[i] = source.Y;
break;


}

}
}

}

}
