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
/// Provides a strongly-typed structure for the database table `world_stats_npc_kill_user`.
/// </summary>
public class WorldStatsNpcKillUserTable : IWorldStatsNpcKillUserTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"id", "map_id", "npc_template_id", "npc_x", "npc_y", "user_id", "user_level", "user_x", "user_y", "when" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"map_id", "npc_template_id", "npc_x", "npc_y", "user_id", "user_level", "user_x", "user_y", "when" };
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
public const System.String TableName = "world_stats_npc_kill_user";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 10;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _iD;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.Nullable<System.UInt16> _mapID;
/// <summary>
/// The field that maps onto the database column `npc_template_id`.
/// </summary>
System.Nullable<System.UInt16> _nPCTemplateID;
/// <summary>
/// The field that maps onto the database column `npc_x`.
/// </summary>
System.UInt16 _npcX;
/// <summary>
/// The field that maps onto the database column `npc_y`.
/// </summary>
System.UInt16 _npcY;
/// <summary>
/// The field that maps onto the database column `user_id`.
/// </summary>
System.Int32 _userID;
/// <summary>
/// The field that maps onto the database column `user_level`.
/// </summary>
System.Int16 _userLevel;
/// <summary>
/// The field that maps onto the database column `user_x`.
/// </summary>
System.UInt16 _userX;
/// <summary>
/// The field that maps onto the database column `user_y`.
/// </summary>
System.UInt16 _userY;
/// <summary>
/// The field that maps onto the database column `when`.
/// </summary>
System.DateTime _when;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(10) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt32 ID
{
get
{
return (System.UInt32)_iD;
}
set
{
this._iD = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The ID of the map this event took place on.".
/// </summary>
[System.ComponentModel.Description("The ID of the map this event took place on.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.World.MapID> MapID
{
get
{
return (System.Nullable<NetGore.World.MapID>)_mapID;
}
set
{
this._mapID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `npc_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The template ID of the NPC. Only valid when the NPC has a template ID set.".
/// </summary>
[System.ComponentModel.Description("The template ID of the NPC. Only valid when the NPC has a template ID set.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.CharacterTemplateID> NPCTemplateID
{
get
{
return (System.Nullable<DemoGame.CharacterTemplateID>)_nPCTemplateID;
}
set
{
this._nPCTemplateID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `npc_x`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.".
/// </summary>
[System.ComponentModel.Description("The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 NpcX
{
get
{
return (System.UInt16)_npcX;
}
set
{
this._npcX = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `npc_y`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.".
/// </summary>
[System.ComponentModel.Description("The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 NpcY
{
get
{
return (System.UInt16)_npcY;
}
set
{
this._npcY = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The ID of the user.".
/// </summary>
[System.ComponentModel.Description("The ID of the user.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterID UserID
{
get
{
return (DemoGame.CharacterID)_userID;
}
set
{
this._userID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_level`.
/// The underlying database type is `smallint(6)`.The database column contains the comment: 
/// "The level of the user was when this event took place.".
/// </summary>
[System.ComponentModel.Description("The level of the user was when this event took place.")]
[NetGore.SyncValueAttribute()]
public System.Int16 UserLevel
{
get
{
return (System.Int16)_userLevel;
}
set
{
this._userLevel = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_x`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map x coordinate of the user when this event took place.".
/// </summary>
[System.ComponentModel.Description("The map x coordinate of the user when this event took place.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 UserX
{
get
{
return (System.UInt16)_userX;
}
set
{
this._userX = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_y`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map y coordinate of the user when this event took place.".
/// </summary>
[System.ComponentModel.Description("The map y coordinate of the user when this event took place.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 UserY
{
get
{
return (System.UInt16)_userY;
}
set
{
this._userY = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `when`.
/// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`.The database column contains the comment: 
/// "When this event took place.".
/// </summary>
[System.ComponentModel.Description("When this event took place.")]
[NetGore.SyncValueAttribute()]
public System.DateTime When
{
get
{
return (System.DateTime)_when;
}
set
{
this._when = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IWorldStatsNpcKillUserTable DeepCopy()
{
return new WorldStatsNpcKillUserTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
/// </summary>
public WorldStatsNpcKillUserTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
/// </summary>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="nPCTemplateID">The initial value for the corresponding property.</param>
/// <param name="npcX">The initial value for the corresponding property.</param>
/// <param name="npcY">The initial value for the corresponding property.</param>
/// <param name="userID">The initial value for the corresponding property.</param>
/// <param name="userLevel">The initial value for the corresponding property.</param>
/// <param name="userX">The initial value for the corresponding property.</param>
/// <param name="userY">The initial value for the corresponding property.</param>
/// <param name="when">The initial value for the corresponding property.</param>
public WorldStatsNpcKillUserTable(System.UInt32 @iD, System.Nullable<NetGore.World.MapID> @mapID, System.Nullable<DemoGame.CharacterTemplateID> @nPCTemplateID, System.UInt16 @npcX, System.UInt16 @npcY, DemoGame.CharacterID @userID, System.Int16 @userLevel, System.UInt16 @userX, System.UInt16 @userY, System.DateTime @when)
{
this.ID = (System.UInt32)@iD;
this.MapID = (System.Nullable<NetGore.World.MapID>)@mapID;
this.NPCTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)@nPCTemplateID;
this.NpcX = (System.UInt16)@npcX;
this.NpcY = (System.UInt16)@npcY;
this.UserID = (DemoGame.CharacterID)@userID;
this.UserLevel = (System.Int16)@userLevel;
this.UserX = (System.UInt16)@userX;
this.UserY = (System.UInt16)@userY;
this.When = (System.DateTime)@when;
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
/// </summary>
/// <param name="source">IWorldStatsNpcKillUserTable to copy the initial values from.</param>
public WorldStatsNpcKillUserTable(IWorldStatsNpcKillUserTable source)
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
public static void CopyValues(IWorldStatsNpcKillUserTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["id"] = (System.UInt32)source.ID;
dic["map_id"] = (System.Nullable<NetGore.World.MapID>)source.MapID;
dic["npc_template_id"] = (System.Nullable<DemoGame.CharacterTemplateID>)source.NPCTemplateID;
dic["npc_x"] = (System.UInt16)source.NpcX;
dic["npc_y"] = (System.UInt16)source.NpcY;
dic["user_id"] = (DemoGame.CharacterID)source.UserID;
dic["user_level"] = (System.Int16)source.UserLevel;
dic["user_x"] = (System.UInt16)source.UserX;
dic["user_y"] = (System.UInt16)source.UserY;
dic["when"] = (System.DateTime)source.When;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsNpcKillUserTable.
/// </summary>
/// <param name="source">The IWorldStatsNpcKillUserTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsNpcKillUserTable source)
{
this.ID = (System.UInt32)source.ID;
this.MapID = (System.Nullable<NetGore.World.MapID>)source.MapID;
this.NPCTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)source.NPCTemplateID;
this.NpcX = (System.UInt16)source.NpcX;
this.NpcY = (System.UInt16)source.NpcY;
this.UserID = (DemoGame.CharacterID)source.UserID;
this.UserLevel = (System.Int16)source.UserLevel;
this.UserX = (System.UInt16)source.UserX;
this.UserY = (System.UInt16)source.UserY;
this.When = (System.DateTime)source.When;
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
case "id":
return ID;

case "map_id":
return MapID;

case "npc_template_id":
return NPCTemplateID;

case "npc_x":
return NpcX;

case "npc_y":
return NpcY;

case "user_id":
return UserID;

case "user_level":
return UserLevel;

case "user_x":
return UserX;

case "user_y":
return UserY;

case "when":
return When;

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
case "id":
this.ID = (System.UInt32)value;
break;

case "map_id":
this.MapID = (System.Nullable<NetGore.World.MapID>)value;
break;

case "npc_template_id":
this.NPCTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)value;
break;

case "npc_x":
this.NpcX = (System.UInt16)value;
break;

case "npc_y":
this.NpcY = (System.UInt16)value;
break;

case "user_id":
this.UserID = (DemoGame.CharacterID)value;
break;

case "user_level":
this.UserLevel = (System.Int16)value;
break;

case "user_x":
this.UserX = (System.UInt16)value;
break;

case "user_y":
this.UserY = (System.UInt16)value;
break;

case "when":
this.When = (System.DateTime)value;
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
case "id":
return new ColumnMetadata("id", "", "int(10) unsigned", null, typeof(System.UInt32), false, true, false);

case "map_id":
return new ColumnMetadata("map_id", "The ID of the map this event took place on.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "npc_template_id":
return new ColumnMetadata("npc_template_id", "The template ID of the NPC. Only valid when the NPC has a template ID set.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "npc_x":
return new ColumnMetadata("npc_x", "The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "npc_y":
return new ColumnMetadata("npc_y", "The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "user_id":
return new ColumnMetadata("user_id", "The ID of the user.", "int(11)", null, typeof(System.Int32), false, false, true);

case "user_level":
return new ColumnMetadata("user_level", "The level of the user was when this event took place.", "smallint(6)", null, typeof(System.Int16), false, false, false);

case "user_x":
return new ColumnMetadata("user_x", "The map x coordinate of the user when this event took place.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "user_y":
return new ColumnMetadata("user_y", "The map y coordinate of the user when this event took place.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "when":
return new ColumnMetadata("when", "When this event took place.", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, false, false);

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
