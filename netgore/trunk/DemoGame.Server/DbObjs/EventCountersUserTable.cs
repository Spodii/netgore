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
/// Provides a strongly-typed structure for the database table `event_counters_user`.
/// </summary>
public class EventCountersUserTable : IEventCountersUserTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"counter", "user_event_counter_id", "user_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"user_event_counter_id", "user_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"counter" };
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
public const System.String TableName = "event_counters_user";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `counter`.
/// </summary>
System.Int64 _counter;
/// <summary>
/// The field that maps onto the database column `user_event_counter_id`.
/// </summary>
System.Byte _userEventCounterId;
/// <summary>
/// The field that maps onto the database column `user_id`.
/// </summary>
System.Int32 _userID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `counter`.
/// The underlying database type is `bigint(20)`.The database column contains the comment: 
/// "The event counter.".
/// </summary>
[System.ComponentModel.Description("The event counter.")]
[NetGore.SyncValueAttribute()]
public System.Int64 Counter
{
get
{
return (System.Int64)_counter;
}
set
{
this._counter = (System.Int64)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_event_counter_id`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The ID of the event that the counter is for.".
/// </summary>
[System.ComponentModel.Description("The ID of the event that the counter is for.")]
[NetGore.SyncValueAttribute()]
public System.Byte UserEventCounterId
{
get
{
return (System.Byte)_userEventCounterId;
}
set
{
this._userEventCounterId = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `user_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character ID for the user character the event occured on.".
/// </summary>
[System.ComponentModel.Description("The character ID for the user character the event occured on.")]
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
public virtual IEventCountersUserTable DeepCopy()
{
return new EventCountersUserTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="EventCountersUserTable"/> class.
/// </summary>
public EventCountersUserTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="EventCountersUserTable"/> class.
/// </summary>
/// <param name="counter">The initial value for the corresponding property.</param>
/// <param name="userEventCounterId">The initial value for the corresponding property.</param>
/// <param name="userID">The initial value for the corresponding property.</param>
public EventCountersUserTable(System.Int64 @counter, System.Byte @userEventCounterId, DemoGame.CharacterID @userID)
{
this.Counter = (System.Int64)@counter;
this.UserEventCounterId = (System.Byte)@userEventCounterId;
this.UserID = (DemoGame.CharacterID)@userID;
}
/// <summary>
/// Initializes a new instance of the <see cref="EventCountersUserTable"/> class.
/// </summary>
/// <param name="source">IEventCountersUserTable to copy the initial values from.</param>
public EventCountersUserTable(IEventCountersUserTable source)
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
public static void CopyValues(IEventCountersUserTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["counter"] = (System.Int64)source.Counter;
dic["user_event_counter_id"] = (System.Byte)source.UserEventCounterId;
dic["user_id"] = (DemoGame.CharacterID)source.UserID;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this EventCountersUserTable.
/// </summary>
/// <param name="source">The IEventCountersUserTable to copy the values from.</param>
public void CopyValuesFrom(IEventCountersUserTable source)
{
this.Counter = (System.Int64)source.Counter;
this.UserEventCounterId = (System.Byte)source.UserEventCounterId;
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
case "counter":
return Counter;

case "user_event_counter_id":
return UserEventCounterId;

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
case "counter":
this.Counter = (System.Int64)value;
break;

case "user_event_counter_id":
this.UserEventCounterId = (System.Byte)value;
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
case "counter":
return new ColumnMetadata("counter", "The event counter.", "bigint(20)", null, typeof(System.Int64), false, false, false);

case "user_event_counter_id":
return new ColumnMetadata("user_event_counter_id", "The ID of the event that the counter is for.", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

case "user_id":
return new ColumnMetadata("user_id", "The character ID for the user character the event occured on.", "int(11)", null, typeof(System.Int32), false, true, false);

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
