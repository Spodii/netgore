using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
public static  class ItemTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IItemTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@amount"] = (System.Byte)source.Amount;
paramValues["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@bra"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@description"] = (System.String)source.Description;
paramValues["@dex"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@graphic"] = (NetGore.GrhIndex)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (DemoGame.SPValueType)source.HP;
paramValues["@id"] = (DemoGame.Server.ItemID)source.ID;
paramValues["@imm"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@mp"] = (DemoGame.SPValueType)source.MP;
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
paramValues["@reqacc"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc);
paramValues["@reqagi"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@reqarmor"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@reqbra"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@reqdex"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@reqevade"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@reqimm"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@reqint"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@type"] = (DemoGame.ItemType)source.Type;
paramValues["@value"] = (System.Int32)source.Value;
paramValues["@width"] = (System.Byte)source.Width;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this ItemTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("agi");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("amount");
source.Amount = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("armor");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("bra");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("defence");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("description");
source.Description = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("dex");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("evade");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("graphic");
source.Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("height");
source.Height = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("hp");
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");
source.ID = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("imm");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("int");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxhp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxmp");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("minhit");
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("mp");
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");
source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
source.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("reqacc");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqagi");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqarmor");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqbra");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqdex");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqevade");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqimm");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqint");
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("type");
source.Type = (DemoGame.ItemType)(DemoGame.ItemType)dataReader.GetByte(i);

i = dataReader.GetOrdinal("value");
source.Value = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("width");
source.Width = (System.Byte)(System.Byte)dataReader.GetByte(i);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this ItemTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "agi":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "amount":
source.Amount = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "armor":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.UInt16)dataReader.GetUInt16(i));
break;


case "bra":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "defence":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "description":
source.Description = (System.String)(System.String)dataReader.GetString(i);
break;


case "dex":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "evade":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "graphic":
source.Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataReader.GetUInt16(i);
break;


case "height":
source.Height = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "hp":
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "id":
source.ID = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(i);
break;


case "imm":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "int":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxhp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxmp":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "minhit":
source.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "perc":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "reqacc":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqagi":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqarmor":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqbra":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqdex":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqevade":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqimm":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqint":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "type":
source.Type = (DemoGame.ItemType)(DemoGame.ItemType)dataReader.GetByte(i);
break;


case "value":
source.Value = (System.Int32)(System.Int32)dataReader.GetInt32(i);
break;


case "width":
source.Width = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
public static void TryCopyValues(this IItemTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@agi":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@amount":
paramValues[i] = source.Amount;
break;


case "@armor":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
break;


case "@bra":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
break;


case "@defence":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "@description":
paramValues[i] = source.Description;
break;


case "@dex":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
break;


case "@evade":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
break;


case "@graphic":
paramValues[i] = source.Graphic;
break;


case "@height":
paramValues[i] = source.Height;
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


case "@perc":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
break;


case "@reqacc":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc);
break;


case "@reqagi":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@reqarmor":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor);
break;


case "@reqbra":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra);
break;


case "@reqdex":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex);
break;


case "@reqevade":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade);
break;


case "@reqimm":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm);
break;


case "@reqint":
paramValues[i] = source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "@type":
paramValues[i] = source.Type;
break;


case "@value":
paramValues[i] = source.Value;
break;


case "@width":
paramValues[i] = source.Width;
break;


}

}
}

}

}
