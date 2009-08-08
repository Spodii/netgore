using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `item`.
/// </summary>
public interface IItemTable
{
/// <summary>
/// Gets the value of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetStat(DemoGame.StatType key);

/// <summary>
/// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
void SetStat(DemoGame.StatType key, System.Int32 value);

/// <summary>
/// Gets the value of the database column `amount`.
/// </summary>
System.Byte Amount
{
get;
}
/// <summary>
/// Gets the value of the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value of the database column `graphic`.
/// </summary>
System.UInt16 Graphic
{
get;
}
/// <summary>
/// Gets the value of the database column `height`.
/// </summary>
System.Byte Height
{
get;
}
/// <summary>
/// Gets the value of the database column `hp`.
/// </summary>
System.UInt16 Hp
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.ItemID Id
{
get;
}
/// <summary>
/// Gets the value of the database column `mp`.
/// </summary>
System.UInt16 Mp
{
get;
}
/// <summary>
/// Gets the value of the database column `name`.
/// </summary>
System.String Name
{
get;
}
/// <summary>
/// Gets the value of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetReqStat(DemoGame.StatType key);

/// <summary>
/// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
void SetReqStat(DemoGame.StatType key, System.Int32 value);

/// <summary>
/// Gets the value of the database column `type`.
/// </summary>
System.Byte Type
{
get;
}
/// <summary>
/// Gets the value of the database column `value`.
/// </summary>
System.Int32 Value
{
get;
}
/// <summary>
/// Gets the value of the database column `width`.
/// </summary>
System.Byte Width
{
get;
}
}

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
public System.Collections.Generic.IEnumerable<System.String> DbColumns
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
public System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
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
public System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
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
public System.Collections.Generic.IEnumerable<System.String> StatColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_statColumns;
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
public System.Collections.Generic.IEnumerable<System.String> ReqStatColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_reqStatColumns;
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
System.UInt16 _hp;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _id;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.UInt16 _mp;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
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
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.UInt16)_stat[(DemoGame.StatType)key];
}
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.UInt16)value;
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
public System.UInt16 Graphic
{
get
{
return (System.UInt16)_graphic;
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Hp
{
get
{
return (System.UInt16)_hp;
}
set
{
this._hp = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.
/// </summary>
public DemoGame.Server.ItemID Id
{
get
{
return (DemoGame.Server.ItemID)_id;
}
set
{
this._id = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Mp
{
get
{
return (System.UInt16)_mp;
}
set
{
this._mp = (System.UInt16)value;
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
public System.Int32 GetReqStat(DemoGame.StatType key)
{
return (System.Byte)_reqStat[(DemoGame.StatType)key];
}
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
/// <param name="hp">The initial value for the corresponding property.</param>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="imm">The initial value for the corresponding property.</param>
/// <param name="int">The initial value for the corresponding property.</param>
/// <param name="maxhit">The initial value for the corresponding property.</param>
/// <param name="maxhp">The initial value for the corresponding property.</param>
/// <param name="maxmp">The initial value for the corresponding property.</param>
/// <param name="minhit">The initial value for the corresponding property.</param>
/// <param name="mp">The initial value for the corresponding property.</param>
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
public ItemTable(System.UInt16 @agi, System.Byte @amount, System.UInt16 @armor, System.UInt16 @bra, System.UInt16 @defence, System.String @description, System.UInt16 @dex, System.UInt16 @evade, System.UInt16 @graphic, System.Byte @height, System.UInt16 @hp, DemoGame.Server.ItemID @id, System.UInt16 @imm, System.UInt16 @int, System.UInt16 @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.UInt16 @minhit, System.UInt16 @mp, System.String @name, System.UInt16 @perc, System.Byte @reqacc, System.Byte @reqagi, System.Byte @reqarmor, System.Byte @reqbra, System.Byte @reqdex, System.Byte @reqevade, System.Byte @reqimm, System.Byte @reqint, System.Byte @type, System.Int32 @value, System.Byte @width)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
Amount = (System.Byte)@amount;
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
Description = (System.String)@description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
Graphic = (System.UInt16)@graphic;
Height = (System.Byte)@height;
Hp = (System.UInt16)@hp;
Id = (DemoGame.Server.ItemID)@id;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxhit);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxhp);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxmp);
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minhit);
Mp = (System.UInt16)@mp;
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
/// ItemTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public ItemTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public ItemTable(IItemTable source)
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
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("agi")));
Amount = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("amount"));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("armor")));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("bra")));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("defence")));
Description = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("description"));
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("dex")));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("evade")));
Graphic = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("graphic"));
Height = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("height"));
Hp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
Id = (DemoGame.Server.ItemID)(DemoGame.Server.ItemID)dataReader.GetInt32(dataReader.GetOrdinal("id"));
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("imm")));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("int")));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhit")));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp")));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp")));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("minhit")));
Mp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("mp"));
Name = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("perc")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqacc")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqagi")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqarmor")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqbra")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqdex")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqevade")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqimm")));
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqint")));
Type = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("type"));
Value = (System.Int32)(System.Int32)dataReader.GetInt32(dataReader.GetOrdinal("value"));
Width = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("width"));
}

/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IItemTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@agi"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@amount"] = (System.Byte)source.Amount;
dic["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@bra"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@description"] = (System.String)source.Description;
dic["@dex"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@graphic"] = (System.UInt16)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (System.UInt16)source.Hp;
dic["@id"] = (DemoGame.Server.ItemID)source.Id;
dic["@imm"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@int"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@maxhit"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@minhit"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@mp"] = (System.UInt16)source.Mp;
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
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
public static void CopyValues(IItemTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@agi"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@amount"] = (System.Byte)source.Amount;
paramValues["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@bra"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@description"] = (System.String)source.Description;
paramValues["@dex"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@graphic"] = (System.UInt16)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (System.UInt16)source.Hp;
paramValues["@id"] = (DemoGame.Server.ItemID)source.Id;
paramValues["@imm"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@maxhit"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@mp"] = (System.UInt16)source.Mp;
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
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

public void CopyValuesFrom(IItemTable source)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
Amount = (System.Byte)source.Amount;
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
Description = (System.String)source.Description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
Graphic = (System.UInt16)source.Graphic;
Height = (System.Byte)source.Height;
Hp = (System.UInt16)source.Hp;
Id = (DemoGame.Server.ItemID)source.Id;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
Mp = (System.UInt16)source.Mp;
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
return Hp;

case "id":
return Id;

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
return Mp;

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

case "amount":
Amount = (System.Byte)value;
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
Graphic = (System.UInt16)value;
break;

case "height":
Height = (System.Byte)value;
break;

case "hp":
Hp = (System.UInt16)value;
break;

case "id":
Id = (DemoGame.Server.ItemID)value;
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
Mp = (System.UInt16)value;
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
return new ColumnMetadata("agi", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "amount":
return new ColumnMetadata("amount", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "armor":
return new ColumnMetadata("armor", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "bra":
return new ColumnMetadata("bra", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "defence":
return new ColumnMetadata("defence", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "description":
return new ColumnMetadata("description", "", "varchar(255)", null, typeof(System.String), false, false, false);

case "dex":
return new ColumnMetadata("dex", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "evade":
return new ColumnMetadata("evade", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "graphic":
return new ColumnMetadata("graphic", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "height":
return new ColumnMetadata("height", "", "tinyint(3) unsigned", "16", typeof(System.Byte), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "imm":
return new ColumnMetadata("imm", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "int":
return new ColumnMetadata("int", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "maxhit":
return new ColumnMetadata("maxhit", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "maxhp":
return new ColumnMetadata("maxhp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "maxmp":
return new ColumnMetadata("maxmp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "minhit":
return new ColumnMetadata("minhit", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(255)", null, typeof(System.String), false, false, false);

case "perc":
return new ColumnMetadata("perc", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

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
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)dataReader.GetUInt16(i));
break;


case "amount":
Amount = (System.Byte)dataReader.GetByte(i);
break;


case "armor":
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)dataReader.GetUInt16(i));
break;


case "bra":
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)dataReader.GetUInt16(i));
break;


case "defence":
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)dataReader.GetUInt16(i));
break;


case "description":
Description = (System.String)dataReader.GetString(i);
break;


case "dex":
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)dataReader.GetUInt16(i));
break;


case "evade":
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)dataReader.GetUInt16(i));
break;


case "graphic":
Graphic = (System.UInt16)dataReader.GetUInt16(i);
break;


case "height":
Height = (System.Byte)dataReader.GetByte(i);
break;


case "hp":
Hp = (System.UInt16)dataReader.GetUInt16(i);
break;


case "id":
Id = (DemoGame.Server.ItemID)dataReader.GetInt32(i);
break;


case "imm":
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)dataReader.GetUInt16(i));
break;


case "int":
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)dataReader.GetUInt16(i));
break;


case "maxhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)dataReader.GetUInt16(i));
break;


case "maxhp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)dataReader.GetUInt16(i));
break;


case "maxmp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)dataReader.GetUInt16(i));
break;


case "minhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)dataReader.GetUInt16(i));
break;


case "mp":
Mp = (System.UInt16)dataReader.GetUInt16(i);
break;


case "name":
Name = (System.String)dataReader.GetString(i);
break;


case "perc":
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)dataReader.GetUInt16(i));
break;


case "reqacc":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)dataReader.GetByte(i));
break;


case "reqagi":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)dataReader.GetByte(i));
break;


case "reqarmor":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)dataReader.GetByte(i));
break;


case "reqbra":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)dataReader.GetByte(i));
break;


case "reqdex":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)dataReader.GetByte(i));
break;


case "reqevade":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)dataReader.GetByte(i));
break;


case "reqimm":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)dataReader.GetByte(i));
break;


case "reqint":
SetReqStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)dataReader.GetByte(i));
break;


case "type":
Type = (System.Byte)dataReader.GetByte(i);
break;


case "value":
Value = (System.Int32)dataReader.GetInt32(i);
break;


case "width":
Width = (System.Byte)dataReader.GetByte(i);
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
public static void TryCopyValues(IItemTable source, NetGore.Db.DbParameterValues paramValues)
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
paramValues[i] = source.Hp;
break;


case "@id":
paramValues[i] = source.Id;
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
paramValues[i] = source.Mp;
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
private class StatConstDictionary
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
}
/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `ReqStat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
private class ReqStatConstDictionary
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
}
}

}
