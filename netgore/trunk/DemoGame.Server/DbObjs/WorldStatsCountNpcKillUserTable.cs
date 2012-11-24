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
/// Provides a strongly-typed structure for the database table `world_stats_count_npc_kill_user`.
/// </summary>
public class WorldStatsCountNpcKillUserTable : IWorldStatsCountNpcKillUserTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"count", "last_update", "npc_template_id", "user_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"npc_template_id", "user_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"count", "last_update" };
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
public const System.String TableName = "world_stats_count_npc_kill_user";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `count`.
/// </summary>
System.Int32 _count;
/// <summary>
/// The field that maps onto the database column `last_update`.
/// </summary>
System.DateTime _lastUpdate;
/// <summary>
/// The field that maps onto the database column `npc_template_id`.
/// </summary>
System.UInt16 _nPCTemplateID;
/// <summary>
/// The field that maps onto the database column `user_id`.
/// </summary>
System.Int32 _userID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `count`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "The number of times NPCs of this character template have killed the user.".
/// </summary>
[System.ComponentModel.Description("The number of times NPCs of this character template have killed the user.")]
[NetGore.SyncValueAttribute()]
public System.Int32 Count
{
get
{
return (System.Int32)_count;
}
set
{
this._count = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `last_update`.
/// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`.The database column contains the comment: 
/// "When this counter was last updated.".
/// </summary>
[System.ComponentModel.Description("When this counter was last updated.")]
[NetGore.SyncValueAttribute()]
public System.DateTime LastUpdate
{
get
{
return (System.DateTime)_lastUpdate;
}
set
{
this._lastUpdate = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `npc_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The character template this counter is for.".
/// </summary>
[System.ComponentModel.Description("The character template this counter is for.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterTemplateID NPCTemplateID
{
get
{
return (DemoGame.CharacterTemplateID)_nPCTemplateID;
}
set
{
this._nPCTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character this counter is for (logically, should be a user).".
/// </summary>
[System.ComponentModel.Description("The character this counter is for (logically, should be a user).")]
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IWorldStatsCountNpcKillUserTable DeepCopy()
{
return new WorldStatsCountNpcKillUserTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsCountNpcKillUserTable"/> class.
/// </summary>
public WorldStatsCountNpcKillUserTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsCountNpcKillUserTable"/> class.
/// </summary>
/// <param name="count">The initial value for the corresponding property.</param>
/// <param name="lastUpdate">The initial value for the corresponding property.</param>
/// <param name="nPCTemplateID">The initial value for the corresponding property.</param>
/// <param name="userID">The initial value for the corresponding property.</param>
public WorldStatsCountNpcKillUserTable(System.Int32 @count, System.DateTime @lastUpdate, DemoGame.CharacterTemplateID @nPCTemplateID, DemoGame.CharacterID @userID)
{
this.Count = (System.Int32)@count;
this.LastUpdate = (System.DateTime)@lastUpdate;
this.NPCTemplateID = (DemoGame.CharacterTemplateID)@nPCTemplateID;
this.UserID = (DemoGame.CharacterID)@userID;
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsCountNpcKillUserTable"/> class.
/// </summary>
/// <param name="source">IWorldStatsCountNpcKillUserTable to copy the initial values from.</param>
public WorldStatsCountNpcKillUserTable(IWorldStatsCountNpcKillUserTable source)
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
public static void CopyValues(IWorldStatsCountNpcKillUserTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["count"] = (System.Int32)source.Count;
dic["last_update"] = (System.DateTime)source.LastUpdate;
dic["npc_template_id"] = (DemoGame.CharacterTemplateID)source.NPCTemplateID;
dic["user_id"] = (DemoGame.CharacterID)source.UserID;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsCountNpcKillUserTable.
/// </summary>
/// <param name="source">The IWorldStatsCountNpcKillUserTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsCountNpcKillUserTable source)
{
this.Count = (System.Int32)source.Count;
this.LastUpdate = (System.DateTime)source.LastUpdate;
this.NPCTemplateID = (DemoGame.CharacterTemplateID)source.NPCTemplateID;
this.UserID = (DemoGame.CharacterID)source.UserID;
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
case "count":
return Count;

case "last_update":
return LastUpdate;

case "npc_template_id":
return NPCTemplateID;

case "user_id":
return UserID;

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
case "count":
this.Count = (System.Int32)value;
break;

case "last_update":
this.LastUpdate = (System.DateTime)value;
break;

case "npc_template_id":
this.NPCTemplateID = (DemoGame.CharacterTemplateID)value;
break;

case "user_id":
this.UserID = (DemoGame.CharacterID)value;
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
case "count":
return new ColumnMetadata("count", "The number of times NPCs of this character template have killed the user.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "last_update":
return new ColumnMetadata("last_update", "When this counter was last updated.", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, false, false);

case "npc_template_id":
return new ColumnMetadata("npc_template_id", "The character template this counter is for.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "user_id":
return new ColumnMetadata("user_id", "The character this counter is for (logically, should be a user).", "int(11)", null, typeof(System.Int32), false, true, false);

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
