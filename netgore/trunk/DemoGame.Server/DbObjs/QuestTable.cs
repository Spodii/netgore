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
/// Provides a strongly-typed structure for the database table `quest`.
/// </summary>
public class QuestTable : IQuestTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"id", "repeatable", "reward_cash", "reward_exp" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"repeatable", "reward_cash", "reward_exp" };
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
public const System.String TableName = "quest";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _iD;
/// <summary>
/// The field that maps onto the database column `repeatable`.
/// </summary>
System.Boolean _repeatable;
/// <summary>
/// The field that maps onto the database column `reward_cash`.
/// </summary>
System.Int32 _rewardCash;
/// <summary>
/// The field that maps onto the database column `reward_exp`.
/// </summary>
System.Int32 _rewardExp;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The unique ID of the quest. Note: This table is like a template. Quest and character_quest_status are like character_template and character, respectively.".
/// </summary>
[System.ComponentModel.Description("The unique ID of the quest. Note: This table is like a template. Quest and character_quest_status are like character_template and character, respectively.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Quests.QuestID ID
{
get
{
return (NetGore.Features.Quests.QuestID)_iD;
}
set
{
this._iD = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `repeatable`.
/// The underlying database type is `tinyint(1) unsigned` with the default value of `0`.The database column contains the comment: 
/// "If this quest can be repeated by a character after they have completed it.".
/// </summary>
[System.ComponentModel.Description("If this quest can be repeated by a character after they have completed it.")]
[NetGore.SyncValueAttribute()]
public System.Boolean Repeatable
{
get
{
return (System.Boolean)_repeatable;
}
set
{
this._repeatable = (System.Boolean)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reward_cash`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "The base cash reward for completing this quest.".
/// </summary>
[System.ComponentModel.Description("The base cash reward for completing this quest.")]
[NetGore.SyncValueAttribute()]
public System.Int32 RewardCash
{
get
{
return (System.Int32)_rewardCash;
}
set
{
this._rewardCash = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reward_exp`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "The base experience reward for completing this quest.".
/// </summary>
[System.ComponentModel.Description("The base experience reward for completing this quest.")]
[NetGore.SyncValueAttribute()]
public System.Int32 RewardExp
{
get
{
return (System.Int32)_rewardExp;
}
set
{
this._rewardExp = (System.Int32)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IQuestTable DeepCopy()
{
return new QuestTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestTable"/> class.
/// </summary>
public QuestTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestTable"/> class.
/// </summary>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="repeatable">The initial value for the corresponding property.</param>
/// <param name="rewardCash">The initial value for the corresponding property.</param>
/// <param name="rewardExp">The initial value for the corresponding property.</param>
public QuestTable(NetGore.Features.Quests.QuestID @iD, System.Boolean @repeatable, System.Int32 @rewardCash, System.Int32 @rewardExp)
{
this.ID = (NetGore.Features.Quests.QuestID)@iD;
this.Repeatable = (System.Boolean)@repeatable;
this.RewardCash = (System.Int32)@rewardCash;
this.RewardExp = (System.Int32)@rewardExp;
}
/// <summary>
/// Initializes a new instance of the <see cref="QuestTable"/> class.
/// </summary>
/// <param name="source">IQuestTable to copy the initial values from.</param>
public QuestTable(IQuestTable source)
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
public static void CopyValues(IQuestTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["id"] = (NetGore.Features.Quests.QuestID)source.ID;
dic["repeatable"] = (System.Boolean)source.Repeatable;
dic["reward_cash"] = (System.Int32)source.RewardCash;
dic["reward_exp"] = (System.Int32)source.RewardExp;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this QuestTable.
/// </summary>
/// <param name="source">The IQuestTable to copy the values from.</param>
public void CopyValuesFrom(IQuestTable source)
{
this.ID = (NetGore.Features.Quests.QuestID)source.ID;
this.Repeatable = (System.Boolean)source.Repeatable;
this.RewardCash = (System.Int32)source.RewardCash;
this.RewardExp = (System.Int32)source.RewardExp;
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

case "repeatable":
return Repeatable;

case "reward_cash":
return RewardCash;

case "reward_exp":
return RewardExp;

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
this.ID = (NetGore.Features.Quests.QuestID)value;
break;

case "repeatable":
this.Repeatable = (System.Boolean)value;
break;

case "reward_cash":
this.RewardCash = (System.Int32)value;
break;

case "reward_exp":
this.RewardExp = (System.Int32)value;
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
return new ColumnMetadata("id", "The unique ID of the quest. Note: This table is like a template. Quest and character_quest_status are like character_template and character, respectively.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "repeatable":
return new ColumnMetadata("repeatable", "If this quest can be repeated by a character after they have completed it.", "tinyint(1) unsigned", "0", typeof(System.Boolean), false, false, false);

case "reward_cash":
return new ColumnMetadata("reward_cash", "The base cash reward for completing this quest.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "reward_exp":
return new ColumnMetadata("reward_exp", "The base experience reward for completing this quest.", "int(11)", "0", typeof(System.Int32), false, false, false);

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
