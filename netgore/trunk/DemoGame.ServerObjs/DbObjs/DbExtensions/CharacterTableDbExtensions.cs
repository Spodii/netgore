using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class CharacterTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class CharacterTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this ICharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
paramValues["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
paramValues["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@cash"] = (System.UInt32)source.Cash;
paramValues["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
paramValues["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@hp"] = (DemoGame.SPValueType)source.HP;
paramValues["@id"] = (DemoGame.Server.CharacterID)source.ID;
paramValues["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@map_id"] = (NetGore.MapIndex)source.MapID;
paramValues["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@mp"] = (DemoGame.SPValueType)source.MP;
paramValues["@name"] = (System.String)source.Name;
paramValues["@password"] = (System.String)source.Password;
paramValues["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
paramValues["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
paramValues["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
paramValues["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
paramValues["@respawn_x"] = (System.Single)source.RespawnX;
paramValues["@respawn_y"] = (System.Single)source.RespawnY;
paramValues["@statpoints"] = (System.UInt32)source.StatPoints;
paramValues["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
paramValues["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
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
public static void ReadValues(this CharacterTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("acc");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("agi");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("armor");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("body_id");
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("bra");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("cash");
source.Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("character_template_id");
source.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("defence");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("dex");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("evade");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("exp");
source.Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("hp");
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");
source.ID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("imm");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("int");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("level");
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("map_id");
source.MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("maxhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("maxhp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxmp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("minhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("mp");
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");
source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("password");
source.Password = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("recov");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("regen");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("respawn_map");
source.RespawnMap = (System.Nullable<NetGore.MapIndex>)(System.Nullable<NetGore.MapIndex>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("respawn_x");
source.RespawnX = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("respawn_y");
source.RespawnY = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("statpoints");
source.StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("str");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("tact");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("ws");
source.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(i));

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
public static void TryReadValues(this CharacterTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "acc":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "agi":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "armor":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "body_id":
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);
break;


case "bra":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "cash":
source.Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "character_template_id":
source.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "defence":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "dex":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "evade":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "exp":
source.Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "hp":
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "id":
source.ID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);
break;


case "imm":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "int":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "level":
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "map_id":
source.MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);
break;


case "maxhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "maxhp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxmp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "minhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "password":
source.Password = (System.String)(System.String)dataReader.GetString(i);
break;


case "perc":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "recov":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "regen":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(i));
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
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "tact":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "ws":
source.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(i));
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
public static void TryCopyValues(this ICharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@acc":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
break;


case "@agi":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@armor":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
break;


case "@body_id":
paramValues[i] = source.BodyID;
break;


case "@bra":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
break;


case "@cash":
paramValues[i] = source.Cash;
break;


case "@character_template_id":
paramValues[i] = source.CharacterTemplateID;
break;


case "@defence":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "@dex":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
break;


case "@evade":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
break;


case "@exp":
paramValues[i] = source.Exp;
break;


case "@hp":
paramValues[i] = source.HP;
break;


case "@id":
paramValues[i] = source.ID;
break;


case "@imm":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
break;


case "@int":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "@level":
paramValues[i] = source.Level;
break;


case "@map_id":
paramValues[i] = source.MapID;
break;


case "@maxhit":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
break;


case "@maxhp":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
break;


case "@maxmp":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
break;


case "@minhit":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
break;


case "@mp":
paramValues[i] = source.MP;
break;


case "@name":
paramValues[i] = source.Name;
break;


case "@password":
paramValues[i] = source.Password;
break;


case "@perc":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
break;


case "@recov":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
break;


case "@regen":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
break;


case "@respawn_map":
paramValues[i] = source.RespawnMap;
break;


case "@respawn_x":
paramValues[i] = source.RespawnX;
break;


case "@respawn_y":
paramValues[i] = source.RespawnY;
break;


case "@statpoints":
paramValues[i] = source.StatPoints;
break;


case "@str":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@tact":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
break;


case "@ws":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
break;


case "@x":
paramValues[i] = source.X;
break;


case "@y":
paramValues[i] = source.Y;
break;


}

}
}

}

}
