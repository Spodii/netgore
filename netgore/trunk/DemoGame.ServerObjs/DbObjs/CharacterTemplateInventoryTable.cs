using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_template_inventory`.
/// </summary>
public interface ICharacterTemplateInventoryTable
{
/// <summary>
/// Gets the value of the database column `chance`.
/// </summary>
System.UInt16 Chance
{
get;
}
/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.Server.CharacterID CharacterId
{
get;
}
/// <summary>
/// Gets the value of the database column `item_id`.
/// </summary>
DemoGame.Server.ItemID ItemId
{
get;
}
/// <summary>
/// Gets the value of the database column `max`.
/// </summary>
System.Byte Max
{
get;
}
/// <summary>
/// Gets the value of the database column `min`.
/// </summary>
System.Byte Min
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `character_template_inventory`.
/// </summary>
public class CharacterTemplateInventoryTable : ICharacterTemplateInventoryTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"chance", "character_id", "item_id", "max", "min" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] { };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"chance", "character_id", "item_id", "max", "min" };
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
public const System.String TableName = "character_template_inventory";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 5;
/// <summary>
/// The field that maps onto the database column `chance`.
/// </summary>
System.UInt16 _chance;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.UInt16 _characterId;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.UInt16 _itemId;
/// <summary>
/// The field that maps onto the database column `max`.
/// </summary>
System.Byte _max;
/// <summary>
/// The field that maps onto the database column `min`.
/// </summary>
System.Byte _min;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `chance`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Chance
{
get
{
return (System.UInt16)_chance;
}
set
{
this._chance = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.CharacterID CharacterId
{
get
{
return (DemoGame.Server.CharacterID)_characterId;
}
set
{
this._characterId = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemID ItemId
{
get
{
return (DemoGame.Server.ItemID)_itemId;
}
set
{
this._itemId = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Max
{
get
{
return (System.Byte)_max;
}
set
{
this._max = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `min`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Min
{
get
{
return (System.Byte)_min;
}
set
{
this._min = (System.Byte)value;
}
}

/// <summary>
/// CharacterTemplateInventoryTable constructor.
/// </summary>
public CharacterTemplateInventoryTable()
{
}
/// <summary>
/// CharacterTemplateInventoryTable constructor.
/// </summary>
/// <param name="chance">The initial value for the corresponding property.</param>
/// <param name="characterId">The initial value for the corresponding property.</param>
/// <param name="itemId">The initial value for the corresponding property.</param>
/// <param name="max">The initial value for the corresponding property.</param>
/// <param name="min">The initial value for the corresponding property.</param>
public CharacterTemplateInventoryTable(System.UInt16 @chance, DemoGame.Server.CharacterID @characterId, DemoGame.Server.ItemID @itemId, System.Byte @max, System.Byte @min)
{
Chance = (System.UInt16)@chance;
CharacterId = (DemoGame.Server.CharacterID)@characterId;
ItemId = (DemoGame.Server.ItemID)@itemId;
Max = (System.Byte)@max;
Min = (System.Byte)@min;
}
/// <summary>
/// CharacterTemplateInventoryTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public CharacterTemplateInventoryTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public CharacterTemplateInventoryTable(ICharacterTemplateInventoryTable source)
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

i = dataReader.GetOrdinal("chance");
Chance = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("character_id");
CharacterId = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("item_id");
ItemId = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("max");
Max = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("min");
Min = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
public static void CopyValues(ICharacterTemplateInventoryTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@chance"] = (System.UInt16)source.Chance;
dic["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
dic["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
dic["@max"] = (System.Byte)source.Max;
dic["@min"] = (System.Byte)source.Min;
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
public static void CopyValues(ICharacterTemplateInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@chance"] = (System.UInt16)source.Chance;
paramValues["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
paramValues["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
paramValues["@max"] = (System.Byte)source.Max;
paramValues["@min"] = (System.Byte)source.Min;
}

public void CopyValuesFrom(ICharacterTemplateInventoryTable source)
{
Chance = (System.UInt16)source.Chance;
CharacterId = (DemoGame.Server.CharacterID)source.CharacterId;
ItemId = (DemoGame.Server.ItemID)source.ItemId;
Max = (System.Byte)source.Max;
Min = (System.Byte)source.Min;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "chance":
return Chance;

case "character_id":
return CharacterId;

case "item_id":
return ItemId;

case "max":
return Max;

case "min":
return Min;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "chance":
Chance = (System.UInt16)value;
break;

case "character_id":
CharacterId = (DemoGame.Server.CharacterID)value;
break;

case "item_id":
ItemId = (DemoGame.Server.ItemID)value;
break;

case "max":
Max = (System.Byte)value;
break;

case "min":
Min = (System.Byte)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "chance":
return new ColumnMetadata("chance", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "character_id":
return new ColumnMetadata("character_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "item_id":
return new ColumnMetadata("item_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "max":
return new ColumnMetadata("max", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "min":
return new ColumnMetadata("min", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

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
case "chance":
Chance = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "character_id":
CharacterId = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetUInt16(i);
break;


case "item_id":
ItemId = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetUInt16(i);
break;


case "max":
Max = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "min":
Min = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
public static void TryCopyValues(ICharacterTemplateInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@chance":
paramValues[i] = source.Chance;
break;


case "@character_id":
paramValues[i] = source.CharacterId;
break;


case "@item_id":
paramValues[i] = source.ItemId;
break;


case "@max":
paramValues[i] = source.Max;
break;


case "@min":
paramValues[i] = source.Min;
break;


}

}
}

}

}
