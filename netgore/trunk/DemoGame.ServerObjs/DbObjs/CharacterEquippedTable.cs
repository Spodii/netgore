using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_equipped`.
/// </summary>
public interface ICharacterEquippedTable
{
/// <summary>
/// Gets the value for the database column `character_id`.
/// </summary>
DemoGame.Server.CharacterID CharacterId
{
get;
}
/// <summary>
/// Gets the value for the database column `item_id`.
/// </summary>
DemoGame.Server.ItemID ItemId
{
get;
}
/// <summary>
/// Gets the value for the database column `slot`.
/// </summary>
System.Byte Slot
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `character_equipped`.
/// </summary>
public class CharacterEquippedTable : ICharacterEquippedTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "item_id", "slot" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id", "item_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"slot" };
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
public const System.String TableName = "character_equipped";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterId;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.Int32 _itemId;
/// <summary>
/// The field that maps onto the database column `slot`.
/// </summary>
System.Byte _slot;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.CharacterID CharacterId
{
get
{
return (DemoGame.Server.CharacterID)_characterId;
}
set
{
this._characterId = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.ItemID ItemId
{
get
{
return (DemoGame.Server.ItemID)_itemId;
}
set
{
this._itemId = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `slot`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Slot
{
get
{
return (System.Byte)_slot;
}
set
{
this._slot = (System.Byte)value;
}
}

/// <summary>
/// CharacterEquippedTable constructor.
/// </summary>
public CharacterEquippedTable()
{
}
/// <summary>
/// CharacterEquippedTable constructor.
/// </summary>
/// <param name="characterId">The initial value for the corresponding property.</param>
/// <param name="itemId">The initial value for the corresponding property.</param>
/// <param name="slot">The initial value for the corresponding property.</param>
public CharacterEquippedTable(DemoGame.Server.CharacterID @characterId, DemoGame.Server.ItemID @itemId, System.Byte @slot)
{
CharacterId = (DemoGame.Server.CharacterID)@characterId;
ItemId = (DemoGame.Server.ItemID)@itemId;
Slot = (System.Byte)@slot;
}
/// <summary>
/// CharacterEquippedTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public CharacterEquippedTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public CharacterEquippedTable(ICharacterEquippedTable source)
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
CharacterId = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(dataReader.GetOrdinal("character_id"));
ItemId = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(dataReader.GetOrdinal("item_id"));
Slot = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("slot"));
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
public static void CopyValues(ICharacterEquippedTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
dic["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
dic["@slot"] = (System.Byte)source.Slot;
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
public static void CopyValues(ICharacterEquippedTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
paramValues["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
paramValues["@slot"] = (System.Byte)source.Slot;
}

public void CopyValuesFrom(ICharacterEquippedTable source)
{
CharacterId = (DemoGame.Server.CharacterID)source.CharacterId;
ItemId = (DemoGame.Server.ItemID)source.ItemId;
Slot = (System.Byte)source.Slot;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "character_id":
return CharacterId;
case "item_id":
return ItemId;
case "slot":
return Slot;
default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "character_id":
CharacterId = (DemoGame.Server.CharacterID)value;
break;
case "item_id":
ItemId = (DemoGame.Server.ItemID)value;
break;
case "slot":
Slot = (System.Byte)value;
break;
default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "character_id":
return new ColumnMetadata("character_id", "", "int(11)", null, typeof(System.Int32), false, true, false);
case "item_id":
return new ColumnMetadata("item_id", "", "int(11)", null, typeof(System.Int32), false, true, false);
case "slot":
return new ColumnMetadata("slot", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);
default:
throw new ArgumentException("Field not found.","fieldName");
}
}

}

}
