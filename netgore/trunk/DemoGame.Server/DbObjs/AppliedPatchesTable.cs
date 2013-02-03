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
/// Provides a strongly-typed structure for the database table `applied_patches`.
/// </summary>
public class AppliedPatchesTable : IAppliedPatchesTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"date_applied", "file_name" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"file_name" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"date_applied" };
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
public const System.String TableName = "applied_patches";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `date_applied`.
/// </summary>
System.DateTime _dateApplied;
/// <summary>
/// The field that maps onto the database column `file_name`.
/// </summary>
System.String _fileName;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `date_applied`.
/// The underlying database type is `datetime`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.DateTime DateApplied
{
get
{
return (System.DateTime)_dateApplied;
}
set
{
this._dateApplied = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `file_name`.
/// The underlying database type is `varchar(255)`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.String FileName
{
get
{
return (System.String)_fileName;
}
set
{
this._fileName = (System.String)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IAppliedPatchesTable DeepCopy()
{
return new AppliedPatchesTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="AppliedPatchesTable"/> class.
/// </summary>
public AppliedPatchesTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="AppliedPatchesTable"/> class.
/// </summary>
/// <param name="dateApplied">The initial value for the corresponding property.</param>
/// <param name="fileName">The initial value for the corresponding property.</param>
public AppliedPatchesTable(System.DateTime @dateApplied, System.String @fileName)
{
this.DateApplied = (System.DateTime)@dateApplied;
this.FileName = (System.String)@fileName;
}
/// <summary>
/// Initializes a new instance of the <see cref="AppliedPatchesTable"/> class.
/// </summary>
/// <param name="source">IAppliedPatchesTable to copy the initial values from.</param>
public AppliedPatchesTable(IAppliedPatchesTable source)
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
public static void CopyValues(IAppliedPatchesTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["date_applied"] = (System.DateTime)source.DateApplied;
dic["file_name"] = (System.String)source.FileName;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AppliedPatchesTable.
/// </summary>
/// <param name="source">The IAppliedPatchesTable to copy the values from.</param>
public void CopyValuesFrom(IAppliedPatchesTable source)
{
this.DateApplied = (System.DateTime)source.DateApplied;
this.FileName = (System.String)source.FileName;
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
case "date_applied":
return DateApplied;

case "file_name":
return FileName;

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
case "date_applied":
this.DateApplied = (System.DateTime)value;
break;

case "file_name":
this.FileName = (System.String)value;
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
case "date_applied":
return new ColumnMetadata("date_applied", "", "datetime", null, typeof(System.DateTime), false, false, false);

case "file_name":
return new ColumnMetadata("file_name", "", "varchar(255)", null, typeof(System.String), false, true, false);

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
