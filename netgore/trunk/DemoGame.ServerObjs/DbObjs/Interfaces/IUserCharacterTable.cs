using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `user_character`.
/// </summary>
public interface IUserCharacterTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IUserCharacterTable DeepCopy();

/// <summary>
/// Gets the value of the database column `acc`.
/// </summary>
System.Byte Acc
{
get;
}
/// <summary>
/// Gets the value of the database column `account_id`.
/// </summary>
System.Nullable<DemoGame.Server.AccountID> AccountID
{
get;
}
/// <summary>
/// Gets the value of the database column `agi`.
/// </summary>
System.Byte Agi
{
get;
}
/// <summary>
/// Gets the value of the database column `armor`.
/// </summary>
System.Byte Armor
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
/// Gets the value of the database column `bra`.
/// </summary>
System.Byte Bra
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
/// Gets the value of the database column `chat_dialog`.
/// </summary>
System.Nullable<System.UInt16> ChatDialog
{
get;
}
/// <summary>
/// Gets the value of the database column `defence`.
/// </summary>
System.Byte Defence
{
get;
}
/// <summary>
/// Gets the value of the database column `dex`.
/// </summary>
System.Byte Dex
{
get;
}
/// <summary>
/// Gets the value of the database column `evade`.
/// </summary>
System.Byte Evade
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
DemoGame.SPValueType HP
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
System.Int32 ID
{
get;
}
/// <summary>
/// Gets the value of the database column `imm`.
/// </summary>
System.Byte Imm
{
get;
}
/// <summary>
/// Gets the value of the database column `int`.
/// </summary>
System.Byte Int
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
/// Gets the value of the database column `maxhit`.
/// </summary>
System.Byte MaxHit
{
get;
}
/// <summary>
/// Gets the value of the database column `maxhp`.
/// </summary>
System.Int16 MaxHP
{
get;
}
/// <summary>
/// Gets the value of the database column `maxmp`.
/// </summary>
System.Int16 MaxMP
{
get;
}
/// <summary>
/// Gets the value of the database column `minhit`.
/// </summary>
System.Byte MinHit
{
get;
}
/// <summary>
/// Gets the value of the database column `mp`.
/// </summary>
DemoGame.SPValueType MP
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
/// Gets the value of the database column `perc`.
/// </summary>
System.Byte Perc
{
get;
}
/// <summary>
/// Gets the value of the database column `recov`.
/// </summary>
System.Byte Recov
{
get;
}
/// <summary>
/// Gets the value of the database column `regen`.
/// </summary>
System.Byte Regen
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
/// Gets the value of the database column `str`.
/// </summary>
System.Byte Str
{
get;
}
/// <summary>
/// Gets the value of the database column `tact`.
/// </summary>
System.Byte Tact
{
get;
}
/// <summary>
/// Gets the value of the database column `ws`.
/// </summary>
System.Byte WS
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

}
