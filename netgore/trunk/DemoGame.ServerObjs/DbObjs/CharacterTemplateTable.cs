using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_template`.
/// </summary>
public interface ICharacterTemplateTable
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
/// Gets the value for the database column `ai`.
/// </summary>
System.String Ai
{
get;
}
/// <summary>
/// Gets the value for the database column `alliance_id`.
/// </summary>
System.Byte AllianceId
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
/// Gets the value for the database column `give_cash`.
/// </summary>
System.UInt16 GiveCash
{
get;
}
/// <summary>
/// Gets the value for the database column `give_exp`.
/// </summary>
System.UInt16 GiveExp
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
/// Gets the value for the database column `name`.
/// </summary>
System.String Name
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
/// Gets the value for the database column `respawn`.
/// </summary>
System.UInt16 Respawn
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
/// Gets the value for the database column `ws`.
/// </summary>
System.Byte Ws
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `character_template`.
/// </summary>
public class CharacterTemplateTable : ICharacterTemplateTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"acc", "agi", "ai", "alliance_id", "armor", "body", "bra", "defence", "dex", "evade", "exp", "give_cash", "give_exp", "id", "imm", "int", "level", "maxhit", "maxhp", "maxmp", "minhit", "name", "perc", "recov", "regen", "respawn", "statpoints", "str", "tact", "ws" };
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
public const System.String TableName = "character_template";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 30;
/// <summary>
/// The field that maps onto the database column `acc`.
/// </summary>
System.Byte _acc;
/// <summary>
/// The field that maps onto the database column `agi`.
/// </summary>
System.Byte _agi;
/// <summary>
/// The field that maps onto the database column `ai`.
/// </summary>
System.String _ai;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceId;
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
/// The field that maps onto the database column `give_cash`.
/// </summary>
System.UInt16 _giveCash;
/// <summary>
/// The field that maps onto the database column `give_exp`.
/// </summary>
System.UInt16 _giveExp;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _id;
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
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
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
/// The field that maps onto the database column `respawn`.
/// </summary>
System.UInt16 _respawn;
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
/// The field that maps onto the database column `ws`.
/// </summary>
System.Byte _ws;
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
/// Gets or sets the value for the field that maps onto the database column `ai`.
/// The underlying database type is `varchar(255)`.
/// </summary>
public System.String Ai
{
get
{
return _ai;
}
set
{
this._ai = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte AllianceId
{
get
{
return _allianceId;
}
set
{
this._allianceId = value;
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
/// The underlying database type is `int(10) unsigned`.
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
/// Gets or sets the value for the field that maps onto the database column `give_cash`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 GiveCash
{
get
{
return _giveCash;
}
set
{
this._giveCash = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `give_exp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 GiveExp
{
get
{
return _giveExp;
}
set
{
this._giveExp = value;
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
/// Gets or sets the value for the field that maps onto the database column `maxhit`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `2`.
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
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(50)` with the default value of `New NPC`.
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
/// Gets or sets the value for the field that maps onto the database column `respawn`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `5`.
/// </summary>
public System.UInt16 Respawn
{
get
{
return _respawn;
}
set
{
this._respawn = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(10) unsigned`.
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
/// Gets or sets the value for the field that maps onto the database column `ws`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
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
/// CharacterTemplateTable constructor.
/// </summary>
public CharacterTemplateTable()
{
}
/// <summary>
/// CharacterTemplateTable constructor.
/// </summary>
/// <param name="acc">The initial value for the corresponding property.</param>
/// <param name="agi">The initial value for the corresponding property.</param>
/// <param name="ai">The initial value for the corresponding property.</param>
/// <param name="allianceId">The initial value for the corresponding property.</param>
/// <param name="armor">The initial value for the corresponding property.</param>
/// <param name="body">The initial value for the corresponding property.</param>
/// <param name="bra">The initial value for the corresponding property.</param>
/// <param name="defence">The initial value for the corresponding property.</param>
/// <param name="dex">The initial value for the corresponding property.</param>
/// <param name="evade">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="giveCash">The initial value for the corresponding property.</param>
/// <param name="giveExp">The initial value for the corresponding property.</param>
/// <param name="id">The initial value for the corresponding property.</param>
/// <param name="imm">The initial value for the corresponding property.</param>
/// <param name="int">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="maxhit">The initial value for the corresponding property.</param>
/// <param name="maxhp">The initial value for the corresponding property.</param>
/// <param name="maxmp">The initial value for the corresponding property.</param>
/// <param name="minhit">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="perc">The initial value for the corresponding property.</param>
/// <param name="recov">The initial value for the corresponding property.</param>
/// <param name="regen">The initial value for the corresponding property.</param>
/// <param name="respawn">The initial value for the corresponding property.</param>
/// <param name="statpoints">The initial value for the corresponding property.</param>
/// <param name="str">The initial value for the corresponding property.</param>
/// <param name="tact">The initial value for the corresponding property.</param>
/// <param name="ws">The initial value for the corresponding property.</param>
public CharacterTemplateTable(System.Byte @acc, System.Byte @agi, System.String @ai, System.Byte @allianceId, System.Byte @armor, System.UInt16 @body, System.Byte @bra, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, System.UInt16 @giveCash, System.UInt16 @giveExp, System.UInt16 @id, System.Byte @imm, System.Byte @int, System.Byte @level, System.Byte @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.Byte @minhit, System.String @name, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.UInt16 @respawn, System.UInt32 @statpoints, System.Byte @str, System.Byte @tact, System.Byte @ws)
{
Acc = (System.Byte)@acc;
Agi = (System.Byte)@agi;
Ai = (System.String)@ai;
AllianceId = (System.Byte)@allianceId;
Armor = (System.Byte)@armor;
Body = (System.UInt16)@body;
Bra = (System.Byte)@bra;
Defence = (System.Byte)@defence;
Dex = (System.Byte)@dex;
Evade = (System.Byte)@evade;
Exp = (System.UInt32)@exp;
GiveCash = (System.UInt16)@giveCash;
GiveExp = (System.UInt16)@giveExp;
Id = (System.UInt16)@id;
Imm = (System.Byte)@imm;
Int = (System.Byte)@int;
Level = (System.Byte)@level;
Maxhit = (System.Byte)@maxhit;
Maxhp = (System.UInt16)@maxhp;
Maxmp = (System.UInt16)@maxmp;
Minhit = (System.Byte)@minhit;
Name = (System.String)@name;
Perc = (System.Byte)@perc;
Recov = (System.Byte)@recov;
Regen = (System.Byte)@regen;
Respawn = (System.UInt16)@respawn;
Statpoints = (System.UInt32)@statpoints;
Str = (System.Byte)@str;
Tact = (System.Byte)@tact;
Ws = (System.Byte)@ws;
}
/// <summary>
/// CharacterTemplateTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public CharacterTemplateTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public CharacterTemplateTable(ICharacterTemplateTable source)
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
Acc = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("acc"));
Agi = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("agi"));
Ai = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("ai"));
AllianceId = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("alliance_id"));
Armor = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("armor"));
Body = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("body"));
Bra = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("bra"));
Defence = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("defence"));
Dex = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("dex"));
Evade = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("evade"));
Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
GiveCash = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_cash"));
GiveExp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_exp"));
Id = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
Imm = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("imm"));
Int = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("int"));
Level = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("level"));
Maxhit = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("maxhit"));
Maxhp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp"));
Maxmp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp"));
Minhit = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("minhit"));
Name = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
Perc = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("perc"));
Recov = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("recov"));
Regen = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("regen"));
Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("respawn"));
Statpoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
Str = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("str"));
Tact = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("tact"));
Ws = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("ws"));
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
public static void CopyValues(ICharacterTemplateTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@acc"] = (System.Byte)source.Acc;
dic["@agi"] = (System.Byte)source.Agi;
dic["@ai"] = (System.String)source.Ai;
dic["@alliance_id"] = (System.Byte)source.AllianceId;
dic["@armor"] = (System.Byte)source.Armor;
dic["@body"] = (System.UInt16)source.Body;
dic["@bra"] = (System.Byte)source.Bra;
dic["@defence"] = (System.Byte)source.Defence;
dic["@dex"] = (System.Byte)source.Dex;
dic["@evade"] = (System.Byte)source.Evade;
dic["@exp"] = (System.UInt32)source.Exp;
dic["@give_cash"] = (System.UInt16)source.GiveCash;
dic["@give_exp"] = (System.UInt16)source.GiveExp;
dic["@id"] = (System.UInt16)source.Id;
dic["@imm"] = (System.Byte)source.Imm;
dic["@int"] = (System.Byte)source.Int;
dic["@level"] = (System.Byte)source.Level;
dic["@maxhit"] = (System.Byte)source.Maxhit;
dic["@maxhp"] = (System.UInt16)source.Maxhp;
dic["@maxmp"] = (System.UInt16)source.Maxmp;
dic["@minhit"] = (System.Byte)source.Minhit;
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.Byte)source.Perc;
dic["@recov"] = (System.Byte)source.Recov;
dic["@regen"] = (System.Byte)source.Regen;
dic["@respawn"] = (System.UInt16)source.Respawn;
dic["@statpoints"] = (System.UInt32)source.Statpoints;
dic["@str"] = (System.Byte)source.Str;
dic["@tact"] = (System.Byte)source.Tact;
dic["@ws"] = (System.Byte)source.Ws;
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
public static void CopyValues(ICharacterTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@acc"] = (System.Byte)source.Acc;
paramValues["@agi"] = (System.Byte)source.Agi;
paramValues["@ai"] = (System.String)source.Ai;
paramValues["@alliance_id"] = (System.Byte)source.AllianceId;
paramValues["@armor"] = (System.Byte)source.Armor;
paramValues["@body"] = (System.UInt16)source.Body;
paramValues["@bra"] = (System.Byte)source.Bra;
paramValues["@defence"] = (System.Byte)source.Defence;
paramValues["@dex"] = (System.Byte)source.Dex;
paramValues["@evade"] = (System.Byte)source.Evade;
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@give_cash"] = (System.UInt16)source.GiveCash;
paramValues["@give_exp"] = (System.UInt16)source.GiveExp;
paramValues["@id"] = (System.UInt16)source.Id;
paramValues["@imm"] = (System.Byte)source.Imm;
paramValues["@int"] = (System.Byte)source.Int;
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@maxhit"] = (System.Byte)source.Maxhit;
paramValues["@maxhp"] = (System.UInt16)source.Maxhp;
paramValues["@maxmp"] = (System.UInt16)source.Maxmp;
paramValues["@minhit"] = (System.Byte)source.Minhit;
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.Byte)source.Perc;
paramValues["@recov"] = (System.Byte)source.Recov;
paramValues["@regen"] = (System.Byte)source.Regen;
paramValues["@respawn"] = (System.UInt16)source.Respawn;
paramValues["@statpoints"] = (System.UInt32)source.Statpoints;
paramValues["@str"] = (System.Byte)source.Str;
paramValues["@tact"] = (System.Byte)source.Tact;
paramValues["@ws"] = (System.Byte)source.Ws;
}

public void CopyValuesFrom(ICharacterTemplateTable source)
{
Acc = (System.Byte)source.Acc;
Agi = (System.Byte)source.Agi;
Ai = (System.String)source.Ai;
AllianceId = (System.Byte)source.AllianceId;
Armor = (System.Byte)source.Armor;
Body = (System.UInt16)source.Body;
Bra = (System.Byte)source.Bra;
Defence = (System.Byte)source.Defence;
Dex = (System.Byte)source.Dex;
Evade = (System.Byte)source.Evade;
Exp = (System.UInt32)source.Exp;
GiveCash = (System.UInt16)source.GiveCash;
GiveExp = (System.UInt16)source.GiveExp;
Id = (System.UInt16)source.Id;
Imm = (System.Byte)source.Imm;
Int = (System.Byte)source.Int;
Level = (System.Byte)source.Level;
Maxhit = (System.Byte)source.Maxhit;
Maxhp = (System.UInt16)source.Maxhp;
Maxmp = (System.UInt16)source.Maxmp;
Minhit = (System.Byte)source.Minhit;
Name = (System.String)source.Name;
Perc = (System.Byte)source.Perc;
Recov = (System.Byte)source.Recov;
Regen = (System.Byte)source.Regen;
Respawn = (System.UInt16)source.Respawn;
Statpoints = (System.UInt32)source.Statpoints;
Str = (System.Byte)source.Str;
Tact = (System.Byte)source.Tact;
Ws = (System.Byte)source.Ws;
}

}

}
