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
/// Provides a strongly-typed structure for the database table `world_stats_user_shopping`.
/// </summary>
public class WorldStatsUserShoppingTable : IWorldStatsUserShoppingTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"amount", "character_id", "cost", "id", "item_template_id", "map_id", "sale_type", "shop_id", "when", "x", "y" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"amount", "character_id", "cost", "item_template_id", "map_id", "sale_type", "shop_id", "when", "x", "y" };
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
public const System.String TableName = "world_stats_user_shopping";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 11;
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `cost`.
/// </summary>
System.Int32 _cost;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _iD;
/// <summary>
/// The field that maps onto the database column `item_template_id`.
/// </summary>
System.Nullable<System.UInt16> _itemTemplateID;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.Nullable<System.UInt16> _mapID;
/// <summary>
/// The field that maps onto the database column `sale_type`.
/// </summary>
System.SByte _saleType;
/// <summary>
/// The field that maps onto the database column `shop_id`.
/// </summary>
System.UInt16 _shopID;
/// <summary>
/// The field that maps onto the database column `when`.
/// </summary>
System.DateTime _when;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.UInt16 _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.UInt16 _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.".
/// </summary>
[System.ComponentModel.Description("The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.")]
[NetGore.SyncValueAttribute()]
public System.Byte Amount
{
get
{
return (System.Byte)_amount;
}
set
{
this._amount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The ID of the character that performed this transaction with the shop.".
/// </summary>
[System.ComponentModel.Description("The ID of the character that performed this transaction with the shop.")]
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
/// Gets or sets the value for the field that maps onto the database column `cost`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ".
/// </summary>
[System.ComponentModel.Description("The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ")]
[NetGore.SyncValueAttribute()]
public System.Int32 Cost
{
get
{
return (System.Int32)_cost;
}
set
{
this._cost = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(10) unsigned`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `item_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.".
/// </summary>
[System.ComponentModel.Description("The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.ItemTemplateID> ItemTemplateID
{
get
{
return (System.Nullable<DemoGame.ItemTemplateID>)_itemTemplateID;
}
set
{
this._itemTemplateID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The ID of the map the event took place on.".
/// </summary>
[System.ComponentModel.Description("The ID of the map the event took place on.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.World.MapID> MapID
{
get
{
return (System.Nullable<NetGore.World.MapID>)_mapID;
}
set
{
this._mapID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sale_type`.
/// The underlying database type is `tinyint(4)`.The database column contains the comment: 
/// "Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.".
/// </summary>
[System.ComponentModel.Description("Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.")]
[NetGore.SyncValueAttribute()]
public System.SByte SaleType
{
get
{
return (System.SByte)_saleType;
}
set
{
this._saleType = (System.SByte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `shop_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The ID of the shop the event took place at.".
/// </summary>
[System.ComponentModel.Description("The ID of the shop the event took place at.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Shops.ShopID ShopID
{
get
{
return (NetGore.Features.Shops.ShopID)_shopID;
}
set
{
this._shopID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `when`.
/// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`.The database column contains the comment: 
/// "When this event took place.".
/// </summary>
[System.ComponentModel.Description("When this event took place.")]
[NetGore.SyncValueAttribute()]
public System.DateTime When
{
get
{
return (System.DateTime)_when;
}
set
{
this._when = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.".
/// </summary>
[System.ComponentModel.Description("The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 X
{
get
{
return (System.UInt16)_x;
}
set
{
this._x = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `y`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.".
/// </summary>
[System.ComponentModel.Description("The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 Y
{
get
{
return (System.UInt16)_y;
}
set
{
this._y = (System.UInt16)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IWorldStatsUserShoppingTable DeepCopy()
{
return new WorldStatsUserShoppingTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsUserShoppingTable"/> class.
/// </summary>
public WorldStatsUserShoppingTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsUserShoppingTable"/> class.
/// </summary>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="cost">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="itemTemplateID">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="saleType">The initial value for the corresponding property.</param>
/// <param name="shopID">The initial value for the corresponding property.</param>
/// <param name="when">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public WorldStatsUserShoppingTable(System.Byte @amount, DemoGame.CharacterID @characterID, System.Int32 @cost, System.UInt32 @iD, System.Nullable<DemoGame.ItemTemplateID> @itemTemplateID, System.Nullable<NetGore.World.MapID> @mapID, System.SByte @saleType, NetGore.Features.Shops.ShopID @shopID, System.DateTime @when, System.UInt16 @x, System.UInt16 @y)
{
this.Amount = (System.Byte)@amount;
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.Cost = (System.Int32)@cost;
this.ID = (System.UInt32)@iD;
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)@itemTemplateID;
this.MapID = (System.Nullable<NetGore.World.MapID>)@mapID;
this.SaleType = (System.SByte)@saleType;
this.ShopID = (NetGore.Features.Shops.ShopID)@shopID;
this.When = (System.DateTime)@when;
this.X = (System.UInt16)@x;
this.Y = (System.UInt16)@y;
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsUserShoppingTable"/> class.
/// </summary>
/// <param name="source">IWorldStatsUserShoppingTable to copy the initial values from.</param>
public WorldStatsUserShoppingTable(IWorldStatsUserShoppingTable source)
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
public static void CopyValues(IWorldStatsUserShoppingTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["amount"] = (System.Byte)source.Amount;
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["cost"] = (System.Int32)source.Cost;
dic["id"] = (System.UInt32)source.ID;
dic["item_template_id"] = (System.Nullable<DemoGame.ItemTemplateID>)source.ItemTemplateID;
dic["map_id"] = (System.Nullable<NetGore.World.MapID>)source.MapID;
dic["sale_type"] = (System.SByte)source.SaleType;
dic["shop_id"] = (NetGore.Features.Shops.ShopID)source.ShopID;
dic["when"] = (System.DateTime)source.When;
dic["x"] = (System.UInt16)source.X;
dic["y"] = (System.UInt16)source.Y;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsUserShoppingTable.
/// </summary>
/// <param name="source">The IWorldStatsUserShoppingTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsUserShoppingTable source)
{
this.Amount = (System.Byte)source.Amount;
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.Cost = (System.Int32)source.Cost;
this.ID = (System.UInt32)source.ID;
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)source.ItemTemplateID;
this.MapID = (System.Nullable<NetGore.World.MapID>)source.MapID;
this.SaleType = (System.SByte)source.SaleType;
this.ShopID = (NetGore.Features.Shops.ShopID)source.ShopID;
this.When = (System.DateTime)source.When;
this.X = (System.UInt16)source.X;
this.Y = (System.UInt16)source.Y;
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
case "amount":
return Amount;

case "character_id":
return CharacterID;

case "cost":
return Cost;

case "id":
return ID;

case "item_template_id":
return ItemTemplateID;

case "map_id":
return MapID;

case "sale_type":
return SaleType;

case "shop_id":
return ShopID;

case "when":
return When;

case "x":
return X;

case "y":
return Y;

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
case "amount":
this.Amount = (System.Byte)value;
break;

case "character_id":
this.CharacterID = (DemoGame.CharacterID)value;
break;

case "cost":
this.Cost = (System.Int32)value;
break;

case "id":
this.ID = (System.UInt32)value;
break;

case "item_template_id":
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)value;
break;

case "map_id":
this.MapID = (System.Nullable<NetGore.World.MapID>)value;
break;

case "sale_type":
this.SaleType = (System.SByte)value;
break;

case "shop_id":
this.ShopID = (NetGore.Features.Shops.ShopID)value;
break;

case "when":
this.When = (System.DateTime)value;
break;

case "x":
this.X = (System.UInt16)value;
break;

case "y":
this.Y = (System.UInt16)value;
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
case "amount":
return new ColumnMetadata("amount", "The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "character_id":
return new ColumnMetadata("character_id", "The ID of the character that performed this transaction with the shop.", "int(11)", null, typeof(System.Int32), false, false, true);

case "cost":
return new ColumnMetadata("cost", "The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ", "int(11)", null, typeof(System.Int32), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(10) unsigned", null, typeof(System.UInt32), false, true, false);

case "item_template_id":
return new ColumnMetadata("item_template_id", "The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "map_id":
return new ColumnMetadata("map_id", "The ID of the map the event took place on.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "sale_type":
return new ColumnMetadata("sale_type", "Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.", "tinyint(4)", null, typeof(System.SByte), false, false, false);

case "shop_id":
return new ColumnMetadata("shop_id", "The ID of the shop the event took place at.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "when":
return new ColumnMetadata("when", "When this event took place.", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, false, false);

case "x":
return new ColumnMetadata("x", "The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "y":
return new ColumnMetadata("y", "The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

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
