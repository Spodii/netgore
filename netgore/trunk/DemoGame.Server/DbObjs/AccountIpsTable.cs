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
/// Provides a strongly-typed structure for the database table `account_ips`.
/// </summary>
public class AccountIpsTable : IAccountIpsTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "id", "ip", "time" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"account_id", "ip", "time" };
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
public const System.String TableName = "account_ips";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Int32 _accountID;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _iD;
/// <summary>
/// The field that maps onto the database column `ip`.
/// </summary>
System.UInt32 _ip;
/// <summary>
/// The field that maps onto the database column `time`.
/// </summary>
System.DateTime _time;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The ID of the account.".
/// </summary>
[System.ComponentModel.Description("The ID of the account.")]
[NetGore.SyncValueAttribute()]
public DemoGame.AccountID AccountID
{
get
{
return (DemoGame.AccountID)_accountID;
}
set
{
this._accountID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(10) unsigned`.The database column contains the comment: 
/// "The unique row ID.".
/// </summary>
[System.ComponentModel.Description("The unique row ID.")]
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
/// Gets or sets the value for the field that maps onto the database column `ip`.
/// The underlying database type is `int(10) unsigned`.The database column contains the comment: 
/// "The IP that logged into the account.".
/// </summary>
[System.ComponentModel.Description("The IP that logged into the account.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 Ip
{
get
{
return (System.UInt32)_ip;
}
set
{
this._ip = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When this IP last logged into this account.".
/// </summary>
[System.ComponentModel.Description("When this IP last logged into this account.")]
[NetGore.SyncValueAttribute()]
public System.DateTime Time
{
get
{
return (System.DateTime)_time;
}
set
{
this._time = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IAccountIpsTable DeepCopy()
{
return new AccountIpsTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
/// </summary>
public AccountIpsTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
/// </summary>
/// <param name="accountID">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="ip">The initial value for the corresponding property.</param>
/// <param name="time">The initial value for the corresponding property.</param>
public AccountIpsTable(DemoGame.AccountID @accountID, System.UInt32 @iD, System.UInt32 @ip, System.DateTime @time)
{
this.AccountID = (DemoGame.AccountID)@accountID;
this.ID = (System.UInt32)@iD;
this.Ip = (System.UInt32)@ip;
this.Time = (System.DateTime)@time;
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
/// </summary>
/// <param name="source">IAccountIpsTable to copy the initial values from.</param>
public AccountIpsTable(IAccountIpsTable source)
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
public static void CopyValues(IAccountIpsTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["account_id"] = (DemoGame.AccountID)source.AccountID;
dic["id"] = (System.UInt32)source.ID;
dic["ip"] = (System.UInt32)source.Ip;
dic["time"] = (System.DateTime)source.Time;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AccountIpsTable.
/// </summary>
/// <param name="source">The IAccountIpsTable to copy the values from.</param>
public void CopyValuesFrom(IAccountIpsTable source)
{
this.AccountID = (DemoGame.AccountID)source.AccountID;
this.ID = (System.UInt32)source.ID;
this.Ip = (System.UInt32)source.Ip;
this.Time = (System.DateTime)source.Time;
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
case "account_id":
return AccountID;

case "id":
return ID;

case "ip":
return Ip;

case "time":
return Time;

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
case "account_id":
this.AccountID = (DemoGame.AccountID)value;
break;

case "id":
this.ID = (System.UInt32)value;
break;

case "ip":
this.Ip = (System.UInt32)value;
break;

case "time":
this.Time = (System.DateTime)value;
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
case "account_id":
return new ColumnMetadata("account_id", "The ID of the account.", "int(11)", null, typeof(System.Int32), false, false, true);

case "id":
return new ColumnMetadata("id", "The unique row ID.", "int(10) unsigned", null, typeof(System.UInt32), false, true, false);

case "ip":
return new ColumnMetadata("ip", "The IP that logged into the account.", "int(10) unsigned", null, typeof(System.UInt32), false, false, false);

case "time":
return new ColumnMetadata("time", "When this IP last logged into this account.", "datetime", null, typeof(System.DateTime), false, false, false);

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
