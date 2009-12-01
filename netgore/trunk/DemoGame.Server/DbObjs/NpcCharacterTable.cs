using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `npc_character`.
/// </summary>
public class NpcCharacterTable : INpcCharacterTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "map_id", "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "statpoints", "x", "y" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "map_id", "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "statpoints", "x", "y" };
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
public const System.String TableName = "npc_character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 28;
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
/// The field that maps onto the database column `stat_agi`.
/// </summary>
System.Int16 _statAgi;
/// <summary>
/// The field that maps onto the database column `stat_defence`.
/// </summary>
System.Int16 _statDefence;
/// <summary>
/// The field that maps onto the database column `stat_int`.
/// </summary>
System.Int16 _statInt;
/// <summary>
/// The field that maps onto the database column `stat_maxhit`.
/// </summary>
System.Int16 _statMaxhit;
/// <summary>
/// The field that maps onto the database column `stat_maxhp`.
/// </summary>
System.Int16 _statMaxhp;
/// <summary>
/// The field that maps onto the database column `stat_maxmp`.
/// </summary>
System.Int16 _statMaxmp;
/// <summary>
/// The field that maps onto the database column `stat_minhit`.
/// </summary>
System.Int16 _statMinhit;
/// <summary>
/// The field that maps onto the database column `stat_str`.
/// </summary>
System.Int16 _statStr;
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.Int32 _statPoints;
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
/// Gets or sets the value for the field that maps onto the database column `exp`.
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
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
/// Gets or sets the value for the field that maps onto the database column `shop_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<DemoGame.Server.ShopID> ShopID
{
get
{
return (System.Nullable<DemoGame.Server.ShopID>)_shopID;
}
set
{
this._shopID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_agi`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatAgi
{
get
{
return (System.Int16)_statAgi;
}
set
{
this._statAgi = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_defence`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatDefence
{
get
{
return (System.Int16)_statDefence;
}
set
{
this._statDefence = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_int`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatInt
{
get
{
return (System.Int16)_statInt;
}
set
{
this._statInt = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_maxhit`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatMaxhit
{
get
{
return (System.Int16)_statMaxhit;
}
set
{
this._statMaxhit = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_maxhp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 StatMaxhp
{
get
{
return (System.Int16)_statMaxhp;
}
set
{
this._statMaxhp = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_maxmp`.
/// The underlying database type is `smallint(6)` with the default value of `50`.
/// </summary>
public System.Int16 StatMaxmp
{
get
{
return (System.Int16)_statMaxmp;
}
set
{
this._statMaxmp = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_minhit`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatMinhit
{
get
{
return (System.Int16)_statMinhit;
}
set
{
this._statMinhit = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `stat_str`.
/// The underlying database type is `smallint(6)` with the default value of `1`.
/// </summary>
public System.Int16 StatStr
{
get
{
return (System.Int16)_statStr;
}
set
{
this._statStr = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
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
public INpcCharacterTable DeepCopy()
{
return new NpcCharacterTable(this);
}
/// <summary>
/// NpcCharacterTable constructor.
/// </summary>
public NpcCharacterTable()
{
}
/// <summary>
/// NpcCharacterTable constructor.
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
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="respawnMap">The initial value for the corresponding property.</param>
/// <param name="respawnX">The initial value for the corresponding property.</param>
/// <param name="respawnY">The initial value for the corresponding property.</param>
/// <param name="shopID">The initial value for the corresponding property.</param>
/// <param name="statAgi">The initial value for the corresponding property.</param>
/// <param name="statDefence">The initial value for the corresponding property.</param>
/// <param name="statInt">The initial value for the corresponding property.</param>
/// <param name="statMaxhit">The initial value for the corresponding property.</param>
/// <param name="statMaxhp">The initial value for the corresponding property.</param>
/// <param name="statMaxmp">The initial value for the corresponding property.</param>
/// <param name="statMinhit">The initial value for the corresponding property.</param>
/// <param name="statStr">The initial value for the corresponding property.</param>
/// <param name="statPoints">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public NpcCharacterTable(System.Nullable<DemoGame.Server.AccountID> @accountID, System.Nullable<NetGore.AI.AIID> @aIID, DemoGame.BodyIndex @bodyID, System.Int32 @cash, System.Nullable<DemoGame.Server.CharacterTemplateID> @characterTemplateID, System.Nullable<System.UInt16> @chatDialog, System.Int32 @exp, DemoGame.SPValueType @hP, System.Int32 @iD, System.Byte @level, NetGore.MapIndex @mapID, DemoGame.SPValueType @mP, System.String @name, System.Nullable<NetGore.MapIndex> @respawnMap, System.Single @respawnX, System.Single @respawnY, System.Nullable<DemoGame.Server.ShopID> @shopID, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statStr, System.Int32 @statPoints, System.Single @x, System.Single @y)
{
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)@accountID;
this.AIID = (System.Nullable<NetGore.AI.AIID>)@aIID;
this.BodyID = (DemoGame.BodyIndex)@bodyID;
this.Cash = (System.Int32)@cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)@characterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)@chatDialog;
this.Exp = (System.Int32)@exp;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (System.Int32)@iD;
this.Level = (System.Byte)@level;
this.MapID = (NetGore.MapIndex)@mapID;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)@respawnMap;
this.RespawnX = (System.Single)@respawnX;
this.RespawnY = (System.Single)@respawnY;
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)@shopID;
this.StatAgi = (System.Int16)@statAgi;
this.StatDefence = (System.Int16)@statDefence;
this.StatInt = (System.Int16)@statInt;
this.StatMaxhit = (System.Int16)@statMaxhit;
this.StatMaxhp = (System.Int16)@statMaxhp;
this.StatMaxmp = (System.Int16)@statMaxmp;
this.StatMinhit = (System.Int16)@statMinhit;
this.StatStr = (System.Int16)@statStr;
this.StatPoints = (System.Int32)@statPoints;
this.X = (System.Single)@x;
this.Y = (System.Single)@y;
}
/// <summary>
/// NpcCharacterTable constructor.
/// </summary>
/// <param name="source">INpcCharacterTable to copy the initial values from.</param>
public NpcCharacterTable(INpcCharacterTable source)
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
public static void CopyValues(INpcCharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@account_id"] = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
dic["@ai_id"] = (System.Nullable<NetGore.AI.AIID>)source.AIID;
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@cash"] = (System.Int32)source.Cash;
dic["@character_template_id"] = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
dic["@chat_dialog"] = (System.Nullable<System.UInt16>)source.ChatDialog;
dic["@exp"] = (System.Int32)source.Exp;
dic["@hp"] = (DemoGame.SPValueType)source.HP;
dic["@id"] = (System.Int32)source.ID;
dic["@level"] = (System.Byte)source.Level;
dic["@map_id"] = (NetGore.MapIndex)source.MapID;
dic["@mp"] = (DemoGame.SPValueType)source.MP;
dic["@name"] = (System.String)source.Name;
dic["@respawn_map"] = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
dic["@respawn_x"] = (System.Single)source.RespawnX;
dic["@respawn_y"] = (System.Single)source.RespawnY;
dic["@shop_id"] = (System.Nullable<DemoGame.Server.ShopID>)source.ShopID;
dic["@stat_agi"] = (System.Int16)source.StatAgi;
dic["@stat_defence"] = (System.Int16)source.StatDefence;
dic["@stat_int"] = (System.Int16)source.StatInt;
dic["@stat_maxhit"] = (System.Int16)source.StatMaxhit;
dic["@stat_maxhp"] = (System.Int16)source.StatMaxhp;
dic["@stat_maxmp"] = (System.Int16)source.StatMaxmp;
dic["@stat_minhit"] = (System.Int16)source.StatMinhit;
dic["@stat_str"] = (System.Int16)source.StatStr;
dic["@statpoints"] = (System.Int32)source.StatPoints;
dic["@x"] = (System.Single)source.X;
dic["@y"] = (System.Single)source.Y;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this NpcCharacterTable.
/// </summary>
/// <param name="source">The INpcCharacterTable to copy the values from.</param>
public void CopyValuesFrom(INpcCharacterTable source)
{
this.AccountID = (System.Nullable<DemoGame.Server.AccountID>)source.AccountID;
this.AIID = (System.Nullable<NetGore.AI.AIID>)source.AIID;
this.BodyID = (DemoGame.BodyIndex)source.BodyID;
this.Cash = (System.Int32)source.Cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.Server.CharacterTemplateID>)source.CharacterTemplateID;
this.ChatDialog = (System.Nullable<System.UInt16>)source.ChatDialog;
this.Exp = (System.Int32)source.Exp;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (System.Int32)source.ID;
this.Level = (System.Byte)source.Level;
this.MapID = (NetGore.MapIndex)source.MapID;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.RespawnMap = (System.Nullable<NetGore.MapIndex>)source.RespawnMap;
this.RespawnX = (System.Single)source.RespawnX;
this.RespawnY = (System.Single)source.RespawnY;
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)source.ShopID;
this.StatAgi = (System.Int16)source.StatAgi;
this.StatDefence = (System.Int16)source.StatDefence;
this.StatInt = (System.Int16)source.StatInt;
this.StatMaxhit = (System.Int16)source.StatMaxhit;
this.StatMaxhp = (System.Int16)source.StatMaxhp;
this.StatMaxmp = (System.Int16)source.StatMaxmp;
this.StatMinhit = (System.Int16)source.StatMinhit;
this.StatStr = (System.Int16)source.StatStr;
this.StatPoints = (System.Int32)source.StatPoints;
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

case "stat_agi":
return StatAgi;

case "stat_defence":
return StatDefence;

case "stat_int":
return StatInt;

case "stat_maxhit":
return StatMaxhit;

case "stat_maxhp":
return StatMaxhp;

case "stat_maxmp":
return StatMaxmp;

case "stat_minhit":
return StatMinhit;

case "stat_str":
return StatStr;

case "statpoints":
return StatPoints;

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
this.ID = (System.Int32)value;
break;

case "level":
this.Level = (System.Byte)value;
break;

case "map_id":
this.MapID = (NetGore.MapIndex)value;
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
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)value;
break;

case "stat_agi":
this.StatAgi = (System.Int16)value;
break;

case "stat_defence":
this.StatDefence = (System.Int16)value;
break;

case "stat_int":
this.StatInt = (System.Int16)value;
break;

case "stat_maxhit":
this.StatMaxhit = (System.Int16)value;
break;

case "stat_maxhp":
this.StatMaxhp = (System.Int16)value;
break;

case "stat_maxmp":
this.StatMaxmp = (System.Int16)value;
break;

case "stat_minhit":
this.StatMinhit = (System.Int16)value;
break;

case "stat_str":
this.StatStr = (System.Int16)value;
break;

case "statpoints":
this.StatPoints = (System.Int32)value;
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
return new ColumnMetadata("account_id", "", "int(11)", null, typeof(System.Nullable<System.Int32>), true, false, false);

case "ai_id":
return new ColumnMetadata("ai_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "body_id":
return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "cash":
return new ColumnMetadata("cash", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "chat_dialog":
return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "exp":
return new ColumnMetadata("exp", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "hp":
return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(11)", null, typeof(System.Int32), false, false, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "map_id":
return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "mp":
return new ColumnMetadata("mp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(30)", null, typeof(System.String), false, false, false);

case "respawn_map":
return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "respawn_x":
return new ColumnMetadata("respawn_x", "", "float", "50", typeof(System.Single), false, false, false);

case "respawn_y":
return new ColumnMetadata("respawn_y", "", "float", "50", typeof(System.Single), false, false, false);

case "shop_id":
return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

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

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(11)", "0", typeof(System.Int32), false, false, false);

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
