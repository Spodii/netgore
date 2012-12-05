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
/// Interface for a class that can be used to serialize values to the database table `world_stats_user_kill_npc`.
/// </summary>
public interface IWorldStatsUserKillNpcTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IWorldStatsUserKillNpcTable DeepCopy();

/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
System.UInt32 ID
{
get;
}
/// <summary>
/// Gets the value of the database column `map_id`.
/// </summary>
System.Nullable<NetGore.World.MapID> MapID
{
get;
}
/// <summary>
/// Gets the value of the database column `npc_template_id`.
/// </summary>
System.Nullable<DemoGame.CharacterTemplateID> NPCTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `npc_x`.
/// </summary>
System.UInt16 NpcX
{
get;
}
/// <summary>
/// Gets the value of the database column `npc_y`.
/// </summary>
System.UInt16 NpcY
{
get;
}
/// <summary>
/// Gets the value of the database column `user_id`.
/// </summary>
DemoGame.CharacterID UserID
{
get;
}
/// <summary>
/// Gets the value of the database column `user_level`.
/// </summary>
System.Int16 UserLevel
{
get;
}
/// <summary>
/// Gets the value of the database column `user_x`.
/// </summary>
System.UInt16 UserX
{
get;
}
/// <summary>
/// Gets the value of the database column `user_y`.
/// </summary>
System.UInt16 UserY
{
get;
}
/// <summary>
/// Gets the value of the database column `when`.
/// </summary>
System.DateTime When
{
get;
}
}

}
