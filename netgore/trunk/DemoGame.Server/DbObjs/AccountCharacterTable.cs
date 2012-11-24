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
/// Provides a strongly-typed structure for the database table `account_character`.
/// </summary>
public class AccountCharacterTable : IAccountCharacterTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "character_id", "time_deleted" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"account_id", "time_deleted" };
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
public const System.String TableName = "account_character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Int32 _accountID;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `time_deleted`.
/// </summary>
System.Nullable<System.DateTime> _timeDeleted;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The account the character is on.".
/// </summary>
[System.ComponentModel.Description("The account the character is on.")]
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
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character in the account.".
/// </summary>
[System.ComponentModel.Description("The character in the account.")]
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
/// Gets or sets the value for the field that maps onto the database column `time_deleted`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the character was removed from the account (NULL if not removed).".
/// </summary>
[System.ComponentModel.Description("When the character was removed from the account (NULL if not removed).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.DateTime> TimeDeleted
{
get
{
return (System.Nullable<System.DateTime>)_timeDeleted;
}
set
{
this._timeDeleted = (System.Nullable<System.DateTime>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IAccountCharacterTable DeepCopy()
{
return new AccountCharacterTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountCharacterTable"/> class.
/// </summary>
public AccountCharacterTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountCharacterTable"/> class.
/// </summary>
/// <param name="accountID">The initial value for the corresponding property.</param>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="timeDeleted">The initial value for the corresponding property.</param>
public AccountCharacterTable(DemoGame.AccountID @accountID, DemoGame.CharacterID @characterID, System.Nullable<System.DateTime> @timeDeleted)
{
this.AccountID = (DemoGame.AccountID)@accountID;
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.TimeDeleted = (System.Nullable<System.DateTime>)@timeDeleted;
}
/// <summary>
/// Initializes a new instance of the <see cref="AccountCharacterTable"/> class.
/// </summary>
/// <param name="source">IAccountCharacterTable to copy the initial values from.</param>
public AccountCharacterTable(IAccountCharacterTable source)
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
public static void CopyValues(IAccountCharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["account_id"] = (DemoGame.AccountID)source.AccountID;
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["time_deleted"] = (System.Nullable<System.DateTime>)source.TimeDeleted;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AccountCharacterTable.
/// </summary>
/// <param name="source">The IAccountCharacterTable to copy the values from.</param>
public void CopyValuesFrom(IAccountCharacterTable source)
{
this.AccountID = (DemoGame.AccountID)source.AccountID;
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.TimeDeleted = (System.Nullable<System.DateTime>)source.TimeDeleted;
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

case "character_id":
return CharacterID;

case "time_deleted":
return TimeDeleted;

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

case "character_id":
this.CharacterID = (DemoGame.CharacterID)value;
break;

case "time_deleted":
this.TimeDeleted = (System.Nullable<System.DateTime>)value;
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
return new ColumnMetadata("account_id", "The account the character is on.", "int(11)", null, typeof(System.Int32), false, false, true);

case "character_id":
return new ColumnMetadata("character_id", "The character in the account.", "int(11)", null, typeof(System.Int32), false, true, false);

case "time_deleted":
return new ColumnMetadata("time_deleted", "When the character was removed from the account (NULL if not removed).", "datetime", null, typeof(System.Nullable<System.DateTime>), true, false, false);

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
