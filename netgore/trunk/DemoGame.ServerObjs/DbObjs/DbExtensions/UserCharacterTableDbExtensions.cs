using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class UserCharacterTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class UserCharacterTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IUserCharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@acc"] = (System.Byte)source.Acc;
paramValues["@account_id"] = (System.Nullable<System.Int32>)source.AccountID;
paramValues["@agi"] = (System.Byte)source.Agi;
paramValues["@armor"] = (System.Byte)source.Armor;
paramValues["@body_id"] = (System.UInt16)source.BodyID;
paramValues["@bra"] = (System.Byte)source.Bra;
paramValues["@cash"] = (System.UInt32)source.Cash;
paramValues["@character_template_id"] = (System.Nullable<System.UInt16>)source.CharacterTemplateID;
paramValues["@chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
paramValues["@defence"] = (System.Byte)source.Defence;
paramValues["@dex"] = (System.Byte)source.Dex;
paramValues["@evade"] = (System.Byte)source.Evade;
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@hp"] = (System.Int16)source.HP;
paramValues["@id"] = (System.Int32)source.ID;
paramValues["@imm"] = (System.Byte)source.Imm;
paramValues["@int"] = (System.Byte)source.Int;
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@map_id"] = (System.UInt16)source.MapID;
paramValues["@maxhit"] = (System.Byte)source.MaxHit;
paramValues["@maxhp"] = (System.Int16)source.MaxHP;
paramValues["@maxmp"] = (System.Int16)source.MaxMP;
paramValues["@minhit"] = (System.Byte)source.MinHit;
paramValues["@mp"] = (System.Int16)source.MP;
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.Byte)source.Perc;
paramValues["@recov"] = (System.Byte)source.Recov;
paramValues["@regen"] = (System.Byte)source.Regen;
paramValues["@respawn_map"] = (System.Nullable<System.UInt16>)source.RespawnMap;
paramValues["@respawn_x"] = (System.Single)source.RespawnX;
paramValues["@respawn_y"] = (System.Single)source.RespawnY;
paramValues["@statpoints"] = (System.UInt32)source.StatPoints;
paramValues["@str"] = (System.Byte)source.Str;
paramValues["@tact"] = (System.Byte)source.Tact;
paramValues["@ws"] = (System.Byte)source.WS;
paramValues["@x"] = (System.Single)source.X;
paramValues["@y"] = (System.Single)source.Y;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this UserCharacterTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("acc");
source.Acc = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("account_id");
source.AccountID = (System.Nullable<DemoGame.Server.AccountID>)(System.Nullable<DemoGame.Server.AccountID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Int32>)null : dataReader.GetInt32(i));

i = dataReader.GetOrdinal("agi");
source.Agi = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("armor");
source.Armor = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("body_id");
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("bra");
source.Bra = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("cash");
source.Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("character_template_id");
source.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("chat_dialog");
source.ChatDialog = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("defence");
source.Defence = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("dex");
source.Dex = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("evade");
source.Evade = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("exp");
source.Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("hp");
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");
source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("imm");
source.Imm = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("int");
source.Int = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("level");
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("map_id");
source.MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("maxhit");
source.MaxHit = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("maxhp");
source.MaxHP = (System.Int16)(System.Int16)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("maxmp");
source.MaxMP = (System.Int16)(System.Int16)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("minhit");
source.MinHit = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("mp");
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");
source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
source.Perc = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("recov");
source.Recov = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("regen");
source.Regen = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("respawn_map");
source.RespawnMap = (System.Nullable<NetGore.MapIndex>)(System.Nullable<NetGore.MapIndex>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("respawn_x");
source.RespawnX = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("respawn_y");
source.RespawnY = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("statpoints");
source.StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("str");
source.Str = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("tact");
source.Tact = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("ws");
source.WS = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("x");
source.X = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("y");
source.Y = (System.Single)(System.Single)dataReader.GetFloat(i);
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
public static void TryReadValues(this UserCharacterTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "acc":
source.Acc = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "account_id":
source.AccountID = (System.Nullable<DemoGame.Server.AccountID>)(System.Nullable<DemoGame.Server.AccountID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Int32>)null : dataReader.GetInt32(i));
break;


case "agi":
source.Agi = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "armor":
source.Armor = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "body_id":
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);
break;


case "bra":
source.Bra = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "cash":
source.Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "character_template_id":
source.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "chat_dialog":
source.ChatDialog = (System.Nullable<System.UInt16>)(System.Nullable<System.UInt16>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "defence":
source.Defence = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "dex":
source.Dex = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "evade":
source.Evade = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "exp":
source.Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "hp":
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "id":
source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);
break;


case "imm":
source.Imm = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "int":
source.Int = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "level":
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "map_id":
source.MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);
break;


case "maxhit":
source.MaxHit = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "maxhp":
source.MaxHP = (System.Int16)(System.Int16)dataReader.GetInt16(i);
break;


case "maxmp":
source.MaxMP = (System.Int16)(System.Int16)dataReader.GetInt16(i);
break;


case "minhit":
source.MinHit = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "perc":
source.Perc = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "recov":
source.Recov = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "regen":
source.Regen = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "respawn_map":
source.RespawnMap = (System.Nullable<NetGore.MapIndex>)(System.Nullable<NetGore.MapIndex>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "respawn_x":
source.RespawnX = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "respawn_y":
source.RespawnY = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "statpoints":
source.StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "str":
source.Str = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "tact":
source.Tact = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "ws":
source.WS = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "x":
source.X = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "y":
source.Y = (System.Single)(System.Single)dataReader.GetFloat(i);
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
public static void TryCopyValues(this IUserCharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@acc":
paramValues[i] = (System.Byte)source.Acc;
break;


case "@account_id":
paramValues[i] = (System.Nullable<System.Int32>)source.AccountID;
break;


case "@agi":
paramValues[i] = (System.Byte)source.Agi;
break;


case "@armor":
paramValues[i] = (System.Byte)source.Armor;
break;


case "@body_id":
paramValues[i] = (System.UInt16)source.BodyID;
break;


case "@bra":
paramValues[i] = (System.Byte)source.Bra;
break;


case "@cash":
paramValues[i] = (System.UInt32)source.Cash;
break;


case "@character_template_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.CharacterTemplateID;
break;


case "@chat_dialog":
paramValues[i] = (System.Nullable<System.UInt16>)source.ChatDialog;
break;


case "@defence":
paramValues[i] = (System.Byte)source.Defence;
break;


case "@dex":
paramValues[i] = (System.Byte)source.Dex;
break;


case "@evade":
paramValues[i] = (System.Byte)source.Evade;
break;


case "@exp":
paramValues[i] = (System.UInt32)source.Exp;
break;


case "@hp":
paramValues[i] = (System.Int16)source.HP;
break;


case "@id":
paramValues[i] = (System.Int32)source.ID;
break;


case "@imm":
paramValues[i] = (System.Byte)source.Imm;
break;


case "@int":
paramValues[i] = (System.Byte)source.Int;
break;


case "@level":
paramValues[i] = (System.Byte)source.Level;
break;


case "@map_id":
paramValues[i] = (System.UInt16)source.MapID;
break;


case "@maxhit":
paramValues[i] = (System.Byte)source.MaxHit;
break;


case "@maxhp":
paramValues[i] = (System.Int16)source.MaxHP;
break;


case "@maxmp":
paramValues[i] = (System.Int16)source.MaxMP;
break;


case "@minhit":
paramValues[i] = (System.Byte)source.MinHit;
break;


case "@mp":
paramValues[i] = (System.Int16)source.MP;
break;


case "@name":
paramValues[i] = (System.String)source.Name;
break;


case "@perc":
paramValues[i] = (System.Byte)source.Perc;
break;


case "@recov":
paramValues[i] = (System.Byte)source.Recov;
break;


case "@regen":
paramValues[i] = (System.Byte)source.Regen;
break;


case "@respawn_map":
paramValues[i] = (System.Nullable<System.UInt16>)source.RespawnMap;
break;


case "@respawn_x":
paramValues[i] = (System.Single)source.RespawnX;
break;


case "@respawn_y":
paramValues[i] = (System.Single)source.RespawnY;
break;


case "@statpoints":
paramValues[i] = (System.UInt32)source.StatPoints;
break;


case "@str":
paramValues[i] = (System.Byte)source.Str;
break;


case "@tact":
paramValues[i] = (System.Byte)source.Tact;
break;


case "@ws":
paramValues[i] = (System.Byte)source.WS;
break;


case "@x":
paramValues[i] = (System.Single)source.X;
break;


case "@y":
paramValues[i] = (System.Single)source.Y;
break;


}

}
}

}

}
