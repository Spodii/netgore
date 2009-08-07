using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_inventory`.
/// </summary>
public interface ICharacterInventoryTable
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
}

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
public System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
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
System.Int32 _characterId;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.Int32 _itemId;
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
/// CharacterInventoryTable constructor.
/// </summary>
public CharacterInventoryTable()
{
}
/// <summary>
/// CharacterInventoryTable constructor.
/// </summary>
/// <param name="characterId">The initial value for the corresponding property.</param>
/// <param name="itemId">The initial value for the corresponding property.</param>
public CharacterInventoryTable(DemoGame.Server.CharacterID @characterId, DemoGame.Server.ItemID @itemId)
{
CharacterId = (DemoGame.Server.CharacterID)@characterId;
ItemId = (DemoGame.Server.ItemID)@itemId;
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
CharacterId = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(dataReader.GetOrdinal("character_id"));
ItemId = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(dataReader.GetOrdinal("item_id"));
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
public static void CopyValues(ICharacterInventoryTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
dic["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
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
paramValues["@character_id"] = (DemoGame.Server.CharacterID)source.CharacterId;
paramValues["@item_id"] = (DemoGame.Server.ItemID)source.ItemId;
}

public void CopyValuesFrom(ICharacterInventoryTable source)
{
CharacterId = (DemoGame.Server.CharacterID)source.CharacterId;
ItemId = (DemoGame.Server.ItemID)source.ItemId;
}

}

}
