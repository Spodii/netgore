using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `item`.
/// </summary>
public class ItemTable : IItemTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"agi", "amount", "armor", "bra", "defence", "description", "dex", "evade", "graphic", "height", "hp", "id", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "reqacc", "reqagi", "reqarmor", "reqbra", "reqdex", "reqevade", "reqimm", "reqint", "type", "value", "width" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"agi", "amount", "armor", "bra", "defence", "description", "dex", "evade", "graphic", "height", "hp", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "reqacc", "reqagi", "reqarmor", "reqbra", "reqdex", "reqevade", "reqimm", "reqint", "type", "value", "width" };
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
/// The fields that are used in the column collection `Stat`.
/// </summary>
 static  readonly System.String[] _statColumns = new string[] {"agi", "armor", "bra", "defence", "dex", "evade", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "perc" };
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
public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get
{
return (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>)_stat;
}
}
/// <summary>
/// The fields that are used in the column collection `ReqStat`.
/// </summary>
 static  readonly System.String[] _reqStatColumns = new string[] {"reqacc", "reqagi", "reqarmor", "reqbra", "reqdex", "reqevade", "reqimm", "reqint" };
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
public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> ReqStats
{
get
{
return (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>)_reqStat;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "item";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 32;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `description`.
/// </summary>
System.String _description;
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
/// The field that maps onto the database column `mp`.
/// </summary>
System.Int16 _mP;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// Dictionary containing the values for the column collection `ReqStat`.
/// </summary>
 readonly ReqStatConstDictionary _reqStat = new ReqStatConstDictionary();
/// <summary>
/// The field that maps onto the database column `type`.
/// </summary>
System.Byte _type;
/// <summary>
/// The field that maps onto the database column `value`.
/// </summary>
System.Int32 _value;
/// <summary>
/// The field that maps onto the database column `width`.
/// </summary>
System.Byte _width;
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Int16)_stat[(DemoGame.StatType)key];
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
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
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
/// The underlying database type is `varchar(255)`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `graphic`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.ItemID ID
{
get
{
return (DemoGame.Server.ItemID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `0`.
/// </summary>
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
/// The underlying database type is `varchar(255)`.
/// </summary>
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
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `ReqStat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetReqStat(DemoGame.StatType key)
{
return (System.Byte)_reqStat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `ReqStat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetReqStat(DemoGame.StatType key, System.Int32 value)
{
this._reqStat[(DemoGame.StatType)key] = (System.Byte)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `type`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `width`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.
/// </summary>
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
public IItemTable DeepCopy()
{
return new ItemTable(this);
}
/// <summary>
/// ItemTable constructor.
/// </summary>
public ItemTable()
{
}
/// <summary>
/// ItemTable constructor.
/// </summary>
/// <param name="agi">The initial value for the corresponding property.</param>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="armor">The initial value for the corresponding property.</param>
/// <param name="bra">The initial value for the corresponding property.</param>
/// <param name="defence">The initial value for the corresponding property.</param>
/// <param name="description">The initial value for the corresponding property.</param>
/// <param name="dex">The initial value for the corresponding property.</param>
/// <param name="evade">The initial value for the corresponding property.</param>
/// <param name="graphic">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="imm">The initial value for the corresponding property.</param>
/// <param name="int">The initial value for the corresponding property.</param>
/// <param name="maxHit">The initial value for the corresponding property.</param>
/// <param name="maxHP">The initial value for the corresponding property.</param>
/// <param name="maxMP">The initial value for the corresponding property.</param>
/// <param name="minHit">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="perc">The initial value for the corresponding property.</param>
/// <param name="reqacc">The initial value for the corresponding property.</param>
/// <param name="reqagi">The initial value for the corresponding property.</param>
/// <param name="reqarmor">The initial value for the corresponding property.</param>
/// <param name="reqbra">The initial value for the corresponding property.</param>
/// <param name="reqdex">The initial value for the corresponding property.</param>
/// <param name="reqevade">The initial value for the corresponding property.</param>
/// <param name="reqimm">The initial value for the corresponding property.</param>
/// <param name="reqint">The initial value for the corresponding property.</param>
/// <param name="type">The initial value for the corresponding property.</param>
/// <param name="value">The initial value for the corresponding property.</param>
/// <param name="width">The initial value for the corresponding property.</param>
public ItemTable(System.Int16 @agi, System.Byte @amount, System.UInt16 @armor, System.Int16 @bra, System.Int16 @defence, System.String @description, System.Int16 @dex, System.Int16 @evade, NetGore.GrhIndex @graphic, System.Byte @height, DemoGame.SPValueType @hP, DemoGame.Server.ItemID @iD, System.Int16 @imm, System.Int16 @int, System.Int16 @maxHit, System.Int16 @maxHP, System.Int16 @maxMP, System.Int16 @minHit, DemoGame.SPValueType @mP, System.String @name, System.Int16 @perc, System.Byte @reqacc, System.Byte @reqagi, System.Byte @reqarmor, System.Byte @reqbra, System.Byte @reqdex, System.Byte @reqevade, System.Byte @reqimm, System.Byte @reqint, DemoGame.ItemType @type, System.Int32 @value, System.Byte @width)
{
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
this.Amount = (System.Byte)@amount;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
this.Description = (System.String)@description;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
this.Graphic = (NetGore.GrhIndex)@graphic;
this.Height = (System.Byte)@height;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (DemoGame.Server.ItemID)@iD;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxHit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxHP);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxMP);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minHit);
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)@perc);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)@reqacc);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@reqagi);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@reqarmor);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@reqbra);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@reqdex);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@reqevade);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@reqimm);
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@reqint);
this.Type = (DemoGame.ItemType)@type;
this.Value = (System.Int32)@value;
this.Width = (System.Byte)@width;
}
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
dic["@agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@amount"] = (System.Byte)source.Amount;
dic["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@bra"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@description"] = (System.String)source.Description;
dic["@dex"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@graphic"] = (NetGore.GrhIndex)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (DemoGame.Server.ItemID)source.ID;
dic["@imm"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@mp"] = (DemoGame.SPValueType)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
dic["@reqacc"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc);
dic["@reqagi"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@reqarmor"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@reqbra"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@reqdex"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@reqevade"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@reqimm"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@reqint"] = (System.Byte)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@type"] = (DemoGame.ItemType)source.Type;
dic["@value"] = (System.Int32)source.Value;
dic["@width"] = (System.Byte)source.Width;
}

public void CopyValuesFrom(IItemTable source)
{
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.Amount = (System.Byte)source.Amount;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
this.Description = (System.String)source.Description;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
this.Graphic = (NetGore.GrhIndex)source.Graphic;
this.Height = (System.Byte)source.Height;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (DemoGame.Server.ItemID)source.ID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm));
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int));
this.Type = (DemoGame.ItemType)source.Type;
this.Value = (System.Int32)source.Value;
this.Width = (System.Byte)source.Width;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "amount":
return Amount;

case "armor":
return GetStat((DemoGame.StatType)DemoGame.StatType.Armor);

case "bra":
return GetStat((DemoGame.StatType)DemoGame.StatType.Bra);

case "defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "description":
return Description;

case "dex":
return GetStat((DemoGame.StatType)DemoGame.StatType.Dex);

case "evade":
return GetStat((DemoGame.StatType)DemoGame.StatType.Evade);

case "graphic":
return Graphic;

case "height":
return Height;

case "hp":
return HP;

case "id":
return ID;

case "imm":
return GetStat((DemoGame.StatType)DemoGame.StatType.Imm);

case "int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "maxhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);

case "maxhp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);

case "maxmp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);

case "minhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);

case "mp":
return MP;

case "name":
return Name;

case "perc":
return GetStat((DemoGame.StatType)DemoGame.StatType.Perc);

case "reqacc":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc);

case "reqagi":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "reqarmor":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor);

case "reqbra":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra);

case "reqdex":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex);

case "reqevade":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade);

case "reqimm":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm);

case "reqint":
return GetReqStat((DemoGame.StatType)DemoGame.StatType.Int);

case "type":
return Type;

case "value":
return Value;

case "width":
return Width;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "agi":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "amount":
this.Amount = (System.Byte)value;
break;

case "armor":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "bra":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "defence":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "description":
this.Description = (System.String)value;
break;

case "dex":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "evade":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
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
this.ID = (DemoGame.Server.ItemID)value;
break;

case "imm":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "int":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "maxhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "maxhp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "maxmp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "minhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "mp":
this.MP = (DemoGame.SPValueType)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "perc":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)value);
break;

case "reqacc":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)value);
break;

case "reqagi":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "reqarmor":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "reqbra":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "reqdex":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "reqevade":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
break;

case "reqimm":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "reqint":
this.SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "type":
this.Type = (DemoGame.ItemType)value;
break;

case "value":
this.Value = (System.Int32)value;
break;

case "width":
this.Width = (System.Byte)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "agi":
return new ColumnMetadata("agi", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "amount":
return new ColumnMetadata("amount", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "armor":
return new ColumnMetadata("armor", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "bra":
return new ColumnMetadata("bra", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "defence":
return new ColumnMetadata("defence", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "description":
return new ColumnMetadata("description", "", "varchar(255)", null, typeof(System.String), false, false, false);

case "dex":
return new ColumnMetadata("dex", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "evade":
return new ColumnMetadata("evade", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "graphic":
return new ColumnMetadata("graphic", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "height":
return new ColumnMetadata("height", "", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "imm":
return new ColumnMetadata("imm", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "int":
return new ColumnMetadata("int", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "maxhit":
return new ColumnMetadata("maxhit", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "maxhp":
return new ColumnMetadata("maxhp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "maxmp":
return new ColumnMetadata("maxmp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "minhit":
return new ColumnMetadata("minhit", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(255)", null, typeof(System.String), false, false, false);

case "perc":
return new ColumnMetadata("perc", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "reqacc":
return new ColumnMetadata("reqacc", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqagi":
return new ColumnMetadata("reqagi", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqarmor":
return new ColumnMetadata("reqarmor", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqbra":
return new ColumnMetadata("reqbra", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqdex":
return new ColumnMetadata("reqdex", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqevade":
return new ColumnMetadata("reqevade", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqimm":
return new ColumnMetadata("reqimm", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "reqint":
return new ColumnMetadata("reqint", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "type":
return new ColumnMetadata("type", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "value":
return new ColumnMetadata("value", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "width":
return new ColumnMetadata("width", "", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
}

/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
private class StatConstDictionary : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>
{
    /// <summary>
    /// Array containing the actual values. The index of this array is found through the value returned
    /// from the _lookupTable.
    /// </summary>
    readonly System.Int32[] _values;
    
    /// <summary>
    /// Array that maps the integer value of key type to the index of the _values array.
    /// </summary>
    static readonly System.Int32[] _lookupTable;

    /// <summary>
    /// StatConstDictionary static constructor.
    /// </summary>
    static StatConstDictionary()
    {
        DemoGame.StatType[] asArray = Enum.GetValues(typeof(DemoGame.StatType)).Cast<DemoGame.StatType>().ToArray();
        _lookupTable = new System.Int32[asArray.Length];

        for (System.Int32 i = 0; i < _lookupTable.Length; i++)
            _lookupTable[i] = (System.Int32)asArray[i];
    }
    
    /// <summary>
    /// StatConstDictionary constructor.
    /// </summary>
    public StatConstDictionary()
    {
        _values = new System.Int32[_lookupTable.Length];
    }
    
	/// <summary>
	/// Gets or sets an item's value using the <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key for the value to get or set.</param>
	/// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
    public System.Int32 this[DemoGame.StatType key]
    {
        get
        {
            return _values[_lookupTable[(System.Int32)key]];
        }
        set
        {
            _values[_lookupTable[(System.Int32)key]] = value;
        }
    }

    #region IEnumerable<KeyValuePair<DemoGame.StatType,System.Int32>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> GetEnumerator()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            yield return new System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>((DemoGame.StatType)i, _values[i]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `ReqStat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
private class ReqStatConstDictionary : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>
{
    /// <summary>
    /// Array containing the actual values. The index of this array is found through the value returned
    /// from the _lookupTable.
    /// </summary>
    readonly System.Int32[] _values;
    
    /// <summary>
    /// Array that maps the integer value of key type to the index of the _values array.
    /// </summary>
    static readonly System.Int32[] _lookupTable;

    /// <summary>
    /// ReqStatConstDictionary static constructor.
    /// </summary>
    static ReqStatConstDictionary()
    {
        DemoGame.StatType[] asArray = Enum.GetValues(typeof(DemoGame.StatType)).Cast<DemoGame.StatType>().ToArray();
        _lookupTable = new System.Int32[asArray.Length];

        for (System.Int32 i = 0; i < _lookupTable.Length; i++)
            _lookupTable[i] = (System.Int32)asArray[i];
    }
    
    /// <summary>
    /// ReqStatConstDictionary constructor.
    /// </summary>
    public ReqStatConstDictionary()
    {
        _values = new System.Int32[_lookupTable.Length];
    }
    
	/// <summary>
	/// Gets or sets an item's value using the <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key for the value to get or set.</param>
	/// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
    public System.Int32 this[DemoGame.StatType key]
    {
        get
        {
            return _values[_lookupTable[(System.Int32)key]];
        }
        set
        {
            _values[_lookupTable[(System.Int32)key]] = value;
        }
    }

    #region IEnumerable<KeyValuePair<DemoGame.StatType,System.Int32>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> GetEnumerator()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            yield return new System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>((DemoGame.StatType)i, _values[i]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
}

}
