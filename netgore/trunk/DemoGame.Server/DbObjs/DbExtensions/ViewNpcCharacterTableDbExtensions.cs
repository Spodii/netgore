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
/// Contains extension methods for class ViewNpcCharacterTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class ViewNpcCharacterTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IViewNpcCharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["ai_id"] = (System.Nullable<System.UInt16>)source.AIID;
paramValues["body_id"] = (System.UInt16)source.BodyID;
paramValues["cash"] = (System.Int32)source.Cash;
paramValues["character_template_id"] = (System.Nullable<System.UInt16>)source.CharacterTemplateID;
paramValues["chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
paramValues["exp"] = (System.Int32)source.Exp;
paramValues["hp"] = (System.Int16)source.HP;
paramValues["id"] = (System.Int32)source.ID;
paramValues["level"] = (System.Int16)source.Level;
paramValues["load_map_id"] = (System.UInt16)source.LoadMapID;
paramValues["load_x"] = (System.UInt16)source.LoadX;
paramValues["load_y"] = (System.UInt16)source.LoadY;
paramValues["move_speed"] = (System.UInt16)source.MoveSpeed;
paramValues["mp"] = (System.Int16)source.MP;
paramValues["name"] = (System.String)source.Name;
paramValues["respawn_map_id"] = (System.Nullable<System.UInt16>)source.RespawnMapID;
paramValues["respawn_x"] = (System.Single)source.RespawnX;
paramValues["respawn_y"] = (System.Single)source.RespawnY;
paramValues["shop_id"] = (System.Nullable<System.UInt16>)source.ShopID;
paramValues["statpoints"] = (System.Int32)source.StatPoints;
paramValues["stat_agi"] = (System.Int16)source.StatAgi;
paramValues["stat_defence"] = (System.Int16)source.StatDefence;
paramValues["stat_int"] = (System.Int16)source.StatInt;
paramValues["stat_maxhit"] = (System.Int16)source.StatMaxhit;
paramValues["stat_maxhp"] = (System.Int16)source.StatMaxhp;
paramValues["stat_maxmp"] = (System.Int16)source.StatMaxmp;
paramValues["stat_minhit"] = (System.Int16)source.StatMinhit;
paramValues["stat_str"] = (System.Int16)source.StatStr;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this ViewNpcCharacterTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("ai_id");

source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("body_id");

source.BodyID = (DemoGame.BodyID)(DemoGame.BodyID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("cash");

source.Cash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("character_template_id");

source.CharacterTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)(System.Nullable<DemoGame.CharacterTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("chat_dialog");

source.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("exp");

source.Exp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("hp");

source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("id");

source.ID = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("level");

source.Level = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("load_map_id");

source.LoadMapID = (NetGore.World.MapID)(NetGore.World.MapID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("load_x");

source.LoadX = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("load_y");

source.LoadY = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("move_speed");

source.MoveSpeed = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("mp");

source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("name");

source.Name = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("respawn_map_id");

source.RespawnMapID = (System.Nullable<NetGore.World.MapID>)(System.Nullable<NetGore.World.MapID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("respawn_x");

source.RespawnX = (System.Single)(System.Single)dataRecord.GetFloat(i);

i = dataRecord.GetOrdinal("respawn_y");

source.RespawnY = (System.Single)(System.Single)dataRecord.GetFloat(i);

i = dataRecord.GetOrdinal("shop_id");

source.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)(System.Nullable<NetGore.Features.Shops.ShopID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("statpoints");

source.StatPoints = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("stat_agi");

source.StatAgi = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_defence");

source.StatDefence = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_int");

source.StatInt = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_maxhit");

source.StatMaxhit = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_maxhp");

source.StatMaxhp = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_maxmp");

source.StatMaxmp = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_minhit");

source.StatMinhit = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("stat_str");

source.StatStr = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
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
public static void TryReadValues(this ViewNpcCharacterTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "ai_id":
source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "body_id":
source.BodyID = (DemoGame.BodyID)(DemoGame.BodyID)dataRecord.GetUInt16(i);
break;


case "cash":
source.Cash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "character_template_id":
source.CharacterTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)(System.Nullable<DemoGame.CharacterTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "chat_dialog":
source.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "exp":
source.Exp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "hp":
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);
break;


case "id":
source.ID = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "level":
source.Level = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "load_map_id":
source.LoadMapID = (NetGore.World.MapID)(NetGore.World.MapID)dataRecord.GetUInt16(i);
break;


case "load_x":
source.LoadX = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "load_y":
source.LoadY = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "move_speed":
source.MoveSpeed = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataRecord.GetString(i);
break;


case "respawn_map_id":
source.RespawnMapID = (System.Nullable<NetGore.World.MapID>)(System.Nullable<NetGore.World.MapID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "respawn_x":
source.RespawnX = (System.Single)(System.Single)dataRecord.GetFloat(i);
break;


case "respawn_y":
source.RespawnY = (System.Single)(System.Single)dataRecord.GetFloat(i);
break;


case "shop_id":
source.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)(System.Nullable<NetGore.Features.Shops.ShopID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "statpoints":
source.StatPoints = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "stat_agi":
source.StatAgi = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_defence":
source.StatDefence = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_int":
source.StatInt = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_maxhit":
source.StatMaxhit = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_maxhp":
source.StatMaxhp = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_maxmp":
source.StatMaxmp = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_minhit":
source.StatMinhit = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "stat_str":
source.StatStr = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
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
public static void TryCopyValues(this IViewNpcCharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "ai_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.AIID;
break;


case "body_id":
paramValues[i] = (System.UInt16)source.BodyID;
break;


case "cash":
paramValues[i] = (System.Int32)source.Cash;
break;


case "character_template_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.CharacterTemplateID;
break;


case "chat_dialog":
paramValues[i] = (System.Nullable<System.UInt16>)source.ChatDialog;
break;


case "exp":
paramValues[i] = (System.Int32)source.Exp;
break;


case "hp":
paramValues[i] = (System.Int16)source.HP;
break;


case "id":
paramValues[i] = (System.Int32)source.ID;
break;


case "level":
paramValues[i] = (System.Int16)source.Level;
break;


case "load_map_id":
paramValues[i] = (System.UInt16)source.LoadMapID;
break;


case "load_x":
paramValues[i] = (System.UInt16)source.LoadX;
break;


case "load_y":
paramValues[i] = (System.UInt16)source.LoadY;
break;


case "move_speed":
paramValues[i] = (System.UInt16)source.MoveSpeed;
break;


case "mp":
paramValues[i] = (System.Int16)source.MP;
break;


case "name":
paramValues[i] = (System.String)source.Name;
break;


case "respawn_map_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.RespawnMapID;
break;


case "respawn_x":
paramValues[i] = (System.Single)source.RespawnX;
break;


case "respawn_y":
paramValues[i] = (System.Single)source.RespawnY;
break;


case "shop_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ShopID;
break;


case "statpoints":
paramValues[i] = (System.Int32)source.StatPoints;
break;


case "stat_agi":
paramValues[i] = (System.Int16)source.StatAgi;
break;


case "stat_defence":
paramValues[i] = (System.Int16)source.StatDefence;
break;


case "stat_int":
paramValues[i] = (System.Int16)source.StatInt;
break;


case "stat_maxhit":
paramValues[i] = (System.Int16)source.StatMaxhit;
break;


case "stat_maxhp":
paramValues[i] = (System.Int16)source.StatMaxhp;
break;


case "stat_maxmp":
paramValues[i] = (System.Int16)source.StatMaxmp;
break;


case "stat_minhit":
paramValues[i] = (System.Int16)source.StatMinhit;
break;


case "stat_str":
paramValues[i] = (System.Int16)source.StatStr;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IViewNpcCharacterTable"/> contains the same values as another <see cref="IViewNpcCharacterTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IViewNpcCharacterTable"/>.</param>
/// <param name="otherItem">The <see cref="IViewNpcCharacterTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IViewNpcCharacterTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IViewNpcCharacterTable source, IViewNpcCharacterTable otherItem)
{
return Equals(source.AIID, otherItem.AIID) && 
Equals(source.BodyID, otherItem.BodyID) && 
Equals(source.Cash, otherItem.Cash) && 
Equals(source.CharacterTemplateID, otherItem.CharacterTemplateID) && 
Equals(source.ChatDialog, otherItem.ChatDialog) && 
Equals(source.Exp, otherItem.Exp) && 
Equals(source.HP, otherItem.HP) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.Level, otherItem.Level) && 
Equals(source.LoadMapID, otherItem.LoadMapID) && 
Equals(source.LoadX, otherItem.LoadX) && 
Equals(source.LoadY, otherItem.LoadY) && 
Equals(source.MoveSpeed, otherItem.MoveSpeed) && 
Equals(source.MP, otherItem.MP) && 
Equals(source.Name, otherItem.Name) && 
Equals(source.RespawnMapID, otherItem.RespawnMapID) && 
Equals(source.RespawnX, otherItem.RespawnX) && 
Equals(source.RespawnY, otherItem.RespawnY) && 
Equals(source.ShopID, otherItem.ShopID) && 
Equals(source.StatPoints, otherItem.StatPoints) && 
Equals(source.StatAgi, otherItem.StatAgi) && 
Equals(source.StatDefence, otherItem.StatDefence) && 
Equals(source.StatInt, otherItem.StatInt) && 
Equals(source.StatMaxhit, otherItem.StatMaxhit) && 
Equals(source.StatMaxhp, otherItem.StatMaxhp) && 
Equals(source.StatMaxmp, otherItem.StatMaxmp) && 
Equals(source.StatMinhit, otherItem.StatMinhit) && 
Equals(source.StatStr, otherItem.StatStr);
}

}

}
