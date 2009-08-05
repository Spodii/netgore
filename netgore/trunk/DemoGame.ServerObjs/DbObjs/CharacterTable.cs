using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character`.
/// </summary>
public interface ICharacterTable
{
/// <summary>
/// Gets the value for the database column `acc`.
/// </summary>
System.Byte Acc
{
get;
}
/// <summary>
/// Gets the value for the database column `agi`.
/// </summary>
System.Byte Agi
{
get;
}
/// <summary>
/// Gets the value for the database column `armor`.
/// </summary>
System.Byte Armor
{
get;
}
/// <summary>
/// Gets the value for the database column `body`.
/// </summary>
System.UInt16 Body
{
get;
}
/// <summary>
/// Gets the value for the database column `bra`.
/// </summary>
System.Byte Bra
{
get;
}
/// <summary>
/// Gets the value for the database column `cash`.
/// </summary>
System.UInt32 Cash
{
get;
}
/// <summary>
/// Gets the value for the database column `defence`.
/// </summary>
System.Byte Defence
{
get;
}
/// <summary>
/// Gets the value for the database column `dex`.
/// </summary>
System.Byte Dex
{
get;
}
/// <summary>
/// Gets the value for the database column `evade`.
/// </summary>
System.Byte Evade
{
get;
}
/// <summary>
/// Gets the value for the database column `exp`.
/// </summary>
System.UInt32 Exp
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
System.UInt32 Id
{
get;
}
/// <summary>
/// Gets the value for the database column `imm`.
/// </summary>
System.Byte Imm
{
get;
}
/// <summary>
/// Gets the value for the database column `int`.
/// </summary>
System.Byte Int
{
get;
}
/// <summary>
/// Gets the value for the database column `level`.
/// </summary>
System.Byte Level
{
get;
}
/// <summary>
/// Gets the value for the database column `map_id`.
/// </summary>
System.UInt16 MapId
{
get;
}
/// <summary>
/// Gets the value for the database column `maxhit`.
/// </summary>
System.Byte Maxhit
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
System.Byte Minhit
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
/// Gets the value for the database column `password`.
/// </summary>
System.String Password
{
get;
}
/// <summary>
/// Gets the value for the database column `perc`.
/// </summary>
System.Byte Perc
{
get;
}
/// <summary>
/// Gets the value for the database column `recov`.
/// </summary>
System.Byte Recov
{
get;
}
/// <summary>
/// Gets the value for the database column `regen`.
/// </summary>
System.Byte Regen
{
get;
}
/// <summary>
/// Gets the value for the database column `respawn_map`.
/// </summary>
System.UInt16 RespawnMap
{
get;
}
/// <summary>
/// Gets the value for the database column `respawn_x`.
/// </summary>
System.Single RespawnX
{
get;
}
/// <summary>
/// Gets the value for the database column `respawn_y`.
/// </summary>
System.Single RespawnY
{
get;
}
/// <summary>
/// Gets the value for the database column `statpoints`.
/// </summary>
System.UInt32 Statpoints
{
get;
}
/// <summary>
/// Gets the value for the database column `str`.
/// </summary>
System.Byte Str
{
get;
}
/// <summary>
/// Gets the value for the database column `tact`.
/// </summary>
System.Byte Tact
{
get;
}
/// <summary>
/// Gets the value for the database column `template_id`.
/// </summary>
System.UInt16 TemplateId
{
get;
}
/// <summary>
/// Gets the value for the database column `ws`.
/// </summary>
System.Byte Ws
{
get;
}
/// <summary>
/// Gets the value for the database column `x`.
/// </summary>
System.Single X
{
get;
}
/// <summary>
/// Gets the value for the database column `y`.
/// </summary>
System.Single Y
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `character`.
/// </summary>
public class CharacterTable : ICharacterTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"acc", "agi", "armor", "body", "bra", "cash", "defence", "dex", "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "password", "perc", "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "template_id", "ws", "x", "y" };
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
public const System.String TableName = "character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 36;
/// <summary>
/// The field that maps onto the database column `acc`.
/// </summary>
System.Byte _acc;
/// <summary>
/// The field that maps onto the database column `agi`.
/// </summary>
System.Byte _agi;
/// <summary>
/// The field that maps onto the database column `armor`.
/// </summary>
System.Byte _armor;
/// <summary>
/// The field that maps onto the database column `body`.
/// </summary>
System.UInt16 _body;
/// <summary>
/// The field that maps onto the database column `bra`.
/// </summary>
System.Byte _bra;
/// <summary>
/// The field that maps onto the database column `cash`.
/// </summary>
System.UInt32 _cash;
/// <summary>
/// The field that maps onto the database column `defence`.
/// </summary>
System.Byte _defence;
/// <summary>
/// The field that maps onto the database column `dex`.
/// </summary>
System.Byte _dex;
/// <summary>
/// The field that maps onto the database column `evade`.
/// </summary>
System.Byte _evade;
/// <summary>
/// The field that maps onto the database column `exp`.
/// </summary>
System.UInt32 _exp;
/// <summary>
/// The field that maps onto the database column `hp`.
/// </summary>
System.UInt16 _hp;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _id;
/// <summary>
/// The field that maps onto the database column `imm`.
/// </summary>
System.Byte _imm;
/// <summary>
/// The field that maps onto the database column `int`.
/// </summary>
System.Byte _int;
/// <summary>
/// The field that maps onto the database column `level`.
/// </summary>
System.Byte _level;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapId;
/// <summary>
/// The field that maps onto the database column `maxhit`.
/// </summary>
System.Byte _maxhit;
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
System.Byte _minhit;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.UInt16 _mp;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `password`.
/// </summary>
System.String _password;
/// <summary>
/// The field that maps onto the database column `perc`.
/// </summary>
System.Byte _perc;
/// <summary>
/// The field that maps onto the database column `recov`.
/// </summary>
System.Byte _recov;
/// <summary>
/// The field that maps onto the database column `regen`.
/// </summary>
System.Byte _regen;
/// <summary>
/// The field that maps onto the database column `respawn_map`.
/// </summary>
System.UInt16 _respawnMap;
/// <summary>
/// The field that maps onto the database column `respawn_x`.
/// </summary>
System.Single _respawnX;
/// <summary>
/// The field that maps onto the database column `respawn_y`.
/// </summary>
System.Single _respawnY;
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.UInt32 _statpoints;
/// <summary>
/// The field that maps onto the database column `str`.
/// </summary>
System.Byte _str;
/// <summary>
/// The field that maps onto the database column `tact`.
/// </summary>
System.Byte _tact;
/// <summary>
/// The field that maps onto the database column `template_id`.
/// </summary>
System.UInt16 _templateId;
/// <summary>
/// The field that maps onto the database column `ws`.
/// </summary>
System.Byte _ws;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Single _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Single _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `acc`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Acc
{
get
{
return _acc;
}
set
{
this._acc = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `agi`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Agi
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Armor
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
/// Gets or sets the value for the field that maps onto the database column `body`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
public System.UInt16 Body
{
get
{
return _body;
}
set
{
this._body = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `bra`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Bra
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
/// Gets or sets the value for the field that maps onto the database column `cash`.
/// The underlying database type is `int(10) unsigned` with the default value of `0`.
/// </summary>
public System.UInt32 Cash
{
get
{
return _cash;
}
set
{
this._cash = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `defence`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
/// </summary>
public System.Byte Defence
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
/// Gets or sets the value for the field that maps onto the database column `dex`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Dex
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Evade
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
/// Gets or sets the value for the field that maps onto the database column `exp`.
/// The underlying database type is `int(10) unsigned` with the default value of `0`.
/// </summary>
public System.UInt32 Exp
{
get
{
return _exp;
}
set
{
this._exp = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `hp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
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
/// The underlying database type is `int(10) unsigned`.
/// </summary>
public System.UInt32 Id
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Imm
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Int
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
/// Gets or sets the value for the field that maps onto the database column `level`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Level
{
get
{
return _level;
}
set
{
this._level = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
public System.UInt16 MapId
{
get
{
return _mapId;
}
set
{
this._mapId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxhit`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Maxhit
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
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
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Minhit
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
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
/// The underlying database type is `varchar(50)`.
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
/// Gets or sets the value for the field that maps onto the database column `password`.
/// The underlying database type is `varchar(50)`.
/// </summary>
public System.String Password
{
get
{
return _password;
}
set
{
this._password = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `perc`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Perc
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
/// Gets or sets the value for the field that maps onto the database column `recov`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Recov
{
get
{
return _recov;
}
set
{
this._recov = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `regen`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Regen
{
get
{
return _regen;
}
set
{
this._regen = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn_map`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 RespawnMap
{
get
{
return _respawnMap;
}
set
{
this._respawnMap = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn_x`.
/// The underlying database type is `float`.
/// </summary>
public System.Single RespawnX
{
get
{
return _respawnX;
}
set
{
this._respawnX = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn_y`.
/// The underlying database type is `float`.
/// </summary>
public System.Single RespawnY
{
get
{
return _respawnY;
}
set
{
this._respawnY = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(10) unsigned` with the default value of `0`.
/// </summary>
public System.UInt32 Statpoints
{
get
{
return _statpoints;
}
set
{
this._statpoints = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `str`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Str
{
get
{
return _str;
}
set
{
this._str = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `tact`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Tact
{
get
{
return _tact;
}
set
{
this._tact = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.UInt16 TemplateId
{
get
{
return _templateId;
}
set
{
this._templateId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ws`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte Ws
{
get
{
return _ws;
}
set
{
this._ws = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `float` with the default value of `100`.
/// </summary>
public System.Single X
{
get
{
return _x;
}
set
{
this._x = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `y`.
/// The underlying database type is `float` with the default value of `100`.
/// </summary>
public System.Single Y
{
get
{
return _y;
}
set
{
this._y = value;
}
}

/// <summary>
/// CharacterTable constructor.
/// </summary>
public CharacterTable()
{
}
/// <summary>
/// CharacterTable constructor.
/// </summary>
/// <param name="acc">The initial value for the corresponding property.</param>
/// <param name="agi">The initial value for the corresponding property.</param>
/// <param name="armor">The initial value for the corresponding property.</param>
/// <param name="body">The initial value for the corresponding property.</param>
/// <param name="bra">The initial value for the corresponding property.</param>
/// <param name="cash">The initial value for the corresponding property.</param>
/// <param name="defence">The initial value for the corresponding property.</param>
/// <param name="dex">The initial value for the corresponding property.</param>
/// <param name="evade">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="hp">The initial value for the corresponding property.</param>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="imm">The initial value for the corresponding property.</param>
/// <param name="int">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="mapId">The initial value for the corresponding property.</param>
/// <param name="maxhit">The initial value for the corresponding property.</param>
/// <param name="maxhp">The initial value for the corresponding property.</param>
/// <param name="maxmp">The initial value for the corresponding property.</param>
/// <param name="minhit">The initial value for the corresponding property.</param>
/// <param name="mp">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="password">The initial value for the corresponding property.</param>
/// <param name="perc">The initial value for the corresponding property.</param>
/// <param name="recov">The initial value for the corresponding property.</param>
/// <param name="regen">The initial value for the corresponding property.</param>
/// <param name="respawnMap">The initial value for the corresponding property.</param>
/// <param name="respawnX">The initial value for the corresponding property.</param>
/// <param name="respawnY">The initial value for the corresponding property.</param>
/// <param name="statpoints">The initial value for the corresponding property.</param>
/// <param name="str">The initial value for the corresponding property.</param>
/// <param name="tact">The initial value for the corresponding property.</param>
/// <param name="templateId">The initial value for the corresponding property.</param>
/// <param name="ws">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public CharacterTable(System.Byte @acc, System.Byte @agi, System.Byte @armor, System.UInt16 @body, System.Byte @bra, System.UInt32 @cash, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, System.UInt16 @hp, System.UInt32 @id, System.Byte @imm, System.Byte @int, System.Byte @level, System.UInt16 @mapId, System.Byte @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.Byte @minhit, System.UInt16 @mp, System.String @name, System.String @password, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.UInt16 @respawnMap, System.Single @respawnX, System.Single @respawnY, System.UInt32 @statpoints, System.Byte @str, System.Byte @tact, System.UInt16 @templateId, System.Byte @ws, System.Single @x, System.Single @y)
{
this.Acc = @acc;
this.Agi = @agi;
this.Armor = @armor;
this.Body = @body;
this.Bra = @bra;
this.Cash = @cash;
this.Defence = @defence;
this.Dex = @dex;
this.Evade = @evade;
this.Exp = @exp;
this.Hp = @hp;
this.Id = @id;
this.Imm = @imm;
this.Int = @int;
this.Level = @level;
this.MapId = @mapId;
this.Maxhit = @maxhit;
this.Maxhp = @maxhp;
this.Maxmp = @maxmp;
this.Minhit = @minhit;
this.Mp = @mp;
this.Name = @name;
this.Password = @password;
this.Perc = @perc;
this.Recov = @recov;
this.Regen = @regen;
this.RespawnMap = @respawnMap;
this.RespawnX = @respawnX;
this.RespawnY = @respawnY;
this.Statpoints = @statpoints;
this.Str = @str;
this.Tact = @tact;
this.TemplateId = @templateId;
this.Ws = @ws;
this.X = @x;
this.Y = @y;
}
/// <summary>
/// CharacterTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public CharacterTable(System.Data.IDataReader dataReader)
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
this.Acc = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("acc"));
this.Agi = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("agi"));
this.Armor = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("armor"));
this.Body = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("body"));
this.Bra = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("bra"));
this.Cash = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("cash"));
this.Defence = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("defence"));
this.Dex = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("dex"));
this.Evade = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("evade"));
this.Exp = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
this.Hp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
this.Id = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("id"));
this.Imm = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("imm"));
this.Int = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("int"));
this.Level = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("level"));
this.MapId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("map_id"));
this.Maxhit = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("maxhit"));
this.Maxhp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp"));
this.Maxmp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp"));
this.Minhit = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("minhit"));
this.Mp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("mp"));
this.Name = (System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
this.Password = (System.String)dataReader.GetString(dataReader.GetOrdinal("password"));
this.Perc = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("perc"));
this.Recov = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("recov"));
this.Regen = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("regen"));
this.RespawnMap = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("respawn_map"));
this.RespawnX = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("respawn_x"));
this.RespawnY = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("respawn_y"));
this.Statpoints = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
this.Str = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("str"));
this.Tact = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("tact"));
this.TemplateId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("template_id"));
this.Ws = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("ws"));
this.X = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("x"));
this.Y = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("y"));
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
public static void CopyValues(ICharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@acc"] = (System.Byte)source.Acc;
dic["@agi"] = (System.Byte)source.Agi;
dic["@armor"] = (System.Byte)source.Armor;
dic["@body"] = (System.UInt16)source.Body;
dic["@bra"] = (System.Byte)source.Bra;
dic["@cash"] = (System.UInt32)source.Cash;
dic["@defence"] = (System.Byte)source.Defence;
dic["@dex"] = (System.Byte)source.Dex;
dic["@evade"] = (System.Byte)source.Evade;
dic["@exp"] = (System.UInt32)source.Exp;
dic["@hp"] = (System.UInt16)source.Hp;
dic["@id"] = (System.UInt32)source.Id;
dic["@imm"] = (System.Byte)source.Imm;
dic["@int"] = (System.Byte)source.Int;
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (System.UInt16)source.MapId;
dic["@maxhit"] = (System.Byte)source.Maxhit;
dic["@maxhp"] = (System.UInt16)source.Maxhp;
dic["@maxmp"] = (System.UInt16)source.Maxmp;
dic["@minhit"] = (System.Byte)source.Minhit;
dic["@mp"] = (System.UInt16)source.Mp;
dic["@name"] = (System.String)source.Name;
dic["@password"] = (System.String)source.Password;
dic["@perc"] = (System.Byte)source.Perc;
dic["@recov"] = (System.Byte)source.Recov;
dic["@regen"] = (System.Byte)source.Regen;
dic["@respawn_map"] = (System.UInt16)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@statpoints"] = (System.UInt32)source.Statpoints;
dic["@str"] = (System.Byte)source.Str;
dic["@tact"] = (System.Byte)source.Tact;
dic["@template_id"] = (System.UInt16)source.TemplateId;
dic["@ws"] = (System.Byte)source.Ws;
dic["@x"] = (System.Single)source.X;
dic["@y"] = (System.Single)source.Y;
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
public static void CopyValues(ICharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@acc"] = (System.Byte)source.Acc;
paramValues["@agi"] = (System.Byte)source.Agi;
paramValues["@armor"] = (System.Byte)source.Armor;
paramValues["@body"] = (System.UInt16)source.Body;
paramValues["@bra"] = (System.Byte)source.Bra;
paramValues["@cash"] = (System.UInt32)source.Cash;
paramValues["@defence"] = (System.Byte)source.Defence;
paramValues["@dex"] = (System.Byte)source.Dex;
paramValues["@evade"] = (System.Byte)source.Evade;
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@hp"] = (System.UInt16)source.Hp;
paramValues["@id"] = (System.UInt32)source.Id;
paramValues["@imm"] = (System.Byte)source.Imm;
paramValues["@int"] = (System.Byte)source.Int;
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@map_id"] = (System.UInt16)source.MapId;
paramValues["@maxhit"] = (System.Byte)source.Maxhit;
paramValues["@maxhp"] = (System.UInt16)source.Maxhp;
paramValues["@maxmp"] = (System.UInt16)source.Maxmp;
paramValues["@minhit"] = (System.Byte)source.Minhit;
paramValues["@mp"] = (System.UInt16)source.Mp;
paramValues["@name"] = (System.String)source.Name;
paramValues["@password"] = (System.String)source.Password;
paramValues["@perc"] = (System.Byte)source.Perc;
paramValues["@recov"] = (System.Byte)source.Recov;
paramValues["@regen"] = (System.Byte)source.Regen;
paramValues["@respawn_map"] = (System.UInt16)source.RespawnMap;
paramValues["@respawn_x"] = (System.Single)source.RespawnX;
paramValues["@respawn_y"] = (System.Single)source.RespawnY;
paramValues["@statpoints"] = (System.UInt32)source.Statpoints;
paramValues["@str"] = (System.Byte)source.Str;
paramValues["@tact"] = (System.Byte)source.Tact;
paramValues["@template_id"] = (System.UInt16)source.TemplateId;
paramValues["@ws"] = (System.Byte)source.Ws;
paramValues["@x"] = (System.Single)source.X;
paramValues["@y"] = (System.Single)source.Y;
}

}

}
