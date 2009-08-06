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
System.Collections.Generic.Dictionary<DemoGame.StatType, System.Int32> Stats
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
/// Gets the value for the database column `cash`.
/// </summary>
System.UInt32 Cash
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
/// Gets the value for the database column `template_id`.
/// </summary>
System.UInt16 TemplateId
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
 readonly System.Collections.Generic.Dictionary<DemoGame.StatType, System.Int32> _stats;
/// <summary>
/// The field that maps onto the database column `body`.
/// </summary>
System.UInt16 _body;
/// <summary>
/// The field that maps onto the database column `cash`.
/// </summary>
System.UInt32 _cash;
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
/// The field that maps onto the database column `level`.
/// </summary>
System.Byte _level;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapId;
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
/// The field that maps onto the database column `template_id`.
/// </summary>
System.UInt16 _templateId;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Single _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Single _y;
public System.Collections.Generic.Dictionary<DemoGame.StatType, System.Int32> Stats
{
get
{
return _stats;
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
this.Stats[(DemoGame.StatType)DemoGame.StatType.Acc] = @acc;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Agi] = @agi;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Armor] = @armor;
this.Body = @body;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Bra] = @bra;
this.Cash = @cash;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Defence] = @defence;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Dex] = @dex;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Evade] = @evade;
this.Exp = @exp;
this.Hp = @hp;
this.Id = @id;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Imm] = @imm;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Int] = @int;
this.Level = @level;
this.MapId = @mapId;
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit] = @maxhit;
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP] = @maxhp;
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP] = @maxmp;
this.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit] = @minhit;
this.Mp = @mp;
this.Name = @name;
this.Password = @password;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Perc] = @perc;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Recov] = @recov;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Regen] = @regen;
this.RespawnMap = @respawnMap;
this.RespawnX = @respawnX;
this.RespawnY = @respawnY;
this.Statpoints = @statpoints;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Str] = @str;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Tact] = @tact;
this.TemplateId = @templateId;
this.Stats[(DemoGame.StatType)DemoGame.StatType.WS] = @ws;
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
public CharacterTable(ICharacterTable source)
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
this.Stats[(DemoGame.StatType)DemoGame.StatType.Acc] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("acc"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Agi] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("agi"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Armor] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("armor"));
this.Body = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("body"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Bra] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("bra"));
this.Cash = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("cash"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Defence] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("defence"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Dex] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("dex"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Evade] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("evade"));
this.Exp = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
this.Hp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
this.Id = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("id"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Imm] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("imm"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Int] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("int"));
this.Level = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("level"));
this.MapId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("map_id"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("maxhit"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP] = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP] = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("minhit"));
this.Mp = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("mp"));
this.Name = (System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
this.Password = (System.String)dataReader.GetString(dataReader.GetOrdinal("password"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Perc] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("perc"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Recov] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("recov"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Regen] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("regen"));
this.RespawnMap = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("respawn_map"));
this.RespawnX = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("respawn_x"));
this.RespawnY = (System.Single)dataReader.GetFloat(dataReader.GetOrdinal("respawn_y"));
this.Statpoints = (System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Str] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("str"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.Tact] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("tact"));
this.TemplateId = (System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("template_id"));
this.Stats[(DemoGame.StatType)DemoGame.StatType.WS] = (System.Byte)dataReader.GetByte(dataReader.GetOrdinal("ws"));
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
dic["@acc"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Acc];
dic["@agi"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Agi];
dic["@armor"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Armor];
dic["@body"] = (System.UInt16)source.Body;
dic["@bra"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Bra];
dic["@cash"] = (System.UInt32)source.Cash;
dic["@defence"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Defence];
dic["@dex"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Dex];
dic["@evade"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Evade];
dic["@exp"] = (System.UInt32)source.Exp;
dic["@hp"] = (System.UInt16)source.Hp;
dic["@id"] = (System.UInt32)source.Id;
dic["@imm"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Imm];
dic["@int"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Int];
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (System.UInt16)source.MapId;
dic["@maxhit"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit];
dic["@maxhp"] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP];
dic["@maxmp"] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP];
dic["@minhit"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit];
dic["@mp"] = (System.UInt16)source.Mp;
dic["@name"] = (System.String)source.Name;
dic["@password"] = (System.String)source.Password;
dic["@perc"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Perc];
dic["@recov"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Recov];
dic["@regen"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Regen];
dic["@respawn_map"] = (System.UInt16)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@statpoints"] = (System.UInt32)source.Statpoints;
dic["@str"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Str];
dic["@tact"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Tact];
dic["@template_id"] = (System.UInt16)source.TemplateId;
dic["@ws"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.WS];
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
paramValues["@acc"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Acc];
paramValues["@agi"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Agi];
paramValues["@armor"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Armor];
paramValues["@body"] = (System.UInt16)source.Body;
paramValues["@bra"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Bra];
paramValues["@cash"] = (System.UInt32)source.Cash;
paramValues["@defence"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Defence];
paramValues["@dex"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Dex];
paramValues["@evade"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Evade];
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@hp"] = (System.UInt16)source.Hp;
paramValues["@id"] = (System.UInt32)source.Id;
paramValues["@imm"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Imm];
paramValues["@int"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Int];
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@map_id"] = (System.UInt16)source.MapId;
paramValues["@maxhit"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit];
paramValues["@maxhp"] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP];
paramValues["@maxmp"] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP];
paramValues["@minhit"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit];
paramValues["@mp"] = (System.UInt16)source.Mp;
paramValues["@name"] = (System.String)source.Name;
paramValues["@password"] = (System.String)source.Password;
paramValues["@perc"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Perc];
paramValues["@recov"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Recov];
paramValues["@regen"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Regen];
paramValues["@respawn_map"] = (System.UInt16)source.RespawnMap;
paramValues["@respawn_x"] = (System.Single)source.RespawnX;
paramValues["@respawn_y"] = (System.Single)source.RespawnY;
paramValues["@statpoints"] = (System.UInt32)source.Statpoints;
paramValues["@str"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Str];
paramValues["@tact"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Tact];
paramValues["@template_id"] = (System.UInt16)source.TemplateId;
paramValues["@ws"] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.WS];
paramValues["@x"] = (System.Single)source.X;
paramValues["@y"] = (System.Single)source.Y;
}

public void CopyValuesFrom(ICharacterTable source)
{
this.Stats[(DemoGame.StatType)DemoGame.StatType.Acc] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Acc];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Agi] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Agi];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Armor] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Armor];
this.Body = (System.UInt16)source.Body;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Bra] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Bra];
this.Cash = (System.UInt32)source.Cash;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Defence] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Defence];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Dex] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Dex];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Evade] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Evade];
this.Exp = (System.UInt32)source.Exp;
this.Hp = (System.UInt16)source.Hp;
this.Id = (System.UInt32)source.Id;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Imm] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Imm];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Int] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Int];
this.Level = (System.Byte)source.Level;
this.MapId = (System.UInt16)source.MapId;
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHit];
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxHP];
this.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP] = (System.UInt16)source.Stats[(DemoGame.StatType)DemoGame.StatType.MaxMP];
this.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.MinHit];
this.Mp = (System.UInt16)source.Mp;
this.Name = (System.String)source.Name;
this.Password = (System.String)source.Password;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Perc] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Perc];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Recov] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Recov];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Regen] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Regen];
this.RespawnMap = (System.UInt16)source.RespawnMap;
this.RespawnX = (System.Single)source.RespawnX;
this.RespawnY = (System.Single)source.RespawnY;
this.Statpoints = (System.UInt32)source.Statpoints;
this.Stats[(DemoGame.StatType)DemoGame.StatType.Str] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Str];
this.Stats[(DemoGame.StatType)DemoGame.StatType.Tact] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.Tact];
this.TemplateId = (System.UInt16)source.TemplateId;
this.Stats[(DemoGame.StatType)DemoGame.StatType.WS] = (System.Byte)source.Stats[(DemoGame.StatType)DemoGame.StatType.WS];
this.X = (System.Single)source.X;
this.Y = (System.Single)source.Y;
}

}

}
