/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

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
/// Provides a strongly-typed structure for the database table `view_npc_character`.
/// </summary>
public class ViewNpcCharacterTable : IViewNpcCharacterTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "load_map_id", "load_x", "load_y", "move_speed", "mp", "name", "respawn_map_id", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "load_map_id", "load_x", "load_y", "move_speed", "mp", "name", "respawn_map_id", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };
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
public const System.String TableName = "view_npc_character";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 28;
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
System.Int16 _level;
/// <summary>
/// The field that maps onto the database column `load_map_id`.
/// </summary>
System.UInt16 _loadMapID;
/// <summary>
/// The field that maps onto the database column `load_x`.
/// </summary>
System.UInt16 _loadX;
/// <summary>
/// The field that maps onto the database column `load_y`.
/// </summary>
System.UInt16 _loadY;
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
/// The field that maps onto the database column `respawn_map_id`.
/// </summary>
System.Nullable<System.UInt16> _respawnMapID;
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
/// Gets or sets the value for the field that maps onto the database column `ai_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The AI used by this character. Null for no AI (does nothing, or is user-controller). Intended for NPCs only.".
/// </summary>
[System.ComponentModel.Description("The AI used by this character. Null for no AI (does nothing, or is user-controller). Intended for NPCs only.")]
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
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.The database column contains the comment: 
/// "The body to use to display this character.".
/// </summary>
[System.ComponentModel.Description("The body to use to display this character.")]
[NetGore.SyncValueAttribute()]
public DemoGame.BodyID BodyID
{
get
{
return (DemoGame.BodyID)_bodyID;
}
set
{
this._bodyID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `cash`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "Amount of cash.".
/// </summary>
[System.ComponentModel.Description("Amount of cash.")]
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
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The template that this character was created from (not required - mostly for developer reference).".
/// </summary>
[System.ComponentModel.Description("The template that this character was created from (not required - mostly for developer reference).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<DemoGame.CharacterTemplateID> CharacterTemplateID
{
get
{
return (System.Nullable<DemoGame.CharacterTemplateID>)_characterTemplateID;
}
set
{
this._characterTemplateID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `chat_dialog`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The chat dialog that this character displays. Null for no chat. Intended for NPCs only.".
/// </summary>
[System.ComponentModel.Description("The chat dialog that this character displays. Null for no chat. Intended for NPCs only.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID> ChatDialog
{
get
{
return (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)_chatDialog;
}
set
{
this._chatDialog = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `exp`.
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "Experience points.".
/// </summary>
[System.ComponentModel.Description("Experience points.")]
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
/// The underlying database type is `smallint(6)` with the default value of `50`.The database column contains the comment: 
/// "Current health points.".
/// </summary>
[System.ComponentModel.Description("Current health points.")]
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
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "The unique ID of the character.".
/// </summary>
[System.ComponentModel.Description("The unique ID of the character.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "Current level.".
/// </summary>
[System.ComponentModel.Description("Current level.")]
[NetGore.SyncValueAttribute()]
public System.Int16 Level
{
get
{
return (System.Int16)_level;
}
set
{
this._level = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `load_map_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.The database column contains the comment: 
/// "The map to load on (when logging in / being created).".
/// </summary>
[System.ComponentModel.Description("The map to load on (when logging in / being created).")]
[NetGore.SyncValueAttribute()]
public NetGore.World.MapID LoadMapID
{
get
{
return (NetGore.World.MapID)_loadMapID;
}
set
{
this._loadMapID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `load_x`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `512`.The database column contains the comment: 
/// "The x coordinate to load at.".
/// </summary>
[System.ComponentModel.Description("The x coordinate to load at.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 LoadX
{
get
{
return (System.UInt16)_loadX;
}
set
{
this._loadX = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `load_y`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `512`.The database column contains the comment: 
/// "The y coordinate to load at.".
/// </summary>
[System.ComponentModel.Description("The y coordinate to load at.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 LoadY
{
get
{
return (System.UInt16)_loadY;
}
set
{
this._loadY = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `move_speed`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1800`.The database column contains the comment: 
/// "The movement speed of the character.".
/// </summary>
[System.ComponentModel.Description("The movement speed of the character.")]
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
/// The underlying database type is `smallint(6)` with the default value of `50`.The database column contains the comment: 
/// "Current mana points.".
/// </summary>
[System.ComponentModel.Description("Current mana points.")]
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
/// The underlying database type is `varchar(60)`.The database column contains the comment: 
/// "The character's name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.".
/// </summary>
[System.ComponentModel.Description("The character's name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.")]
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
/// Gets or sets the value for the field that maps onto the database column `respawn_map_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.The database column contains the comment: 
/// "The map to respawn on (when null, cannot respawn). Used to reposition character after death.".
/// </summary>
[System.ComponentModel.Description("The map to respawn on (when null, cannot respawn). Used to reposition character after death.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<NetGore.World.MapID> RespawnMapID
{
get
{
return (System.Nullable<NetGore.World.MapID>)_respawnMapID;
}
set
{
this._respawnMapID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn_x`.
/// The underlying database type is `float` with the default value of `512`.The database column contains the comment: 
/// "The x coordinate to respawn at.".
/// </summary>
[System.ComponentModel.Description("The x coordinate to respawn at.")]
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
/// The underlying database type is `float` with the default value of `512`.The database column contains the comment: 
/// "The y coordinate to respawn at.".
/// </summary>
[System.ComponentModel.Description("The y coordinate to respawn at.")]
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
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The shop that this character runs. Null if not a shopkeeper.".
/// </summary>
[System.ComponentModel.Description("The shop that this character runs. Null if not a shopkeeper.")]
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
/// The underlying database type is `int(11)` with the default value of `0`.The database column contains the comment: 
/// "Stat points available to be spent.".
/// </summary>
[System.ComponentModel.Description("Stat points available to be spent.")]
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
/// Gets or sets the value for the field that maps onto the database column `stat_agi`.
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "Agi stat.".
/// </summary>
[System.ComponentModel.Description("Agi stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "Defence stat.".
/// </summary>
[System.ComponentModel.Description("Defence stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "Int stat.".
/// </summary>
[System.ComponentModel.Description("Int stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "MaxHit stat.".
/// </summary>
[System.ComponentModel.Description("MaxHit stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `50`.The database column contains the comment: 
/// "MaxHP stat.".
/// </summary>
[System.ComponentModel.Description("MaxHP stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `50`.The database column contains the comment: 
/// "MaxMP stat.".
/// </summary>
[System.ComponentModel.Description("MaxMP stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "MinHit stat.".
/// </summary>
[System.ComponentModel.Description("MinHit stat.")]
[NetGore.SyncValueAttribute()]
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
/// The underlying database type is `smallint(6)` with the default value of `1`.The database column contains the comment: 
/// "Str stat.".
/// </summary>
[System.ComponentModel.Description("Str stat.")]
[NetGore.SyncValueAttribute()]
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
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IViewNpcCharacterTable DeepCopy()
{
return new ViewNpcCharacterTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="ViewNpcCharacterTable"/> class.
/// </summary>
public ViewNpcCharacterTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="ViewNpcCharacterTable"/> class.
/// </summary>
/// <param name="aIID">The initial value for the corresponding property.</param>
/// <param name="bodyID">The initial value for the corresponding property.</param>
/// <param name="cash">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="chatDialog">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="hP">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="loadMapID">The initial value for the corresponding property.</param>
/// <param name="loadX">The initial value for the corresponding property.</param>
/// <param name="loadY">The initial value for the corresponding property.</param>
/// <param name="moveSpeed">The initial value for the corresponding property.</param>
/// <param name="mP">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="respawnMapID">The initial value for the corresponding property.</param>
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
public ViewNpcCharacterTable(System.Nullable<NetGore.AI.AIID> @aIID, DemoGame.BodyID @bodyID, System.Int32 @cash, System.Nullable<DemoGame.CharacterTemplateID> @characterTemplateID, System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID> @chatDialog, System.Int32 @exp, DemoGame.SPValueType @hP, System.Int32 @iD, System.Int16 @level, NetGore.World.MapID @loadMapID, System.UInt16 @loadX, System.UInt16 @loadY, System.UInt16 @moveSpeed, DemoGame.SPValueType @mP, System.String @name, System.Nullable<NetGore.World.MapID> @respawnMapID, System.Single @respawnX, System.Single @respawnY, System.Nullable<NetGore.Features.Shops.ShopID> @shopID, System.Int32 @statPoints, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statStr)
{
this.AIID = (System.Nullable<NetGore.AI.AIID>)@aIID;
this.BodyID = (DemoGame.BodyID)@bodyID;
this.Cash = (System.Int32)@cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)@characterTemplateID;
this.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)@chatDialog;
this.Exp = (System.Int32)@exp;
this.HP = (DemoGame.SPValueType)@hP;
this.ID = (System.Int32)@iD;
this.Level = (System.Int16)@level;
this.LoadMapID = (NetGore.World.MapID)@loadMapID;
this.LoadX = (System.UInt16)@loadX;
this.LoadY = (System.UInt16)@loadY;
this.MoveSpeed = (System.UInt16)@moveSpeed;
this.MP = (DemoGame.SPValueType)@mP;
this.Name = (System.String)@name;
this.RespawnMapID = (System.Nullable<NetGore.World.MapID>)@respawnMapID;
this.RespawnX = (System.Single)@respawnX;
this.RespawnY = (System.Single)@respawnY;
this.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)@shopID;
this.StatPoints = (System.Int32)@statPoints;
this.StatAgi = (System.Int16)@statAgi;
this.StatDefence = (System.Int16)@statDefence;
this.StatInt = (System.Int16)@statInt;
this.StatMaxhit = (System.Int16)@statMaxhit;
this.StatMaxhp = (System.Int16)@statMaxhp;
this.StatMaxmp = (System.Int16)@statMaxmp;
this.StatMinhit = (System.Int16)@statMinhit;
this.StatStr = (System.Int16)@statStr;
}
/// <summary>
/// Initializes a new instance of the <see cref="ViewNpcCharacterTable"/> class.
/// </summary>
/// <param name="source">IViewNpcCharacterTable to copy the initial values from.</param>
public ViewNpcCharacterTable(IViewNpcCharacterTable source)
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
public static void CopyValues(IViewNpcCharacterTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["ai_id"] = (System.Nullable<NetGore.AI.AIID>)source.AIID;
dic["body_id"] = (DemoGame.BodyID)source.BodyID;
dic["cash"] = (System.Int32)source.Cash;
dic["character_template_id"] = (System.Nullable<DemoGame.CharacterTemplateID>)source.CharacterTemplateID;
dic["chat_dialog"] = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)source.ChatDialog;
dic["exp"] = (System.Int32)source.Exp;
dic["hp"] = (DemoGame.SPValueType)source.HP;
dic["id"] = (System.Int32)source.ID;
dic["level"] = (System.Int16)source.Level;
dic["load_map_id"] = (NetGore.World.MapID)source.LoadMapID;
dic["load_x"] = (System.UInt16)source.LoadX;
dic["load_y"] = (System.UInt16)source.LoadY;
dic["move_speed"] = (System.UInt16)source.MoveSpeed;
dic["mp"] = (DemoGame.SPValueType)source.MP;
dic["name"] = (System.String)source.Name;
dic["respawn_map_id"] = (System.Nullable<NetGore.World.MapID>)source.RespawnMapID;
dic["respawn_x"] = (System.Single)source.RespawnX;
dic["respawn_y"] = (System.Single)source.RespawnY;
dic["shop_id"] = (System.Nullable<NetGore.Features.Shops.ShopID>)source.ShopID;
dic["statpoints"] = (System.Int32)source.StatPoints;
dic["stat_agi"] = (System.Int16)source.StatAgi;
dic["stat_defence"] = (System.Int16)source.StatDefence;
dic["stat_int"] = (System.Int16)source.StatInt;
dic["stat_maxhit"] = (System.Int16)source.StatMaxhit;
dic["stat_maxhp"] = (System.Int16)source.StatMaxhp;
dic["stat_maxmp"] = (System.Int16)source.StatMaxmp;
dic["stat_minhit"] = (System.Int16)source.StatMinhit;
dic["stat_str"] = (System.Int16)source.StatStr;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this ViewNpcCharacterTable.
/// </summary>
/// <param name="source">The IViewNpcCharacterTable to copy the values from.</param>
public void CopyValuesFrom(IViewNpcCharacterTable source)
{
this.AIID = (System.Nullable<NetGore.AI.AIID>)source.AIID;
this.BodyID = (DemoGame.BodyID)source.BodyID;
this.Cash = (System.Int32)source.Cash;
this.CharacterTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)source.CharacterTemplateID;
this.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)source.ChatDialog;
this.Exp = (System.Int32)source.Exp;
this.HP = (DemoGame.SPValueType)source.HP;
this.ID = (System.Int32)source.ID;
this.Level = (System.Int16)source.Level;
this.LoadMapID = (NetGore.World.MapID)source.LoadMapID;
this.LoadX = (System.UInt16)source.LoadX;
this.LoadY = (System.UInt16)source.LoadY;
this.MoveSpeed = (System.UInt16)source.MoveSpeed;
this.MP = (DemoGame.SPValueType)source.MP;
this.Name = (System.String)source.Name;
this.RespawnMapID = (System.Nullable<NetGore.World.MapID>)source.RespawnMapID;
this.RespawnX = (System.Single)source.RespawnX;
this.RespawnY = (System.Single)source.RespawnY;
this.ShopID = (System.Nullable<NetGore.Features.Shops.ShopID>)source.ShopID;
this.StatPoints = (System.Int32)source.StatPoints;
this.StatAgi = (System.Int16)source.StatAgi;
this.StatDefence = (System.Int16)source.StatDefence;
this.StatInt = (System.Int16)source.StatInt;
this.StatMaxhit = (System.Int16)source.StatMaxhit;
this.StatMaxhp = (System.Int16)source.StatMaxhp;
this.StatMaxmp = (System.Int16)source.StatMaxmp;
this.StatMinhit = (System.Int16)source.StatMinhit;
this.StatStr = (System.Int16)source.StatStr;
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

case "load_map_id":
return LoadMapID;

case "load_x":
return LoadX;

case "load_y":
return LoadY;

case "move_speed":
return MoveSpeed;

case "mp":
return MP;

case "name":
return Name;

case "respawn_map_id":
return RespawnMapID;

case "respawn_x":
return RespawnX;

case "respawn_y":
return RespawnY;

case "shop_id":
return ShopID;

case "statpoints":
return StatPoints;

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
case "ai_id":
this.AIID = (System.Nullable<NetGore.AI.AIID>)value;
break;

case "body_id":
this.BodyID = (DemoGame.BodyID)value;
break;

case "cash":
this.Cash = (System.Int32)value;
break;

case "character_template_id":
this.CharacterTemplateID = (System.Nullable<DemoGame.CharacterTemplateID>)value;
break;

case "chat_dialog":
this.ChatDialog = (System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID>)value;
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
this.Level = (System.Int16)value;
break;

case "load_map_id":
this.LoadMapID = (NetGore.World.MapID)value;
break;

case "load_x":
this.LoadX = (System.UInt16)value;
break;

case "load_y":
this.LoadY = (System.UInt16)value;
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

case "respawn_map_id":
this.RespawnMapID = (System.Nullable<NetGore.World.MapID>)value;
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
case "ai_id":
return new ColumnMetadata("ai_id", "The AI used by this character. Null for no AI (does nothing, or is user-controller). Intended for NPCs only.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "body_id":
return new ColumnMetadata("body_id", "The body to use to display this character.", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "cash":
return new ColumnMetadata("cash", "Amount of cash.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "The template that this character was created from (not required - mostly for developer reference).", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "chat_dialog":
return new ColumnMetadata("chat_dialog", "The chat dialog that this character displays. Null for no chat. Intended for NPCs only.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "exp":
return new ColumnMetadata("exp", "Experience points.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "hp":
return new ColumnMetadata("hp", "Current health points.", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "id":
return new ColumnMetadata("id", "The unique ID of the character.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "level":
return new ColumnMetadata("level", "Current level.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "load_map_id":
return new ColumnMetadata("load_map_id", "The map to load on (when logging in / being created).", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "load_x":
return new ColumnMetadata("load_x", "The x coordinate to load at.", "smallint(5) unsigned", "512", typeof(System.UInt16), false, false, false);

case "load_y":
return new ColumnMetadata("load_y", "The y coordinate to load at.", "smallint(5) unsigned", "512", typeof(System.UInt16), false, false, false);

case "move_speed":
return new ColumnMetadata("move_speed", "The movement speed of the character.", "smallint(5) unsigned", "1800", typeof(System.UInt16), false, false, false);

case "mp":
return new ColumnMetadata("mp", "Current mana points.", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "name":
return new ColumnMetadata("name", "The character's name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.", "varchar(60)", "", typeof(System.String), false, false, false);

case "respawn_map_id":
return new ColumnMetadata("respawn_map_id", "The map to respawn on (when null, cannot respawn). Used to reposition character after death.", "smallint(5) unsigned", "1", typeof(System.Nullable<System.UInt16>), true, false, false);

case "respawn_x":
return new ColumnMetadata("respawn_x", "The x coordinate to respawn at.", "float", "512", typeof(System.Single), false, false, false);

case "respawn_y":
return new ColumnMetadata("respawn_y", "The y coordinate to respawn at.", "float", "512", typeof(System.Single), false, false, false);

case "shop_id":
return new ColumnMetadata("shop_id", "The shop that this character runs. Null if not a shopkeeper.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "statpoints":
return new ColumnMetadata("statpoints", "Stat points available to be spent.", "int(11)", "0", typeof(System.Int32), false, false, false);

case "stat_agi":
return new ColumnMetadata("stat_agi", "Agi stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_defence":
return new ColumnMetadata("stat_defence", "Defence stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_int":
return new ColumnMetadata("stat_int", "Int stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhit":
return new ColumnMetadata("stat_maxhit", "MaxHit stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhp":
return new ColumnMetadata("stat_maxhp", "MaxHP stat.", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_maxmp":
return new ColumnMetadata("stat_maxmp", "MaxMP stat.", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_minhit":
return new ColumnMetadata("stat_minhit", "MinHit stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_str":
return new ColumnMetadata("stat_str", "Str stat.", "smallint(6)", "1", typeof(System.Int16), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
