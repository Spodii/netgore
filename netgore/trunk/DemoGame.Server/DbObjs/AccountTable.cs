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
/// Provides a strongly-typed structure for the database table `account`.
/// </summary>
public class AccountTable : IAccountTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"creator_ip", "current_ip", "email", "friends", "id", "name", "password", "permissions", "time_created", "time_last_login" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"creator_ip", "current_ip", "email", "friends", "name", "password", "permissions", "time_created", "time_last_login" };
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
public const System.Int32 ColumnCount = 10;
/// <summary>
/// The field that maps onto the database column `creator_ip`.
/// </summary>
System.UInt32 _creatorIp;
/// <summary>
/// The field that maps onto the database column `current_ip`.
/// </summary>
System.Nullable<System.UInt32> _currentIp;
/// <summary>
/// The field that maps onto the database column `email`.
/// </summary>
System.String _email;
/// <summary>
/// The field that maps onto the database column `friends`.
/// </summary>
System.String _friends;
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
/// The field that maps onto the database column `permissions`.
/// </summary>
System.Byte _permissions;
/// <summary>
/// The field that maps onto the database column `time_created`.
/// </summary>
System.DateTime _timeCreated;
/// <summary>
/// The field that maps onto the database column `time_last_login`.
/// </summary>
System.DateTime _timeLastLogin;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `creator_ip`.
/// The underlying database type is `int(10) unsigned`.The database column contains the comment: 
/// "The IP address that created the account.".
/// </summary>
[System.ComponentModel.Description("The IP address that created the account.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 CreatorIp
{
get
{
return (System.UInt32)_creatorIp;
}
set
{
this._creatorIp = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `current_ip`.
/// The underlying database type is `int(10) unsigned`.The database column contains the comment: 
/// "IP address currently logged in to the account, or null if nobody is logged in.".
/// </summary>
[System.ComponentModel.Description("IP address currently logged in to the account, or null if nobody is logged in.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `varchar(60)`.The database column contains the comment: 
/// "The email address.".
/// </summary>
[System.ComponentModel.Description("The email address.")]
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `friends`.
/// The underlying database type is `varchar(800)`.The database column contains the comment: 
/// "A list of friends that the user has.".
/// </summary>
[System.ComponentModel.Description("A list of friends that the user has.")]
[NetGore.SyncValueAttribute()]
public System.String Friends
{
get
{
return (System.String)_friends;
}
set
{
this._friends = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The unique ID of the account.".
/// </summary>
[System.ComponentModel.Description("The unique ID of the account.")]
[NetGore.SyncValueAttribute()]
public DemoGame.AccountID ID
{
get
{
return (DemoGame.AccountID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(30)`.The database column contains the comment: 
/// "The account name.".
/// </summary>
[System.ComponentModel.Description("The account name.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `char(32)`.The database column contains the comment: 
/// "The account password (MD5 hashed).".
/// </summary>
[System.ComponentModel.Description("The account password (MD5 hashed).")]
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `permissions`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.The database column contains the comment: 
/// "The permission level bit mask (see UserPermissions enum).".
/// </summary>
[System.ComponentModel.Description("The permission level bit mask (see UserPermissions enum).")]
[NetGore.SyncValueAttribute()]
public DemoGame.UserPermissions Permissions
{
get
{
return (DemoGame.UserPermissions)_permissions;
}
set
{
this._permissions = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time_created`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the account was created.".
/// </summary>
[System.ComponentModel.Description("When the account was created.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the account was last logged in to.".
/// </summary>
[System.ComponentModel.Description("When the account was last logged in to.")]
[NetGore.SyncValueAttribute()]
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
public virtual IAccountTable DeepCopy()
{
return new AccountTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountTable"/> class.
/// </summary>
public AccountTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountTable"/> class.
/// </summary>
/// <param name="creatorIp">The initial value for the corresponding property.</param>
/// <param name="currentIp">The initial value for the corresponding property.</param>
/// <param name="email">The initial value for the corresponding property.</param>
/// <param name="friends">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="password">The initial value for the corresponding property.</param>
/// <param name="permissions">The initial value for the corresponding property.</param>
/// <param name="timeCreated">The initial value for the corresponding property.</param>
/// <param name="timeLastLogin">The initial value for the corresponding property.</param>
public AccountTable(System.UInt32 @creatorIp, System.Nullable<System.UInt32> @currentIp, System.String @email, System.String @friends, DemoGame.AccountID @iD, System.String @name, System.String @password, DemoGame.UserPermissions @permissions, System.DateTime @timeCreated, System.DateTime @timeLastLogin)
{
this.CreatorIp = (System.UInt32)@creatorIp;
this.CurrentIp = (System.Nullable<System.UInt32>)@currentIp;
this.Email = (System.String)@email;
this.Friends = (System.String)@friends;
this.ID = (DemoGame.AccountID)@iD;
this.Name = (System.String)@name;
this.Password = (System.String)@password;
this.Permissions = (DemoGame.UserPermissions)@permissions;
this.TimeCreated = (System.DateTime)@timeCreated;
this.TimeLastLogin = (System.DateTime)@timeLastLogin;
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountTable"/> class.
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
dic["creator_ip"] = (System.UInt32)source.CreatorIp;
dic["current_ip"] = (System.Nullable<System.UInt32>)source.CurrentIp;
dic["email"] = (System.String)source.Email;
dic["friends"] = (System.String)source.Friends;
dic["id"] = (DemoGame.AccountID)source.ID;
dic["name"] = (System.String)source.Name;
dic["password"] = (System.String)source.Password;
dic["permissions"] = (DemoGame.UserPermissions)source.Permissions;
dic["time_created"] = (System.DateTime)source.TimeCreated;
dic["time_last_login"] = (System.DateTime)source.TimeLastLogin;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AccountTable.
/// </summary>
/// <param name="source">The IAccountTable to copy the values from.</param>
public void CopyValuesFrom(IAccountTable source)
{
this.CreatorIp = (System.UInt32)source.CreatorIp;
this.CurrentIp = (System.Nullable<System.UInt32>)source.CurrentIp;
this.Email = (System.String)source.Email;
this.Friends = (System.String)source.Friends;
this.ID = (DemoGame.AccountID)source.ID;
this.Name = (System.String)source.Name;
this.Password = (System.String)source.Password;
this.Permissions = (DemoGame.UserPermissions)source.Permissions;
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
case "creator_ip":
return CreatorIp;

case "current_ip":
return CurrentIp;

case "email":
return Email;

case "friends":
return Friends;

case "id":
return ID;

case "name":
return Name;

case "password":
return Password;

case "permissions":
return Permissions;

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
case "creator_ip":
this.CreatorIp = (System.UInt32)value;
break;

case "current_ip":
this.CurrentIp = (System.Nullable<System.UInt32>)value;
break;

case "email":
this.Email = (System.String)value;
break;

case "friends":
this.Friends = (System.String)value;
break;

case "id":
this.ID = (DemoGame.AccountID)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "password":
this.Password = (System.String)value;
break;

case "permissions":
this.Permissions = (DemoGame.UserPermissions)value;
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
case "creator_ip":
return new ColumnMetadata("creator_ip", "The IP address that created the account.", "int(10) unsigned", null, typeof(System.UInt32), false, false, false);

case "current_ip":
return new ColumnMetadata("current_ip", "IP address currently logged in to the account, or null if nobody is logged in.", "int(10) unsigned", null, typeof(System.Nullable<System.UInt32>), true, false, false);

case "email":
return new ColumnMetadata("email", "The email address.", "varchar(60)", null, typeof(System.String), false, false, false);

case "friends":
return new ColumnMetadata("friends", "A list of friends that the user has.", "varchar(800)", "", typeof(System.String), false, false, false);

case "id":
return new ColumnMetadata("id", "The unique ID of the account.", "int(11)", null, typeof(System.Int32), false, true, false);

case "name":
return new ColumnMetadata("name", "The account name.", "varchar(30)", null, typeof(System.String), false, false, true);

case "password":
return new ColumnMetadata("password", "The account password (MD5 hashed).", "char(32)", null, typeof(System.String), false, false, false);

case "permissions":
return new ColumnMetadata("permissions", "The permission level bit mask (see UserPermissions enum).", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "time_created":
return new ColumnMetadata("time_created", "When the account was created.", "datetime", null, typeof(System.DateTime), false, false, false);

case "time_last_login":
return new ColumnMetadata("time_last_login", "When the account was last logged in to.", "datetime", null, typeof(System.DateTime), false, false, false);

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
