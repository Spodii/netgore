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
/// Provides a strongly-typed structure for the database table `character_template_inventory`.
/// </summary>
public class CharacterTemplateInventoryTable : ICharacterTemplateInventoryTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"chance", "character_template_id", "id", "item_template_id", "max", "min" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"chance", "character_template_id", "item_template_id", "max", "min" };
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
public const System.Int32 ColumnCount = 6;
/// <summary>
/// The field that maps onto the database column `chance`.
/// </summary>
System.UInt16 _chance;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.UInt16 _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `item_template_id`.
/// </summary>
System.UInt16 _itemTemplateID;
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
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "Chance that this item will be created when the character template is instantiated. Item quantity will be between min and max (equal chance distribution).".
/// </summary>
[System.ComponentModel.Description("Chance that this item will be created when the character template is instantiated. Item quantity will be between min and max (equal chance distribution).")]
[NetGore.SyncValueAttribute()]
public DemoGame.ItemChance Chance
{
get
{
return (DemoGame.ItemChance)_chance;
}
set
{
this._chance = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The character template.".
/// </summary>
[System.ComponentModel.Description("The character template.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterTemplateID CharacterTemplateID
{
get
{
return (DemoGame.CharacterTemplateID)_characterTemplateID;
}
set
{
this._characterTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The unique row ID.".
/// </summary>
[System.ComponentModel.Description("The unique row ID.")]
[NetGore.SyncValueAttribute()]
public System.Int32 ID
{
get
{
return (System.Int32)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The item the character template has in their inventory.".
/// </summary>
[System.ComponentModel.Description("The item the character template has in their inventory.")]
[NetGore.SyncValueAttribute()]
public DemoGame.ItemTemplateID ItemTemplateID
{
get
{
return (DemoGame.ItemTemplateID)_itemTemplateID;
}
set
{
this._itemTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The maximum number of items to be created.".
/// </summary>
[System.ComponentModel.Description("The maximum number of items to be created.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The minimum number of items to be created. Doesn't affect item creation chance. Each value in range has equal distribution.".
/// </summary>
[System.ComponentModel.Description("The minimum number of items to be created. Doesn't affect item creation chance. Each value in range has equal distribution.")]
[NetGore.SyncValueAttribute()]
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual ICharacterTemplateInventoryTable DeepCopy()
{
return new CharacterTemplateInventoryTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterTemplateInventoryTable"/> class.
/// </summary>
public CharacterTemplateInventoryTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterTemplateInventoryTable"/> class.
/// </summary>
/// <param name="chance">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="itemTemplateID">The initial value for the corresponding property.</param>
/// <param name="max">The initial value for the corresponding property.</param>
/// <param name="min">The initial value for the corresponding property.</param>
public CharacterTemplateInventoryTable(DemoGame.ItemChance @chance, DemoGame.CharacterTemplateID @characterTemplateID, System.Int32 @iD, DemoGame.ItemTemplateID @itemTemplateID, System.Byte @max, System.Byte @min)
{
this.Chance = (DemoGame.ItemChance)@chance;
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)@characterTemplateID;
this.ID = (System.Int32)@iD;
this.ItemTemplateID = (DemoGame.ItemTemplateID)@itemTemplateID;
this.Max = (System.Byte)@max;
this.Min = (System.Byte)@min;
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterTemplateInventoryTable"/> class.
/// </summary>
/// <param name="source">ICharacterTemplateInventoryTable to copy the initial values from.</param>
public CharacterTemplateInventoryTable(ICharacterTemplateInventoryTable source)
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
public static void CopyValues(ICharacterTemplateInventoryTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["chance"] = (DemoGame.ItemChance)source.Chance;
dic["character_template_id"] = (DemoGame.CharacterTemplateID)source.CharacterTemplateID;
dic["id"] = (System.Int32)source.ID;
dic["item_template_id"] = (DemoGame.ItemTemplateID)source.ItemTemplateID;
dic["max"] = (System.Byte)source.Max;
dic["min"] = (System.Byte)source.Min;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterTemplateInventoryTable.
/// </summary>
/// <param name="source">The ICharacterTemplateInventoryTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterTemplateInventoryTable source)
{
this.Chance = (DemoGame.ItemChance)source.Chance;
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)source.CharacterTemplateID;
this.ID = (System.Int32)source.ID;
this.ItemTemplateID = (DemoGame.ItemTemplateID)source.ItemTemplateID;
this.Max = (System.Byte)source.Max;
this.Min = (System.Byte)source.Min;
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
case "chance":
return Chance;

case "character_template_id":
return CharacterTemplateID;

case "id":
return ID;

case "item_template_id":
return ItemTemplateID;

case "max":
return Max;

case "min":
return Min;

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
case "chance":
this.Chance = (DemoGame.ItemChance)value;
break;

case "character_template_id":
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)value;
break;

case "id":
this.ID = (System.Int32)value;
break;

case "item_template_id":
this.ItemTemplateID = (DemoGame.ItemTemplateID)value;
break;

case "max":
this.Max = (System.Byte)value;
break;

case "min":
this.Min = (System.Byte)value;
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
case "chance":
return new ColumnMetadata("chance", "Chance that this item will be created when the character template is instantiated. Item quantity will be between min and max (equal chance distribution).", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "The character template.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "id":
return new ColumnMetadata("id", "The unique row ID.", "int(11)", null, typeof(System.Int32), false, true, false);

case "item_template_id":
return new ColumnMetadata("item_template_id", "The item the character template has in their inventory.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "max":
return new ColumnMetadata("max", "The maximum number of items to be created.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "min":
return new ColumnMetadata("min", "The minimum number of items to be created. Doesn't affect item creation chance. Each value in range has equal distribution.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

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
