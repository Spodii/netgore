using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `item_template`.
/// </summary>
public interface IItemTemplateTable
{
System.Int32 GetStat(DemoGame.StatType key);

System.Void SetStat(DemoGame.StatType key, System.Int32 value);

/// <summary>
/// Gets the value for the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value for the database column `graphic`.
/// </summary>
System.UInt16 Graphic
{
get;
}
/// <summary>
/// Gets the value for the database column `height`.
/// </summary>
System.Byte Height
{
get;
}
/// <summary>
/// Gets the value for the database column `hp`.
/// </summary>
System.UInt16 Hp
{
get;
}
/// <summary>
/// Gets the value for the database column `id`.
/// </summary>
DemoGame.Server.ItemTemplateID Id
{
get;
}
/// <summary>
/// Gets the value for the database column `mp`.
/// </summary>
System.UInt16 Mp
{
get;
}
/// <summary>
/// Gets the value for the database column `name`.
/// </summary>
System.String Name
{
get;
}
System.Int32 GetReqStat(DemoGame.StatType key);

System.Void SetReqStat(DemoGame.StatType key, System.Int32 value);

/// <summary>
/// Gets the value for the database column `type`.
/// </summary>
System.Byte Type
{
get;
}
/// <summary>
/// Gets the value for the database column `value`.
/// </summary>
System.Int32 Value
{
get;
}
/// <summary>
/// Gets the value for the database column `width`.
/// </summary>
System.Byte Width
{
get;
}
}

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
public System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
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
System.UInt16 _hp;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _id;
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
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.ItemTemplateID Id
{
get
{
return (DemoGame.Server.ItemTemplateID)_id;
}
set
{
this._id = (System.UInt16)value;
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
public ItemTemplateTable(System.UInt16 @agi, System.UInt16 @armor, System.UInt16 @bra, System.UInt16 @defence, System.String @description, System.UInt16 @dex, System.UInt16 @evade, System.UInt16 @graphic, System.Byte @height, System.UInt16 @hp, DemoGame.Server.ItemTemplateID @id, System.UInt16 @imm, System.UInt16 @int, System.UInt16 @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.UInt16 @minhit, System.UInt16 @mp, System.String @name, System.UInt16 @perc, System.Byte @reqacc, System.Byte @reqagi, System.Byte @reqarmor, System.Byte @reqbra, System.Byte @reqdex, System.Byte @reqevade, System.Byte @reqimm, System.Byte @reqint, System.Byte @type, System.Int32 @value, System.Byte @width)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
Description = (System.String)@description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
Graphic = (System.UInt16)@graphic;
Height = (System.Byte)@height;
Hp = (System.UInt16)@hp;
Id = (DemoGame.Server.ItemTemplateID)@id;
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
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("agi")));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("armor")));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("bra")));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("defence")));
Description = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("description"));
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("dex")));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("evade")));
Graphic = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("graphic"));
Height = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("height"));
Hp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
Id = (DemoGame.Server.ItemTemplateID)(DemoGame.Server.ItemTemplateID)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
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
public static void CopyValues(IItemTemplateTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@agi"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@bra"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@description"] = (System.String)source.Description;
dic["@dex"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@graphic"] = (System.UInt16)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (System.UInt16)source.Hp;
dic["@id"] = (DemoGame.Server.ItemTemplateID)source.Id;
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
public static void CopyValues(IItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@agi"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@armor"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@bra"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@description"] = (System.String)source.Description;
paramValues["@dex"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@graphic"] = (System.UInt16)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (System.UInt16)source.Hp;
paramValues["@id"] = (DemoGame.Server.ItemTemplateID)source.Id;
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

public void CopyValuesFrom(IItemTemplateTable source)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
Description = (System.String)source.Description;
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
Graphic = (System.UInt16)source.Graphic;
Height = (System.Byte)source.Height;
Hp = (System.UInt16)source.Hp;
Id = (DemoGame.Server.ItemTemplateID)source.Id;
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
