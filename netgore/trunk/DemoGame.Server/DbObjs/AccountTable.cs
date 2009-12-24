using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `account`.
/// </summary>
public class AccountTable : IAccountTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"current_ip", "email", "id", "name", "password", "time_created", "time_last_login" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"current_ip", "email", "name", "password", "time_created", "time_last_login" };
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
public const System.String TableName = "account";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 7;
/// <summary>
/// The field that maps onto the database column `current_ip`.
/// </summary>
System.Nullable<System.UInt32> _currentIp;
/// <summary>
/// The field that maps onto the database column `email`.
/// </summary>
System.String _email;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `password`.
/// </summary>
System.String _password;
/// <summary>
/// The field that maps onto the database column `time_created`.
/// </summary>
System.DateTime _timeCreated;
/// <summary>
/// The field that maps onto the database column `time_last_login`.
/// </summary>
System.DateTime _timeLastLogin;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `current_ip`.
/// The underlying database type is `int(10) unsigned`. The database column contains the comment: 
/// "IP address currently logged in to the account, or null if nobody is logged in.".
/// </summary>
public System.Nullable<System.UInt32> CurrentIp
{
get
{
return (System.Nullable<System.UInt32>)_currentIp;
}
set
{
this._currentIp = (System.Nullable<System.UInt32>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `email`.
/// The underlying database type is `varchar(60)`. The database column contains the comment: 
/// "The email address.".
/// </summary>
public System.String Email
{
get
{
return (System.String)_email;
}
set
{
this._email = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`. The database column contains the comment: 
/// "The account ID.".
/// </summary>
public DemoGame.Server.AccountID ID
{
get
{
return (DemoGame.Server.AccountID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(30)`. The database column contains the comment: 
/// "The account name.".
/// </summary>
public System.String Name
{
get
{
return (System.String)_name;
}
set
{
this._name = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `password`.
/// The underlying database type is `varchar(40)`. The database column contains the comment: 
/// "The account password.".
/// </summary>
public System.String Password
{
get
{
return (System.String)_password;
}
set
{
this._password = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time_created`.
/// The underlying database type is `datetime`. The database column contains the comment: 
/// "The DateTime of when the account was created.".
/// </summary>
public System.DateTime TimeCreated
{
get
{
return (System.DateTime)_timeCreated;
}
set
{
this._timeCreated = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time_last_login`.
/// The underlying database type is `datetime`. The database column contains the comment: 
/// "The DateTime that the account was last logged in to.".
/// </summary>
public System.DateTime TimeLastLogin
{
get
{
return (System.DateTime)_timeLastLogin;
}
set
{
this._timeLastLogin = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IAccountTable DeepCopy()
{
return new AccountTable(this);
}
/// <summary>
/// AccountTable constructor.
/// </summary>
public AccountTable()
{
}
/// <summary>
/// AccountTable constructor.
/// </summary>
/// <param name="currentIp">The initial value for the corresponding property.</param>
/// <param name="email">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="password">The initial value for the corresponding property.</param>
/// <param name="timeCreated">The initial value for the corresponding property.</param>
/// <param name="timeLastLogin">The initial value for the corresponding property.</param>
public AccountTable(System.Nullable<System.UInt32> @currentIp, System.String @email, DemoGame.Server.AccountID @iD, System.String @name, System.String @password, System.DateTime @timeCreated, System.DateTime @timeLastLogin)
{
this.CurrentIp = (System.Nullable<System.UInt32>)@currentIp;
this.Email = (System.String)@email;
this.ID = (DemoGame.Server.AccountID)@iD;
this.Name = (System.String)@name;
this.Password = (System.String)@password;
this.TimeCreated = (System.DateTime)@timeCreated;
this.TimeLastLogin = (System.DateTime)@timeLastLogin;
}
/// <summary>
/// AccountTable constructor.
/// </summary>
/// <param name="source">IAccountTable to copy the initial values from.</param>
public AccountTable(IAccountTable source)
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
public static void CopyValues(IAccountTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@current_ip"] = (System.Nullable<System.UInt32>)source.CurrentIp;
dic["@email"] = (System.String)source.Email;
dic["@id"] = (DemoGame.Server.AccountID)source.ID;
dic["@name"] = (System.String)source.Name;
dic["@password"] = (System.String)source.Password;
dic["@time_created"] = (System.DateTime)source.TimeCreated;
dic["@time_last_login"] = (System.DateTime)source.TimeLastLogin;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AccountTable.
/// </summary>
/// <param name="source">The IAccountTable to copy the values from.</param>
public void CopyValuesFrom(IAccountTable source)
{
this.CurrentIp = (System.Nullable<System.UInt32>)source.CurrentIp;
this.Email = (System.String)source.Email;
this.ID = (DemoGame.Server.AccountID)source.ID;
this.Name = (System.String)source.Name;
this.Password = (System.String)source.Password;
this.TimeCreated = (System.DateTime)source.TimeCreated;
this.TimeLastLogin = (System.DateTime)source.TimeLastLogin;
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
case "current_ip":
return CurrentIp;

case "email":
return Email;

case "id":
return ID;

case "name":
return Name;

case "password":
return Password;

case "time_created":
return TimeCreated;

case "time_last_login":
return TimeLastLogin;

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
case "current_ip":
this.CurrentIp = (System.Nullable<System.UInt32>)value;
break;

case "email":
this.Email = (System.String)value;
break;

case "id":
this.ID = (DemoGame.Server.AccountID)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "password":
this.Password = (System.String)value;
break;

case "time_created":
this.TimeCreated = (System.DateTime)value;
break;

case "time_last_login":
this.TimeLastLogin = (System.DateTime)value;
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
case "current_ip":
return new ColumnMetadata("current_ip", "IP address currently logged in to the account, or null if nobody is logged in.", "int(10) unsigned", null, typeof(System.Nullable<System.UInt32>), true, false, false);

case "email":
return new ColumnMetadata("email", "The email address.", "varchar(60)", null, typeof(System.String), false, false, false);

case "id":
return new ColumnMetadata("id", "The account ID.", "int(11)", null, typeof(System.Int32), false, true, false);

case "name":
return new ColumnMetadata("name", "The account name.", "varchar(30)", null, typeof(System.String), false, false, true);

case "password":
return new ColumnMetadata("password", "The account password.", "varchar(40)", null, typeof(System.String), false, false, false);

case "time_created":
return new ColumnMetadata("time_created", "The DateTime of when the account was created.", "datetime", null, typeof(System.DateTime), false, false, false);

case "time_last_login":
return new ColumnMetadata("time_last_login", "The DateTime that the account was last logged in to.", "datetime", null, typeof(System.DateTime), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

}

}
