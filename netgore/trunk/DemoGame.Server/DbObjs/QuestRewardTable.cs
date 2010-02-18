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
/// Provides a strongly-typed structure for the database table `quest_reward`.
/// </summary>
public class QuestRewardTable : IQuestRewardTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"id", "quest_id", "type", "value" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"quest_id", "type", "value" };
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
public const System.String TableName = "quest_reward";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `quest_id`.
/// </summary>
System.Int32 _questId;
/// <summary>
/// The field that maps onto the database column `type`.
/// </summary>
System.Byte _type;
/// <summary>
/// The field that maps onto the database column `value`.
/// </summary>
System.String _value;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `type`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte Type
{
get
{
return (System.Byte)_type;
}
set
{
this._type = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `value`.
/// The underlying database type is `text`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.String Value
{
get
{
return (System.String)_value;
}
set
{
this._value = (System.String)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IQuestRewardTable DeepCopy()
{
return new QuestRewardTable(this);
}
/// <summary>
/// QuestRewardTable constructor.
/// </summary>
public QuestRewardTable()
{
}
/// <summary>
/// QuestRewardTable constructor.
/// </summary>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="questId">The initial value for the corresponding property.</param>
/// <param name="type">The initial value for the corresponding property.</param>
/// <param name="value">The initial value for the corresponding property.</param>
public QuestRewardTable(System.Int32 @iD, System.Int32 @questId, System.Byte @type, System.String @value)
{
this.ID = (System.Int32)@iD;
this.QuestId = (System.Int32)@questId;
this.Type = (System.Byte)@type;
this.Value = (System.String)@value;
}
/// <summary>
/// QuestRewardTable constructor.
/// </summary>
/// <param name="source">IQuestRewardTable to copy the initial values from.</param>
public QuestRewardTable(IQuestRewardTable source)
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
public static void CopyValues(IQuestRewardTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@id"] = (System.Int32)source.ID;
dic["@quest_id"] = (System.Int32)source.QuestId;
dic["@type"] = (System.Byte)source.Type;
dic["@value"] = (System.String)source.Value;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this QuestRewardTable.
/// </summary>
/// <param name="source">The IQuestRewardTable to copy the values from.</param>
public void CopyValuesFrom(IQuestRewardTable source)
{
this.ID = (System.Int32)source.ID;
this.QuestId = (System.Int32)source.QuestId;
this.Type = (System.Byte)source.Type;
this.Value = (System.String)source.Value;
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

case "quest_id":
return QuestId;

case "type":
return Type;

case "value":
return Value;

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
this.ID = (System.Int32)value;
break;

case "quest_id":
this.QuestId = (System.Int32)value;
break;

case "type":
this.Type = (System.Byte)value;
break;

case "value":
this.Value = (System.String)value;
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
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "quest_id":
return new ColumnMetadata("quest_id", "", "int(11)", null, typeof(System.Int32), false, false, true);

case "type":
return new ColumnMetadata("type", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "value":
return new ColumnMetadata("value", "", "text", null, typeof(System.String), true, false, false);

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
