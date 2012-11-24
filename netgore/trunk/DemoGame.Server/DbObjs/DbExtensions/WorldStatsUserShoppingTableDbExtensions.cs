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
/// Contains extension methods for class WorldStatsUserShoppingTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class WorldStatsUserShoppingTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IWorldStatsUserShoppingTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["amount"] = (System.Byte)source.Amount;
paramValues["character_id"] = (System.Int32)source.CharacterID;
paramValues["cost"] = (System.Int32)source.Cost;
paramValues["id"] = (System.UInt32)source.ID;
paramValues["item_template_id"] = (System.Nullable<System.UInt16>)source.ItemTemplateID;
paramValues["map_id"] = (System.Nullable<System.UInt16>)source.MapID;
paramValues["sale_type"] = (System.SByte)source.SaleType;
paramValues["shop_id"] = (System.UInt16)source.ShopID;
paramValues["when"] = (System.DateTime)source.When;
paramValues["x"] = (System.UInt16)source.X;
paramValues["y"] = (System.UInt16)source.Y;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this WorldStatsUserShoppingTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("amount");

source.Amount = (System.Byte)(System.Byte)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("character_id");

source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("cost");

source.Cost = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("id");

source.ID = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("item_template_id");

source.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)(System.Nullable<DemoGame.ItemTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("map_id");

source.MapID = (System.Nullable<NetGore.World.MapID>)(System.Nullable<NetGore.World.MapID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("sale_type");

source.SaleType = (System.SByte)(System.SByte)dataRecord.GetSByte(i);

i = dataRecord.GetOrdinal("shop_id");

source.ShopID = (NetGore.Features.Shops.ShopID)(NetGore.Features.Shops.ShopID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("when");

source.When = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);

i = dataRecord.GetOrdinal("x");

source.X = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("y");

source.Y = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
}

/// <summary>
/// Reads the values from an <see cref="IDataReader"/> and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the <see cref="IDataReader"/>, but also does not require the values in
/// the <see cref="IDataReader"/> to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataReader"/> to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this WorldStatsUserShoppingTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "amount":
source.Amount = (System.Byte)(System.Byte)dataRecord.GetByte(i);
break;


case "character_id":
source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataRecord.GetInt32(i);
break;


case "cost":
source.Cost = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "id":
source.ID = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "item_template_id":
source.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)(System.Nullable<DemoGame.ItemTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "map_id":
source.MapID = (System.Nullable<NetGore.World.MapID>)(System.Nullable<NetGore.World.MapID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "sale_type":
source.SaleType = (System.SByte)(System.SByte)dataRecord.GetSByte(i);
break;


case "shop_id":
source.ShopID = (NetGore.Features.Shops.ShopID)(NetGore.Features.Shops.ShopID)dataRecord.GetUInt16(i);
break;


case "when":
source.When = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
break;


case "x":
source.X = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "y":
source.Y = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


}

}
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The key must already exist in the DbParameterValues
/// for the value to be copied over. If any of the keys in the DbParameterValues do not
/// match one of the column names, or if there is no field for a key, then it will be
/// ignored. Because of this, it is important to be careful when using this method
/// since columns or keys can be skipped without any indication.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this IWorldStatsUserShoppingTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "amount":
paramValues[i] = (System.Byte)source.Amount;
break;


case "character_id":
paramValues[i] = (System.Int32)source.CharacterID;
break;


case "cost":
paramValues[i] = (System.Int32)source.Cost;
break;


case "id":
paramValues[i] = (System.UInt32)source.ID;
break;


case "item_template_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ItemTemplateID;
break;


case "map_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.MapID;
break;


case "sale_type":
paramValues[i] = (System.SByte)source.SaleType;
break;


case "shop_id":
paramValues[i] = (System.UInt16)source.ShopID;
break;


case "when":
paramValues[i] = (System.DateTime)source.When;
break;


case "x":
paramValues[i] = (System.UInt16)source.X;
break;


case "y":
paramValues[i] = (System.UInt16)source.Y;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IWorldStatsUserShoppingTable"/> contains the same values as another <see cref="IWorldStatsUserShoppingTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IWorldStatsUserShoppingTable"/>.</param>
/// <param name="otherItem">The <see cref="IWorldStatsUserShoppingTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IWorldStatsUserShoppingTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IWorldStatsUserShoppingTable source, IWorldStatsUserShoppingTable otherItem)
{
return Equals(source.Amount, otherItem.Amount) && 
Equals(source.CharacterID, otherItem.CharacterID) && 
Equals(source.Cost, otherItem.Cost) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.ItemTemplateID, otherItem.ItemTemplateID) && 
Equals(source.MapID, otherItem.MapID) && 
Equals(source.SaleType, otherItem.SaleType) && 
Equals(source.ShopID, otherItem.ShopID) && 
Equals(source.When, otherItem.When) && 
Equals(source.X, otherItem.X) && 
Equals(source.Y, otherItem.Y);
}

}

}
