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
/// Provides a strongly-typed structure for the database table `guild_event`.
/// </summary>
public class GuildEventTable : IGuildEventTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"arg0", "arg1", "arg2", "character_id", "created", "event_id", "guild_id", "id", "target_character_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"arg0", "arg1", "arg2", "character_id", "created", "event_id", "guild_id", "target_character_id" };
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
public const System.String TableName = "guild_event";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 9;
/// <summary>
/// The field that maps onto the database column `arg0`.
/// </summary>
System.String _arg0;
/// <summary>
/// The field that maps onto the database column `arg1`.
/// </summary>
System.String _arg1;
/// <summary>
/// The field that maps onto the database column `arg2`.
/// </summary>
System.String _arg2;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `created`.
/// </summary>
System.DateTime _created;
/// <summary>
/// The field that maps onto the database column `event_id`.
/// </summary>
System.Byte _eventID;
/// <summary>
/// The field that maps onto the database column `guild_id`.
/// </summary>
System.UInt16 _guildID;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `target_character_id`.
/// </summary>
System.Nullable<System.Int32> _targetCharacterID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `arg0`.
/// The underlying database type is `varchar(0)`.The database column contains the comment: 
/// "The first optional event argument.".
/// </summary>
[System.ComponentModel.Description("The first optional event argument.")]
[NetGore.SyncValueAttribute()]
public System.String Arg0
{
get
{
return (System.String)_arg0;
}
set
{
this._arg0 = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `arg1`.
/// The underlying database type is `varchar(0)`.The database column contains the comment: 
/// "The second optional event argument.".
/// </summary>
[System.ComponentModel.Description("The second optional event argument.")]
[NetGore.SyncValueAttribute()]
public System.String Arg1
{
get
{
return (System.String)_arg1;
}
set
{
this._arg1 = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `arg2`.
/// The underlying database type is `varchar(0)`.The database column contains the comment: 
/// "The third optional event argument.".
/// </summary>
[System.ComponentModel.Description("The third optional event argument.")]
[NetGore.SyncValueAttribute()]
public System.String Arg2
{
get
{
return (System.String)_arg2;
}
set
{
this._arg2 = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character that invoked the event.".
/// </summary>
[System.ComponentModel.Description("The character that invoked the event.")]
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
/// Gets or sets the value for the field that maps onto the database column `created`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the event was created.".
/// </summary>
[System.ComponentModel.Description("When the event was created.")]
[NetGore.SyncValueAttribute()]
public System.DateTime Created
{
get
{
return (System.DateTime)_created;
}
set
{
this._created = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `event_id`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The ID of the event that took place.".
/// </summary>
[System.ComponentModel.Description("The ID of the event that took place.")]
[NetGore.SyncValueAttribute()]
public System.Byte EventID
{
get
{
return (System.Byte)_eventID;
}
set
{
this._eventID = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `guild_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The guild the event took place on.".
/// </summary>
[System.ComponentModel.Description("The guild the event took place on.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Guilds.GuildID GuildID
{
get
{
return (NetGore.Features.Guilds.GuildID)_guildID;
}
set
{
this._guildID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The ID of the event.".
/// </summary>
[System.ComponentModel.Description("The ID of the event.")]
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
/// Gets or sets the value for the field that maps onto the database column `target_character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The optional character that the event involves.".
/// </summary>
[System.ComponentModel.Description("The optional character that the event involves.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.CharacterID> TargetCharacterID
{
get
{
return (System.Nullable<DemoGame.CharacterID>)_targetCharacterID;
}
set
{
this._targetCharacterID = (System.Nullable<System.Int32>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IGuildEventTable DeepCopy()
{
return new GuildEventTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildEventTable"/> class.
/// </summary>
public GuildEventTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildEventTable"/> class.
/// </summary>
/// <param name="arg0">The initial value for the corresponding property.</param>
/// <param name="arg1">The initial value for the corresponding property.</param>
/// <param name="arg2">The initial value for the corresponding property.</param>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="created">The initial value for the corresponding property.</param>
/// <param name="eventID">The initial value for the corresponding property.</param>
/// <param name="guildID">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="targetCharacterID">The initial value for the corresponding property.</param>
public GuildEventTable(System.String @arg0, System.String @arg1, System.String @arg2, DemoGame.CharacterID @characterID, System.DateTime @created, System.Byte @eventID, NetGore.Features.Guilds.GuildID @guildID, System.Int32 @iD, System.Nullable<DemoGame.CharacterID> @targetCharacterID)
{
this.Arg0 = (System.String)@arg0;
this.Arg1 = (System.String)@arg1;
this.Arg2 = (System.String)@arg2;
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.Created = (System.DateTime)@created;
this.EventID = (System.Byte)@eventID;
this.GuildID = (NetGore.Features.Guilds.GuildID)@guildID;
this.ID = (System.Int32)@iD;
this.TargetCharacterID = (System.Nullable<DemoGame.CharacterID>)@targetCharacterID;
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildEventTable"/> class.
/// </summary>
/// <param name="source">IGuildEventTable to copy the initial values from.</param>
public GuildEventTable(IGuildEventTable source)
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
public static void CopyValues(IGuildEventTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["arg0"] = (System.String)source.Arg0;
dic["arg1"] = (System.String)source.Arg1;
dic["arg2"] = (System.String)source.Arg2;
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["created"] = (System.DateTime)source.Created;
dic["event_id"] = (System.Byte)source.EventID;
dic["guild_id"] = (NetGore.Features.Guilds.GuildID)source.GuildID;
dic["id"] = (System.Int32)source.ID;
dic["target_character_id"] = (System.Nullable<DemoGame.CharacterID>)source.TargetCharacterID;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this GuildEventTable.
/// </summary>
/// <param name="source">The IGuildEventTable to copy the values from.</param>
public void CopyValuesFrom(IGuildEventTable source)
{
this.Arg0 = (System.String)source.Arg0;
this.Arg1 = (System.String)source.Arg1;
this.Arg2 = (System.String)source.Arg2;
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.Created = (System.DateTime)source.Created;
this.EventID = (System.Byte)source.EventID;
this.GuildID = (NetGore.Features.Guilds.GuildID)source.GuildID;
this.ID = (System.Int32)source.ID;
this.TargetCharacterID = (System.Nullable<DemoGame.CharacterID>)source.TargetCharacterID;
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
case "arg0":
return Arg0;

case "arg1":
return Arg1;

case "arg2":
return Arg2;

case "character_id":
return CharacterID;

case "created":
return Created;

case "event_id":
return EventID;

case "guild_id":
return GuildID;

case "id":
return ID;

case "target_character_id":
return TargetCharacterID;

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
case "arg0":
this.Arg0 = (System.String)value;
break;

case "arg1":
this.Arg1 = (System.String)value;
break;

case "arg2":
this.Arg2 = (System.String)value;
break;

case "character_id":
this.CharacterID = (DemoGame.CharacterID)value;
break;

case "created":
this.Created = (System.DateTime)value;
break;

case "event_id":
this.EventID = (System.Byte)value;
break;

case "guild_id":
this.GuildID = (NetGore.Features.Guilds.GuildID)value;
break;

case "id":
this.ID = (System.Int32)value;
break;

case "target_character_id":
this.TargetCharacterID = (System.Nullable<DemoGame.CharacterID>)value;
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
case "arg0":
return new ColumnMetadata("arg0", "The first optional event argument.", "varchar(0)", null, typeof(System.String), true, false, false);

case "arg1":
return new ColumnMetadata("arg1", "The second optional event argument.", "varchar(0)", null, typeof(System.String), true, false, false);

case "arg2":
return new ColumnMetadata("arg2", "The third optional event argument.", "varchar(0)", null, typeof(System.String), true, false, false);

case "character_id":
return new ColumnMetadata("character_id", "The character that invoked the event.", "int(11)", null, typeof(System.Int32), false, false, true);

case "created":
return new ColumnMetadata("created", "When the event was created.", "datetime", null, typeof(System.DateTime), false, false, false);

case "event_id":
return new ColumnMetadata("event_id", "The ID of the event that took place.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "guild_id":
return new ColumnMetadata("guild_id", "The guild the event took place on.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "id":
return new ColumnMetadata("id", "The ID of the event.", "int(11)", null, typeof(System.Int32), false, true, false);

case "target_character_id":
return new ColumnMetadata("target_character_id", "The optional character that the event involves.", "int(11)", null, typeof(System.Nullable<System.Int32>), true, false, true);

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
