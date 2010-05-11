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
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/11/2010 11:46:42 PM
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
/// Provides a strongly-typed structure for the database table `item_template`.
/// </summary>
public class ItemTemplateTable : IItemTemplateTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"description", "equipped_body", "graphic", "height", "hp", "id", "mp", "name", "range", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_req_agi", "stat_req_int", "stat_req_str", "stat_str", "type", "value", "weapon_type", "width" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"description", "equipped_body", "graphic", "height", "hp", "mp", "name", "range", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_req_agi", "stat_req_int", "stat_req_str", "stat_str", "type", "value", "weapon_type", "width" };
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
public const System.String TableName = "item_template";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 24;
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
System.UInt16 _iD;
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
/// Gets or sets the value for the field that maps onto the database column `description`.
/// The underlying database type is `varchar(255)` with the default value of ` `.
/// </summary>
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
/// The underlying database type is `varchar(255)`.
/// </summary>
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.
/// </summary>
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
/// The underlying database type is `smallint(6)` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public DemoGame.ItemTemplateID ID
{
get
{
return (DemoGame.ItemTemplateID)_iD;
}
set
{
this._iD = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `varchar(255)` with the default value of `New item template`.
/// </summary>
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `10`.
/// </summary>
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.
/// </summary>
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
public IItemTemplateTable DeepCopy()
{
return new ItemTemplateTable(this);
}
/// <summary>
/// ItemTemplateTable constructor.
/// </summary>
public ItemTemplateTable()
{
}
/// <summary>
/// ItemTemplateTable constructor.
/// </summary>
/// <param name="description">The initial value for the corresponding property.</param>
/// <param name="equippedBody">The initial value for the corresponding property.</param>
/// <param name="graphic">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="range">The initial value for the corresponding property.</param>
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
public ItemTemplateTable(System.String @description, System.String @equippedBody, NetGore.GrhIndex @graphic, System.Byte @height, DemoGame.SPValueType @hP, DemoGame.ItemTemplateID @iD, DemoGame.SPValueType @mP, System.String @name, System.UInt16 @range, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statReqAgi, System.Int16 @statReqInt, System.Int16 @statReqStr, System.Int16 @statStr, DemoGame.ItemType @type, System.Int32 @value, DemoGame.WeaponType @weaponType, System.Byte @width)
{
this.Description = (System.String)@description;
this.EquippedBody = (System.String)@equippedBody;
this.Graphic = (NetGore.GrhIndex)@graphic;
this.Height = (System.Byte)@height;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (DemoGame.ItemTemplateID)@iD;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.Range = (System.UInt16)@range;
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
/// ItemTemplateTable constructor.
/// </summary>
/// <param name="source">IItemTemplateTable to copy the initial values from.</param>
public ItemTemplateTable(IItemTemplateTable source)
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
public static void CopyValues(IItemTemplateTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@description"] = (System.String)source.Description;
dic["@equipped_body"] = (System.String)source.EquippedBody;
dic["@graphic"] = (NetGore.GrhIndex)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (DemoGame.ItemTemplateID)source.ID;
dic["@mp"] = (DemoGame.SPValueType)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@range"] = (System.UInt16)source.Range;
dic["@stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@stat_req_agi"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@stat_req_int"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@stat_req_str"] = (System.Int16)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@type"] = (DemoGame.ItemType)source.Type;
dic["@value"] = (System.Int32)source.Value;
dic["@weapon_type"] = (DemoGame.WeaponType)source.WeaponType;
dic["@width"] = (System.Byte)source.Width;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this ItemTemplateTable.
/// </summary>
/// <param name="source">The IItemTemplateTable to copy the values from.</param>
public void CopyValuesFrom(IItemTemplateTable source)
{
this.Description = (System.String)source.Description;
this.EquippedBody = (System.String)source.EquippedBody;
this.Graphic = (NetGore.GrhIndex)source.Graphic;
this.Height = (System.Byte)source.Height;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (DemoGame.ItemTemplateID)source.ID;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.Range = (System.UInt16)source.Range;
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

case "mp":
return MP;

case "name":
return Name;

case "range":
return Range;

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
this.ID = (DemoGame.ItemTemplateID)value;
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
case "description":
return new ColumnMetadata("description", "", "varchar(255)", " ", typeof(System.String), false, false, false);

case "equipped_body":
return new ColumnMetadata("equipped_body", "", "varchar(255)", null, typeof(System.String), true, false, false);

case "graphic":
return new ColumnMetadata("graphic", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "height":
return new ColumnMetadata("height", "", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(255)", "New item template", typeof(System.String), false, false, false);

case "range":
return new ColumnMetadata("range", "", "smallint(5) unsigned", "10", typeof(System.UInt16), false, false, false);

case "stat_agi":
return new ColumnMetadata("stat_agi", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_defence":
return new ColumnMetadata("stat_defence", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_int":
return new ColumnMetadata("stat_int", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxhit":
return new ColumnMetadata("stat_maxhit", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxhp":
return new ColumnMetadata("stat_maxhp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_maxmp":
return new ColumnMetadata("stat_maxmp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_minhit":
return new ColumnMetadata("stat_minhit", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_agi":
return new ColumnMetadata("stat_req_agi", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_int":
return new ColumnMetadata("stat_req_int", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_req_str":
return new ColumnMetadata("stat_req_str", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "stat_str":
return new ColumnMetadata("stat_str", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "type":
return new ColumnMetadata("type", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "value":
return new ColumnMetadata("value", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "weapon_type":
return new ColumnMetadata("weapon_type", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "width":
return new ColumnMetadata("width", "", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
