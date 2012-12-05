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
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `view_user_character`.
/// </summary>
public interface IViewUserCharacterTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IViewUserCharacterTable DeepCopy();

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
DemoGame.BodyID BodyID
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
System.Nullable<NetGore.Features.NPCChat.NPCChatDialogID> ChatDialog
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
System.Int16 Level
{
get;
}
/// <summary>
/// Gets the value of the database column `load_map_id`.
/// </summary>
NetGore.World.MapID LoadMapID
{
get;
}
/// <summary>
/// Gets the value of the database column `load_x`.
/// </summary>
System.UInt16 LoadX
{
get;
}
/// <summary>
/// Gets the value of the database column `load_y`.
/// </summary>
System.UInt16 LoadY
{
get;
}
/// <summary>
/// Gets the value of the database column `move_speed`.
/// </summary>
System.UInt16 MoveSpeed
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
/// Gets the value of the database column `respawn_map_id`.
/// </summary>
System.Nullable<NetGore.World.MapID> RespawnMapID
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
}

}
