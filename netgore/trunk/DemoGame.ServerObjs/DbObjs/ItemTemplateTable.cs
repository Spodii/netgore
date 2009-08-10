using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `item_template`.
/// </summary>
public class ItemTemplateTable : IItemTemplateTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"agi", "armor", "bra", "defence", "description", "dex", "evade", "graphic", "height", "hp", "id", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "reqacc", "reqagi", "reqarmor", "reqbra", "reqdex", "reqevade", "reqimm", "reqint", "type", "value", "width" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"agi", "armor", "bra", "defence", "description", "dex", "evade", "graphic", "height", "hp", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "reqacc", "reqagi", "reqarmor", "reqbra", "reqdex", "reqevade", "reqimm", "reqint", "type", "value", "width" };
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
public const System.String TableName = "item_template";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 31;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
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
/// The underlying database type is `smallint(5) unsigned`.
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
/// The underlying database type is `tinyint(3) unsigned`.
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
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemTemplateID ID
{
get
{
return (DemoGame.Server.ItemTemplateID)_iD;
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
public System.Byte Type
{
get
{
return (System.Byte)_type;
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
/// The underlying database type is `tinyint(3) unsigned`.
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
/// <param name="agi">The initial value for the corresponding property.</param>
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
public ItemTemplateTable(System.Int16 @agi, System.Int16 @armor, System.Int16 @bra, System.Int16 @defence, System.String @description, System.Int16 @dex, System.Int16 @evade, NetGore.GrhIndex @graphic, System.Byte @height, DemoGame.SPValueType @hP, DemoGame.Server.ItemTemplateID @iD, System.Int16 @imm, System.Int16 @int, System.Int16 @maxHit, System.Int16 @maxHP, System.Int16 @maxMP, System.Int16 @minHit, DemoGame.SPValueType @mP, System.String @name, System.Int16 @perc, System.Byte @reqacc, System.Byte @reqagi, System.Byte @reqarmor, System.Byte @reqbra, System.Byte @reqdex, System.Byte @reqevade, System.Byte @reqimm, System.Byte @reqint, System.Byte @type, System.Int32 @value, System.Byte @width)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
Description = (System.String)@description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
Graphic = (NetGore.GrhIndex)@graphic;
Height = (System.Byte)@height;
HP = (DemoGame.SPValueType)@hP;
ID = (DemoGame.Server.ItemTemplateID)@iD;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxHit);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxHP);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxMP);
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minHit);
MP = (DemoGame.SPValueType)@mP;
Name = (System.String)@name;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)@perc);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)@reqacc);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@reqagi);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@reqarmor);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@reqbra);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@reqdex);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@reqevade);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@reqimm);
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@reqint);
Type = (System.Byte)@type;
Value = (System.Int32)@value;
Width = (System.Byte)@width;
}
/// <summary>
/// ItemTemplateTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public ItemTemplateTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public ItemTemplateTable(IItemTemplateTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("agi");
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("armor");
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("bra");
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("defence");
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("description");
Description = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("dex");
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("evade");
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("graphic");
Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("height");
Height = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("hp");
HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");
ID = (DemoGame.Server.ItemTemplateID)(DemoGame.Server.ItemTemplateID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("imm");
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("int");
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxhit");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxhp");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxmp");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("minhit");
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("mp");
MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");
Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("reqacc");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqagi");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqarmor");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqbra");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqdex");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqevade");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqimm");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("reqint");
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("type");
Type = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("value");
Value = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("width");
Width = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
dic["@agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@armor"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@bra"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@description"] = (System.String)source.Description;
dic["@dex"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@graphic"] = (NetGore.GrhIndex)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (DemoGame.Server.ItemTemplateID)source.ID;
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
dic["@type"] = (System.Byte)source.Type;
dic["@value"] = (System.Int32)source.Value;
dic["@width"] = (System.Byte)source.Width;
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public void CopyValues(NetGore.Db.DbParameterValues paramValues)
{
CopyValues(this, paramValues);
}
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(IItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@armor"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@bra"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@description"] = (System.String)source.Description;
paramValues["@dex"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@graphic"] = (NetGore.GrhIndex)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (DemoGame.SPValueType)source.HP;
paramValues["@id"] = (DemoGame.Server.ItemTemplateID)source.ID;
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
paramValues["@type"] = (System.Byte)source.Type;
paramValues["@value"] = (System.Int32)source.Value;
paramValues["@width"] = (System.Byte)source.Width;
}

public void CopyValuesFrom(IItemTemplateTable source)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
Description = (System.String)source.Description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
Graphic = (NetGore.GrhIndex)source.Graphic;
Height = (System.Byte)source.Height;
HP = (DemoGame.SPValueType)source.HP;
ID = (DemoGame.Server.ItemTemplateID)source.ID;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
MP = (DemoGame.SPValueType)source.MP;
Name = (System.String)source.Name;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Acc));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Agi));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Armor));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Bra));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Evade));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Imm));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetReqStat((DemoGame.StatType)DemoGame.StatType.Int));
Type = (System.Byte)source.Type;
Value = (System.Int32)source.Value;
Width = (System.Byte)source.Width;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

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
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "armor":
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "bra":
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "defence":
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "description":
Description = (System.String)value;
break;

case "dex":
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "evade":
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
break;

case "graphic":
Graphic = (NetGore.GrhIndex)value;
break;

case "height":
Height = (System.Byte)value;
break;

case "hp":
HP = (DemoGame.SPValueType)value;
break;

case "id":
ID = (DemoGame.Server.ItemTemplateID)value;
break;

case "imm":
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "int":
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "maxhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "maxhp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "maxmp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "minhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "mp":
MP = (DemoGame.SPValueType)value;
break;

case "name":
Name = (System.String)value;
break;

case "perc":
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)value);
break;

case "reqacc":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)value);
break;

case "reqagi":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "reqarmor":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "reqbra":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "reqdex":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "reqevade":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
break;

case "reqimm":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "reqint":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "type":
Type = (System.Byte)value;
break;

case "value":
Value = (System.Int32)value;
break;

case "width":
Width = (System.Byte)value;
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

case "armor":
return new ColumnMetadata("armor", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

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
return new ColumnMetadata("graphic", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "height":
return new ColumnMetadata("height", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "0", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

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
return new ColumnMetadata("width", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
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
public void TryReadValues(System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "agi":
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "armor":
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "bra":
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "defence":
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "description":
Description = (System.String)(System.String)dataReader.GetString(i);
break;


case "dex":
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "evade":
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "graphic":
Graphic = (NetGore.GrhIndex)(NetGore.GrhIndex)dataReader.GetUInt16(i);
break;


case "height":
Height = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "hp":
HP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "id":
ID = (DemoGame.Server.ItemTemplateID)(DemoGame.Server.ItemTemplateID)dataReader.GetUInt16(i);
break;


case "imm":
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "int":
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxhp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxmp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "minhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "mp":
MP = (DemoGame.SPValueType)(DemoGame.SPValueType)dataReader.GetInt16(i);
break;


case "name":
Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "perc":
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "reqacc":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqagi":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqarmor":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqbra":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqdex":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqevade":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqimm":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "reqint":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "type":
Type = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "value":
Value = (System.Int32)(System.Int32)dataReader.GetInt32(i);
break;


case "width":
Width = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public void TryCopyValues(NetGore.Db.DbParameterValues paramValues)
{
TryCopyValues(this, paramValues);
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
public static void TryCopyValues(IItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@agi":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
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
