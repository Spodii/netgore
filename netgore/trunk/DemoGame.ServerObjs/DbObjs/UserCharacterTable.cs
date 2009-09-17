using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `user_character`.
/// </summary>
public class UserCharacterTable : IUserCharacterTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex", "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] { };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex", "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc", "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y" };
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
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "user_character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 37;
/// <summary>
/// The field that maps onto the database column `acc`.
/// </summary>
System.Byte _acc;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Nullable<System.Int32> _accountID;
/// <summary>
/// The field that maps onto the database column `agi`.
/// </summary>
System.Byte _agi;
/// <summary>
/// The field that maps onto the database column `armor`.
/// </summary>
System.Byte _armor;
/// <summary>
/// The field that maps onto the database column `body_id`.
/// </summary>
System.UInt16 _bodyID;
/// <summary>
/// The field that maps onto the database column `bra`.
/// </summary>
System.Byte _bra;
/// <summary>
/// The field that maps onto the database column `cash`.
/// </summary>
System.UInt32 _cash;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.Nullable<System.UInt16> _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `chat_dialog`.
/// </summary>
System.Nullable<System.UInt16> _chatDialog;
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
System.Int16 _hP;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
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
System.UInt16 _mapID;
/// <summary>
/// The field that maps onto the database column `maxhit`.
/// </summary>
System.Byte _maxHit;
/// <summary>
/// The field that maps onto the database column `maxhp`.
/// </summary>
System.Int16 _maxHP;
/// <summary>
/// The field that maps onto the database column `maxmp`.
/// </summary>
System.Int16 _maxMP;
/// <summary>
/// The field that maps onto the database column `minhit`.
/// </summary>
System.Byte _minHit;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.Int16 _mP;
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
System.Byte _wS;
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
return (System.Byte)_acc;
}
set
{
this._acc = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`.
/// </summary>
public System.Nullable<DemoGame.Server.AccountID> AccountID
{
get
{
return (System.Nullable<DemoGame.Server.AccountID>)_accountID;
}
set
{
this._accountID = (System.Nullable<System.Int32>)value;
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
return (System.Byte)_agi;
}
set
{
this._agi = (System.Byte)value;
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
return (System.Byte)_armor;
}
set
{
this._armor = (System.Byte)value;
}
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
/// Gets or sets the value for the field that maps onto the database column `bra`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Bra
{
get
{
return (System.Byte)_bra;
}
set
{
this._bra = (System.Byte)value;
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
/// Gets or sets the value for the field that maps onto the database column `chat_dialog`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<System.UInt16> ChatDialog
{
get
{
return (System.Nullable<System.UInt16>)_chatDialog;
}
set
{
this._chatDialog = (System.Nullable<System.UInt16>)value;
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
return (System.Byte)_defence;
}
set
{
this._defence = (System.Byte)value;
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
return (System.Byte)_dex;
}
set
{
this._dex = (System.Byte)value;
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
return (System.Byte)_evade;
}
set
{
this._evade = (System.Byte)value;
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
public System.Int32 ID
{
get
{
return (System.Int32)_iD;
}
set
{
this._iD = (System.Int32)value;
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
return (System.Byte)_imm;
}
set
{
this._imm = (System.Byte)value;
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
return (System.Byte)_int;
}
set
{
this._int = (System.Byte)value;
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
/// Gets or sets the value for the field that maps onto the database column `maxhit`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte MaxHit
{
get
{
return (System.Byte)_maxHit;
}
set
{
this._maxHit = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxhp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 MaxHP
{
get
{
return (System.Int16)_maxHP;
}
set
{
this._maxHP = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `maxmp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 MaxMP
{
get
{
return (System.Int16)_maxMP;
}
set
{
this._maxMP = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `minhit`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte MinHit
{
get
{
return (System.Byte)_minHit;
}
set
{
this._minHit = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
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
/// The underlying database type is `varchar(30)`.
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
/// Gets or sets the value for the field that maps onto the database column `perc`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Perc
{
get
{
return (System.Byte)_perc;
}
set
{
this._perc = (System.Byte)value;
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
return (System.Byte)_recov;
}
set
{
this._recov = (System.Byte)value;
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
return (System.Byte)_regen;
}
set
{
this._regen = (System.Byte)value;
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
/// The underlying database type is `float` with the default value of `50`.
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
/// The underlying database type is `float` with the default value of `50`.
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
/// Gets or sets the value for the field that maps onto the database column `str`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Str
{
get
{
return (System.Byte)_str;
}
set
{
this._str = (System.Byte)value;
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
return (System.Byte)_tact;
}
set
{
this._tact = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ws`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte WS
{
get
{
return (System.Byte)_wS;
}
set
{
this._wS = (System.Byte)value;
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IUserCharacterTable DeepCopy()
{
return new UserCharacterTable(this);
}
/// <summary>
/// UserCharacterTable constructor.
/// </summary>
public UserCharacterTable()
{
}
/// <summary>
/// UserCharacterTable constructor.
/// </summary>
/// <param name="acc">The initial value for the corresponding property.</param>
/// <param name="accountID">The initial value for the corresponding property.</param>
/// <param name="agi">The initial value for the corresponding property.</param>
/// <param name="armor">The initial value for the corresponding property.</param>
/// <param name="bodyID">The initial value for the corresponding property.</param>
/// <param name="bra">The initial value for the corresponding property.</param>
/// <param name="cash">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="chatDialog">The initial value for the corresponding property.</param>
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
public UserCharacterTable(System.Byte @acc, System.Nullable<DemoGame.Server.AccountID> @accountID, System.Byte @agi, System.Byte @armor, DemoGame.BodyIndex @bodyID, System.Byte @bra, System.UInt32 @cash, System.Nullable<DemoGame.Server.CharacterTemplateID> @characterTemplateID, System.Nullable<System.UInt16> @chatDialog, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, DemoGame.SPValueType @hP, System.Int32 @iD, System.Byte @imm, System.Byte @int, System.Byte @level, NetGore.MapIndex @mapID, System.Byte @maxHit, System.Int16 @maxHP, System.Int16 @maxMP, System.Byte @minHit, DemoGame.SPValueType @mP, System.String @name, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.Nullable<NetGore.MapIndex> @respawnMap, System.Single @respawnX, System.Single @respawnY, System.UInt32 @statPoints, System.Byte @str, System.Byte @tact, System.Byte @wS, System.Single @x, System.Single @y)
{
this.Acc = (System.Byte)@acc;
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)@accountID;
this.Agi = (System.Byte)@agi;
this.Armor = (System.Byte)@armor;
this.BodyID = (DemoGame.BodyIndex)@bodyID;
this.Bra = (System.Byte)@bra;
this.Cash = (System.UInt32)@cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)@characterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)@chatDialog;
this.Defence = (System.Byte)@defence;
this.Dex = (System.Byte)@dex;
this.Evade = (System.Byte)@evade;
this.Exp = (System.UInt32)@exp;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (System.Int32)@iD;
this.Imm = (System.Byte)@imm;
this.Int = (System.Byte)@int;
this.Level = (System.Byte)@level;
this.MapID = (NetGore.MapIndex)@mapID;
this.MaxHit = (System.Byte)@maxHit;
this.MaxHP = (System.Int16)@maxHP;
this.MaxMP = (System.Int16)@maxMP;
this.MinHit = (System.Byte)@minHit;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.Perc = (System.Byte)@perc;
this.Recov = (System.Byte)@recov;
this.Regen = (System.Byte)@regen;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)@respawnMap;
this.RespawnX = (System.Single)@respawnX;
this.RespawnY = (System.Single)@respawnY;
this.StatPoints = (System.UInt32)@statPoints;
this.Str = (System.Byte)@str;
this.Tact = (System.Byte)@tact;
this.WS = (System.Byte)@wS;
this.X = (System.Single)@x;
this.Y = (System.Single)@y;
}
/// <summary>
/// UserCharacterTable constructor.
/// </summary>
/// <param name="source">IUserCharacterTable to copy the initial values from.</param>
public UserCharacterTable(IUserCharacterTable source)
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
public static void CopyValues(IUserCharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@acc"] = (System.Byte)source.Acc;
dic["@account_id"] = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
dic["@agi"] = (System.Byte)source.Agi;
dic["@armor"] = (System.Byte)source.Armor;
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@bra"] = (System.Byte)source.Bra;
dic["@cash"] = (System.UInt32)source.Cash;
dic["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
dic["@chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
dic["@defence"] = (System.Byte)source.Defence;
dic["@dex"] = (System.Byte)source.Dex;
dic["@evade"] = (System.Byte)source.Evade;
dic["@exp"] = (System.UInt32)source.Exp;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (System.Int32)source.ID;
dic["@imm"] = (System.Byte)source.Imm;
dic["@int"] = (System.Byte)source.Int;
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (NetGore.MapIndex)source.MapID;
dic["@maxhit"] = (System.Byte)source.MaxHit;
dic["@maxhp"] = (System.Int16)source.MaxHP;
dic["@maxmp"] = (System.Int16)source.MaxMP;
dic["@minhit"] = (System.Byte)source.MinHit;
dic["@mp"] = (DemoGame.SPValueType)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.Byte)source.Perc;
dic["@recov"] = (System.Byte)source.Recov;
dic["@regen"] = (System.Byte)source.Regen;
dic["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@statpoints"] = (System.UInt32)source.StatPoints;
dic["@str"] = (System.Byte)source.Str;
dic["@tact"] = (System.Byte)source.Tact;
dic["@ws"] = (System.Byte)source.WS;
dic["@x"] = (System.Single)source.X;
dic["@y"] = (System.Single)source.Y;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this UserCharacterTable.
/// </summary>
/// <param name="source">The IUserCharacterTable to copy the values from.</param>
public void CopyValuesFrom(IUserCharacterTable source)
{
this.Acc = (System.Byte)source.Acc;
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
this.Agi = (System.Byte)source.Agi;
this.Armor = (System.Byte)source.Armor;
this.BodyID = (DemoGame.BodyIndex)source.BodyID;
this.Bra = (System.Byte)source.Bra;
this.Cash = (System.UInt32)source.Cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)source.ChatDialog;
this.Defence = (System.Byte)source.Defence;
this.Dex = (System.Byte)source.Dex;
this.Evade = (System.Byte)source.Evade;
this.Exp = (System.UInt32)source.Exp;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (System.Int32)source.ID;
this.Imm = (System.Byte)source.Imm;
this.Int = (System.Byte)source.Int;
this.Level = (System.Byte)source.Level;
this.MapID = (NetGore.MapIndex)source.MapID;
this.MaxHit = (System.Byte)source.MaxHit;
this.MaxHP = (System.Int16)source.MaxHP;
this.MaxMP = (System.Int16)source.MaxMP;
this.MinHit = (System.Byte)source.MinHit;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.Perc = (System.Byte)source.Perc;
this.Recov = (System.Byte)source.Recov;
this.Regen = (System.Byte)source.Regen;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
this.RespawnX = (System.Single)source.RespawnX;
this.RespawnY = (System.Single)source.RespawnY;
this.StatPoints = (System.UInt32)source.StatPoints;
this.Str = (System.Byte)source.Str;
this.Tact = (System.Byte)source.Tact;
this.WS = (System.Byte)source.WS;
this.X = (System.Single)source.X;
this.Y = (System.Single)source.Y;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "acc":
return Acc;

case "account_id":
return AccountID;

case "agi":
return Agi;

case "armor":
return Armor;

case "body_id":
return BodyID;

case "bra":
return Bra;

case "cash":
return Cash;

case "character_template_id":
return CharacterTemplateID;

case "chat_dialog":
return ChatDialog;

case "defence":
return Defence;

case "dex":
return Dex;

case "evade":
return Evade;

case "exp":
return Exp;

case "hp":
return HP;

case "id":
return ID;

case "imm":
return Imm;

case "int":
return Int;

case "level":
return Level;

case "map_id":
return MapID;

case "maxhit":
return MaxHit;

case "maxhp":
return MaxHP;

case "maxmp":
return MaxMP;

case "minhit":
return MinHit;

case "mp":
return MP;

case "name":
return Name;

case "perc":
return Perc;

case "recov":
return Recov;

case "regen":
return Regen;

case "respawn_map":
return RespawnMap;

case "respawn_x":
return RespawnX;

case "respawn_y":
return RespawnY;

case "statpoints":
return StatPoints;

case "str":
return Str;

case "tact":
return Tact;

case "ws":
return WS;

case "x":
return X;

case "y":
return Y;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "acc":
this.Acc = (System.Byte)value;
break;

case "account_id":
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)value;
break;

case "agi":
this.Agi = (System.Byte)value;
break;

case "armor":
this.Armor = (System.Byte)value;
break;

case "body_id":
this.BodyID = (DemoGame.BodyIndex)value;
break;

case "bra":
this.Bra = (System.Byte)value;
break;

case "cash":
this.Cash = (System.UInt32)value;
break;

case "character_template_id":
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)value;
break;

case "chat_dialog":
this.ChatDialog = (System.Nullable<System.UInt16>)value;
break;

case "defence":
this.Defence = (System.Byte)value;
break;

case "dex":
this.Dex = (System.Byte)value;
break;

case "evade":
this.Evade = (System.Byte)value;
break;

case "exp":
this.Exp = (System.UInt32)value;
break;

case "hp":
this.HP = (DemoGame.SPValueType)value;
break;

case "id":
this.ID = (System.Int32)value;
break;

case "imm":
this.Imm = (System.Byte)value;
break;

case "int":
this.Int = (System.Byte)value;
break;

case "level":
this.Level = (System.Byte)value;
break;

case "map_id":
this.MapID = (NetGore.MapIndex)value;
break;

case "maxhit":
this.MaxHit = (System.Byte)value;
break;

case "maxhp":
this.MaxHP = (System.Int16)value;
break;

case "maxmp":
this.MaxMP = (System.Int16)value;
break;

case "minhit":
this.MinHit = (System.Byte)value;
break;

case "mp":
this.MP = (DemoGame.SPValueType)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "perc":
this.Perc = (System.Byte)value;
break;

case "recov":
this.Recov = (System.Byte)value;
break;

case "regen":
this.Regen = (System.Byte)value;
break;

case "respawn_map":
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)value;
break;

case "respawn_x":
this.RespawnX = (System.Single)value;
break;

case "respawn_y":
this.RespawnY = (System.Single)value;
break;

case "statpoints":
this.StatPoints = (System.UInt32)value;
break;

case "str":
this.Str = (System.Byte)value;
break;

case "tact":
this.Tact = (System.Byte)value;
break;

case "ws":
this.WS = (System.Byte)value;
break;

case "x":
this.X = (System.Single)value;
break;

case "y":
this.Y = (System.Single)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "acc":
return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "account_id":
return new ColumnMetadata("account_id", "", "int(11)", null, typeof(System.Nullable<System.Int32>), true, false, false);

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
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "chat_dialog":
return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

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
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, false, false);

case "imm":
return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "int":
return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "map_id":
return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

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
return new ColumnMetadata("name", "", "varchar(30)", null, typeof(System.String), false, false, false);

case "perc":
return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "recov":
return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "regen":
return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "respawn_map":
return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "respawn_x":
return new ColumnMetadata("respawn_x", "", "float", "50", typeof(System.Single), false, false, false);

case "respawn_y":
return new ColumnMetadata("respawn_y", "", "float", "50", typeof(System.Single), false, false, false);

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(10) unsigned", "0", typeof(System.UInt32), false, false, false);

case "str":
return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "tact":
return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "ws":
return new ColumnMetadata("ws", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "x":
return new ColumnMetadata("x", "", "float", "100", typeof(System.Single), false, false, false);

case "y":
return new ColumnMetadata("y", "", "float", "100", typeof(System.Single), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

}

}
