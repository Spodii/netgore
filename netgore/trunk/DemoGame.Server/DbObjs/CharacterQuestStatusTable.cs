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
/// Provides a strongly-typed structure for the database table `character_quest_status`.
/// </summary>
public class CharacterQuestStatusTable : ICharacterQuestStatusTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "completed_on", "quest_id", "started_on" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id", "quest_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"completed_on", "started_on" };
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
public const System.String TableName = "character_quest_status";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `completed_on`.
/// </summary>
System.Nullable<System.DateTime> _completedOn;
/// <summary>
/// The field that maps onto the database column `quest_id`.
/// </summary>
System.UInt16 _questID;
/// <summary>
/// The field that maps onto the database column `started_on`.
/// </summary>
System.DateTime _startedOn;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "Character this quest status info is for.".
/// </summary>
[System.ComponentModel.Description("Character this quest status info is for.")]
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
/// Gets or sets the value for the field that maps onto the database column `completed_on`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the quest was completed. Null if incomplete. Repeatable quests hold time is was most recently completed.".
/// </summary>
[System.ComponentModel.Description("When the quest was completed. Null if incomplete. Repeatable quests hold time is was most recently completed.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.DateTime> CompletedOn
{
get
{
return (System.Nullable<System.DateTime>)_completedOn;
}
set
{
this._completedOn = (System.Nullable<System.DateTime>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `quest_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The quest this information is for.".
/// </summary>
[System.ComponentModel.Description("The quest this information is for.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Quests.QuestID QuestID
{
get
{
return (NetGore.Features.Quests.QuestID)_questID;
}
set
{
this._questID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `started_on`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the quest was started.".
/// </summary>
[System.ComponentModel.Description("When the quest was started.")]
[NetGore.SyncValueAttribute()]
public System.DateTime StartedOn
{
get
{
return (System.DateTime)_startedOn;
}
set
{
this._startedOn = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual ICharacterQuestStatusTable DeepCopy()
{
return new CharacterQuestStatusTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterQuestStatusTable"/> class.
/// </summary>
public CharacterQuestStatusTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterQuestStatusTable"/> class.
/// </summary>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="completedOn">The initial value for the corresponding property.</param>
/// <param name="questID">The initial value for the corresponding property.</param>
/// <param name="startedOn">The initial value for the corresponding property.</param>
public CharacterQuestStatusTable(DemoGame.CharacterID @characterID, System.Nullable<System.DateTime> @completedOn, NetGore.Features.Quests.QuestID @questID, System.DateTime @startedOn)
{
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.CompletedOn = (System.Nullable<System.DateTime>)@completedOn;
this.QuestID = (NetGore.Features.Quests.QuestID)@questID;
this.StartedOn = (System.DateTime)@startedOn;
}
/// <summary>
/// Initializes a new instance of the <see cref="CharacterQuestStatusTable"/> class.
/// </summary>
/// <param name="source">ICharacterQuestStatusTable to copy the initial values from.</param>
public CharacterQuestStatusTable(ICharacterQuestStatusTable source)
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
public static void CopyValues(ICharacterQuestStatusTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["completed_on"] = (System.Nullable<System.DateTime>)source.CompletedOn;
dic["quest_id"] = (NetGore.Features.Quests.QuestID)source.QuestID;
dic["started_on"] = (System.DateTime)source.StartedOn;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterQuestStatusTable.
/// </summary>
/// <param name="source">The ICharacterQuestStatusTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterQuestStatusTable source)
{
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.CompletedOn = (System.Nullable<System.DateTime>)source.CompletedOn;
this.QuestID = (NetGore.Features.Quests.QuestID)source.QuestID;
this.StartedOn = (System.DateTime)source.StartedOn;
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

case "completed_on":
return CompletedOn;

case "quest_id":
return QuestID;

case "started_on":
return StartedOn;

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

case "completed_on":
this.CompletedOn = (System.Nullable<System.DateTime>)value;
break;

case "quest_id":
this.QuestID = (NetGore.Features.Quests.QuestID)value;
break;

case "started_on":
this.StartedOn = (System.DateTime)value;
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
return new ColumnMetadata("character_id", "Character this quest status info is for.", "int(11)", null, typeof(System.Int32), false, true, false);

case "completed_on":
return new ColumnMetadata("completed_on", "When the quest was completed. Null if incomplete. Repeatable quests hold time is was most recently completed.", "datetime", null, typeof(System.Nullable<System.DateTime>), true, false, false);

case "quest_id":
return new ColumnMetadata("quest_id", "The quest this information is for.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "started_on":
return new ColumnMetadata("started_on", "When the quest was started.", "datetime", null, typeof(System.DateTime), false, false, false);

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
