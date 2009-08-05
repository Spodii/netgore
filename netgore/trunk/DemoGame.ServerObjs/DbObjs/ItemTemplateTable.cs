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
/// <summary>
/// Gets the value for the database column `agi`.
/// </summary>
System.UInt16 Agi
{
get;
}
/// <summary>
/// Gets the value for the database column `armor`.
/// </summary>
System.UInt16 Armor
{
get;
}
/// <summary>
/// Gets the value for the database column `bra`.
/// </summary>
System.UInt16 Bra
{
get;
}
/// <summary>
/// Gets the value for the database column `defence`.
/// </summary>
System.UInt16 Defence
{
get;
}
/// <summary>
/// Gets the value for the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value for the database column `dex`.
/// </summary>
System.UInt16 Dex
{
get;
}
/// <summary>
/// Gets the value for the database column `evade`.
/// </summary>
System.UInt16 Evade
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
System.UInt16 Id
{
get;
}
/// <summary>
/// Gets the value for the database column `imm`.
/// </summary>
System.UInt16 Imm
{
get;
}
/// <summary>
/// Gets the value for the database column `int`.
/// </summary>
System.UInt16 Int
{
get;
}
/// <summary>
/// Gets the value for the database column `maxhit`.
/// </summary>
System.UInt16 Maxhit
{
get;
}
/// <summary>
/// Gets the value for the database column `maxhp`.
/// </summary>
System.UInt16 Maxhp
{
get;
}
/// <summary>
/// Gets the value for the database column `maxmp`.
/// </summary>
System.UInt16 Maxmp
{
get;
}
/// <summary>
/// Gets the value for the database column `minhit`.
/// </summary>
System.UInt16 Minhit
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
/// <summary>
/// Gets the value for the database column `perc`.
/// </summary>
System.UInt16 Perc
{
get;
}
/// <summary>
/// Gets the value for the database column `reqacc`.
/// </summary>
System.Byte Reqacc
{
get;
}
/// <summary>
/// Gets the value for the database column `reqagi`.
/// </summary>
System.Byte Reqagi
{
get;
}
/// <summary>
/// Gets the value for the database column `reqarmor`.
/// </summary>
System.Byte Reqarmor
{
get;
}
/// <summary>
/// Gets the value for the database column `reqbra`.
/// </summary>
System.Byte Reqbra
{
get;
}
/// <summary>
/// Gets the value for the database column `reqdex`.
/// </summary>
System.Byte Reqdex
{
get;
}
/// <summary>
/// Gets the value for the database column `reqevade`.
/// </summary>
System.Byte Reqevade
{
get;
}
/// <summary>
/// Gets the value for the database column `reqimm`.
/// </summary>
System.Byte Reqimm
{
get;
}
/// <summary>
/// Gets the value for the database column `reqint`.
/// </summary>
System.Byte Reqint
{
get;
}
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
return _dbColumns;
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
/// The field that maps onto the database column `agi`.
/// </summary>
System.UInt16 _agi;
/// <summary>
/// The field that maps onto the database column `armor`.
/// </summary>
System.UInt16 _armor;
/// <summary>
/// The field that maps onto the database column `bra`.
/// </summary>
System.UInt16 _bra;
/// <summary>
/// The field that maps onto the database column `defence`.
/// </summary>
System.UInt16 _defence;
/// <summary>
/// The field that maps onto the database column `description`.
/// </summary>
System.String _description;
/// <summary>
/// The field that maps onto the database column `dex`.
/// </summary>
System.UInt16 _dex;
/// <summary>
/// The field that maps onto the database column `evade`.
/// </summary>
System.UInt16 _evade;
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
/// The field that maps onto the database column `imm`.
/// </summary>
System.UInt16 _imm;
/// <summary>
/// The field that maps onto the database column `int`.
/// </summary>
System.UInt16 _int;
/// <summary>
/// The field that maps onto the database column `maxhit`.
/// </summary>
System.UInt16 _maxhit;
/// <summary>
/// The field that maps onto the database column `maxhp`.
/// </summary>
System.UInt16 _maxhp;
/// <summary>
/// The field that maps onto the database column `maxmp`.
/// </summary>
System.UInt16 _maxmp;
/// <summary>
/// The field that maps onto the database column `minhit`.
/// </summary>
System.UInt16 _minhit;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.UInt16 _mp;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `perc`.
/// </summary>
System.UInt16 _perc;
/// <summary>
/// The field that maps onto the database column `reqacc`.
/// </summary>
System.Byte _reqacc;
/// <summary>
/// The field that maps onto the database column `reqagi`.
/// </summary>
System.Byte _reqagi;
/// <summary>
/// The field that maps onto the database column `reqarmor`.
/// </summary>
System.Byte _reqarmor;
/// <summary>
/// The field that maps onto the database column `reqbra`.
/// </summary>
System.Byte _reqbra;
/// <summary>
/// The field that maps onto the database column `reqdex`.
/// </summary>
System.Byte _reqdex;
/// <summary>
/// The field that maps onto the database column `reqevade`.
/// </summary>
System.Byte _reqevade;
/// <summary>
/// The field that maps onto the database column `reqimm`.
/// </summary>
System.Byte _reqimm;
/// <summary>
/// The field that maps onto the database column `reqint`.
/// </summary>
System.Byte _reqint;
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
/// Gets or sets the value for the field that maps onto the database column `agi`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Agi
{
get
{
return _agi;
}
set
{
this._agi = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `armor`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Armor
{
get
{
return _armor;
}
set
{
this._armor = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `bra`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Bra
{
get
{
return _bra;
}
set
{
this._bra = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `defence`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Defence
{
get
{
return _defence;
}
set
{
this._defence = value;
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
return _description;
}
set
{
this._description = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `dex`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Dex
{
get
{
return _dex;
}
set
{
this._dex = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `evade`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Evade
{
get
{
return _evade;
}
set
{
this._evade = value;
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
return _graphic;
}
set
{
this._graphic = value;
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
return _height;
}
set
{
this._height = value;
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
return _hp;
}
set
{
this._hp = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 Id
{
get
{
return _id;
}
set
{
this._id = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `imm`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Imm
{
get
{
return _imm;
}
set
{
this._imm = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `int`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Int
{
get
{
return _int;
}
set
{
this._int = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxhit`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Maxhit
{
get
{
return _maxhit;
}
set
{
this._maxhit = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxhp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Maxhp
{
get
{
return _maxhp;
}
set
{
this._maxhp = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxmp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Maxmp
{
get
{
return _maxmp;
}
set
{
this._maxmp = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `minhit`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Minhit
{
get
{
return _minhit;
}
set
{
this._minhit = value;
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
return _mp;
}
set
{
this._mp = value;
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
return _name;
}
set
{
this._name = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `perc`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 Perc
{
get
{
return _perc;
}
set
{
this._perc = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqacc`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqacc
{
get
{
return _reqacc;
}
set
{
this._reqacc = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqagi`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqagi
{
get
{
return _reqagi;
}
set
{
this._reqagi = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqarmor`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqarmor
{
get
{
return _reqarmor;
}
set
{
this._reqarmor = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqbra`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqbra
{
get
{
return _reqbra;
}
set
{
this._reqbra = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqdex`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqdex
{
get
{
return _reqdex;
}
set
{
this._reqdex = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqevade`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqevade
{
get
{
return _reqevade;
}
set
{
this._reqevade = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqimm`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqimm
{
get
{
return _reqimm;
}
set
{
this._reqimm = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `reqint`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Reqint
{
get
{
return _reqint;
}
set
{
this._reqint = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `type`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Type
{
get
{
return _type;
}
set
{
this._type = value;
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
return _value;
}
set
{
this._value = value;
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
return _width;
}
set
{
this._width = value;
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
public ItemTemplateTable(System.UInt16 @agi, System.UInt16 @armor, System.UInt16 @bra, System.UInt16 @defence, System.String @description, System.UInt16 @dex, System.UInt16 @evade, System.UInt16 @graphic, System.Byte @height, System.UInt16 @hp, System.UInt16 @id, System.UInt16 @imm, System.UInt16 @int, System.UInt16 @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.UInt16 @minhit, System.UInt16 @mp, System.String @name, System.UInt16 @perc, System.Byte @reqacc, System.Byte @reqagi, System.Byte @reqarmor, System.Byte @reqbra, System.Byte @reqdex, System.Byte @reqevade, System.Byte @reqimm, System.Byte @reqint, System.Byte @type, System.Int32 @value, System.Byte @width)
{
this.Agi = @agi;
this.Armor = @armor;
this.Bra = @bra;
this.Defence = @defence;
this.Description = @description;
this.Dex = @dex;
this.Evade = @evade;
this.Graphic = @graphic;
this.Height = @height;
this.Hp = @hp;
this.Id = @id;
this.Imm = @imm;
this.Int = @int;
this.Maxhit = @maxhit;
this.Maxhp = @maxhp;
this.Maxmp = @maxmp;
this.Minhit = @minhit;
this.Mp = @mp;
this.Name = @name;
this.Perc = @perc;
this.Reqacc = @reqacc;
this.Reqagi = @reqagi;
this.Reqarmor = @reqarmor;
this.Reqbra = @reqbra;
this.Reqdex = @reqdex;
this.Reqevade = @reqevade;
this.Reqimm = @reqimm;
this.Reqint = @reqint;
this.Type = @type;
this.Value = @value;
this.Width = @width;
}
/// <summary>
/// ItemTemplateTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public ItemTemplateTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
this.Agi = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("agi"));
this.Armor = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("armor"));
this.Bra = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("bra"));
this.Defence = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("defence"));
this.Description = (System.String)dataReader.GetString(dataReader.GetOrdinal("description"));
this.Dex = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("dex"));
this.Evade = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("evade"));
this.Graphic = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("graphic"));
this.Height = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("height"));
this.Hp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
this.Id = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
this.Imm = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("imm"));
this.Int = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("int"));
this.Maxhit = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhit"));
this.Maxhp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp"));
this.Maxmp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp"));
this.Minhit = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("minhit"));
this.Mp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("mp"));
this.Name = (System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
this.Perc = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("perc"));
this.Reqacc = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqacc"));
this.Reqagi = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqagi"));
this.Reqarmor = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqarmor"));
this.Reqbra = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqbra"));
this.Reqdex = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqdex"));
this.Reqevade = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqevade"));
this.Reqimm = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqimm"));
this.Reqint = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("reqint"));
this.Type = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("type"));
this.Value = (System.Int32)dataReader.GetInt32(dataReader.GetOrdinal("value"));
this.Width = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("width"));
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
dic["@agi"] = (System.UInt16)source.Agi;
dic["@armor"] = (System.UInt16)source.Armor;
dic["@bra"] = (System.UInt16)source.Bra;
dic["@defence"] = (System.UInt16)source.Defence;
dic["@description"] = (System.String)source.Description;
dic["@dex"] = (System.UInt16)source.Dex;
dic["@evade"] = (System.UInt16)source.Evade;
dic["@graphic"] = (System.UInt16)source.Graphic;
dic["@height"] = (System.Byte)source.Height;
dic["@hp"] = (System.UInt16)source.Hp;
dic["@id"] = (System.UInt16)source.Id;
dic["@imm"] = (System.UInt16)source.Imm;
dic["@int"] = (System.UInt16)source.Int;
dic["@maxhit"] = (System.UInt16)source.Maxhit;
dic["@maxhp"] = (System.UInt16)source.Maxhp;
dic["@maxmp"] = (System.UInt16)source.Maxmp;
dic["@minhit"] = (System.UInt16)source.Minhit;
dic["@mp"] = (System.UInt16)source.Mp;
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.UInt16)source.Perc;
dic["@reqacc"] = (System.Byte)source.Reqacc;
dic["@reqagi"] = (System.Byte)source.Reqagi;
dic["@reqarmor"] = (System.Byte)source.Reqarmor;
dic["@reqbra"] = (System.Byte)source.Reqbra;
dic["@reqdex"] = (System.Byte)source.Reqdex;
dic["@reqevade"] = (System.Byte)source.Reqevade;
dic["@reqimm"] = (System.Byte)source.Reqimm;
dic["@reqint"] = (System.Byte)source.Reqint;
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
paramValues["@agi"] = (System.UInt16)source.Agi;
paramValues["@armor"] = (System.UInt16)source.Armor;
paramValues["@bra"] = (System.UInt16)source.Bra;
paramValues["@defence"] = (System.UInt16)source.Defence;
paramValues["@description"] = (System.String)source.Description;
paramValues["@dex"] = (System.UInt16)source.Dex;
paramValues["@evade"] = (System.UInt16)source.Evade;
paramValues["@graphic"] = (System.UInt16)source.Graphic;
paramValues["@height"] = (System.Byte)source.Height;
paramValues["@hp"] = (System.UInt16)source.Hp;
paramValues["@id"] = (System.UInt16)source.Id;
paramValues["@imm"] = (System.UInt16)source.Imm;
paramValues["@int"] = (System.UInt16)source.Int;
paramValues["@maxhit"] = (System.UInt16)source.Maxhit;
paramValues["@maxhp"] = (System.UInt16)source.Maxhp;
paramValues["@maxmp"] = (System.UInt16)source.Maxmp;
paramValues["@minhit"] = (System.UInt16)source.Minhit;
paramValues["@mp"] = (System.UInt16)source.Mp;
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.UInt16)source.Perc;
paramValues["@reqacc"] = (System.Byte)source.Reqacc;
paramValues["@reqagi"] = (System.Byte)source.Reqagi;
paramValues["@reqarmor"] = (System.Byte)source.Reqarmor;
paramValues["@reqbra"] = (System.Byte)source.Reqbra;
paramValues["@reqdex"] = (System.Byte)source.Reqdex;
paramValues["@reqevade"] = (System.Byte)source.Reqevade;
paramValues["@reqimm"] = (System.Byte)source.Reqimm;
paramValues["@reqint"] = (System.Byte)source.Reqint;
paramValues["@type"] = (System.Byte)source.Type;
paramValues["@value"] = (System.Int32)source.Value;
paramValues["@width"] = (System.Byte)source.Width;
}

}

}
