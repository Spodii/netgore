using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `log_account_activity`.
/// </summary>
public class LogAccountActivityTable : ILogAccountActivityTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "id", "ip", "time_login", "time_logout" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"account_id", "ip", "time_login", "time_logout" };
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
public const System.String TableName = "log_account_activity";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 5;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Int32 _accountId;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `ip`.
/// </summary>
System.UInt32 _ip;
/// <summary>
/// The field that maps onto the database column `time_login`.
/// </summary>
System.DateTime _timeLogin;
/// <summary>
/// The field that maps onto the database column `time_logout`.
/// </summary>
System.Nullable<System.DateTime> _timeLogout;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public System.Int32 AccountId
{
get
{
return (System.Int32)_accountId;
}
set
{
this._accountId = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `ip`.
/// The underlying database type is `int(11) unsigned`. The database column contains the comment: 
/// "The IP address used, formatted as an unsigned 32-bit integer".
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `time_login`.
/// The underlying database type is `datetime`.
/// </summary>
public System.DateTime TimeLogin
{
get
{
return (System.DateTime)_timeLogin;
}
set
{
this._timeLogin = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time_logout`.
/// The underlying database type is `datetime`.
/// </summary>
public System.Nullable<System.DateTime> TimeLogout
{
get
{
return (System.Nullable<System.DateTime>)_timeLogout;
}
set
{
this._timeLogout = (System.Nullable<System.DateTime>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public ILogAccountActivityTable DeepCopy()
{
return new LogAccountActivityTable(this);
}
/// <summary>
/// LogAccountActivityTable constructor.
/// </summary>
public LogAccountActivityTable()
{
}
/// <summary>
/// LogAccountActivityTable constructor.
/// </summary>
/// <param name="accountId">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="ip">The initial value for the corresponding property.</param>
/// <param name="timeLogin">The initial value for the corresponding property.</param>
/// <param name="timeLogout">The initial value for the corresponding property.</param>
public LogAccountActivityTable(System.Int32 @accountId, System.Int32 @iD, System.UInt32 @ip, System.DateTime @timeLogin, System.Nullable<System.DateTime> @timeLogout)
{
this.AccountId = (System.Int32)@accountId;
this.ID = (System.Int32)@iD;
this.Ip = (System.UInt32)@ip;
this.TimeLogin = (System.DateTime)@timeLogin;
this.TimeLogout = (System.Nullable<System.DateTime>)@timeLogout;
}
/// <summary>
/// LogAccountActivityTable constructor.
/// </summary>
/// <param name="source">ILogAccountActivityTable to copy the initial values from.</param>
public LogAccountActivityTable(ILogAccountActivityTable source)
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
public static void CopyValues(ILogAccountActivityTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@account_id"] = (System.Int32)source.AccountId;
dic["@id"] = (System.Int32)source.ID;
dic["@ip"] = (System.UInt32)source.Ip;
dic["@time_login"] = (System.DateTime)source.TimeLogin;
dic["@time_logout"] = (System.Nullable<System.DateTime>)source.TimeLogout;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this LogAccountActivityTable.
/// </summary>
/// <param name="source">The ILogAccountActivityTable to copy the values from.</param>
public void CopyValuesFrom(ILogAccountActivityTable source)
{
this.AccountId = (System.Int32)source.AccountId;
this.ID = (System.Int32)source.ID;
this.Ip = (System.UInt32)source.Ip;
this.TimeLogin = (System.DateTime)source.TimeLogin;
this.TimeLogout = (System.Nullable<System.DateTime>)source.TimeLogout;
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
return AccountId;

case "id":
return ID;

case "ip":
return Ip;

case "time_login":
return TimeLogin;

case "time_logout":
return TimeLogout;

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
this.AccountId = (System.Int32)value;
break;

case "id":
this.ID = (System.Int32)value;
break;

case "ip":
this.Ip = (System.UInt32)value;
break;

case "time_login":
this.TimeLogin = (System.DateTime)value;
break;

case "time_logout":
this.TimeLogout = (System.Nullable<System.DateTime>)value;
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
return new ColumnMetadata("account_id", "", "int(11)", null, typeof(System.Int32), false, false, true);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "ip":
return new ColumnMetadata("ip", "The IP address used, formatted as an unsigned 32-bit integer", "int(11) unsigned", null, typeof(System.UInt32), false, false, false);

case "time_login":
return new ColumnMetadata("time_login", "", "datetime", null, typeof(System.DateTime), false, false, false);

case "time_logout":
return new ColumnMetadata("time_logout", "", "datetime", null, typeof(System.Nullable<System.DateTime>), true, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

}

}
