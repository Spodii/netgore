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

using DemoGame.DbObjs;
namespace DemoGame.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `item`.
/// </summary>
public class ItemTable : IItemTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"action_display_id", "amount", "description", "equipped_body", "graphic", "height", "hp", "id", "item_template_id", "mp", "name", "range", "skill_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_req_agi", "stat_req_int", "stat_req_str", "stat_str", "type", "value", "weapon_type", "width" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"id" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"action_display_id", "amount", "description", "equipped_body", "graphic", "height", "hp", "item_template_id", "mp", "name", "range", "skill_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_req_agi", "stat_req_int", "stat_req_str", "stat_str", "type", "value", "weapon_type", "width" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The fields that are used in the column collection `ReqStat`.
/// </summary>
 static  readonly System.String[] _reqStatColumns = new string[] {"stat_req_agi", "stat_req_int", "stat_req_str" };
/// <summary>
/// Gets an IEnumerable of strings containing the name of the database
/// columns used in the column collection `ReqStat`.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> ReqStatColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_reqStatColumns;
}
}
/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `ReqStat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> ReqStats
{
get
{
return (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>)_reqStat;
}
}
/// <summary>
/// The fields that are used in the column collection `Stat`.
/// </summary>
 static  readonly System.String[] _statColumns = new string[] {"stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };
/// <summary>
/// Gets an IEnumerable of strings containing the name of the database
/// columns used in the column collection `Stat`.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> StatColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_statColumns;
}
}
/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get
{
return (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>)_stat;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "item";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 28;
/// <summary>
/// The field that maps onto the database column `action_display_id`.
/// </summary>
System.Nullable<System.UInt16> _actionDisplayID;
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `description`.
/// </summary>
System.String _description;
/// <summary>
/// The field that maps onto the database column `equipped_body`.
/// </summary>
System.String _equippedBody;
/// <summary>
/// The field that maps onto the database column `graphic`.
/// </summary>
System.UInt16 _graphic;
/// <summary>
/// The field that maps onto the database column `height`.
/// </summary>
System.Byte _height;
/// <summary>
/// The field that maps onto the database column `hp`.
/// </summary>
System.Int16 _hP;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `item_template_id`.
/// </summary>
System.Nullable<System.UInt16> _itemTemplateID;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.Int16 _mP;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `range`.
/// </summary>
System.UInt16 _range;
/// <summary>
/// The field that maps onto the database column `skill_id`.
/// </summary>
System.Nullable<System.Byte> _skillID;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatTypeConstDictionary _stat = new StatTypeConstDictionary();
/// <summary>
/// Dictionary containing the values for the column collection `ReqStat`.
/// </summary>
 readonly StatTypeConstDictionary _reqStat = new StatTypeConstDictionary();
/// <summary>
/// The field that maps onto the database column `type`.
/// </summary>
System.Byte _type;
/// <summary>
/// The field that maps onto the database column `value`.
/// </summary>
System.Int32 _value;
/// <summary>
/// The field that maps onto the database column `weapon_type`.
/// </summary>
System.Byte _weaponType;
/// <summary>
/// The field that maps onto the database column `width`.
/// </summary>
System.Byte _width;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `action_display_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The ActionDisplayID to use when using this item (e.g. drink potion, attack with sword, etc).".
/// </summary>
[System.ComponentModel.Description("The ActionDisplayID to use when using this item (e.g. drink potion, attack with sword, etc).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID> ActionDisplayID
{
get
{
return (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)_actionDisplayID;
}
set
{
this._actionDisplayID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.The database column contains the comment: 
/// "The quantity of the item (for stacked items). Stacks of items count as one single item instance with an amount greater than zero.".
/// </summary>
[System.ComponentModel.Description("The quantity of the item (for stacked items). Stacks of items count as one single item instance with an amount greater than zero.")]
[NetGore.SyncValueAttribute()]
public System.Byte Amount
{
get
{
return (System.Byte)_amount;
}
set
{
this._amount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `description`.
/// The underlying database type is `varchar(255)`.The database column contains the comment: 
/// "The item's textual description (don't include stuff like stats).".
/// </summary>
[System.ComponentModel.Description("The item's textual description (don't include stuff like stats).")]
[NetGore.SyncValueAttribute()]
public System.String Description
{
get
{
return (System.String)_description;
}
set
{
this._description = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `equipped_body`.
/// The underlying database type is `varchar(255)`.The database column contains the comment: 
/// "When equipped and not null, sets the character's paper doll to include this layer.".
/// </summary>
[System.ComponentModel.Description("When equipped and not null, sets the character's paper doll to include this layer.")]
[NetGore.SyncValueAttribute()]
public System.String EquippedBody
{
get
{
return (System.String)_equippedBody;
}
set
{
this._equippedBody = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `graphic`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.The database column contains the comment: 
/// "The GrhData to use to display this item, both in GUI (inventory, equipped) and on the map.".
/// </summary>
[System.ComponentModel.Description("The GrhData to use to display this item, both in GUI (inventory, equipped) and on the map.")]
[NetGore.SyncValueAttribute()]
public NetGore.GrhIndex Graphic
{
get
{
return (NetGore.GrhIndex)_graphic;
}
set
{
this._graphic = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `height`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.The database column contains the comment: 
/// "Height of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.".
/// </summary>
[System.ComponentModel.Description("Height of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.")]
[NetGore.SyncValueAttribute()]
public System.Byte Height
{
get
{
return (System.Byte)_height;
}
set
{
this._height = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `hp`.
/// The underlying database type is `smallint(6)` with the default value of `0`.The database column contains the comment: 
/// "Amount of health gained from using this item (mostly for use-once items).".
/// </summary>
[System.ComponentModel.Description("Amount of health gained from using this item (mostly for use-once items).")]
[NetGore.SyncValueAttribute()]
public DemoGame.SPValueType HP
{
get
{
return (DemoGame.SPValueType)_hP;
}
set
{
this._hP = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The unique ID of the item.".
/// </summary>
[System.ComponentModel.Description("The unique ID of the item.")]
[NetGore.SyncValueAttribute()]
public DemoGame.ItemID ID
{
get
{
return (DemoGame.ItemID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `item_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The template the item was created from. Not required. Mostly for development reference.".
/// </summary>
[System.ComponentModel.Description("The template the item was created from. Not required. Mostly for development reference.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.ItemTemplateID> ItemTemplateID
{
get
{
return (System.Nullable<DemoGame.ItemTemplateID>)_itemTemplateID;
}
set
{
this._itemTemplateID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `0`.The database column contains the comment: 
/// "Amount of mana gained from using this item (mostly for use-once items).".
/// </summary>
[System.ComponentModel.Description("Amount of mana gained from using this item (mostly for use-once items).")]
[NetGore.SyncValueAttribute()]
public DemoGame.SPValueType MP
{
get
{
return (DemoGame.SPValueType)_mP;
}
set
{
this._mP = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(255)`.The database column contains the comment: 
/// "The name of the item.".
/// </summary>
[System.ComponentModel.Description("The name of the item.")]
[NetGore.SyncValueAttribute()]
public System.String Name
{
get
{
return (System.String)_name;
}
set
{
this._name = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `range`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The range of the item. Usually for attack range, but can depend on ItemType and/or WeaponType.".
/// </summary>
[System.ComponentModel.Description("The range of the item. Usually for attack range, but can depend on ItemType and/or WeaponType.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 Range
{
get
{
return (System.UInt16)_range;
}
set
{
this._range = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `skill_id`.
/// The underlying database type is `tinyint(5) unsigned`.The database column contains the comment: 
/// "The skill the item can set for a user.".
/// </summary>
[System.ComponentModel.Description("The skill the item can set for a user.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.SkillType> SkillID
{
get
{
return (System.Nullable<DemoGame.SkillType>)_skillID;
}
set
{
this._skillID = (System.Nullable<System.Byte>)value;
}
}
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Int32)_stat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.Int16)value;
}
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `ReqStat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetReqStat(DemoGame.StatType key)
{
return (System.Int32)_reqStat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `ReqStat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetReqStat(DemoGame.StatType key, System.Int32 value)
{
this._reqStat[(DemoGame.StatType)key] = (System.Int16)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `type`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.The database column contains the comment: 
/// "The type of item (see ItemType enum).".
/// </summary>
[System.ComponentModel.Description("The type of item (see ItemType enum).")]
[NetGore.SyncValueAttribute()]
public DemoGame.ItemType Type
{
get
{
return (DemoGame.ItemType)_type;
}
set
{
this._type = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `value`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "The base monetary value of the item.".
/// </summary>
[System.ComponentModel.Description("The base monetary value of the item.")]
[NetGore.SyncValueAttribute()]
public System.Int32 Value
{
get
{
return (System.Int32)_value;
}
set
{
this._value = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `weapon_type`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "When used as a weapon, the type of weapon (see WeaponType enum).".
/// </summary>
[System.ComponentModel.Description("When used as a weapon, the type of weapon (see WeaponType enum).")]
[NetGore.SyncValueAttribute()]
public DemoGame.WeaponType WeaponType
{
get
{
return (DemoGame.WeaponType)_weaponType;
}
set
{
this._weaponType = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `width`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.The database column contains the comment: 
/// "Width of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.".
/// </summary>
[System.ComponentModel.Description("Width of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.")]
[NetGore.SyncValueAttribute()]
public System.Byte Width
{
get
{
return (System.Byte)_width;
}
set
{
this._width = (System.Byte)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IItemTable DeepCopy()
{
return new ItemTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="ItemTable"/> class.
/// </summary>
public ItemTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="ItemTable"/> class.
/// </summary>
/// <param name="actionDisplayID">The initial value for the corresponding property.</param>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="description">The initial value for the corresponding property.</param>
/// <param name="equippedBody">The initial value for the corresponding property.</param>
/// <param name="graphic">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="itemTemplateID">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="range">The initial value for the corresponding property.</param>
/// <param name="skillID">The initial value for the corresponding property.</param>
/// <param name="statAgi">The initial value for the corresponding property.</param>
/// <param name="statDefence">The initial value for the corresponding property.</param>
/// <param name="statInt">The initial value for the corresponding property.</param>
/// <param name="statMaxhit">The initial value for the corresponding property.</param>
/// <param name="statMaxhp">The initial value for the corresponding property.</param>
/// <param name="statMaxmp">The initial value for the corresponding property.</param>
/// <param name="statMinhit">The initial value for the corresponding property.</param>
/// <param name="statReqAgi">The initial value for the corresponding property.</param>
/// <param name="statReqInt">The initial value for the corresponding property.</param>
/// <param name="statReqStr">The initial value for the corresponding property.</param>
/// <param name="statStr">The initial value for the corresponding property.</param>
/// <param name="type">The initial value for the corresponding property.</param>
/// <param name="value">The initial value for the corresponding property.</param>
/// <param name="weaponType">The initial value for the corresponding property.</param>
/// <param name="width">The initial value for the corresponding property.</param>
public ItemTable(System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID> @actionDisplayID, System.Byte @amount, System.String @description, System.String @equippedBody, NetGore.GrhIndex @graphic, System.Byte @height, DemoGame.SPValueType @hP, DemoGame.ItemID @iD, System.Nullable<DemoGame.ItemTemplateID> @itemTemplateID, DemoGame.SPValueType @mP, System.String @name, System.UInt16 @range, System.Nullable<DemoGame.SkillType> @skillID, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statReqAgi, System.Int16 @statReqInt, System.Int16 @statReqStr, System.Int16 @statStr, DemoGame.ItemType @type, System.Int32 @value, DemoGame.WeaponType @weaponType, System.Byte @width)
{
this.ActionDisplayID = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)@actionDisplayID;
this.Amount = (System.Byte)@amount;
this.Description = (System.String)@description;
this.EquippedBody = (System.String)@equippedBody;
this.Graphic = (NetGore.GrhIndex)@graphic;
this.Height = (System.Byte)@height;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (DemoGame.ItemID)@iD;
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)@itemTemplateID;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.Range = (System.UInt16)@range;
this.SkillID = (System.Nullable<DemoGame.SkillType>)@skillID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@statAgi);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@statDefence);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@statInt);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@statMaxhit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@statMaxhp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@statMaxmp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@statMinhit);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@statReqAgi);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@statReqInt);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@statReqStr);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@statStr);
this.Type = (DemoGame.ItemType)@type;
this.Value = (System.Int32)@value;
this.WeaponType = (DemoGame.WeaponType)@weaponType;
this.Width = (System.Byte)@width;
}
/// <summary>
/// Initializes a new instance of the <see cref="ItemTable"/> class.
/// </summary>
/// <param name="source">IItemTable to copy the initial values from.</param>
public ItemTable(IItemTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IItemTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["action_display_id"] = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)source.ActionDisplayID;
dic["amount"] = (System.Byte)source.Amount;
dic["description"] = (System.String)source.Description;
dic["equipped_body"] = (System.String)source.EquippedBody;
dic["graphic"] = (NetGore.GrhIndex)source.Graphic;
dic["height"] = (System.Byte)source.Height;
dic["hp"] = (DemoGame.SPValueType)source.HP;
dic["id"] = (DemoGame.ItemID)source.ID;
dic["item_template_id"] = (System.Nullable<DemoGame.ItemTemplateID>)source.ItemTemplateID;
dic["mp"] = (DemoGame.SPValueType)source.MP;
dic["name"] = (System.String)source.Name;
dic["range"] = (System.UInt16)source.Range;
dic["skill_id"] = (System.Nullable<DemoGame.SkillType>)source.SkillID;
dic["stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["stat_req_agi"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["stat_req_int"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["stat_req_str"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["type"] = (DemoGame.ItemType)source.Type;
dic["value"] = (System.Int32)source.Value;
dic["weapon_type"] = (DemoGame.WeaponType)source.WeaponType;
dic["width"] = (System.Byte)source.Width;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this ItemTable.
/// </summary>
/// <param name="source">The IItemTable to copy the values from.</param>
public void CopyValuesFrom(IItemTable source)
{
this.ActionDisplayID = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)source.ActionDisplayID;
this.Amount = (System.Byte)source.Amount;
this.Description = (System.String)source.Description;
this.EquippedBody = (System.String)source.EquippedBody;
this.Graphic = (NetGore.GrhIndex)source.Graphic;
this.Height = (System.Byte)source.Height;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (DemoGame.ItemID)source.ID;
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)source.ItemTemplateID;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.Range = (System.UInt16)source.Range;
this.SkillID = (System.Nullable<DemoGame.SkillType>)source.SkillID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
this.Type = (DemoGame.ItemType)source.Type;
this.Value = (System.Int32)source.Value;
this.WeaponType = (DemoGame.WeaponType)source.WeaponType;
this.Width = (System.Byte)source.Width;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "action_display_id":
return ActionDisplayID;

case "amount":
return Amount;

case "description":
return Description;

case "equipped_body":
return EquippedBody;

case "graphic":
return Graphic;

case "height":
return Height;

case "hp":
return HP;

case "id":
return ID;

case "item_template_id":
return ItemTemplateID;

case "mp":
return MP;

case "name":
return Name;

case "range":
return Range;

case "skill_id":
return SkillID;

case "stat_agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "stat_defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "stat_int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "stat_maxhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);

case "stat_maxhp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);

case "stat_maxmp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);

case "stat_minhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);

case "stat_req_agi":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "stat_req_int":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);

case "stat_req_str":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);

case "stat_str":
return GetStat((DemoGame.StatType)DemoGame.StatType.Str);

case "type":
return Type;

case "value":
return Value;

case "weapon_type":
return WeaponType;

case "width":
return Width;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "action_display_id":
this.ActionDisplayID = (System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID>)value;
break;

case "amount":
this.Amount = (System.Byte)value;
break;

case "description":
this.Description = (System.String)value;
break;

case "equipped_body":
this.EquippedBody = (System.String)value;
break;

case "graphic":
this.Graphic = (NetGore.GrhIndex)value;
break;

case "height":
this.Height = (System.Byte)value;
break;

case "hp":
this.HP = (DemoGame.SPValueType)value;
break;

case "id":
this.ID = (DemoGame.ItemID)value;
break;

case "item_template_id":
this.ItemTemplateID = (System.Nullable<DemoGame.ItemTemplateID>)value;
break;

case "mp":
this.MP = (DemoGame.SPValueType)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "range":
this.Range = (System.UInt16)value;
break;

case "skill_id":
this.SkillID = (System.Nullable<DemoGame.SkillType>)value;
break;

case "stat_agi":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "stat_defence":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "stat_int":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "stat_maxhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "stat_maxhp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "stat_maxmp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "stat_minhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "stat_req_agi":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "stat_req_int":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "stat_req_str":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
break;

case "stat_str":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
break;

case "type":
this.Type = (DemoGame.ItemType)value;
break;

case "value":
this.Value = (System.Int32)value;
break;

case "weapon_type":
this.WeaponType = (DemoGame.WeaponType)value;
break;

case "width":
this.Width = (System.Byte)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "action_display_id":
return new ColumnMetadata("action_display_id", "The ActionDisplayID to use when using this item (e.g. drink potion, attack with sword, etc).", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "amount":
return new ColumnMetadata("amount", "The quantity of the item (for stacked items). Stacks of items count as one single item instance with an amount greater than zero.", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "description":
return new ColumnMetadata("description", "The item's textual description (don't include stuff like stats).", "varchar(255)", null, typeof(System.String), false, false, false);

case "equipped_body":
return new ColumnMetadata("equipped_body", "When equipped and not null, sets the character's paper doll to include this layer.", "varchar(255)", null, typeof(System.String), true, false, false);

case "graphic":
return new ColumnMetadata("graphic", "The GrhData to use to display this item, both in GUI (inventory, equipped) and on the map.", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "height":
return new ColumnMetadata("height", "Height of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

case "hp":
return new ColumnMetadata("hp", "Amount of health gained from using this item (mostly for use-once items).", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "The unique ID of the item.", "int(11)", null, typeof(System.Int32), false, true, false);

case "item_template_id":
return new ColumnMetadata("item_template_id", "The template the item was created from. Not required. Mostly for development reference.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "mp":
return new ColumnMetadata("mp", "Amount of mana gained from using this item (mostly for use-once items).", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "The name of the item.", "varchar(255)", null, typeof(System.String), false, false, false);

case "range":
return new ColumnMetadata("range", "The range of the item. Usually for attack range, but can depend on ItemType and/or WeaponType.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "skill_id":
return new ColumnMetadata("skill_id", "The skill the item can set for a user.", "tinyint(5) unsigned", null, typeof(System.Nullable<System.Byte>), true, false, false);

case "stat_agi":
return new ColumnMetadata("stat_agi", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_defence":
return new ColumnMetadata("stat_defence", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_int":
return new ColumnMetadata("stat_int", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxhit":
return new ColumnMetadata("stat_maxhit", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxhp":
return new ColumnMetadata("stat_maxhp", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxmp":
return new ColumnMetadata("stat_maxmp", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_minhit":
return new ColumnMetadata("stat_minhit", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_agi":
return new ColumnMetadata("stat_req_agi", "Required amount of the corresponding stat to use this item.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_int":
return new ColumnMetadata("stat_req_int", "Required amount of the corresponding stat to use this item.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_str":
return new ColumnMetadata("stat_req_str", "Required amount of the corresponding stat to use this item.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_str":
return new ColumnMetadata("stat_str", "Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "type":
return new ColumnMetadata("type", "The type of item (see ItemType enum).", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "value":
return new ColumnMetadata("value", "The base monetary value of the item.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "weapon_type":
return new ColumnMetadata("weapon_type", "When used as a weapon, the type of weapon (see WeaponType enum).", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "width":
return new ColumnMetadata("width", "Width of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item's sprite.", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
