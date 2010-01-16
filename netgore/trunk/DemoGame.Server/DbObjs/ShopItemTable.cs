using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `shop_item`.
/// </summary>
public class ShopItemTable : IShopItemTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"item_template_id", "shop_id" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"item_template_id", "shop_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] { };
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
public const System.String TableName = "shop_item";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 2;
/// <summary>
/// The field that maps onto the database column `item_template_id`.
/// </summary>
System.UInt16 _itemTemplateID;
/// <summary>
/// The field that maps onto the database column `shop_id`.
/// </summary>
System.UInt16 _shopID;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemTemplateID ItemTemplateID
{
get
{
return (DemoGame.Server.ItemTemplateID)_itemTemplateID;
}
set
{
this._itemTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `shop_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ShopID ShopID
{
get
{
return (DemoGame.Server.ShopID)_shopID;
}
set
{
this._shopID = (System.UInt16)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IShopItemTable DeepCopy()
{
return new ShopItemTable(this);
}
/// <summary>
/// ShopItemTable constructor.
/// </summary>
public ShopItemTable()
{
}
/// <summary>
/// ShopItemTable constructor.
/// </summary>
/// <param name="itemTemplateID">The initial value for the corresponding property.</param>
/// <param name="shopID">The initial value for the corresponding property.</param>
public ShopItemTable(DemoGame.Server.ItemTemplateID @itemTemplateID, DemoGame.Server.ShopID @shopID)
{
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)@itemTemplateID;
this.ShopID = (DemoGame.Server.ShopID)@shopID;
}
/// <summary>
/// ShopItemTable constructor.
/// </summary>
/// <param name="source">IShopItemTable to copy the initial values from.</param>
public ShopItemTable(IShopItemTable source)
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
public static void CopyValues(IShopItemTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@item_template_id"] = (DemoGame.Server.ItemTemplateID)source.ItemTemplateID;
dic["@shop_id"] = (DemoGame.Server.ShopID)source.ShopID;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this ShopItemTable.
/// </summary>
/// <param name="source">The IShopItemTable to copy the values from.</param>
public void CopyValuesFrom(IShopItemTable source)
{
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)source.ItemTemplateID;
this.ShopID = (DemoGame.Server.ShopID)source.ShopID;
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
case "item_template_id":
return ItemTemplateID;

case "shop_id":
return ShopID;

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
case "item_template_id":
this.ItemTemplateID = (DemoGame.Server.ItemTemplateID)value;
break;

case "shop_id":
this.ShopID = (DemoGame.Server.ShopID)value;
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
case "item_template_id":
return new ColumnMetadata("item_template_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "shop_id":
return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

}

}
