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
/// Contains extension methods for class ItemTemplateTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class ItemTemplateTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@description"] = (System.String)source.Description;
paramValues["@equipped_body"] = (System.String)source.EquippedBody;
paramValues["@graphic"] = (System.UInt16)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (System.Int16)source.HP;
paramValues["@id"] = (System.UInt16)source.ID;
paramValues["@mp"] = (System.Int16)source.MP;
paramValues["@name"] = (System.String)source.Name;
paramValues["@stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@stat_req_agi"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@stat_req_int"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@stat_req_str"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@type"] = (System.Byte)source.Type;
paramValues["@value"] = (System.Int32)source.Value;
paramValues["@width"] = (System.Byte)source.Width;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this ItemTemplateTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("description");

source.Description = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("equipped_body");

source.EquippedBody = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));

i = dataReader.GetOrdinal("graphic");

source.Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("height");

source.Height = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("hp");

source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");

source.ID = (DemoGame.Server.ItemTemplateID)(DemoGame.Server.ItemTemplateID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("mp");

source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");

source.Name = (System.String)(System.String)dataReader.GetString(i);

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

i = dataReader.GetOrdinal("stat_req_agi");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_req_int");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_req_str");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("stat_str");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("type");

source.Type = (System.Byte)(System.Byte)dataReader.GetByte(i);

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
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this ItemTemplateTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "description":
source.Description = (System.String)(System.String)dataReader.GetString(i);
break;


case "equipped_body":
source.EquippedBody = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));
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
source.ID = (DemoGame.Server.ItemTemplateID)(DemoGame.Server.ItemTemplateID)dataReader.GetUInt16(i);
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
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


case "stat_req_agi":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_req_int":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_req_str":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "stat_str":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "type":
source.Type = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
public static void TryCopyValues(this IItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@description":
paramValues[i] = (System.String)source.Description;
break;


case "@equipped_body":
paramValues[i] = (System.String)source.EquippedBody;
break;


case "@graphic":
paramValues[i] = (System.UInt16)source.Graphic;
break;


case "@height":
paramValues[i] = (System.Byte)source.Height;
break;


case "@hp":
paramValues[i] = (System.Int16)source.HP;
break;


case "@id":
paramValues[i] = (System.UInt16)source.ID;
break;


case "@mp":
paramValues[i] = (System.Int16)source.MP;
break;


case "@name":
paramValues[i] = (System.String)source.Name;
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


case "@stat_req_agi":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@stat_req_int":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "@stat_req_str":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@stat_str":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@type":
paramValues[i] = (System.Byte)source.Type;
break;


case "@value":
paramValues[i] = (System.Int32)source.Value;
break;


case "@width":
paramValues[i] = (System.Byte)source.Width;
break;


}

}
}

}

}
