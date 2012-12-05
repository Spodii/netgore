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
/// Interface for a class that can be used to serialize values to the database table `character`.
/// </summary>
public interface ICharacterTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
ICharacterTable DeepCopy();

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
DemoGame.CharacterID ID
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
/// Gets the value of the database column in the column collection `Stat`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetStat(DemoGame.StatType key);

/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get;
}
}

}
