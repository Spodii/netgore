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

System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get;
}
/// <summary>
/// Gets the value of the database column `body_id`.
/// </summary>
DemoGame.BodyIndex BodyID
{
get;
}
/// <summary>
/// Gets the value of the database column `cash`.
/// </summary>
System.UInt32 Cash
{
get;
}
/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
System.Nullable<DemoGame.Server.CharacterTemplateID> CharacterTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `exp`.
/// </summary>
System.UInt32 Exp
{
get;
}
/// <summary>
/// Gets the value of the database column `hp`.
/// </summary>
System.Int16 HP
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.CharacterID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `level`.
/// </summary>
System.Byte Level
{
get;
}
/// <summary>
/// Gets the value of the database column `map_id`.
/// </summary>
NetGore.MapIndex MapID
{
get;
}
/// <summary>
/// Gets the value of the database column `mp`.
/// </summary>
System.Int16 MP
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
/// Gets the value of the database column `password`.
/// </summary>
System.String Password
{
get;
}
/// <summary>
/// Gets the value of the database column `respawn_map`.
/// </summary>
System.Nullable<NetGore.MapIndex> RespawnMap
{
get;
}
/// <summary>
/// Gets the value of the database column `respawn_x`.
/// </summary>
System.Single RespawnX
{
get;
}
/// <summary>
/// Gets the value of the database column `respawn_y`.
/// </summary>
System.Single RespawnY
{
get;
}
/// <summary>
/// Gets the value of the database column `statpoints`.
/// </summary>
System.UInt32 StatPoints
{
get;
}
/// <summary>
/// Gets the value of the database column `x`.
/// </summary>
System.Single X
{
get;
}
/// <summary>
/// Gets the value of the database column `y`.
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
 static  readonly System.String[] _dbColumns = new string[] {"acc", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "defence", "dex", "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "password", "perc", "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"acc", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "defence", "dex", "evade", "exp", "hp", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "password", "perc", "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y" };
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
 static  readonly System.String[] _statColumns = new string[] {"acc", "agi", "armor", "bra", "defence", "dex", "evade", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "perc", "recov", "regen", "str", "tact", "ws" };
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
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 36;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `body_id`.
/// </summary>
System.UInt16 _bodyID;
/// <summary>
/// The field that maps onto the database column `cash`.
/// </summary>
System.UInt32 _cash;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.Nullable<System.UInt16> _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `exp`.
/// </summary>
System.UInt32 _exp;
/// <summary>
/// The field that maps onto the database column `hp`.
/// </summary>
System.Int16 _hP;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `level`.
/// </summary>
System.Byte _level;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapID;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.Int16 _mP;
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
System.Nullable<System.UInt16> _respawnMap;
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
System.UInt32 _statPoints;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Single _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Single _y;
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Byte)_stat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.Byte)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `body_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
public DemoGame.BodyIndex BodyID
{
get
{
return (DemoGame.BodyIndex)_bodyID;
}
set
{
this._bodyID = (System.UInt16)value;
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
return (System.UInt32)_cash;
}
set
{
this._cash = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<DemoGame.Server.CharacterTemplateID> CharacterTemplateID
{
get
{
return (System.Nullable<DemoGame.Server.CharacterTemplateID>)_characterTemplateID;
}
set
{
this._characterTemplateID = (System.Nullable<System.UInt16>)value;
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
return (System.UInt32)_exp;
}
set
{
this._exp = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `hp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 HP
{
get
{
return (System.Int16)_hP;
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
public DemoGame.Server.CharacterID ID
{
get
{
return (DemoGame.Server.CharacterID)_iD;
}
set
{
this._iD = (System.Int32)value;
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
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
public NetGore.MapIndex MapID
{
get
{
return (NetGore.MapIndex)_mapID;
}
set
{
this._mapID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 MP
{
get
{
return (System.Int16)_mP;
}
set
{
this._mP = (System.Int16)value;
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
return (System.String)_name;
}
set
{
this._name = (System.String)value;
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
return (System.String)_password;
}
set
{
this._password = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn_map`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<NetGore.MapIndex> RespawnMap
{
get
{
return (System.Nullable<NetGore.MapIndex>)_respawnMap;
}
set
{
this._respawnMap = (System.Nullable<System.UInt16>)value;
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
return (System.Single)_respawnX;
}
set
{
this._respawnX = (System.Single)value;
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
return (System.Single)_respawnY;
}
set
{
this._respawnY = (System.Single)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(10) unsigned` with the default value of `0`.
/// </summary>
public System.UInt32 StatPoints
{
get
{
return (System.UInt32)_statPoints;
}
set
{
this._statPoints = (System.UInt32)value;
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
return (System.Single)_x;
}
set
{
this._x = (System.Single)value;
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
return (System.Single)_y;
}
set
{
this._y = (System.Single)value;
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
/// <param name="bodyID">The initial value for the corresponding property.</param>
/// <param name="bra">The initial value for the corresponding property.</param>
/// <param name="cash">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="defence">The initial value for the corresponding property.</param>
/// <param name="dex">The initial value for the corresponding property.</param>
/// <param name="evade">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="imm">The initial value for the corresponding property.</param>
/// <param name="int">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="maxHit">The initial value for the corresponding property.</param>
/// <param name="maxHP">The initial value for the corresponding property.</param>
/// <param name="maxMP">The initial value for the corresponding property.</param>
/// <param name="minHit">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="password">The initial value for the corresponding property.</param>
/// <param name="perc">The initial value for the corresponding property.</param>
/// <param name="recov">The initial value for the corresponding property.</param>
/// <param name="regen">The initial value for the corresponding property.</param>
/// <param name="respawnMap">The initial value for the corresponding property.</param>
/// <param name="respawnX">The initial value for the corresponding property.</param>
/// <param name="respawnY">The initial value for the corresponding property.</param>
/// <param name="statPoints">The initial value for the corresponding property.</param>
/// <param name="str">The initial value for the corresponding property.</param>
/// <param name="tact">The initial value for the corresponding property.</param>
/// <param name="wS">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public CharacterTable(System.Byte @acc, System.Byte @agi, System.Byte @armor, DemoGame.BodyIndex @bodyID, System.Byte @bra, System.UInt32 @cash, System.Nullable<DemoGame.Server.CharacterTemplateID> @characterTemplateID, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, System.Int16 @hP, DemoGame.Server.CharacterID @iD, System.Byte @imm, System.Byte @int, System.Byte @level, NetGore.MapIndex @mapID, System.Byte @maxHit, System.Int16 @maxHP, System.Int16 @maxMP, System.Byte @minHit, System.Int16 @mP, System.String @name, System.String @password, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.Nullable<NetGore.MapIndex> @respawnMap, System.Single @respawnX, System.Single @respawnY, System.UInt32 @statPoints, System.Byte @str, System.Byte @tact, System.Byte @wS, System.Single @x, System.Single @y)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)@acc);
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
BodyID = (DemoGame.BodyIndex)@bodyID;
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
Cash = (System.UInt32)@cash;
CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)@characterTemplateID;
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
Exp = (System.UInt32)@exp;
HP = (System.Int16)@hP;
ID = (DemoGame.Server.CharacterID)@iD;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
Level = (System.Byte)@level;
MapID = (NetGore.MapIndex)@mapID;
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxHit);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxHP);
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxMP);
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minHit);
MP = (System.Int16)@mP;
Name = (System.String)@name;
Password = (System.String)@password;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)@perc);
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)@recov);
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)@regen);
RespawnMap = (System.Nullable<NetGore.MapIndex>)@respawnMap;
RespawnX = (System.Single)@respawnX;
RespawnY = (System.Single)@respawnY;
StatPoints = (System.UInt32)@statPoints;
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@str);
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)@tact);
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)@wS);
X = (System.Single)@x;
Y = (System.Single)@y;
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
System.Int32 i;

i = dataReader.GetOrdinal("acc");
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("agi");
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("armor");
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("body_id");
BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("bra");
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("cash");
Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("character_template_id");
CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("defence");
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("dex");
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("evade");
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("exp");
Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("hp");
HP = (System.Int16)(System.Int16)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("id");
ID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("imm");
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("int");
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("level");
Level = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("map_id");
MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("maxhit");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("maxhp");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("maxmp");
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));

i = dataReader.GetOrdinal("minhit");
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("mp");
MP = (System.Int16)(System.Int16)dataReader.GetInt16(i);

i = dataReader.GetOrdinal("name");
Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("password");
Password = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("perc");
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("recov");
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("regen");
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("respawn_map");
RespawnMap = (System.Nullable<NetGore.MapIndex>)(System.Nullable<NetGore.MapIndex>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));

i = dataReader.GetOrdinal("respawn_x");
RespawnX = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("respawn_y");
RespawnY = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("statpoints");
StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("str");
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("tact");
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("ws");
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(i));

i = dataReader.GetOrdinal("x");
X = (System.Single)(System.Single)dataReader.GetFloat(i);

i = dataReader.GetOrdinal("y");
Y = (System.Single)(System.Single)dataReader.GetFloat(i);
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
public static void CopyValues(ICharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
dic["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@cash"] = (System.UInt32)source.Cash;
dic["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
dic["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@exp"] = (System.UInt32)source.Exp;
dic["@hp"] = (System.Int16)source.HP;
dic["@id"] = (DemoGame.Server.CharacterID)source.ID;
dic["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (NetGore.MapIndex)source.MapID;
dic["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@mp"] = (System.Int16)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@password"] = (System.String)source.Password;
dic["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
dic["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
dic["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
dic["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@statpoints"] = (System.UInt32)source.StatPoints;
dic["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
dic["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
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
paramValues["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
paramValues["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
paramValues["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
paramValues["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
paramValues["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
paramValues["@cash"] = (System.UInt32)source.Cash;
paramValues["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
paramValues["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
paramValues["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
paramValues["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
paramValues["@exp"] = (System.UInt32)source.Exp;
paramValues["@hp"] = (System.Int16)source.HP;
paramValues["@id"] = (DemoGame.Server.CharacterID)source.ID;
paramValues["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
paramValues["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
paramValues["@level"] = (System.Byte)source.Level;
paramValues["@map_id"] = (NetGore.MapIndex)source.MapID;
paramValues["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
paramValues["@maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
paramValues["@maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
paramValues["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
paramValues["@mp"] = (System.Int16)source.MP;
paramValues["@name"] = (System.String)source.Name;
paramValues["@password"] = (System.String)source.Password;
paramValues["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
paramValues["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
paramValues["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
paramValues["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
paramValues["@respawn_x"] = (System.Single)source.RespawnX;
paramValues["@respawn_y"] = (System.Single)source.RespawnY;
paramValues["@statpoints"] = (System.UInt32)source.StatPoints;
paramValues["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
paramValues["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
paramValues["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
paramValues["@x"] = (System.Single)source.X;
paramValues["@y"] = (System.Single)source.Y;
}

public void CopyValuesFrom(ICharacterTable source)
{
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc));
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
BodyID = (DemoGame.BodyIndex)source.BodyID;
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
Cash = (System.UInt32)source.Cash;
CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
Exp = (System.UInt32)source.Exp;
HP = (System.Int16)source.HP;
ID = (DemoGame.Server.CharacterID)source.ID;
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
Level = (System.Byte)source.Level;
MapID = (NetGore.MapIndex)source.MapID;
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
MP = (System.Int16)source.MP;
Name = (System.String)source.Name;
Password = (System.String)source.Password;
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc));
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov));
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen));
RespawnMap = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
RespawnX = (System.Single)source.RespawnX;
RespawnY = (System.Single)source.RespawnY;
StatPoints = (System.UInt32)source.StatPoints;
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact));
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS));
X = (System.Single)source.X;
Y = (System.Single)source.Y;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "acc":
return GetStat((DemoGame.StatType)DemoGame.StatType.Acc);

case "agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "armor":
return GetStat((DemoGame.StatType)DemoGame.StatType.Armor);

case "body_id":
return BodyID;

case "bra":
return GetStat((DemoGame.StatType)DemoGame.StatType.Bra);

case "cash":
return Cash;

case "character_template_id":
return CharacterTemplateID;

case "defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "dex":
return GetStat((DemoGame.StatType)DemoGame.StatType.Dex);

case "evade":
return GetStat((DemoGame.StatType)DemoGame.StatType.Evade);

case "exp":
return Exp;

case "hp":
return HP;

case "id":
return ID;

case "imm":
return GetStat((DemoGame.StatType)DemoGame.StatType.Imm);

case "int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "level":
return Level;

case "map_id":
return MapID;

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

case "password":
return Password;

case "perc":
return GetStat((DemoGame.StatType)DemoGame.StatType.Perc);

case "recov":
return GetStat((DemoGame.StatType)DemoGame.StatType.Recov);

case "regen":
return GetStat((DemoGame.StatType)DemoGame.StatType.Regen);

case "respawn_map":
return RespawnMap;

case "respawn_x":
return RespawnX;

case "respawn_y":
return RespawnY;

case "statpoints":
return StatPoints;

case "str":
return GetStat((DemoGame.StatType)DemoGame.StatType.Str);

case "tact":
return GetStat((DemoGame.StatType)DemoGame.StatType.Tact);

case "ws":
return GetStat((DemoGame.StatType)DemoGame.StatType.WS);

case "x":
return X;

case "y":
return Y;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "acc":
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)value);
break;

case "agi":
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "armor":
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "body_id":
BodyID = (DemoGame.BodyIndex)value;
break;

case "bra":
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "cash":
Cash = (System.UInt32)value;
break;

case "character_template_id":
CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)value;
break;

case "defence":
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "dex":
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "evade":
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
break;

case "exp":
Exp = (System.UInt32)value;
break;

case "hp":
HP = (System.Int16)value;
break;

case "id":
ID = (DemoGame.Server.CharacterID)value;
break;

case "imm":
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "int":
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "level":
Level = (System.Byte)value;
break;

case "map_id":
MapID = (NetGore.MapIndex)value;
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
MP = (System.Int16)value;
break;

case "name":
Name = (System.String)value;
break;

case "password":
Password = (System.String)value;
break;

case "perc":
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)value);
break;

case "recov":
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)value);
break;

case "regen":
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)value);
break;

case "respawn_map":
RespawnMap = (System.Nullable<NetGore.MapIndex>)value;
break;

case "respawn_x":
RespawnX = (System.Single)value;
break;

case "respawn_y":
RespawnY = (System.Single)value;
break;

case "statpoints":
StatPoints = (System.UInt32)value;
break;

case "str":
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
break;

case "tact":
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)value);
break;

case "ws":
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)value);
break;

case "x":
X = (System.Single)value;
break;

case "y":
Y = (System.Single)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "acc":
return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "agi":
return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "armor":
return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "body_id":
return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "bra":
return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "cash":
return new ColumnMetadata("cash", "", "int(10) unsigned", "0", typeof(System.UInt32), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "defence":
return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "dex":
return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "evade":
return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "exp":
return new ColumnMetadata("exp", "", "int(10) unsigned", "0", typeof(System.UInt32), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "imm":
return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "int":
return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "map_id":
return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, true);

case "maxhit":
return new ColumnMetadata("maxhit", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "maxhp":
return new ColumnMetadata("maxhp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "maxmp":
return new ColumnMetadata("maxmp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "minhit":
return new ColumnMetadata("minhit", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(50)", null, typeof(System.String), false, false, true);

case "password":
return new ColumnMetadata("password", "", "varchar(50)", null, typeof(System.String), false, false, false);

case "perc":
return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "recov":
return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "regen":
return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "respawn_map":
return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "respawn_x":
return new ColumnMetadata("respawn_x", "", "float", null, typeof(System.Single), false, false, false);

case "respawn_y":
return new ColumnMetadata("respawn_y", "", "float", null, typeof(System.Single), false, false, false);

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(10) unsigned", "0", typeof(System.UInt32), false, false, false);

case "str":
return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "tact":
return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "ws":
return new ColumnMetadata("ws", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "x":
return new ColumnMetadata("x", "", "float", "100", typeof(System.Single), false, false, false);

case "y":
return new ColumnMetadata("y", "", "float", "100", typeof(System.Single), false, false, false);

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
case "acc":
SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "agi":
SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "armor":
SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "body_id":
BodyID = (DemoGame.BodyIndex)(DemoGame.BodyIndex)dataReader.GetUInt16(i);
break;


case "bra":
SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "cash":
Cash = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "character_template_id":
CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)(System.Nullable<DemoGame.Server.CharacterTemplateID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "defence":
SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "dex":
SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "evade":
SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "exp":
Exp = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "hp":
HP = (System.Int16)(System.Int16)dataReader.GetInt16(i);
break;


case "id":
ID = (DemoGame.Server.CharacterID)(DemoGame.Server.CharacterID)dataReader.GetInt32(i);
break;


case "imm":
SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "int":
SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "level":
Level = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "map_id":
MapID = (NetGore.MapIndex)(NetGore.MapIndex)dataReader.GetUInt16(i);
break;


case "maxhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "maxhp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "maxmp":
SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)(System.Int16)dataReader.GetInt16(i));
break;


case "minhit":
SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "mp":
MP = (System.Int16)(System.Int16)dataReader.GetInt16(i);
break;


case "name":
Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "password":
Password = (System.String)(System.String)dataReader.GetString(i);
break;


case "perc":
SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "recov":
SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "regen":
SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "respawn_map":
RespawnMap = (System.Nullable<NetGore.MapIndex>)(System.Nullable<NetGore.MapIndex>)(dataReader.IsDBNull(i) ? (System.Nullable<System.UInt16>)null : dataReader.GetUInt16(i));
break;


case "respawn_x":
RespawnX = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "respawn_y":
RespawnY = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "statpoints":
StatPoints = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "str":
SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "tact":
SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "ws":
SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)(System.Byte)dataReader.GetByte(i));
break;


case "x":
X = (System.Single)(System.Single)dataReader.GetFloat(i);
break;


case "y":
Y = (System.Single)(System.Single)dataReader.GetFloat(i);
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
public static void TryCopyValues(ICharacterTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@acc":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
break;


case "@agi":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
break;


case "@armor":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
break;


case "@body_id":
paramValues[i] = source.BodyID;
break;


case "@bra":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
break;


case "@cash":
paramValues[i] = source.Cash;
break;


case "@character_template_id":
paramValues[i] = source.CharacterTemplateID;
break;


case "@defence":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
break;


case "@dex":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
break;


case "@evade":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
break;


case "@exp":
paramValues[i] = source.Exp;
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


case "@level":
paramValues[i] = source.Level;
break;


case "@map_id":
paramValues[i] = source.MapID;
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


case "@password":
paramValues[i] = source.Password;
break;


case "@perc":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
break;


case "@recov":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
break;


case "@regen":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
break;


case "@respawn_map":
paramValues[i] = source.RespawnMap;
break;


case "@respawn_x":
paramValues[i] = source.RespawnX;
break;


case "@respawn_y":
paramValues[i] = source.RespawnY;
break;


case "@statpoints":
paramValues[i] = source.StatPoints;
break;


case "@str":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
break;


case "@tact":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
break;


case "@ws":
paramValues[i] = source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
break;


case "@x":
paramValues[i] = source.X;
break;


case "@y":
paramValues[i] = source.Y;
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
}

}
