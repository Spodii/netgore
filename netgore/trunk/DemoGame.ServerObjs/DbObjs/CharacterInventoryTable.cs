using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `character_inventory`.
/// </summary>
public class CharacterInventoryTable : ICharacterInventoryTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "item_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id", "item_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] { };
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
public const System.String TableName = "character_inventory";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.Int32 _itemID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.CharacterID CharacterID
{
get
{
return (DemoGame.Server.CharacterID)_characterID;
}
set
{
this._characterID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.ItemID ItemID
{
get
{
return (DemoGame.Server.ItemID)_itemID;
}
set
{
this._itemID = (System.Int32)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public ICharacterInventoryTable DeepCopy()
{
return new CharacterInventoryTable(this);
}
/// <summary>
/// CharacterInventoryTable constructor.
/// </summary>
public CharacterInventoryTable()
{
}
/// <summary>
/// CharacterInventoryTable constructor.
/// </summary>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="itemID">The initial value for the corresponding property.</param>
public CharacterInventoryTable(DemoGame.Server.CharacterID @characterID, DemoGame.Server.ItemID @itemID)
{
CharacterID = (DemoGame.Server.CharacterID)@characterID;
ItemID = (DemoGame.Server.ItemID)@itemID;
}
/// <summary>
/// CharacterInventoryTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public CharacterInventoryTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public CharacterInventoryTable(ICharacterInventoryTable source)
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

i = dataReader.GetOrdinal("character_id");
CharacterID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("item_id");
ItemID = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(i);
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
public static void CopyValues(ICharacterInventoryTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterID;
dic["@item_id"] = (DemoGame.Server.ItemID)source.ItemID;
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
public static void CopyValues(ICharacterInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterID;
paramValues["@item_id"] = (DemoGame.Server.ItemID)source.ItemID;
}

public void CopyValuesFrom(ICharacterInventoryTable source)
{
CharacterID = (DemoGame.Server.CharacterID)source.CharacterID;
ItemID = (DemoGame.Server.ItemID)source.ItemID;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "character_id":
return CharacterID;

case "item_id":
return ItemID;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "character_id":
CharacterID = (DemoGame.Server.CharacterID)value;
break;

case "item_id":
ItemID = (DemoGame.Server.ItemID)value;
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
case "character_id":
CharacterID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);
break;


case "item_id":
ItemID = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(i);
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
public static void TryCopyValues(ICharacterInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@character_id":
paramValues[i] = source.CharacterID;
break;


case "@item_id":
paramValues[i] = source.ItemID;
break;


}

}
}

}

}
