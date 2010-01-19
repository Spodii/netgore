using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `game_constant`.
/// </summary>
public interface IGameConstantTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IGameConstantTable DeepCopy();

/// <summary>
/// Gets the value of the database column `max_account_name_length`.
/// </summary>
System.Byte MaxAccountNameLength
{
get;
}
/// <summary>
/// Gets the value of the database column `max_account_password_length`.
/// </summary>
System.Byte MaxAccountPasswordLength
{
get;
}
/// <summary>
/// Gets the value of the database column `max_characters_per_account`.
/// </summary>
System.Byte MaxCharactersPerAccount
{
get;
}
/// <summary>
/// Gets the value of the database column `max_character_name_length`.
/// </summary>
System.Byte MaxCharacterNameLength
{
get;
}
/// <summary>
/// Gets the value of the database column `max_inventory_size`.
/// </summary>
System.Byte MaxInventorySize
{
get;
}
/// <summary>
/// Gets the value of the database column `max_shop_items`.
/// </summary>
System.Byte MaxShopItems
{
get;
}
/// <summary>
/// Gets the value of the database column `max_status_effect_power`.
/// </summary>
System.UInt16 MaxStatusEffectPower
{
get;
}
/// <summary>
/// Gets the value of the database column `min_account_name_length`.
/// </summary>
System.Byte MinAccountNameLength
{
get;
}
/// <summary>
/// Gets the value of the database column `min_account_password_length`.
/// </summary>
System.Byte MinAccountPasswordLength
{
get;
}
/// <summary>
/// Gets the value of the database column `min_character_name_length`.
/// </summary>
System.Byte MinCharacterNameLength
{
get;
}
/// <summary>
/// Gets the value of the database column `screen_height`.
/// </summary>
System.UInt16 ScreenHeight
{
get;
}
/// <summary>
/// Gets the value of the database column `screen_width`.
/// </summary>
System.UInt16 ScreenWidth
{
get;
}
/// <summary>
/// Gets the value of the database column `server_ip`.
/// </summary>
System.String ServerIp
{
get;
}
/// <summary>
/// Gets the value of the database column `server_ping_port`.
/// </summary>
System.UInt16 ServerPingPort
{
get;
}
/// <summary>
/// Gets the value of the database column `server_tcp_port`.
/// </summary>
System.UInt16 ServerTcpPort
{
get;
}
/// <summary>
/// Gets the value of the database column `world_physics_update_rate`.
/// </summary>
System.UInt16 WorldPhysicsUpdateRate
{
get;
}
}

}
