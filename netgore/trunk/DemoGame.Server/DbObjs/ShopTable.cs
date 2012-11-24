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
/// Provides a strongly-typed structure for the database table `shop`.
/// </summary>
public class ShopTable : IShopTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"can_buy", "id", "name" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"can_buy", "name" };
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
public const System.String TableName = "shop";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `can_buy`.
/// </summary>
System.Boolean _canBuy;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _iD;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `can_buy`.
/// The underlying database type is `tinyint(1)`.The database column contains the comment: 
/// "Whether or not this shop can buy items from shoppers. When false, the shop only sells items (users cannot sell to it).".
/// </summary>
[System.ComponentModel.Description("Whether or not this shop can buy items from shoppers. When false, the shop only sells items (users cannot sell to it).")]
[NetGore.SyncValueAttribute()]
public System.Boolean CanBuy
{
get
{
return (System.Boolean)_canBuy;
}
set
{
this._canBuy = (System.Boolean)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The unique ID of the shop.".
/// </summary>
[System.ComponentModel.Description("The unique ID of the shop.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Shops.ShopID ID
{
get
{
return (NetGore.Features.Shops.ShopID)_iD;
}
set
{
this._iD = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(60)`.The database column contains the comment: 
/// "The name of this shop.".
/// </summary>
[System.ComponentModel.Description("The name of this shop.")]
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IShopTable DeepCopy()
{
return new ShopTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="ShopTable"/> class.
/// </summary>
public ShopTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="ShopTable"/> class.
/// </summary>
/// <param name="canBuy">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
public ShopTable(System.Boolean @canBuy, NetGore.Features.Shops.ShopID @iD, System.String @name)
{
this.CanBuy = (System.Boolean)@canBuy;
this.ID = (NetGore.Features.Shops.ShopID)@iD;
this.Name = (System.String)@name;
}
/// <summary>
/// Initializes a new instance of the <see cref="ShopTable"/> class.
/// </summary>
/// <param name="source">IShopTable to copy the initial values from.</param>
public ShopTable(IShopTable source)
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
public static void CopyValues(IShopTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["can_buy"] = (System.Boolean)source.CanBuy;
dic["id"] = (NetGore.Features.Shops.ShopID)source.ID;
dic["name"] = (System.String)source.Name;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this ShopTable.
/// </summary>
/// <param name="source">The IShopTable to copy the values from.</param>
public void CopyValuesFrom(IShopTable source)
{
this.CanBuy = (System.Boolean)source.CanBuy;
this.ID = (NetGore.Features.Shops.ShopID)source.ID;
this.Name = (System.String)source.Name;
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
case "can_buy":
return CanBuy;

case "id":
return ID;

case "name":
return Name;

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
case "can_buy":
this.CanBuy = (System.Boolean)value;
break;

case "id":
this.ID = (NetGore.Features.Shops.ShopID)value;
break;

case "name":
this.Name = (System.String)value;
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
case "can_buy":
return new ColumnMetadata("can_buy", "Whether or not this shop can buy items from shoppers. When false, the shop only sells items (users cannot sell to it).", "tinyint(1)", null, typeof(System.Boolean), false, false, false);

case "id":
return new ColumnMetadata("id", "The unique ID of the shop.", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "name":
return new ColumnMetadata("name", "The name of this shop.", "varchar(60)", null, typeof(System.String), false, false, false);

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
