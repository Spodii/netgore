using System;
using System.Linq;
namespace DemoGame.DbObjs
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
/// Gets the value of the database column `account_id`.
/// </summary>
System.Nullable<DemoGame.AccountID> AccountID
{
get;
}
/// <summary>
/// Gets the value of the database column `ai_id`.
/// </summary>
System.Nullable<NetGore.AI.AIID> AIID
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
System.Int32 Cash
{
get;
}
/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
System.Nullable<DemoGame.CharacterTemplateID> CharacterTemplateID
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
/// Gets the value of the database column `exp`.
/// </summary>
System.Int32 Exp
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
/// Gets the value of the database column `shop_id`.
/// </summary>
System.Nullable<NetGore.Features.Shops.ShopID> ShopID
{
get;
}
/// <summary>
/// Gets the value of the database column `statpoints`.
/// </summary>
System.Int32 StatPoints
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_agi`.
/// </summary>
System.Int16 StatAgi
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_defence`.
/// </summary>
System.Int16 StatDefence
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_int`.
/// </summary>
System.Int16 StatInt
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_maxhit`.
/// </summary>
System.Int16 StatMaxhit
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_maxhp`.
/// </summary>
System.Int16 StatMaxhp
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_maxmp`.
/// </summary>
System.Int16 StatMaxmp
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_minhit`.
/// </summary>
System.Int16 StatMinhit
{
get;
}
/// <summary>
/// Gets the value of the database column `stat_str`.
/// </summary>
System.Int16 StatStr
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
