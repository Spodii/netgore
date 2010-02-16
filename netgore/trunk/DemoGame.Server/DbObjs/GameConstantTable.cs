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
/// Provides a strongly-typed structure for the database table `game_constant`.
/// </summary>
public class GameConstantTable : IGameConstantTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"max_account_name_length", "max_account_password_length", "max_characters_per_account", "max_character_name_length", "max_inventory_size", "max_shop_items", "max_status_effect_power", "min_account_name_length", "min_account_password_length", "min_character_name_length", "screen_height", "screen_width", "server_ip", "server_ping_port", "server_tcp_port", "world_physics_update_rate" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"max_account_name_length", "max_account_password_length", "max_characters_per_account", "max_character_name_length", "max_inventory_size", "max_shop_items", "max_status_effect_power", "min_account_name_length", "min_account_password_length", "min_character_name_length", "screen_height", "screen_width", "server_ip", "server_ping_port", "server_tcp_port", "world_physics_update_rate" };
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
public const System.String TableName = "game_constant";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 16;
/// <summary>
/// The field that maps onto the database column `max_account_name_length`.
/// </summary>
System.Byte _maxAccountNameLength;
/// <summary>
/// The field that maps onto the database column `max_account_password_length`.
/// </summary>
System.Byte _maxAccountPasswordLength;
/// <summary>
/// The field that maps onto the database column `max_characters_per_account`.
/// </summary>
System.Byte _maxCharactersPerAccount;
/// <summary>
/// The field that maps onto the database column `max_character_name_length`.
/// </summary>
System.Byte _maxCharacterNameLength;
/// <summary>
/// The field that maps onto the database column `max_inventory_size`.
/// </summary>
System.Byte _maxInventorySize;
/// <summary>
/// The field that maps onto the database column `max_shop_items`.
/// </summary>
System.Byte _maxShopItems;
/// <summary>
/// The field that maps onto the database column `max_status_effect_power`.
/// </summary>
System.UInt16 _maxStatusEffectPower;
/// <summary>
/// The field that maps onto the database column `min_account_name_length`.
/// </summary>
System.Byte _minAccountNameLength;
/// <summary>
/// The field that maps onto the database column `min_account_password_length`.
/// </summary>
System.Byte _minAccountPasswordLength;
/// <summary>
/// The field that maps onto the database column `min_character_name_length`.
/// </summary>
System.Byte _minCharacterNameLength;
/// <summary>
/// The field that maps onto the database column `screen_height`.
/// </summary>
System.UInt16 _screenHeight;
/// <summary>
/// The field that maps onto the database column `screen_width`.
/// </summary>
System.UInt16 _screenWidth;
/// <summary>
/// The field that maps onto the database column `server_ip`.
/// </summary>
System.String _serverIp;
/// <summary>
/// The field that maps onto the database column `server_ping_port`.
/// </summary>
System.UInt16 _serverPingPort;
/// <summary>
/// The field that maps onto the database column `server_tcp_port`.
/// </summary>
System.UInt16 _serverTcpPort;
/// <summary>
/// The field that maps onto the database column `world_physics_update_rate`.
/// </summary>
System.UInt16 _worldPhysicsUpdateRate;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_account_name_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxAccountNameLength
{
get
{
return (System.Byte)_maxAccountNameLength;
}
set
{
this._maxAccountNameLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_account_password_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxAccountPasswordLength
{
get
{
return (System.Byte)_maxAccountPasswordLength;
}
set
{
this._maxAccountPasswordLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_characters_per_account`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxCharactersPerAccount
{
get
{
return (System.Byte)_maxCharactersPerAccount;
}
set
{
this._maxCharactersPerAccount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_character_name_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxCharacterNameLength
{
get
{
return (System.Byte)_maxCharacterNameLength;
}
set
{
this._maxCharacterNameLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_inventory_size`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxInventorySize
{
get
{
return (System.Byte)_maxInventorySize;
}
set
{
this._maxInventorySize = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_shop_items`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MaxShopItems
{
get
{
return (System.Byte)_maxShopItems;
}
set
{
this._maxShopItems = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `max_status_effect_power`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 MaxStatusEffectPower
{
get
{
return (System.UInt16)_maxStatusEffectPower;
}
set
{
this._maxStatusEffectPower = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `min_account_name_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MinAccountNameLength
{
get
{
return (System.Byte)_minAccountNameLength;
}
set
{
this._minAccountNameLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `min_account_password_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MinAccountPasswordLength
{
get
{
return (System.Byte)_minAccountPasswordLength;
}
set
{
this._minAccountPasswordLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `min_character_name_length`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.Byte MinCharacterNameLength
{
get
{
return (System.Byte)_minCharacterNameLength;
}
set
{
this._minCharacterNameLength = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `screen_height`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 ScreenHeight
{
get
{
return (System.UInt16)_screenHeight;
}
set
{
this._screenHeight = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `screen_width`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 ScreenWidth
{
get
{
return (System.UInt16)_screenWidth;
}
set
{
this._screenWidth = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `server_ip`.
/// The underlying database type is `varchar(15)`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.String ServerIp
{
get
{
return (System.String)_serverIp;
}
set
{
this._serverIp = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `server_ping_port`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 ServerPingPort
{
get
{
return (System.UInt16)_serverPingPort;
}
set
{
this._serverPingPort = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `server_tcp_port`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 ServerTcpPort
{
get
{
return (System.UInt16)_serverTcpPort;
}
set
{
this._serverTcpPort = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `world_physics_update_rate`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt16 WorldPhysicsUpdateRate
{
get
{
return (System.UInt16)_worldPhysicsUpdateRate;
}
set
{
this._worldPhysicsUpdateRate = (System.UInt16)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IGameConstantTable DeepCopy()
{
return new GameConstantTable(this);
}
/// <summary>
/// GameConstantTable constructor.
/// </summary>
public GameConstantTable()
{
}
/// <summary>
/// GameConstantTable constructor.
/// </summary>
/// <param name="maxAccountNameLength">The initial value for the corresponding property.</param>
/// <param name="maxAccountPasswordLength">The initial value for the corresponding property.</param>
/// <param name="maxCharactersPerAccount">The initial value for the corresponding property.</param>
/// <param name="maxCharacterNameLength">The initial value for the corresponding property.</param>
/// <param name="maxInventorySize">The initial value for the corresponding property.</param>
/// <param name="maxShopItems">The initial value for the corresponding property.</param>
/// <param name="maxStatusEffectPower">The initial value for the corresponding property.</param>
/// <param name="minAccountNameLength">The initial value for the corresponding property.</param>
/// <param name="minAccountPasswordLength">The initial value for the corresponding property.</param>
/// <param name="minCharacterNameLength">The initial value for the corresponding property.</param>
/// <param name="screenHeight">The initial value for the corresponding property.</param>
/// <param name="screenWidth">The initial value for the corresponding property.</param>
/// <param name="serverIp">The initial value for the corresponding property.</param>
/// <param name="serverPingPort">The initial value for the corresponding property.</param>
/// <param name="serverTcpPort">The initial value for the corresponding property.</param>
/// <param name="worldPhysicsUpdateRate">The initial value for the corresponding property.</param>
public GameConstantTable(System.Byte @maxAccountNameLength, System.Byte @maxAccountPasswordLength, System.Byte @maxCharactersPerAccount, System.Byte @maxCharacterNameLength, System.Byte @maxInventorySize, System.Byte @maxShopItems, System.UInt16 @maxStatusEffectPower, System.Byte @minAccountNameLength, System.Byte @minAccountPasswordLength, System.Byte @minCharacterNameLength, System.UInt16 @screenHeight, System.UInt16 @screenWidth, System.String @serverIp, System.UInt16 @serverPingPort, System.UInt16 @serverTcpPort, System.UInt16 @worldPhysicsUpdateRate)
{
this.MaxAccountNameLength = (System.Byte)@maxAccountNameLength;
this.MaxAccountPasswordLength = (System.Byte)@maxAccountPasswordLength;
this.MaxCharactersPerAccount = (System.Byte)@maxCharactersPerAccount;
this.MaxCharacterNameLength = (System.Byte)@maxCharacterNameLength;
this.MaxInventorySize = (System.Byte)@maxInventorySize;
this.MaxShopItems = (System.Byte)@maxShopItems;
this.MaxStatusEffectPower = (System.UInt16)@maxStatusEffectPower;
this.MinAccountNameLength = (System.Byte)@minAccountNameLength;
this.MinAccountPasswordLength = (System.Byte)@minAccountPasswordLength;
this.MinCharacterNameLength = (System.Byte)@minCharacterNameLength;
this.ScreenHeight = (System.UInt16)@screenHeight;
this.ScreenWidth = (System.UInt16)@screenWidth;
this.ServerIp = (System.String)@serverIp;
this.ServerPingPort = (System.UInt16)@serverPingPort;
this.ServerTcpPort = (System.UInt16)@serverTcpPort;
this.WorldPhysicsUpdateRate = (System.UInt16)@worldPhysicsUpdateRate;
}
/// <summary>
/// GameConstantTable constructor.
/// </summary>
/// <param name="source">IGameConstantTable to copy the initial values from.</param>
public GameConstantTable(IGameConstantTable source)
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
public static void CopyValues(IGameConstantTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@max_account_name_length"] = (System.Byte)source.MaxAccountNameLength;
dic["@max_account_password_length"] = (System.Byte)source.MaxAccountPasswordLength;
dic["@max_characters_per_account"] = (System.Byte)source.MaxCharactersPerAccount;
dic["@max_character_name_length"] = (System.Byte)source.MaxCharacterNameLength;
dic["@max_inventory_size"] = (System.Byte)source.MaxInventorySize;
dic["@max_shop_items"] = (System.Byte)source.MaxShopItems;
dic["@max_status_effect_power"] = (System.UInt16)source.MaxStatusEffectPower;
dic["@min_account_name_length"] = (System.Byte)source.MinAccountNameLength;
dic["@min_account_password_length"] = (System.Byte)source.MinAccountPasswordLength;
dic["@min_character_name_length"] = (System.Byte)source.MinCharacterNameLength;
dic["@screen_height"] = (System.UInt16)source.ScreenHeight;
dic["@screen_width"] = (System.UInt16)source.ScreenWidth;
dic["@server_ip"] = (System.String)source.ServerIp;
dic["@server_ping_port"] = (System.UInt16)source.ServerPingPort;
dic["@server_tcp_port"] = (System.UInt16)source.ServerTcpPort;
dic["@world_physics_update_rate"] = (System.UInt16)source.WorldPhysicsUpdateRate;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this GameConstantTable.
/// </summary>
/// <param name="source">The IGameConstantTable to copy the values from.</param>
public void CopyValuesFrom(IGameConstantTable source)
{
this.MaxAccountNameLength = (System.Byte)source.MaxAccountNameLength;
this.MaxAccountPasswordLength = (System.Byte)source.MaxAccountPasswordLength;
this.MaxCharactersPerAccount = (System.Byte)source.MaxCharactersPerAccount;
this.MaxCharacterNameLength = (System.Byte)source.MaxCharacterNameLength;
this.MaxInventorySize = (System.Byte)source.MaxInventorySize;
this.MaxShopItems = (System.Byte)source.MaxShopItems;
this.MaxStatusEffectPower = (System.UInt16)source.MaxStatusEffectPower;
this.MinAccountNameLength = (System.Byte)source.MinAccountNameLength;
this.MinAccountPasswordLength = (System.Byte)source.MinAccountPasswordLength;
this.MinCharacterNameLength = (System.Byte)source.MinCharacterNameLength;
this.ScreenHeight = (System.UInt16)source.ScreenHeight;
this.ScreenWidth = (System.UInt16)source.ScreenWidth;
this.ServerIp = (System.String)source.ServerIp;
this.ServerPingPort = (System.UInt16)source.ServerPingPort;
this.ServerTcpPort = (System.UInt16)source.ServerTcpPort;
this.WorldPhysicsUpdateRate = (System.UInt16)source.WorldPhysicsUpdateRate;
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
case "max_account_name_length":
return MaxAccountNameLength;

case "max_account_password_length":
return MaxAccountPasswordLength;

case "max_characters_per_account":
return MaxCharactersPerAccount;

case "max_character_name_length":
return MaxCharacterNameLength;

case "max_inventory_size":
return MaxInventorySize;

case "max_shop_items":
return MaxShopItems;

case "max_status_effect_power":
return MaxStatusEffectPower;

case "min_account_name_length":
return MinAccountNameLength;

case "min_account_password_length":
return MinAccountPasswordLength;

case "min_character_name_length":
return MinCharacterNameLength;

case "screen_height":
return ScreenHeight;

case "screen_width":
return ScreenWidth;

case "server_ip":
return ServerIp;

case "server_ping_port":
return ServerPingPort;

case "server_tcp_port":
return ServerTcpPort;

case "world_physics_update_rate":
return WorldPhysicsUpdateRate;

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
case "max_account_name_length":
this.MaxAccountNameLength = (System.Byte)value;
break;

case "max_account_password_length":
this.MaxAccountPasswordLength = (System.Byte)value;
break;

case "max_characters_per_account":
this.MaxCharactersPerAccount = (System.Byte)value;
break;

case "max_character_name_length":
this.MaxCharacterNameLength = (System.Byte)value;
break;

case "max_inventory_size":
this.MaxInventorySize = (System.Byte)value;
break;

case "max_shop_items":
this.MaxShopItems = (System.Byte)value;
break;

case "max_status_effect_power":
this.MaxStatusEffectPower = (System.UInt16)value;
break;

case "min_account_name_length":
this.MinAccountNameLength = (System.Byte)value;
break;

case "min_account_password_length":
this.MinAccountPasswordLength = (System.Byte)value;
break;

case "min_character_name_length":
this.MinCharacterNameLength = (System.Byte)value;
break;

case "screen_height":
this.ScreenHeight = (System.UInt16)value;
break;

case "screen_width":
this.ScreenWidth = (System.UInt16)value;
break;

case "server_ip":
this.ServerIp = (System.String)value;
break;

case "server_ping_port":
this.ServerPingPort = (System.UInt16)value;
break;

case "server_tcp_port":
this.ServerTcpPort = (System.UInt16)value;
break;

case "world_physics_update_rate":
this.WorldPhysicsUpdateRate = (System.UInt16)value;
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
case "max_account_name_length":
return new ColumnMetadata("max_account_name_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_account_password_length":
return new ColumnMetadata("max_account_password_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_characters_per_account":
return new ColumnMetadata("max_characters_per_account", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_character_name_length":
return new ColumnMetadata("max_character_name_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_inventory_size":
return new ColumnMetadata("max_inventory_size", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_shop_items":
return new ColumnMetadata("max_shop_items", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "max_status_effect_power":
return new ColumnMetadata("max_status_effect_power", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "min_account_name_length":
return new ColumnMetadata("min_account_name_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "min_account_password_length":
return new ColumnMetadata("min_account_password_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "min_character_name_length":
return new ColumnMetadata("min_character_name_length", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "screen_height":
return new ColumnMetadata("screen_height", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "screen_width":
return new ColumnMetadata("screen_width", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "server_ip":
return new ColumnMetadata("server_ip", "", "varchar(15)", null, typeof(System.String), false, false, false);

case "server_ping_port":
return new ColumnMetadata("server_ping_port", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "server_tcp_port":
return new ColumnMetadata("server_tcp_port", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "world_physics_update_rate":
return new ColumnMetadata("world_physics_update_rate", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

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

}

}
