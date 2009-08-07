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
System.Int32 GetStat(DemoGame.StatType key);

System.Void SetStat(DemoGame.StatType key, System.Int32 value);

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
DemoGame.Server.AllianceID AllianceId
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
/// Gets the value for the database column `level`.
/// </summary>
System.Byte Level
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
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
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
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `ai`.
/// </summary>
System.String _ai;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceId;
/// <summary>
/// The field that maps onto the database column `body`.
/// </summary>
System.UInt16 _body;
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
/// The field that maps onto the database column `level`.
/// </summary>
System.Byte _level;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `respawn`.
/// </summary>
System.UInt16 _respawn;
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.UInt32 _statpoints;
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Byte)_stat[(DemoGame.StatType)key];
}
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.Byte)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ai`.
/// The underlying database type is `varchar(255)`.
/// </summary>
public System.String Ai
{
get
{
return (System.String)_ai;
}
set
{
this._ai = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AllianceId
{
get
{
return (DemoGame.Server.AllianceID)_allianceId;
}
set
{
this._allianceId = (System.Byte)value;
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
return (System.UInt16)_body;
}
set
{
this._body = (System.UInt16)value;
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
return (System.UInt32)_exp;
}
set
{
this._exp = (System.UInt32)value;
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
return (System.UInt16)_giveCash;
}
set
{
this._giveCash = (System.UInt16)value;
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
return (System.UInt16)_giveExp;
}
set
{
this._giveExp = (System.UInt16)value;
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
return (System.UInt16)_id;
}
set
{
this._id = (System.UInt16)value;
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
return (System.Byte)_level;
}
set
{
this._level = (System.Byte)value;
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
return (System.String)_name;
}
set
{
this._name = (System.String)value;
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
return (System.UInt16)_respawn;
}
set
{
this._respawn = (System.UInt16)value;
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
return (System.UInt32)_statpoints;
}
set
{
this._statpoints = (System.UInt32)value;
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
public CharacterTemplateTable(System.Byte @acc, System.Byte @agi, System.String @ai, DemoGame.Server.AllianceID @allianceId, System.Byte @armor, System.UInt16 @body, System.Byte @bra, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, System.UInt16 @giveCash, System.UInt16 @giveExp, System.UInt16 @id, System.Byte @imm, System.Byte @int, System.Byte @level, System.Byte @maxhit, System.UInt16 @maxhp, System.UInt16 @maxmp, System.Byte @minhit, System.String @name, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.UInt16 @respawn, System.UInt32 @statpoints, System.Byte @str, System.Byte @tact, System.Byte @ws)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)@acc);
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
Ai = (System.String)@ai;
AllianceId = (DemoGame.Server.AllianceID)@allianceId;
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
Body = (System.UInt16)@body;
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
Exp = (System.UInt32)@exp;
GiveCash = (System.UInt16)@giveCash;
GiveExp = (System.UInt16)@giveExp;
Id = (System.UInt16)@id;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
Level = (System.Byte)@level;
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxhit);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxhp);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxmp);
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minhit);
Name = (System.String)@name;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)@perc);
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)@recov);
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)@regen);
Respawn = (System.UInt16)@respawn;
Statpoints = (System.UInt32)@statpoints;
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@str);
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)@tact);
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)@ws);
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
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("acc")));
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("agi")));
Ai = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("ai"));
AllianceId = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(dataReader.GetOrdinal("alliance_id"));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("armor")));
Body = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("body"));
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("bra")));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("defence")));
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("dex")));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("evade")));
Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
GiveCash = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_cash"));
GiveExp = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_exp"));
Id = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("imm")));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("int")));
Level = (System.Byte)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("level"));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("maxhit")));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp")));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp")));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("minhit")));
Name = (System.String)(System.String)dataReader.GetString(dataReader.GetOrdinal("name"));
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("perc")));
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("recov")));
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("regen")));
Respawn = (System.UInt16)(System.UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("respawn"));
Statpoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("str")));
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("tact")));
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(dataReader.GetOrdinal("ws")));
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
dic["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
dic["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@ai"] = (System.String)source.Ai;
dic["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceId;
dic["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@body"] = (System.UInt16)source.Body;
dic["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@exp"] = (System.UInt32)source.Exp;
dic["@give_cash"] = (System.UInt16)source.GiveCash;
dic["@give_exp"] = (System.UInt16)source.GiveExp;
dic["@id"] = (System.UInt16)source.Id;
dic["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@level"] = (System.Byte)source.Level;
dic["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
dic["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
dic["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
dic["@respawn"] = (System.UInt16)source.Respawn;
dic["@statpoints"] = (System.UInt32)source.Statpoints;
dic["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
dic["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
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
paramValues["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
paramValues["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@ai"] = (System.String)source.Ai;
paramValues["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceId;
paramValues["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@body"] = (System.UInt16)source.Body;
paramValues["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@give_cash"] = (System.UInt16)source.GiveCash;
paramValues["@give_exp"] = (System.UInt16)source.GiveExp;
paramValues["@id"] = (System.UInt16)source.Id;
paramValues["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@name"] = (System.String)source.Name;
paramValues["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
paramValues["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
paramValues["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
paramValues["@respawn"] = (System.UInt16)source.Respawn;
paramValues["@statpoints"] = (System.UInt32)source.Statpoints;
paramValues["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
paramValues["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
}

public void CopyValuesFrom(ICharacterTemplateTable source)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc));
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
Ai = (System.String)source.Ai;
AllianceId = (DemoGame.Server.AllianceID)source.AllianceId;
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
Body = (System.UInt16)source.Body;
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
Exp = (System.UInt32)source.Exp;
GiveCash = (System.UInt16)source.GiveCash;
GiveExp = (System.UInt16)source.GiveExp;
Id = (System.UInt16)source.Id;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
Level = (System.Byte)source.Level;
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
Name = (System.String)source.Name;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc));
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov));
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen));
Respawn = (System.UInt16)source.Respawn;
Statpoints = (System.UInt32)source.Statpoints;
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact));
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS));
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
}

}
