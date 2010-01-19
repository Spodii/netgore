using System;
using System.Linq;
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `character`.
/// </summary>
public class CharacterTable : ICharacterTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "map_id", "move_speed", "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "x", "y" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "level", "map_id", "move_speed", "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "x", "y" };
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
 static  readonly System.String[] _statColumns = new string[] {"stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };
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
/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
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
public const System.Int32 ColumnCount = 29;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Nullable<System.Int32> _accountID;
/// <summary>
/// The field that maps onto the database column `ai_id`.
/// </summary>
System.Nullable<System.UInt16> _aIID;
/// <summary>
/// The field that maps onto the database column `body_id`.
/// </summary>
System.UInt16 _bodyID;
/// <summary>
/// The field that maps onto the database column `cash`.
/// </summary>
System.Int32 _cash;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.Nullable<System.UInt16> _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `chat_dialog`.
/// </summary>
System.Nullable<System.UInt16> _chatDialog;
/// <summary>
/// The field that maps onto the database column `exp`.
/// </summary>
System.Int32 _exp;
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
/// The field that maps onto the database column `move_speed`.
/// </summary>
System.UInt16 _moveSpeed;
/// <summary>
/// The field that maps onto the database column `mp`.
/// </summary>
System.Int16 _mP;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
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
/// The field that maps onto the database column `shop_id`.
/// </summary>
System.Nullable<System.UInt16> _shopID;
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.Int32 _statPoints;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Single _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Single _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `ai_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.AI.AIID> AIID
{
get
{
return (System.Nullable<NetGore.AI.AIID>)_aIID;
}
set
{
this._aIID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `body_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Int32 Cash
{
get
{
return (System.Int32)_cash;
}
set
{
this._cash = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_template_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `exp`.
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Int32 Exp
{
get
{
return (System.Int32)_exp;
}
set
{
this._exp = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `hp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `move_speed`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1800`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 MoveSpeed
{
get
{
return (System.UInt16)_moveSpeed;
}
set
{
this._moveSpeed = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `mp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `respawn_map`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
/// Gets or sets the value for the field that maps onto the database column `shop_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.Features.Shops.ShopID> ShopID
{
get
{
return (System.Nullable<NetGore.Features.Shops.ShopID>)_shopID;
}
set
{
this._shopID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Int32 StatPoints
{
get
{
return (System.Int32)_statPoints;
}
set
{
this._statPoints = (System.Int32)value;
}
}
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
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `float` with the default value of `100`.
/// </summary>
[NetGore.SyncValueAttribute()]
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
[NetGore.SyncValueAttribute()]
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
public ICharacterTable DeepCopy()
{
return new CharacterTable(this);
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
/// <param name="accountID">The initial value for the corresponding property.</param>
/// <param name="aIID">The initial value for the corresponding property.</param>
/// <param name="bodyID">The initial value for the corresponding property.</param>
/// <param name="cash">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="chatDialog">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="moveSpeed">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="respawnMap">The initial value for the corresponding property.</param>
/// <param name="respawnX">The initial value for the corresponding property.</param>
/// <param name="respawnY">The initial value for the corresponding property.</param>
/// <param name="shopID">The initial value for the corresponding property.</param>
/// <param name="statPoints">The initial value for the corresponding property.</param>
/// <param name="statAgi">The initial value for the corresponding property.</param>
/// <param name="statDefence">The initial value for the corresponding property.</param>
/// <param name="statInt">The initial value for the corresponding property.</param>
/// <param name="statMaxhit">The initial value for the corresponding property.</param>
/// <param name="statMaxhp">The initial value for the corresponding property.</param>
/// <param name="statMaxmp">The initial value for the corresponding property.</param>
/// <param name="statMinhit">The initial value for the corresponding property.</param>
/// <param name="statStr">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public CharacterTable(System.Nullable<DemoGame.Server.AccountID> @accountID, System.Nullable<NetGore.AI.AIID> @aIID, DemoGame.BodyIndex @bodyID, System.Int32 @cash, System.Nullable<DemoGame.Server.CharacterTemplateID> @characterTemplateID, System.Nullable<System.UInt16> @chatDialog, System.Int32 @exp, DemoGame.SPValueType @hP, DemoGame.Server.CharacterID @iD, System.Byte @level, NetGore.MapIndex @mapID, System.UInt16 @moveSpeed, DemoGame.SPValueType @mP, System.String @name, System.Nullable<NetGore.MapIndex> @respawnMap, System.Single @respawnX, System.Single @respawnY, System.Nullable<NetGore.Features.Shops.ShopID> @shopID, System.Int32 @statPoints, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statStr, System.Single @x, System.Single @y)
{
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)@accountID;
this.AIID = (System.Nullable<NetGore.AI.AIID>)@aIID;
this.BodyID = (DemoGame.BodyIndex)@bodyID;
this.Cash = (System.Int32)@cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)@characterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)@chatDialog;
this.Exp = (System.Int32)@exp;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (DemoGame.Server.CharacterID)@iD;
this.Level = (System.Byte)@level;
this.MapID = (NetGore.MapIndex)@mapID;
this.MoveSpeed = (System.UInt16)@moveSpeed;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)@respawnMap;
this.RespawnX = (System.Single)@respawnX;
this.RespawnY = (System.Single)@respawnY;
this.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)@shopID;
this.StatPoints = (System.Int32)@statPoints;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@statAgi);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@statDefence);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@statInt);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@statMaxhit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@statMaxhp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@statMaxmp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@statMinhit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@statStr);
this.X = (System.Single)@x;
this.Y = (System.Single)@y;
}
/// <summary>
/// CharacterTable constructor.
/// </summary>
/// <param name="source">ICharacterTable to copy the initial values from.</param>
public CharacterTable(ICharacterTable source)
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
public static void CopyValues(ICharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@account_id"] = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
dic["@ai_id"] = (System.Nullable<NetGore.AI.AIID>)source.AIID;
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@cash"] = (System.Int32)source.Cash;
dic["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
dic["@chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
dic["@exp"] = (System.Int32)source.Exp;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (DemoGame.Server.CharacterID)source.ID;
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (NetGore.MapIndex)source.MapID;
dic["@move_speed"] = (System.UInt16)source.MoveSpeed;
dic["@mp"] = (DemoGame.SPValueType)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@shop_id"] = (System.Nullable<NetGore.Features.Shops.ShopID>)source.ShopID;
dic["@statpoints"] = (System.Int32)source.StatPoints;
dic["@stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@x"] = (System.Single)source.X;
dic["@y"] = (System.Single)source.Y;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterTable.
/// </summary>
/// <param name="source">The ICharacterTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterTable source)
{
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
this.AIID = (System.Nullable<NetGore.AI.AIID>)source.AIID;
this.BodyID = (DemoGame.BodyIndex)source.BodyID;
this.Cash = (System.Int32)source.Cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)source.ChatDialog;
this.Exp = (System.Int32)source.Exp;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (DemoGame.Server.CharacterID)source.ID;
this.Level = (System.Byte)source.Level;
this.MapID = (NetGore.MapIndex)source.MapID;
this.MoveSpeed = (System.UInt16)source.MoveSpeed;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
this.RespawnX = (System.Single)source.RespawnX;
this.RespawnY = (System.Single)source.RespawnY;
this.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)source.ShopID;
this.StatPoints = (System.Int32)source.StatPoints;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
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
case "account_id":
return AccountID;

case "ai_id":
return AIID;

case "body_id":
return BodyID;

case "cash":
return Cash;

case "character_template_id":
return CharacterTemplateID;

case "chat_dialog":
return ChatDialog;

case "exp":
return Exp;

case "hp":
return HP;

case "id":
return ID;

case "level":
return Level;

case "map_id":
return MapID;

case "move_speed":
return MoveSpeed;

case "mp":
return MP;

case "name":
return Name;

case "respawn_map":
return RespawnMap;

case "respawn_x":
return RespawnX;

case "respawn_y":
return RespawnY;

case "shop_id":
return ShopID;

case "statpoints":
return StatPoints;

case "stat_agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "stat_defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "stat_int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "stat_maxhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);

case "stat_maxhp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);

case "stat_maxmp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);

case "stat_minhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);

case "stat_str":
return GetStat((DemoGame.StatType)DemoGame.StatType.Str);

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
case "account_id":
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)value;
break;

case "ai_id":
this.AIID = (System.Nullable<NetGore.AI.AIID>)value;
break;

case "body_id":
this.BodyID = (DemoGame.BodyIndex)value;
break;

case "cash":
this.Cash = (System.Int32)value;
break;

case "character_template_id":
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)value;
break;

case "chat_dialog":
this.ChatDialog = (System.Nullable<System.UInt16>)value;
break;

case "exp":
this.Exp = (System.Int32)value;
break;

case "hp":
this.HP = (DemoGame.SPValueType)value;
break;

case "id":
this.ID = (DemoGame.Server.CharacterID)value;
break;

case "level":
this.Level = (System.Byte)value;
break;

case "map_id":
this.MapID = (NetGore.MapIndex)value;
break;

case "move_speed":
this.MoveSpeed = (System.UInt16)value;
break;

case "mp":
this.MP = (DemoGame.SPValueType)value;
break;

case "name":
this.Name = (System.String)value;
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

case "shop_id":
this.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)value;
break;

case "statpoints":
this.StatPoints = (System.Int32)value;
break;

case "stat_agi":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "stat_defence":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "stat_int":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "stat_maxhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "stat_maxhp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "stat_maxmp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "stat_minhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "stat_str":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
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
case "account_id":
return new ColumnMetadata("account_id", "", "int(11)", null, typeof(System.Nullable<System.Int32>), true, false, true);

case "ai_id":
return new ColumnMetadata("ai_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "body_id":
return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "cash":
return new ColumnMetadata("cash", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "chat_dialog":
return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "exp":
return new ColumnMetadata("exp", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, true, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "map_id":
return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, true);

case "move_speed":
return new ColumnMetadata("move_speed", "", "smallint(5) unsigned", "1800", typeof(System.UInt16), false, false, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(30)", null, typeof(System.String), false, false, true);

case "respawn_map":
return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "respawn_x":
return new ColumnMetadata("respawn_x", "", "float", "50", typeof(System.Single), false, false, false);

case "respawn_y":
return new ColumnMetadata("respawn_y", "", "float", "50", typeof(System.Single), false, false, false);

case "shop_id":
return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "stat_agi":
return new ColumnMetadata("stat_agi", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_defence":
return new ColumnMetadata("stat_defence", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_int":
return new ColumnMetadata("stat_int", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhit":
return new ColumnMetadata("stat_maxhit", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhp":
return new ColumnMetadata("stat_maxhp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_maxmp":
return new ColumnMetadata("stat_maxmp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_minhit":
return new ColumnMetadata("stat_minhit", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_str":
return new ColumnMetadata("stat_str", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "x":
return new ColumnMetadata("x", "", "float", "100", typeof(System.Single), false, false, false);

case "y":
return new ColumnMetadata("y", "", "float", "100", typeof(System.Single), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
private class StatConstDictionary : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>, IPersistable
{
    /// <summary>
    /// Name of the node that contains all the values.
    /// </summary>
    const string _valuesNodeName = "Values";

    /// <summary>
    /// Name of the key for the value's key.
    /// </summary>
    const string _keyKeyName = "Key";

    /// <summary>
    /// Name of the key for the value's value.
    /// </summary>
    const string _valueKeyName = "Value";
    
    /// <summary>
    /// Array that takes in the enum's value (casted to an int) as the array index, and spits out the
    /// corresponding index for the instanced <see name="_values"/> array. This allows us to build an array
    /// of values without wasting any indicies even if the defined enum skips values.
    /// </summary>
    static readonly int[] _enumToValueIndex;

    /// <summary>
    /// Array that takes in the <see cref="_values"/> array index and spits out the enum value that the
    /// index is for. This is to allow for a reverse-lookup on the <see cref="_enumToValueIndex"/>.
    /// </summary>
    static readonly DemoGame.StatType[] _valueIndexToKey;

    /// <summary>
    /// The total number of unique defined enum values. Each instanced <see cref="_values"/> array
    /// will have a length equal to this value.
    /// </summary>
    static readonly int _numEnumValues;

    /// <summary>
    /// Array containing the actual values. The index of this array is found through the value returned
    /// from the _lookupTable.
    /// </summary>
    readonly System.Int32[] _values;

    /// <summary>
    /// Initializes the <see cref="StatConstDictionary"/> class.
    /// </summary>
    static StatConstDictionary()
    {
        _valueIndexToKey = EnumHelper<DemoGame.StatType>.Values.ToArray();
        _numEnumValues = _valueIndexToKey.Length;
        _enumToValueIndex = new int[EnumHelper<DemoGame.StatType>.MaxValue + 1];

        for (int i = 0; i < _valueIndexToKey.Length; i++)
        {
            var key = (int)_valueIndexToKey[i];
            _enumToValueIndex[key] = i;
        }
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StatConstDictionary"/> class.
    /// </summary>
    public StatConstDictionary()
    {
        _values = new int[_numEnumValues];
    }
    
    /// <summary>
    /// Gets or sets an item's value using the <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key for the value to get or set.</param>
    /// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
    public System.Int32 this[DemoGame.StatType key]
    {
        get { return _values[_enumToValueIndex[(int)key]]; }
        set { _values[_enumToValueIndex[(int)key]] = value; }
    }

    #region IEnumerable<KeyValuePair<DemoGame.StatType,System.Int32>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<DemoGame.StatType, System.Int32>> GetEnumerator()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            yield return new KeyValuePair<DemoGame.StatType, System.Int32>(_valueIndexToKey[i], _values[i]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    /// <summary>
    /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
    /// same order as they were written.
    /// </summary>
    /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
    public void ReadState(IValueReader reader)
    {
        // Zero all the existing values
        for (int i = 0; i < _values.Length; i++)
            _values[i] = default(System.Int32);

        // Read and set the values
        var values = reader.ReadManyNodes<KeyValuePair<DemoGame.StatType, System.Int32>>(_valuesNodeName, ReadValueHandler);
        foreach (var value in values)
        {
            this[value.Key] = value.Value;
        }
    }

    /// <summary>
    /// Writes the state of the object to an <see cref="IValueWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
    public void WriteState(IValueWriter writer)
    {
        writer.WriteManyNodes(_valuesNodeName, this.Where(x => x.Value != default(System.Int32)), WriteValueHandler);
    }

    /// <summary>
    /// Reads a <see cref="KeyValuePair{Key, Value}"/>.
    /// </summary>
    /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
    /// <returns>The read <see cref="KeyValuePair{Key, Value}"/>.</returns>
    static KeyValuePair<DemoGame.StatType, System.Int32> ReadValueHandler(IValueReader reader)
    {
        var key = reader.ReadEnum<DemoGame.StatType>(_keyKeyName);
        var value = reader.ReadInt(_valueKeyName);
        return new KeyValuePair<DemoGame.StatType, System.Int32>(key, value);
    }

    /// <summary>
    /// Writes a <see cref="KeyValuePair{Key, Value}"/>.
    /// </summary>
    /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
    /// <param name="value">The <see cref="KeyValuePair{Key, Value}"/> to write.</param>
    static void WriteValueHandler(IValueWriter writer, KeyValuePair<DemoGame.StatType, System.Int32> value)
    {
        writer.WriteEnum(_keyKeyName, value.Key);
        writer.Write(_valueKeyName, value.Value);
    }
}
}

}
