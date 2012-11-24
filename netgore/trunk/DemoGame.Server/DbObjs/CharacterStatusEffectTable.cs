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
/// Provides a strongly-typed structure for the database table `character_status_effect`.
/// </summary>
public class CharacterStatusEffectTable : ICharacterStatusEffectTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "id", "power", "status_effect_id", "time_left_secs" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"character_id", "power", "status_effect_id", "time_left_secs" };
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
public const System.String TableName = "character_status_effect";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 5;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `power`.
/// </summary>
System.UInt16 _power;
/// <summary>
/// The field that maps onto the database column `status_effect_id`.
/// </summary>
System.Byte _statusEffect;
/// <summary>
/// The field that maps onto the database column `time_left_secs`.
/// </summary>
System.UInt16 _timeLeftSecs;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "ID of the Character that the status effect is on.".
/// </summary>
[System.ComponentModel.Description("ID of the Character that the status effect is on.")]
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
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "Unique ID of the status effect instance.".
/// </summary>
[System.ComponentModel.Description("Unique ID of the status effect instance.")]
[NetGore.SyncValueAttribute()]
public DemoGame.ActiveStatusEffectID ID
{
get
{
return (DemoGame.ActiveStatusEffectID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `power`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The power of this status effect instance.".
/// </summary>
[System.ComponentModel.Description("The power of this status effect instance.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 Power
{
get
{
return (System.UInt16)_power;
}
set
{
this._power = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `status_effect_id`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.".
/// </summary>
[System.ComponentModel.Description("ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.")]
[NetGore.SyncValueAttribute()]
public DemoGame.StatusEffectType StatusEffect
{
get
{
return (DemoGame.StatusEffectType)_statusEffect;
}
set
{
this._statusEffect = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time_left_secs`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The amount of time remaining for this status effect in seconds.".
/// </summary>
[System.ComponentModel.Description("The amount of time remaining for this status effect in seconds.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 TimeLeftSecs
{
get
{
return (System.UInt16)_timeLeftSecs;
}
set
{
this._timeLeftSecs = (System.UInt16)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual ICharacterStatusEffectTable DeepCopy()
{
return new CharacterStatusEffectTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
/// </summary>
public CharacterStatusEffectTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
/// </summary>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="power">The initial value for the corresponding property.</param>
/// <param name="statusEffect">The initial value for the corresponding property.</param>
/// <param name="timeLeftSecs">The initial value for the corresponding property.</param>
public CharacterStatusEffectTable(DemoGame.CharacterID @characterID, DemoGame.ActiveStatusEffectID @iD, System.UInt16 @power, DemoGame.StatusEffectType @statusEffect, System.UInt16 @timeLeftSecs)
{
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.ID = (DemoGame.ActiveStatusEffectID)@iD;
this.Power = (System.UInt16)@power;
this.StatusEffect = (DemoGame.StatusEffectType)@statusEffect;
this.TimeLeftSecs = (System.UInt16)@timeLeftSecs;
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
/// </summary>
/// <param name="source">ICharacterStatusEffectTable to copy the initial values from.</param>
public CharacterStatusEffectTable(ICharacterStatusEffectTable source)
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
public static void CopyValues(ICharacterStatusEffectTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["id"] = (DemoGame.ActiveStatusEffectID)source.ID;
dic["power"] = (System.UInt16)source.Power;
dic["status_effect_id"] = (DemoGame.StatusEffectType)source.StatusEffect;
dic["time_left_secs"] = (System.UInt16)source.TimeLeftSecs;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterStatusEffectTable.
/// </summary>
/// <param name="source">The ICharacterStatusEffectTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterStatusEffectTable source)
{
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.ID = (DemoGame.ActiveStatusEffectID)source.ID;
this.Power = (System.UInt16)source.Power;
this.StatusEffect = (DemoGame.StatusEffectType)source.StatusEffect;
this.TimeLeftSecs = (System.UInt16)source.TimeLeftSecs;
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

case "id":
return ID;

case "power":
return Power;

case "status_effect_id":
return StatusEffect;

case "time_left_secs":
return TimeLeftSecs;

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

case "id":
this.ID = (DemoGame.ActiveStatusEffectID)value;
break;

case "power":
this.Power = (System.UInt16)value;
break;

case "status_effect_id":
this.StatusEffect = (DemoGame.StatusEffectType)value;
break;

case "time_left_secs":
this.TimeLeftSecs = (System.UInt16)value;
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
return new ColumnMetadata("character_id", "ID of the Character that the status effect is on.", "int(11)", null, typeof(System.Int32), false, false, true);

case "id":
return new ColumnMetadata("id", "Unique ID of the status effect instance.", "int(11)", null, typeof(System.Int32), false, true, false);

case "power":
return new ColumnMetadata("power", "The power of this status effect instance.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "status_effect_id":
return new ColumnMetadata("status_effect_id", "ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "time_left_secs":
return new ColumnMetadata("time_left_secs", "The amount of time remaining for this status effect in seconds.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

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
