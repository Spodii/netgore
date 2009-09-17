using System;
using System.Linq;
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
paramValues["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
paramValues["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@ai"] = (System.String)source.AI;
paramValues["@alliance_id"] = (System.Byte)source.AllianceID;
paramValues["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@body_id"] = (System.UInt16)source.BodyID;
paramValues["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@give_cash"] = (System.UInt16)source.GiveCash;
paramValues["@give_exp"] = (System.UInt16)source.GiveExp;
paramValues["@id"] = (System.UInt16)source.ID;
paramValues["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
paramValues["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
paramValues["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
paramValues["@respawn"] = (System.UInt16)source.Respawn;
paramValues["@shop_id"] = (System.Nullable<System.UInt16>)source.ShopID;
paramValues["@statpoints"] = (System.UInt32)source.StatPoints;
paramValues["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
paramValues["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this CharacterTemplateTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("acc");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("agi");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("ai");
source.AI = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));

i = dataReader.GetOrdinal("alliance_id");
source.AllianceID = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);

i = dataReader.GetOrdinal("armor");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("body_id");
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("bra");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("defence");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("dex");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("evade");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("exp");
source.Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("give_cash");
source.GiveCash = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("give_exp");
source.GiveExp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("id");
source.ID = (DemoGame.Server.CharacterTemplateID)(DemoGame.Server.CharacterTemplateID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("imm");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("int");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("level");
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("maxhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("maxhp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("maxmp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("minhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("name");
source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("recov");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("regen");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("respawn");
source.Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("shop_id");
source.ShopID = (System.Nullable<DemoGame.Server.ShopID>)(System.Nullable<DemoGame.Server.ShopID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("statpoints");
source.StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("str");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("tact");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("ws");
source.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(i));
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
public static void TryReadValues(this CharacterTemplateTable source, System.Data.IDataReader dataReader)
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


case "ai":
source.AI = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));
break;


case "alliance_id":
source.AllianceID = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);
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


case "give_cash":
source.GiveCash = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "give_exp":
source.GiveExp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "id":
source.ID = (DemoGame.Server.CharacterTemplateID)(DemoGame.Server.CharacterTemplateID)dataReader.GetUInt16(i);
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


case "maxhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "maxhp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));
break;


case "maxmp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));
break;


case "minhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
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


case "respawn":
source.Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "shop_id":
source.ShopID = (System.Nullable<DemoGame.Server.ShopID>)(System.Nullable<DemoGame.Server.ShopID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
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
case "@acc":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
break;


case "@agi":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@ai":
paramValues[i] = (System.String)source.AI;
break;


case "@alliance_id":
paramValues[i] = (System.Byte)source.AllianceID;
break;


case "@armor":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
break;


case "@body_id":
paramValues[i] = (System.UInt16)source.BodyID;
break;


case "@bra":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
break;


case "@defence":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "@dex":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
break;


case "@evade":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
break;


case "@exp":
paramValues[i] = (System.UInt32)source.Exp;
break;


case "@give_cash":
paramValues[i] = (System.UInt16)source.GiveCash;
break;


case "@give_exp":
paramValues[i] = (System.UInt16)source.GiveExp;
break;


case "@id":
paramValues[i] = (System.UInt16)source.ID;
break;


case "@imm":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
break;


case "@int":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "@level":
paramValues[i] = (System.Byte)source.Level;
break;


case "@maxhit":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
break;


case "@maxhp":
paramValues[i] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
break;


case "@maxmp":
paramValues[i] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
break;


case "@minhit":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
break;


case "@name":
paramValues[i] = (System.String)source.Name;
break;


case "@perc":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
break;


case "@recov":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
break;


case "@regen":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
break;


case "@respawn":
paramValues[i] = (System.UInt16)source.Respawn;
break;


case "@shop_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ShopID;
break;


case "@statpoints":
paramValues[i] = (System.UInt32)source.StatPoints;
break;


case "@str":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@tact":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
break;


case "@ws":
paramValues[i] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
break;


}

}
}

}

}
