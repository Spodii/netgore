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
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/16/2010 6:43:47 PM
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
/// Contains extension methods for class GameConstantTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class GameConstantTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IGameConstantTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@max_account_name_length"] = (System.Byte)source.MaxAccountNameLength;
paramValues["@max_account_password_length"] = (System.Byte)source.MaxAccountPasswordLength;
paramValues["@max_characters_per_account"] = (System.Byte)source.MaxCharactersPerAccount;
paramValues["@max_character_name_length"] = (System.Byte)source.MaxCharacterNameLength;
paramValues["@max_inventory_size"] = (System.Byte)source.MaxInventorySize;
paramValues["@max_shop_items"] = (System.Byte)source.MaxShopItems;
paramValues["@max_status_effect_power"] = (System.UInt16)source.MaxStatusEffectPower;
paramValues["@min_account_name_length"] = (System.Byte)source.MinAccountNameLength;
paramValues["@min_account_password_length"] = (System.Byte)source.MinAccountPasswordLength;
paramValues["@min_character_name_length"] = (System.Byte)source.MinCharacterNameLength;
paramValues["@screen_height"] = (System.UInt16)source.ScreenHeight;
paramValues["@screen_width"] = (System.UInt16)source.ScreenWidth;
paramValues["@server_ip"] = (System.String)source.ServerIp;
paramValues["@server_ping_port"] = (System.UInt16)source.ServerPingPort;
paramValues["@server_tcp_port"] = (System.UInt16)source.ServerTcpPort;
paramValues["@world_physics_update_rate"] = (System.UInt16)source.WorldPhysicsUpdateRate;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this GameConstantTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("max_account_name_length");

source.MaxAccountNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_account_password_length");

source.MaxAccountPasswordLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_characters_per_account");

source.MaxCharactersPerAccount = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_character_name_length");

source.MaxCharacterNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_inventory_size");

source.MaxInventorySize = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_shop_items");

source.MaxShopItems = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("max_status_effect_power");

source.MaxStatusEffectPower = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("min_account_name_length");

source.MinAccountNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("min_account_password_length");

source.MinAccountPasswordLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("min_character_name_length");

source.MinCharacterNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("screen_height");

source.ScreenHeight = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("screen_width");

source.ScreenWidth = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("server_ip");

source.ServerIp = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("server_ping_port");

source.ServerPingPort = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("server_tcp_port");

source.ServerTcpPort = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("world_physics_update_rate");

source.WorldPhysicsUpdateRate = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this GameConstantTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "max_account_name_length":
source.MaxAccountNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_account_password_length":
source.MaxAccountPasswordLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_characters_per_account":
source.MaxCharactersPerAccount = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_character_name_length":
source.MaxCharacterNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_inventory_size":
source.MaxInventorySize = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_shop_items":
source.MaxShopItems = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "max_status_effect_power":
source.MaxStatusEffectPower = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "min_account_name_length":
source.MinAccountNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "min_account_password_length":
source.MinAccountPasswordLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "min_character_name_length":
source.MinCharacterNameLength = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "screen_height":
source.ScreenHeight = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "screen_width":
source.ScreenWidth = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "server_ip":
source.ServerIp = (System.String)(System.String)dataReader.GetString(i);
break;


case "server_ping_port":
source.ServerPingPort = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "server_tcp_port":
source.ServerTcpPort = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "world_physics_update_rate":
source.WorldPhysicsUpdateRate = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
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
public static void TryCopyValues(this IGameConstantTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@max_account_name_length":
paramValues[i] = (System.Byte)source.MaxAccountNameLength;
break;


case "@max_account_password_length":
paramValues[i] = (System.Byte)source.MaxAccountPasswordLength;
break;


case "@max_characters_per_account":
paramValues[i] = (System.Byte)source.MaxCharactersPerAccount;
break;


case "@max_character_name_length":
paramValues[i] = (System.Byte)source.MaxCharacterNameLength;
break;


case "@max_inventory_size":
paramValues[i] = (System.Byte)source.MaxInventorySize;
break;


case "@max_shop_items":
paramValues[i] = (System.Byte)source.MaxShopItems;
break;


case "@max_status_effect_power":
paramValues[i] = (System.UInt16)source.MaxStatusEffectPower;
break;


case "@min_account_name_length":
paramValues[i] = (System.Byte)source.MinAccountNameLength;
break;


case "@min_account_password_length":
paramValues[i] = (System.Byte)source.MinAccountPasswordLength;
break;


case "@min_character_name_length":
paramValues[i] = (System.Byte)source.MinCharacterNameLength;
break;


case "@screen_height":
paramValues[i] = (System.UInt16)source.ScreenHeight;
break;


case "@screen_width":
paramValues[i] = (System.UInt16)source.ScreenWidth;
break;


case "@server_ip":
paramValues[i] = (System.String)source.ServerIp;
break;


case "@server_ping_port":
paramValues[i] = (System.UInt16)source.ServerPingPort;
break;


case "@server_tcp_port":
paramValues[i] = (System.UInt16)source.ServerTcpPort;
break;


case "@world_physics_update_rate":
paramValues[i] = (System.UInt16)source.WorldPhysicsUpdateRate;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IGameConstantTable"/> contains the same values as another <see cref="IGameConstantTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IGameConstantTable"/>.</param>
/// <param name="otherItem">The <see cref="IGameConstantTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IGameConstantTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IGameConstantTable source, IGameConstantTable otherItem)
{
return Equals(source.MaxAccountNameLength, otherItem.MaxAccountNameLength) && 
Equals(source.MaxAccountPasswordLength, otherItem.MaxAccountPasswordLength) && 
Equals(source.MaxCharactersPerAccount, otherItem.MaxCharactersPerAccount) && 
Equals(source.MaxCharacterNameLength, otherItem.MaxCharacterNameLength) && 
Equals(source.MaxInventorySize, otherItem.MaxInventorySize) && 
Equals(source.MaxShopItems, otherItem.MaxShopItems) && 
Equals(source.MaxStatusEffectPower, otherItem.MaxStatusEffectPower) && 
Equals(source.MinAccountNameLength, otherItem.MinAccountNameLength) && 
Equals(source.MinAccountPasswordLength, otherItem.MinAccountPasswordLength) && 
Equals(source.MinCharacterNameLength, otherItem.MinCharacterNameLength) && 
Equals(source.ScreenHeight, otherItem.ScreenHeight) && 
Equals(source.ScreenWidth, otherItem.ScreenWidth) && 
Equals(source.ServerIp, otherItem.ServerIp) && 
Equals(source.ServerPingPort, otherItem.ServerPingPort) && 
Equals(source.ServerTcpPort, otherItem.ServerTcpPort) && 
Equals(source.WorldPhysicsUpdateRate, otherItem.WorldPhysicsUpdateRate);
}

}

}
