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
    http://www.netgore.com/wiki/dbclasscreator.html
********************************************************************/

using System;
using System.Linq;
using NetGore.World;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `world_stats_npc_kill_user`.
    /// </summary>
    public interface IWorldStatsNpcKillUserTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        UInt32 ID { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapID? MapID { get; }

        /// <summary>
        /// Gets the value of the database column `npc_template_id`.
        /// </summary>
        CharacterTemplateID? NpcTemplateId { get; }

        /// <summary>
        /// Gets the value of the database column `npc_x`.
        /// </summary>
        UInt16 NpcX { get; }

        /// <summary>
        /// Gets the value of the database column `npc_y`.
        /// </summary>
        UInt16 NpcY { get; }

        /// <summary>
        /// Gets the value of the database column `user_id`.
        /// </summary>
        CharacterID UserId { get; }

        /// <summary>
        /// Gets the value of the database column `user_level`.
        /// </summary>
        Byte UserLevel { get; }

        /// <summary>
        /// Gets the value of the database column `user_x`.
        /// </summary>
        UInt16 UserX { get; }

        /// <summary>
        /// Gets the value of the database column `user_y`.
        /// </summary>
        UInt16 UserY { get; }

        /// <summary>
        /// Gets the value of the database column `when`.
        /// </summary>
        DateTime When { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IWorldStatsNpcKillUserTable DeepCopy();
    }
}