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
/// Provides a strongly-typed structure for the database table `quest_require_start_quest`.
/// </summary>
public class QuestRequireStartQuestTable : IQuestRequireStartQuestTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"quest_id", "req_quest_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"quest_id", "req_quest_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] { };
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
public const System.String TableName = "quest_require_start_quest";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `quest_id`.
/// </summary>
System.UInt16 _questID;
/// <summary>
/// The field that maps onto the database column `req_quest_id`.
/// </summary>
System.UInt16 _reqQuestID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `quest_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The quest that this requirement is for.".
/// </summary>
[System.ComponentModel.Description("The quest that this requirement is for.")]
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
/// Gets or sets the value for the field that maps onto the database column `req_quest_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The quest that is required to be finished before this quest can be started.".
/// </summary>
[System.ComponentModel.Description("The quest that is required to be finished before this quest can be started.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Quests.QuestID ReqQuestID
{
get
{
return (NetGore.Features.Quests.QuestID)_reqQuestID;
}
set
{
this._reqQuestID = (System.UInt16)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IQuestRequireStartQuestTable DeepCopy()
{
return new QuestRequireStartQuestTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestRequireStartQuestTable"/> class.
/// </summary>
public QuestRequireStartQuestTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestRequireStartQuestTable"/> class.
/// </summary>
/// <param name="questID">The initial value for the corresponding property.</param>
/// <param name="reqQuestID">The initial value for the corresponding property.</param>
public QuestRequireStartQuestTable(NetGore.Features.Quests.QuestID @questID, NetGore.Features.Quests.QuestID @reqQuestID)
{
this.QuestID = (NetGore.Features.Quests.QuestID)@questID;
this.ReqQuestID = (NetGore.Features.Quests.QuestID)@reqQuestID;
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestRequireStartQuestTable"/> class.
/// </summary>
/// <param name="source">IQuestRequireStartQuestTable to copy the initial values from.</param>
public QuestRequireStartQuestTable(IQuestRequireStartQuestTable source)
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
public static void CopyValues(IQuestRequireStartQuestTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["quest_id"] = (NetGore.Features.Quests.QuestID)source.QuestID;
dic["req_quest_id"] = (NetGore.Features.Quests.QuestID)source.ReqQuestID;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this QuestRequireStartQuestTable.
/// </summary>
/// <param name="source">The IQuestRequireStartQuestTable to copy the values from.</param>
public void CopyValuesFrom(IQuestRequireStartQuestTable source)
{
this.QuestID = (NetGore.Features.Quests.QuestID)source.QuestID;
this.ReqQuestID = (NetGore.Features.Quests.QuestID)source.ReqQuestID;
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
case "quest_id":
return QuestID;

case "req_quest_id":
return ReqQuestID;

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
case "quest_id":
this.QuestID = (NetGore.Features.Quests.QuestID)value;
break;

case "req_quest_id":
this.ReqQuestID = (NetGore.Features.Quests.QuestID)value;
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
case "quest_id":
return new ColumnMetadata("quest_id", "The quest that this requirement is for.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "req_quest_id":
return new ColumnMetadata("req_quest_id", "The quest that is required to be finished before this quest can be started.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

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
