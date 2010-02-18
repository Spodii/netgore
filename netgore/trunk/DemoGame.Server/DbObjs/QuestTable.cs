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
 static  readonly System.String[] _dbColumns = new string[] {"description", "id", "name", "repeatable" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"description", "name", "repeatable" };
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
/// The field that maps onto the database column `description`.
/// </summary>
System.String _description;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `repeatable`.
/// </summary>
System.Boolean _repeatable;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `description`.
/// The underlying database type is `text`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.String Description
{
get
{
return (System.String)_description;
}
set
{
this._description = (System.String)value;
}
}
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
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(0)`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `repeatable`.
/// The underlying database type is `tinyint(1) unsigned`.
/// </summary>
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IQuestTable DeepCopy()
{
return new QuestTable(this);
}
/// <summary>
/// QuestTable constructor.
/// </summary>
public QuestTable()
{
}
/// <summary>
/// QuestTable constructor.
/// </summary>
/// <param name="description">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="repeatable">The initial value for the corresponding property.</param>
public QuestTable(System.String @description, System.Int32 @iD, System.String @name, System.Boolean @repeatable)
{
this.Description = (System.String)@description;
this.ID = (System.Int32)@iD;
this.Name = (System.String)@name;
this.Repeatable = (System.Boolean)@repeatable;
}
/// <summary>
/// QuestTable constructor.
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
dic["@description"] = (System.String)source.Description;
dic["@id"] = (System.Int32)source.ID;
dic["@name"] = (System.String)source.Name;
dic["@repeatable"] = (System.Boolean)source.Repeatable;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this QuestTable.
/// </summary>
/// <param name="source">The IQuestTable to copy the values from.</param>
public void CopyValuesFrom(IQuestTable source)
{
this.Description = (System.String)source.Description;
this.ID = (System.Int32)source.ID;
this.Name = (System.String)source.Name;
this.Repeatable = (System.Boolean)source.Repeatable;
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
case "description":
return Description;

case "id":
return ID;

case "name":
return Name;

case "repeatable":
return Repeatable;

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
case "description":
this.Description = (System.String)value;
break;

case "id":
this.ID = (System.Int32)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "repeatable":
this.Repeatable = (System.Boolean)value;
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
case "description":
return new ColumnMetadata("description", "", "text", null, typeof(System.String), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "name":
return new ColumnMetadata("name", "", "varchar(0)", null, typeof(System.String), false, false, false);

case "repeatable":
return new ColumnMetadata("repeatable", "", "tinyint(1) unsigned", null, typeof(System.Boolean), false, false, false);

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
