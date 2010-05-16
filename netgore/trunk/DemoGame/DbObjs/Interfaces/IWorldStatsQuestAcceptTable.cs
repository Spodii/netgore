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

This file was generated on (UTC): 5/16/2010 7:45:19 PM
********************************************************************/

using System;
using System.Linq;
using NetGore;
using NetGore.Features.Quests;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `world_stats_quest_accept`.
    /// </summary>
    public interface IWorldStatsQuestAcceptTable
    {
        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapID? MapID { get; }

        /// <summary>
        /// Gets the value of the database column `quest_id`.
        /// </summary>
        QuestID QuestID { get; }

        /// <summary>
        /// Gets the value of the database column `user_id`.
        /// </summary>
        CharacterID UserId { get; }

        /// <summary>
        /// Gets the value of the database column `when`.
        /// </summary>
        DateTime When { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        UInt16 X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        UInt16 Y { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IWorldStatsQuestAcceptTable DeepCopy();
    }
}