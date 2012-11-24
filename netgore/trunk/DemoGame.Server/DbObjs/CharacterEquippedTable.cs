/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

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
/// Provides a strongly-typed structure for the database table `character_equipped`.
/// </summary>
public class CharacterEquippedTable : ICharacterEquippedTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "item_id", "slot" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id", "slot" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"item_id" };
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
public const System.String TableName = "character_equipped";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.Int32 _itemID;
/// <summary>
/// The field that maps onto the database column `slot`.
/// </summary>
System.Byte _slot;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character who the equipped item is on.".
/// </summary>
[System.ComponentModel.Description("The character who the equipped item is on.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterID CharacterID
{
get
{
return (DemoGame.CharacterID)_characterID;
}
set
{
this._characterID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The item that is equipped by the character.".
/// </summary>
[System.ComponentModel.Description("The item that is equipped by the character.")]
[NetGore.SyncValueAttribute()]
public DemoGame.ItemID ItemID
{
get
{
return (DemoGame.ItemID)_itemID;
}
set
{
this._itemID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `slot`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The slot the equipped item is in.".
/// </summary>
[System.ComponentModel.Description("The slot the equipped item is in.")]
[NetGore.SyncValueAttribute()]
public DemoGame.EquipmentSlot Slot
{
get
{
return (DemoGame.EquipmentSlot)_slot;
}
set
{
this._slot = (System.Byte)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual ICharacterEquippedTable DeepCopy()
{
return new CharacterEquippedTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterEquippedTable"/> class.
/// </summary>
public CharacterEquippedTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterEquippedTable"/> class.
/// </summary>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="itemID">The initial value for the corresponding property.</param>
/// <param name="slot">The initial value for the corresponding property.</param>
public CharacterEquippedTable(DemoGame.CharacterID @characterID, DemoGame.ItemID @itemID, DemoGame.EquipmentSlot @slot)
{
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.ItemID = (DemoGame.ItemID)@itemID;
this.Slot = (DemoGame.EquipmentSlot)@slot;
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterEquippedTable"/> class.
/// </summary>
/// <param name="source">ICharacterEquippedTable to copy the initial values from.</param>
public CharacterEquippedTable(ICharacterEquippedTable source)
{
CopyValuesFrom(source);
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
public static void CopyValues(ICharacterEquippedTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["item_id"] = (DemoGame.ItemID)source.ItemID;
dic["slot"] = (DemoGame.EquipmentSlot)source.Slot;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterEquippedTable.
/// </summary>
/// <param name="source">The ICharacterEquippedTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterEquippedTable source)
{
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.ItemID = (DemoGame.ItemID)source.ItemID;
this.Slot = (DemoGame.EquipmentSlot)source.Slot;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "character_id":
return CharacterID;

case "item_id":
return ItemID;

case "slot":
return Slot;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "character_id":
this.CharacterID = (DemoGame.CharacterID)value;
break;

case "item_id":
this.ItemID = (DemoGame.ItemID)value;
break;

case "slot":
this.Slot = (DemoGame.EquipmentSlot)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "character_id":
return new ColumnMetadata("character_id", "The character who the equipped item is on.", "int(11)", null, typeof(System.Int32), false, true, false);

case "item_id":
return new ColumnMetadata("item_id", "The item that is equipped by the character.", "int(11)", null, typeof(System.Int32), false, false, true);

case "slot":
return new ColumnMetadata("slot", "The slot the equipped item is in.", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
