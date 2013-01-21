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
/// Contains extension methods for class ItemTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
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
paramValues["action_display_id"] = (System.Nullable<System.UInt16>)source.ActionDisplayID;
paramValues["amount"] = (System.Byte)source.Amount;
paramValues["description"] = (System.String)source.Description;
paramValues["equipped_body"] = (System.String)source.EquippedBody;
paramValues["graphic"] = (System.UInt16)source.Graphic;
paramValues["height"] = (System.Byte)source.Height;
paramValues["hp"] = (System.Int16)source.HP;
paramValues["id"] = (System.Int32)source.ID;
paramValues["item_template_id"] = (System.Nullable<System.UInt16>)source.ItemTemplateID;
paramValues["mp"] = (System.Int16)source.MP;
paramValues["name"] = (System.String)source.Name;
paramValues["range"] = (System.UInt16)source.Range;
paramValues["skill_id"] = (System.Nullable<System.Byte>)source.SkillID;
paramValues["stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["stat_req_agi"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["stat_req_int"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["stat_req_str"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["type"] = (System.Byte)source.Type;
paramValues["value"] = (System.Int32)source.Value;
paramValues["weapon_type"] = (System.Byte)source.WeaponType;
paramValues["width"] = (System.Byte)source.Width;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this ItemTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("action_display_id");

source.ActionDisplayID = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)(System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("amount");

source.Amount = (System.Byte)(System.Byte)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("description");

source.Description = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("equipped_body");

source.EquippedBody = (System.String)(System.String)(dataRecord.IsDBNull(i) ? (System.String)null : dataRecord.GetString(i));

i = dataRecord.GetOrdinal("graphic");

source.Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("height");

source.Height = (System.Byte)(System.Byte)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("hp");

source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("id");

source.ID = (DemoGame.ItemID)(DemoGame.ItemID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("item_template_id");

source.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)(System.Nullable<DemoGame.ItemTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));

i = dataRecord.GetOrdinal("mp");

source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);

i = dataRecord.GetOrdinal("name");

source.Name = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("range");

source.Range = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("skill_id");

source.SkillID = (System.Nullable<DemoGame.SkillType>)(System.Nullable<DemoGame.SkillType>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.Byte>)null : dataRecord.GetByte(i));

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

i = dataRecord.GetOrdinal("stat_req_agi");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_req_int");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_req_str");

source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("stat_str");

source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));

i = dataRecord.GetOrdinal("type");

source.Type = (DemoGame.ItemType)(DemoGame.ItemType)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("value");

source.Value = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("weapon_type");

source.WeaponType = (DemoGame.WeaponType)(DemoGame.WeaponType)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("width");

source.Width = (System.Byte)(System.Byte)dataRecord.GetByte(i);
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
public static void TryReadValues(this ItemTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "action_display_id":
source.ActionDisplayID = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)(System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "amount":
source.Amount = (System.Byte)(System.Byte)dataRecord.GetByte(i);
break;


case "description":
source.Description = (System.String)(System.String)dataRecord.GetString(i);
break;


case "equipped_body":
source.EquippedBody = (System.String)(System.String)(dataRecord.IsDBNull(i) ? (System.String)null : dataRecord.GetString(i));
break;


case "graphic":
source.Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataRecord.GetUInt16(i);
break;


case "height":
source.Height = (System.Byte)(System.Byte)dataRecord.GetByte(i);
break;


case "hp":
source.HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);
break;


case "id":
source.ID = (DemoGame.ItemID)(DemoGame.ItemID)dataRecord.GetInt32(i);
break;


case "item_template_id":
source.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)(System.Nullable<DemoGame.ItemTemplateID>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataRecord.GetUInt16(i));
break;


case "mp":
source.MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataRecord.GetInt16(i);
break;


case "name":
source.Name = (System.String)(System.String)dataRecord.GetString(i);
break;


case "range":
source.Range = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "skill_id":
source.SkillID = (System.Nullable<DemoGame.SkillType>)(System.Nullable<DemoGame.SkillType>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.Byte>)null : dataRecord.GetByte(i));
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


case "stat_req_agi":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_req_int":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_req_str":
source.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "stat_str":
source.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Int16)dataRecord.GetInt16(i));
break;


case "type":
source.Type = (DemoGame.ItemType)(DemoGame.ItemType)dataRecord.GetByte(i);
break;


case "value":
source.Value = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "weapon_type":
source.WeaponType = (DemoGame.WeaponType)(DemoGame.WeaponType)dataRecord.GetByte(i);
break;


case "width":
source.Width = (System.Byte)(System.Byte)dataRecord.GetByte(i);
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
case "action_display_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ActionDisplayID;
break;


case "amount":
paramValues[i] = (System.Byte)source.Amount;
break;


case "description":
paramValues[i] = (System.String)source.Description;
break;


case "equipped_body":
paramValues[i] = (System.String)source.EquippedBody;
break;


case "graphic":
paramValues[i] = (System.UInt16)source.Graphic;
break;


case "height":
paramValues[i] = (System.Byte)source.Height;
break;


case "hp":
paramValues[i] = (System.Int16)source.HP;
break;


case "id":
paramValues[i] = (System.Int32)source.ID;
break;


case "item_template_id":
paramValues[i] = (System.Nullable<System.UInt16>)source.ItemTemplateID;
break;


case "mp":
paramValues[i] = (System.Int16)source.MP;
break;


case "name":
paramValues[i] = (System.String)source.Name;
break;


case "range":
paramValues[i] = (System.UInt16)source.Range;
break;


case "skill_id":
paramValues[i] = (System.Nullable<System.Byte>)source.SkillID;
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


case "stat_req_agi":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "stat_req_int":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
break;


case "stat_req_str":
paramValues[i] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "stat_str":
paramValues[i] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "type":
paramValues[i] = (System.Byte)source.Type;
break;


case "value":
paramValues[i] = (System.Int32)source.Value;
break;


case "weapon_type":
paramValues[i] = (System.Byte)source.WeaponType;
break;


case "width":
paramValues[i] = (System.Byte)source.Width;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IItemTable"/> contains the same values as another <see cref="IItemTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IItemTable"/>.</param>
/// <param name="otherItem">The <see cref="IItemTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IItemTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IItemTable source, IItemTable otherItem)
{
return Equals(source.ActionDisplayID, otherItem.ActionDisplayID) && 
Equals(source.Amount, otherItem.Amount) && 
Equals(source.Description, otherItem.Description) && 
Equals(source.EquippedBody, otherItem.EquippedBody) && 
Equals(source.Graphic, otherItem.Graphic) && 
Equals(source.Height, otherItem.Height) && 
Equals(source.HP, otherItem.HP) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.ItemTemplateID, otherItem.ItemTemplateID) && 
Equals(source.MP, otherItem.MP) && 
Equals(source.Name, otherItem.Name) && 
Equals(source.Range, otherItem.Range) && 
Equals(source.SkillID, otherItem.SkillID) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Agi)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Defence)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Int), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Int)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit)) && 
Equals(source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi), otherItem.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi)) && 
Equals(source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int), otherItem.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int)) && 
Equals(source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str), otherItem.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str)) && 
Equals(source.GetStat((DemoGame.StatType)DemoGame.StatType.Str), otherItem.GetStat((DemoGame.StatType)DemoGame.StatType.Str)) && 
Equals(source.Type, otherItem.Type) && 
Equals(source.Value, otherItem.Value) && 
Equals(source.WeaponType, otherItem.WeaponType) && 
Equals(source.Width, otherItem.Width);
}

}

}
