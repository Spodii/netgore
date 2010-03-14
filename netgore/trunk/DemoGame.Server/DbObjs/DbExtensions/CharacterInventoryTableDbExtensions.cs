using System;
using System.Linq;
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class CharacterInventoryTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class CharacterInventoryTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this ICharacterInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@character_id"] = (System.Int32)source.CharacterID;
paramValues["@item_id"] = (System.Int32)source.ItemID;
paramValues["@slot"] = (System.Byte)source.Slot;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this CharacterInventoryTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("character_id");

source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("item_id");

source.ItemID = (DemoGame.ItemID)(DemoGame.ItemID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("slot");

source.Slot = (NetGore.InventorySlot)(NetGore.InventorySlot)dataReader.GetByte(i);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this CharacterInventoryTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "character_id":
source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);
break;


case "item_id":
source.ItemID = (DemoGame.ItemID)(DemoGame.ItemID)dataReader.GetInt32(i);
break;


case "slot":
source.Slot = (NetGore.InventorySlot)(NetGore.InventorySlot)dataReader.GetByte(i);
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
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this ICharacterInventoryTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@character_id":
paramValues[i] = (System.Int32)source.CharacterID;
break;


case "@item_id":
paramValues[i] = (System.Int32)source.ItemID;
break;


case "@slot":
paramValues[i] = (System.Byte)source.Slot;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="ICharacterInventoryTable"/> contains the same values as an<paramref name="other"/> <see cref="ICharacterInventoryTable"/>.
/// </summary>
/// <param name="other">The <see cref="ICharacterInventoryTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="ICharacterInventoryTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
/// </returns>
public static System.Boolean HasSameValues(this ICharacterInventoryTable source, ICharacterInventoryTable other)
{
return Equals(source.CharacterID, other.CharacterID) && 
Equals(source.ItemID, other.ItemID) && 
Equals(source.Slot, other.Slot);
}

}

}
