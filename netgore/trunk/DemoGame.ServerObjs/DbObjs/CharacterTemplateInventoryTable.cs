using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `character_template_inventory`.
/// </summary>
public class CharacterTemplateInventoryTable : ICharacterTemplateInventoryTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"chance", "character_template_id", "item_template_id", "max", "min" };
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
public const System.Int32 ColumnCount = 5;
/// <summary>
/// The field that maps onto the database column `chance`.
/// </summary>
System.UInt16 _chance;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.UInt16 _characterTemplateID;
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
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemChance Chance
{
get
{
return (DemoGame.Server.ItemChance)_chance;
}
set
{
this._chance = (System.UInt16)value;
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
/// Gets or sets the value for the field that maps onto the database column `item_template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemTemplateID ItemTemplateID
{
get
{
return (DemoGame.Server.ItemTemplateID)_itemTemplateID;
}
set
{
this._itemTemplateID = (System.UInt16)value;
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public ICharacterTemplateInventoryTable DeepCopy()
{
return new CharacterTemplateInventoryTable(this);
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
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="itemTemplateID">The initial value for the corresponding property.</param>
/// <param name="max">The initial value for the corresponding property.</param>
/// <param name="min">The initial value for the corresponding property.</param>
public CharacterTemplateInventoryTable(DemoGame.Server.ItemChance @chance, DemoGame.Server.CharacterTemplateID @characterTemplateID, DemoGame.Server.ItemTemplateID @itemTemplateID, System.Byte @max, System.Byte @min)
{
this.Chance = (DemoGame.Server.ItemChance)@chance;
this.CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)@characterTemplateID;
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)@itemTemplateID;
this.Max = (System.Byte)@max;
this.Min = (System.Byte)@min;
}
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
dic["@chance"] = (DemoGame.Server.ItemChance)source.Chance;
dic["@character_template_id"] = (DemoGame.Server.CharacterTemplateID)source.CharacterTemplateID;
dic["@item_template_id"] = (DemoGame.Server.ItemTemplateID)source.ItemTemplateID;
dic["@max"] = (System.Byte)source.Max;
dic["@min"] = (System.Byte)source.Min;
}

public void CopyValuesFrom(ICharacterTemplateInventoryTable source)
{
this.Chance = (DemoGame.Server.ItemChance)source.Chance;
this.CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)source.CharacterTemplateID;
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)source.ItemTemplateID;
this.Max = (System.Byte)source.Max;
this.Min = (System.Byte)source.Min;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "chance":
return Chance;

case "character_template_id":
return CharacterTemplateID;

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

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "chance":
this.Chance = (DemoGame.Server.ItemChance)value;
break;

case "character_template_id":
this.CharacterTemplateID = (DemoGame.Server.CharacterTemplateID)value;
break;

case "item_template_id":
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)value;
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

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "chance":
return new ColumnMetadata("chance", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "item_template_id":
return new ColumnMetadata("item_template_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "max":
return new ColumnMetadata("max", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "min":
return new ColumnMetadata("min", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
}

}

}
