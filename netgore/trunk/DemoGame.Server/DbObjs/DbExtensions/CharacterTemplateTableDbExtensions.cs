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
paramValues["@ai_id"] = (System.Nullable<System.UInt16>)source.AIID;
paramValues["@alliance_id"] = (System.Byte)source.AllianceID;
paramValues["@body_id"] = (System.UInt16)source.BodyID;
paramValues["@exp"] = (System.Int32)source.Exp;
paramValues["@give_cash"] = (System.UInt16)source.GiveCash;
paramValues["@give_exp"] = (System.UInt16)source.GiveExp;
paramValues["@id"] = (System.UInt16)source.ID;
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@name"] = (System.String)source.Name;
paramValues["@respawn"] = (System.UInt16)source.Respawn;
paramValues["@shop_id"] = (System.Nullable<System.UInt16>)source.ShopID;
paramValues["@stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@statpoints"] = (System.Int32)source.StatPoints;
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

i = dataReader.GetOrdinal("ai_id");
source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("alliance_id");
source.AllianceID = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);

i = dataReader.GetOrdinal("body_id");
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("exp");
source.Exp = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("give_cash");
source.GiveCash = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("give_exp");
source.GiveExp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("id");
source.ID = (DemoGame.Server.CharacterTemplateID)(DemoGame.Server.CharacterTemplateID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("level");
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("name");
source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("respawn");
source.Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("shop_id");
source.ShopID = (System.Nullable<DemoGame.Server.ShopID>)(System.Nullable<DemoGame.Server.ShopID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("stat_agi");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_defence");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_int");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_maxhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_maxhp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_maxmp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_minhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_str");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("statpoints");
source.StatPoints = (System.Int32)(System.Int32)dataReader.GetInt32(i);
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
case "ai_id":
source.AIID = (System.Nullable<NetGore.AI.AIID>)(System.Nullable<NetGore.AI.AIID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "alliance_id":
source.AllianceID = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);
break;


case "body_id":
source.BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);
break;


case "exp":
source.Exp = (System.Int32)(System.Int32)dataReader.GetInt32(i);
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


case "level":
source.Level = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "respawn":
source.Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "shop_id":
source.ShopID = (System.Nullable<DemoGame.Server.ShopID>)(System.Nullable<DemoGame.Server.ShopID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "stat_agi":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_defence":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_int":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_maxhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_maxhp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_maxmp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_minhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_str":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "statpoints":
source.StatPoints = (System.Int32)(System.Int32)dataReader.GetInt32(i);
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
case "@ai_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.AIID;
break;


case "@alliance_id":
paramValues[i] = (System.Byte)source.AllianceID;
break;


case "@body_id":
paramValues[i] = (System.UInt16)source.BodyID;
break;


case "@exp":
paramValues[i] = (System.Int32)source.Exp;
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


case "@level":
paramValues[i] = (System.Byte)source.Level;
break;


case "@name":
paramValues[i] = (System.String)source.Name;
break;


case "@respawn":
paramValues[i] = (System.UInt16)source.Respawn;
break;


case "@shop_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ShopID;
break;


case "@stat_agi":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@stat_defence":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "@stat_int":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "@stat_maxhit":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
break;


case "@stat_maxhp":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
break;


case "@stat_maxmp":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
break;


case "@stat_minhit":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
break;


case "@stat_str":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@statpoints":
paramValues[i] = (System.Int32)source.StatPoints;
break;


}

}
}

}

}
