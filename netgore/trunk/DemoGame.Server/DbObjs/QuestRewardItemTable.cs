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
/// Provides a strongly-typed structure for the database table `quest_reward_item`.
/// </summary>
public class QuestRewardItemTable : IQuestRewardItemTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"amount", "item_id", "quest_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"item_id", "quest_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"amount" };
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
public const System.String TableName = "quest_reward_item";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `item_id`.
/// </summary>
System.UInt16 _itemID;
/// <summary>
/// The field that maps onto the database column `quest_id`.
/// </summary>
System.Int32 _questId;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte Amount
{
get
{
return (System.Byte)_amount;
}
set
{
this._amount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public DemoGame.ItemID ItemID
{
get
{
return (DemoGame.ItemID)_itemID;
}
set
{
this._itemID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `quest_id`.
/// The underlying database type is `int(11)`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Int32 QuestId
{
get
{
return (System.Int32)_questId;
}
set
{
this._questId = (System.Int32)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IQuestRewardItemTable DeepCopy()
{
return new QuestRewardItemTable(this);
}
/// <summary>
/// QuestRewardItemTable constructor.
/// </summary>
public QuestRewardItemTable()
{
}
/// <summary>
/// QuestRewardItemTable constructor.
/// </summary>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="itemID">The initial value for the corresponding property.</param>
/// <param name="questId">The initial value for the corresponding property.</param>
public QuestRewardItemTable(System.Byte @amount, DemoGame.ItemID @itemID, System.Int32 @questId)
{
this.Amount = (System.Byte)@amount;
this.ItemID = (DemoGame.ItemID)@itemID;
this.QuestId = (System.Int32)@questId;
}
/// <summary>
/// QuestRewardItemTable constructor.
/// </summary>
/// <param name="source">IQuestRewardItemTable to copy the initial values from.</param>
public QuestRewardItemTable(IQuestRewardItemTable source)
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
public static void CopyValues(IQuestRewardItemTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@amount"] = (System.Byte)source.Amount;
dic["@item_id"] = (DemoGame.ItemID)source.ItemID;
dic["@quest_id"] = (System.Int32)source.QuestId;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this QuestRewardItemTable.
/// </summary>
/// <param name="source">The IQuestRewardItemTable to copy the values from.</param>
public void CopyValuesFrom(IQuestRewardItemTable source)
{
this.Amount = (System.Byte)source.Amount;
this.ItemID = (DemoGame.ItemID)source.ItemID;
this.QuestId = (System.Int32)source.QuestId;
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
case "amount":
return Amount;

case "item_id":
return ItemID;

case "quest_id":
return QuestId;

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
case "amount":
this.Amount = (System.Byte)value;
break;

case "item_id":
this.ItemID = (DemoGame.ItemID)value;
break;

case "quest_id":
this.QuestId = (System.Int32)value;
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
case "amount":
return new ColumnMetadata("amount", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "item_id":
return new ColumnMetadata("item_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "quest_id":
return new ColumnMetadata("quest_id", "", "int(11)", null, typeof(System.Int32), false, true, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
