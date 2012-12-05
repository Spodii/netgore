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
/// Contains extension methods for class CharacterTemplateTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class CharacterTemplateTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this ICharacterTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["ai_id"] = (System.Nullable<System.UInt16>)source.AIID;
paramValues["alliance_id"] = (System.Byte)source.AllianceID;
paramValues["body_id"] = (System.UInt16)source.BodyID;
paramValues["chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
paramValues["exp"] = (System.Int32)source.Exp;
paramValues["give_cash"] = (System.Int32)source.GiveCash;
paramValues["give_exp"] = (System.Int32)source.GiveExp;
paramValues["id"] = (System.UInt16)source.ID;
paramValues["level"] = (System.Int16)source.Level;
paramValues["move_speed"] = (System.UInt16)source.MoveSpeed;
paramValues["name"] = (System.String)source.Name;
paramValues["respawn"] = (System.UInt16)source.Respawn;
paramValues["shop_id"] = (System.Nullable<System.UInt16>)source.ShopID;
paramValues["statpoints"] = (System.Int32)source.StatPoints;
paramValues["stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this CharacterTemplateTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("ai_id");

source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("alliance_id");

source.AllianceID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("body_id");

source.BodyID = (DemoGame.BodyID)(DemoGame.BodyID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("chat_dialog");

source.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("exp");

source.Exp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("give_cash");

source.GiveCash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("give_exp");

source.GiveExp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("id");

source.ID = (DemoGame.CharacterTemplateID)(DemoGame.CharacterTemplateID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("level");

source.Level = (System.Int16)(System.Int16)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("move_speed");

source.MoveSpeed = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("name");

source.Name = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("respawn");

source.Respawn = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("shop_id");

source.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)(System.Nullable<NetGore.Features.Shops.ShopID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("statpoints");

source.StatPoints = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("stat_agi");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_defence");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_int");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_maxhit");

source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_maxhp");

source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_maxmp");

source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_minhit");

source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_str");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
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
public static void TryReadValues(this CharacterTemplateTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "ai_id":
source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "alliance_id":
source.AllianceID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataRecord.GetByte(i);
break;


case "body_id":
source.BodyID = (DemoGame.BodyID)(DemoGame.BodyID)dataRecord.GetUInt16(i);
break;


case "chat_dialog":
source.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "exp":
source.Exp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "give_cash":
source.GiveCash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "give_exp":
source.GiveExp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "id":
source.ID = (DemoGame.CharacterTemplateID)(DemoGame.CharacterTemplateID)dataRecord.GetUInt16(i);
break;


case "level":
source.Level = (System.Int16)(System.Int16)dataRecord.GetInt16(i);
break;


case "move_speed":
source.MoveSpeed = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataRecord.GetString(i);
break;


case "respawn":
source.Respawn = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "shop_id":
source.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)(System.Nullable<NetGore.Features.Shops.ShopID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "statpoints":
source.StatPoints = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "stat_agi":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_defence":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_int":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_maxhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_maxhp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_maxmp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_minhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_str":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
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
public static void TryCopyValues(this ICharacterTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "ai_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.AIID;
break;


case "alliance_id":
paramValues[i] = (System.Byte)source.AllianceID;
break;


case "body_id":
paramValues[i] = (System.UInt16)source.BodyID;
break;


case "chat_dialog":
paramValues[i] = (System.Nullable<System.UInt16>)source.ChatDialog;
break;


case "exp":
paramValues[i] = (System.Int32)source.Exp;
break;


case "give_cash":
paramValues[i] = (System.Int32)source.GiveCash;
break;


case "give_exp":
paramValues[i] = (System.Int32)source.GiveExp;
break;


case "id":
paramValues[i] = (System.UInt16)source.ID;
break;


case "level":
paramValues[i] = (System.Int16)source.Level;
break;


case "move_speed":
paramValues[i] = (System.UInt16)source.MoveSpeed;
break;


case "name":
paramValues[i] = (System.String)source.Name;
break;


case "respawn":
paramValues[i] = (System.UInt16)source.Respawn;
break;


case "shop_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ShopID;
break;


case "statpoints":
paramValues[i] = (System.Int32)source.StatPoints;
break;


case "stat_agi":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "stat_defence":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "stat_int":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "stat_maxhit":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
break;


case "stat_maxhp":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
break;


case "stat_maxmp":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
break;


case "stat_minhit":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
break;


case "stat_str":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


}

}
}

/// <summary>
/// Checks if this <see cref="ICharacterTemplateTable"/> contains the same values as another <see cref="ICharacterTemplateTable"/>.
/// </summary>
/// <param name="source">The source <see cref="ICharacterTemplateTable"/>.</param>
/// <param name="otherItem">The <see cref="ICharacterTemplateTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="ICharacterTemplateTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this ICharacterTemplateTable source, ICharacterTemplateTable otherItem)
{
return Equals(source.AIID, otherItem.AIID) && 
Equals(source.AllianceID, otherItem.AllianceID) && 
Equals(source.BodyID, otherItem.BodyID) && 
Equals(source.ChatDialog, otherItem.ChatDialog) && 
Equals(source.Exp, otherItem.Exp) && 
Equals(source.GiveCash, otherItem.GiveCash) && 
Equals(source.GiveExp, otherItem.GiveExp) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.Level, otherItem.Level) && 
Equals(source.MoveSpeed, otherItem.MoveSpeed) && 
Equals(source.Name, otherItem.Name) && 
Equals(source.Respawn, otherItem.Respawn) && 
Equals(source.ShopID, otherItem.ShopID) && 
Equals(source.StatPoints, otherItem.StatPoints) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Agi)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Defence)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Int), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Int)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Str), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
}

}

}
